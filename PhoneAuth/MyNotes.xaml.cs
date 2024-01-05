using Firebase.Database;
using Firebase.Database.Query;
using PhoneAuth.Models;
using System.Collections.ObjectModel;

namespace PhoneAuth;

public partial class MyNotes : ContentPage
{
    public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();
    private FirebaseClient firebaseClient = new FirebaseClient("https://maui-fuelprice-default-rtdb.firebaseio.com/");
    private string _userID { get; set; } = string.Empty;
    public MyNotes(string userID)
	{
		InitializeComponent();
        BindingContext = this;
        _userID = userID;
        var collection = firebaseClient
            .Child("MyNote")
            .AsObservable<Note>()
            .Subscribe((item) =>
            {
                if (item.Object != null && item.Object.UID == _userID)
                {
                    Notes.Add(item.Object);
                }
            });
    }

    private void btnSubmit_Clicked(object sender, EventArgs e)
    {
        firebaseClient.Child("MyNote").PostAsync(new Note
        {
            UID = _userID,
            Remark = TitleEntry.Text,
        });
    }
}