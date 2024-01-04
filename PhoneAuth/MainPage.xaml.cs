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
                var isValidCode = await _authenticationService.ValidateOTP(txtCode.Text);
                if (isValidCode)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new NewPage1());
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
    }

}
