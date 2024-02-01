using PhoneAuth.Services;
using System.Threading.Tasks;
 

namespace PhoneAuth
{
    public partial class MainPage : ContentPage
    {
        private readonly IAuthenticationService _authenticationService;
         
        public MainPage(IAuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
             
        }
         
        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMobileNo.Text))
            { 
                var isValidMobile = await _authenticationService.AuthenticateMobile(txtMobileNo.Text);
                if (isValidMobile)
                {
                    pnlMobileInfo.IsVisible = false;
                    pnlMobileVerification.IsVisible = true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Opps", "Enter Valid Mobile No", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Opps", "Enter Mobile No", "OK");
            }
        }

        private async void btnVerifyCode_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                var result = await _authenticationService.ValidateOTP(txtCode.Text);
 
                if (result.verified)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new MyNotes(result.userToken,result.userID));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Opps", "Enter Valid Code", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Opps", "Please Enter The Code", "OK");
            }
        }

        private void myDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                var code = (string)picker.ItemsSource[selectedIndex];
                txtMobileNo.Text = $"+{ code.Split("---")[0].Trim()}";
            }
        }
    }

}
