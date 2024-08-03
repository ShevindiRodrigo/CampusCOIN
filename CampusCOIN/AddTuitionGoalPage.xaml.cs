using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Pages;
using CampusCOIN.Services;

namespace CampusCOIN;

public partial class AddTuitionGoalPage : ContentPage
{

    private readonly TuitionGoalData goalData;
    private readonly Tuitiongoal goal;
    public string userId;

    public AddTuitionGoalPage(TuitionGoalData data)
	{
		InitializeComponent();
        userId = FirebaseAuthManager.currentUser.UserId;

        goalData = data;
        goal = new Tuitiongoal();

        // Update the UI based on the current user's goal status
        UpdateUIActiveStatus(userId);
	}

    // Function to update the UI elements based on the user's goal status
    private async void UpdateUIActiveStatus(string userId)
    {
        DateTime TodayDate = DateTime.Now;

        List<Tuitiongoal> tuitiongoals = await goalData.GetTuitiongoal();

        List<Tuitiongoal> filterList = tuitiongoals.Where(t => t.UserID == userId).ToList();


        if (filterList.Count >0) 
        {
            foreach(Tuitiongoal t in filterList)
            {
                //Check if today is the goal due date
                if (TodayDate.Equals(t.dueDate))
                {
                    // If due today, delete all goals and reset UI for adding a new goal
                    await goalData.DeleteAllTuitiongoals();
                    GoalPageTitleLabel.IsVisible = true;
                    SetGoalBtn.IsVisible = true;
                    SummaryFrameI.IsVisible = false;
                    SummaryFrameII.IsVisible = false;
                    SummaryPageSubTitle.IsVisible = false;
                }
                else
                {
                    // If not due today, display the goal summary
                    GoalPageTitleLabel.IsVisible = false;
                    SetGoalBtn.IsVisible = false;
                    SummaryFrameI.IsVisible = true;
                    SummaryFrameII.IsVisible = true;
                    SummaryPageSubTitle.IsVisible = true;


                    Date.Text = "Due On: " + t.dueDate.ToShortDateString();
                    Amount.Text = "$ " + t.amount;
                    Goal.Text = "$ " + t.monthlyGoal;

                }
            }
            


        }
        else
        {
            // If no goals exist for the user, show the option to add a new goal
            GoalPageTitleLabel.IsVisible = true;
            SetGoalBtn.IsVisible = true;
            SummaryFrameI.IsVisible = false;
            SummaryFrameII.IsVisible = false;
            SummaryPageSubTitle.IsVisible = false;
        }
    }
	public async void AddTuitionGoal_Clicked(object sender, EventArgs e)
	{
        //Navigate to Tution Goal Form to add new tuition goal
        await Navigation.PushAsync(new AddGoalForm(goalData,userId));
    }
}