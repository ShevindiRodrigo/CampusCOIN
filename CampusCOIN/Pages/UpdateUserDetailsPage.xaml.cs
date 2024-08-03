using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Services;

namespace CampusCOIN.Pages;

public partial class UpdateUserDetailsPage : ContentPage
{
    public string userID;
    private UserModel _user;
    private readonly UserData _userData;
    public List<UserModel> userList;


    public UpdateUserDetailsPage()
	{
		InitializeComponent();
        userID = FirebaseAuthManager.currentUser.UserId;
        _userData = new UserData();
        _user = new UserModel();
        userList = new List<UserModel>();

        BindingContext = _user;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //Fill user details based on current user id
         userList = await _userData.GetUserById(userID);
        if (userList.Count >0)
        {
            _user = userList.FirstOrDefault();
        }
        if(_user != null)
        {
            NameEntry.Text = _user.Name;
            EmailEntry.Text = _user.Email;
            PhoneEntry.Text = _user.Phone;
        }
       
       
    }

    //Click event to update account details
    public async void OnUpdate_Clicked(object sender, EventArgs e)
    {
        // Validate input fields
        if (string.IsNullOrWhiteSpace(_user.Name) || string.IsNullOrEmpty(_user.Email)
            || string.IsNullOrEmpty(_user.Phone))
        {
            await DisplayAlert("Cannot have empty space.Entry required!", "Please enter user details", "OK!");
            return;
        }

        _user.UserId = userID;
        await _userData.UpdateUser(_user);
        await DisplayAlert("YOU Have updated", $"You have updated user {_user.Name} successfully ", "OK!");


    }




}