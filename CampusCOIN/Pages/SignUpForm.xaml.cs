using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Services;
using System.Text.RegularExpressions;

namespace CampusCOIN.Pages;

public partial class SignUpForm : ContentPage
{
    private readonly UserData _userData;
    private readonly UserModel _user;
	public SignUpForm()
	{
		InitializeComponent();
        _userData = new UserData();
        _user = new UserModel();
	}
    public async void EmailValidation(object sender, TextChangedEventArgs e)
    {
        //Email Validation
        EmailError.IsVisible = false;
        EmailError.Text = string.Empty;

        // Check if the email format is invalid
        if (!Regex.IsMatch(EmailEntry.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase))
        {
            EmailError.IsVisible = true;
            EmailError.Text = "Email is invalid.";
        }
        else
        {
            // Check if the email is already in use
            _user.UserId = await _userData.GetUserByEmail(EmailEntry.Text);
            if (_user.UserId != null)
            {
                EmailError.IsVisible = true;
                EmailError.Text = "Entered Email Already in Use! Use Another Email.";
            }

        }
    }

    public void PasswordValidation(object sender, TextChangedEventArgs e)
    {

        Boolean HasError = false;
        String ErrorMessage = string.Empty;

        //Email Validation
        if (string.IsNullOrEmpty(PasswordEntry.Text) || PasswordEntry.Text.Length < 8)
        {
            HasError = true;
            ErrorMessage += "Password must be at least 8 characters long.\n";
        }
        if (!Regex.IsMatch(PasswordEntry.Text, @"[A-Z]"))
        {
            HasError = true;
            ErrorMessage += "Password must contain at least one uppercase letter.\n";
        }
        if (!Regex.IsMatch(PasswordEntry.Text, @"[a-z]"))
        {
            HasError = true;
            ErrorMessage += "Password must contain at least one lowercase letter.\n";
        }
        if (!Regex.IsMatch(PasswordEntry.Text, @"[0-9]"))
        {
            HasError = true;
            ErrorMessage += "Password must contain at least one digit.\n";
        }
        if (!Regex.IsMatch(PasswordEntry.Text, @"[\W_]"))
        {
            HasError = true;
            ErrorMessage += "Password must contain at least one special character.";
        }
        if (HasError)
        {
            PwdError.IsVisible = true;
            PwdError.Text = ErrorMessage;
        }
        else
        {
            PwdError.IsVisible= false;
        }
    }



//function to add new user when Join button clicked

public async void OnJoin_Clicked(object sender, EventArgs e){
        
        UserModel userModal = new UserModel()
        {
            Email = EmailEntry.Text,
            Name = NameEntry.Text,
            Phone = PhoneEntry.Text
        };

        //send Authmanager
        var result = await FirebaseAuthManager.RegisterAccount(userModal, PasswordEntry.Text);
        if (result)
        {
            //save user in local storage
            await _userData.SaveUser(userModal);
            await Navigation.PushModalAsync(new LoginPage());
        }
        else
        {
            await DisplayAlert("Error", "Could not create account", "OK");
        }
    }

}
