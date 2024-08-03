using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Services;


namespace CampusCOIN.Pages;

// Bind query property to handle TuitionGoal data passed through navigation
[QueryProperty("TuitionGoal", "TuitionGoal")]

public partial class AddGoalForm : ContentPage
{ 
	private readonly TuitionGoalData tuitionGoalData;
    public string userID;
	Tuitiongoal tuitiongoal { get; set; }



	public AddGoalForm(TuitionGoalData goalData, string userId)
	{
		InitializeComponent();
		tuitionGoalData = goalData;
        userID = userId;
		tuitiongoal = new Tuitiongoal();

		BindingContext = tuitiongoal;

	}

    //Click event for the WebView button click to navigate to the WebViewPage
    public async void WebView_Clicked(object sender, EventArgs e)
	{
        //Navigate to Web view to search specific due date
        await Navigation.PushAsync(new WebViewPage());
    }

    //Click event for the AddGoal button click to add a new tuition goal
    public async void AddGoal_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tuitiongoal.amount) )
		{
            await DisplayAlert("Cannot have empty space.Entry required!", "Please enter the data", "OK!");
            return;
        }
        tuitiongoal.UserID = userID;
        //System.Diagnostics.Debug.WriteLine($"TuitionGoal is null: {tuitionGoalData == null}");
        //System.Diagnostics.Debug.WriteLine($"Amount: {tuitiongoal.amount}");
        //System.Diagnostics.Debug.WriteLine($"Duedate: {tuitiongoal.dueDate}");
        //System.Diagnostics.Debug.WriteLine($"goal: {tuitiongoal.monthlyGoal}");

        // Save the TuitionGoal data
        tuitiongoal.UserID = userID;
		await tuitionGoalData.AddGoal(tuitiongoal);

        await DisplayAlert("YOU Have added", $"You have added TuitionGoal successfully", "OK!");


        var goalData = new Tuitiongoal();
        goalData.amount = tuitiongoal.amount;
        goalData.dueDate = tuitiongoal.dueDate;
        goalData.monthlyGoal = tuitiongoal.monthlyGoal;

        DateTime NotifyTime = goalData.dueDate.AddDays(-15);
        NotificationManager.SendNotification("CAMPUSCOIN", "You are close to tuition due payment date! Keep On Saving!", NotifyTime);

        await Shell.Current.Navigation.PushAsync(new TuitionGoalSummaryPage(goalData));

    }

  
    //Click event for calculate and view monthly goal
    public void OnTapViewMonthlyGoal(object sender, TappedEventArgs t)
    {
        Binding goal = new Binding();
        goal.Source = tuitiongoal;
        goal.Path = "MonthlyGoal";
        MonthlyGoal.SetBinding(Label.TextProperty, goal);


    }
}