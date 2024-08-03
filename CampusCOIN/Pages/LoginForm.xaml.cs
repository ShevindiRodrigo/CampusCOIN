using CampusCOIN.Services;
using CampusCOIN.Data;
using CampusCOIN.Models;

namespace CampusCOIN.Pages;

public partial class LoginForm : ContentPage
{
    private readonly UserData _userData;
    private readonly UserModel _user;

    public LoginForm()
	{
		InitializeComponent();

        _userData =  new UserData();
        _user = new UserModel();
	}

    private async void OnLogin_Clicked(object sender, EventArgs e)
    {
        //if login failed, display error and return
        if (!await FirebaseAuthManager.Login(EmailEntry.Text, PasswordEntry.Text))
        {
            await Application.Current.MainPage.DisplayAlert("Error!", "Could not log in, Please check credentials and try again", "OK");
            EmailEntry.Text = "";
            PasswordEntry.Text = "";
            return;
        }

        _user.UserId = await _userData.GetUserByEmail(EmailEntry.Text);
        await Shell.Current.GoToAsync("//MainPage");
    }

    //function to send reset password link to email when tapped forgot password
    public async void OnForgotPassword(object sender, TappedEventArgs e)
    {
        if(!string.IsNullOrEmpty(EmailEntry.Text))
        {
            bool reset = await FirebaseAuthManager.ResetPassword(EmailEntry.Text);
            if (reset)
            {
                //Show a success message 
                await Application.Current.MainPage.DisplayAlert("Successful!", "Password reset email sent successfully!", "OK");

            }
            else
            {
                // Show an error message
                await Application.Current.MainPage.DisplayAlert("Error!", "Error resetting password. Please try again.", "OK");

            }
        }
    }

}