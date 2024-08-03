namespace CampusCOIN.Pages;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();

    }

    public void OnJoinBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new SignUpForm());
    }

    public void ExistingUserBtn_Clicked(Object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LoginForm());

    }
}