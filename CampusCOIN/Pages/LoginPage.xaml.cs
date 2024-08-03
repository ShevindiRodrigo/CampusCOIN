using CampusCOIN.Models;

namespace CampusCOIN.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    public void OnLogin_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LoginForm());
    }

}