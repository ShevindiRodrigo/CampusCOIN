using CampusCOIN.Pages;

namespace CampusCOIN;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
    }

    //Click event to naviagte to account settings page
    public async void OnAccountSetting_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new UpdateUserDetailsPage());

    }
}