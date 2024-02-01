using Firebase.Database;
using Firebase.Database.Query;
using PhoneAuth.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace PhoneAuth;

public partial class MyNotes : ContentPage
{
    public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();
    private FirebaseClient firebaseClient;
    private string _userToken { get; set; } = string.Empty;
    private string _userID { get; set; } = string.Empty;
    private string _previousValue { get; set; } = string.Empty;
    public MyNotes(string userToken, string userId)
	{
		InitializeComponent();
        BindingContext = this;
        _userToken = userToken;
        _userID = userId;

        var opt = new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(_userToken) };
        firebaseClient = new FirebaseClient("https://maui-fuelprice-default-rtdb.firebaseio.com/", opt);

        var collection = firebaseClient
            .Child("users")
            .Child(_userID)
            .AsObservable<Note>()
            .Subscribe((item) =>
            {
                if (item.Object != null)
                {
                    var note = Notes.FirstOrDefault(f => f.Key == item.Key);
                    if (note != null) Notes.Remove(note);
                    Notes.Add(item.Object);
                }
            });
    }

    private void btnSubmit_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RemarkEntry.Text)) return;
        if (!string.IsNullOrWhiteSpace(_previousValue))
        { 
            firebaseClient
                        .Child("users")
                        .Child(_userID)
                        .Child(_previousValue)
                        .PutAsync(new Note
                        {
                            UID = _userID,
                            Key = _previousValue,
                            Remark = RemarkEntry.Text,
                        });
        }
        else
        {
            firebaseClient.Child("users").Child(_userID).PostAsync(new Note
            {
                UID = _userID,
                Remark = RemarkEntry.Text,
            });
        }
        
    }
     
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var label = (Label)sender;
        var source = (Note)label.BindingContext;
        _previousValue = source.Key;
        RemarkEntry.Text = source.Remark;
        btnSubmit.Text = "Update";
    }

    private void btnClear_Clicked(object sender, EventArgs e)
    {
        _previousValue = string.Empty;
        RemarkEntry.Text = string.Empty;
        btnSubmit.Text = "Save";
    }
}