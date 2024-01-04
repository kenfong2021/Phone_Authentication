using PhoneAuth.Services;
using System.Threading.Tasks;
 

namespace PhoneAuth
{
    public partial class MainPage : ContentPage
    {
        private readonly IAuthenticationService _authenticationService;
        private const string AndroidSiteKey = "6Le9okQpAAAAAFo_2iBb3pXY8QbnWnU47BGtnUGq";
        private const string AndroidSecretKey = "6Le9okQpAAAAAJepuKL1OirK1arNKVwe2xragBxe";

        public MainPage(IAuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMobileNo.Text))
            {

                //var api = Android.Gms.SafetyNet.SafetyNetClass.GetClient(Platform.CurrentActivity);
                //var response = await api.VerifyWithRecaptchaAsync(AndroidSiteKey);
                //if (response != null && !string.IsNullOrEmpty(response.TokenResult))
                //{
                //    var captchaResponse = await ValidateCaptcha(response.TokenResult, AndroidSecretKey);
                //    if (captchaResponse is null || !captchaResponse.Success)
                //    {
                //        await Toast.Make($"Invalid captcha: {string.Join(",", captchaResponse?.ErrorCodes ?? Enumerable.Empty<object>())}", ToastDuration.Long).Show();
                //        return;
                //    }

                //    if (Platform.CurrentActivity!.PackageName != captchaResponse.ApkPackageName)
                //    {
                //        await Toast.Make($"Package Names do not match: {captchaResponse.ApkPackageName}", ToastDuration.Long).Show();
                //    }
                //    else
                //    {
                //        await Toast.Make("Success", ToastDuration.Long).Show();
                //    }
                //}
                //else
                //{
                //    await Toast.Make("Failed", ToastDuration.Long).Show();
                //}

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
