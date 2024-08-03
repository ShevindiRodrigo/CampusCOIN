using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Services;
using Plugin.LocalNotification;

namespace CampusCOIN;

public partial class GoalTrackerPage : ContentPage
{
    private readonly TuitionGoalData _goalData;
    private readonly BudgetData _budgetData;
    //private readonly Budget budget;
    private readonly Tuitiongoal tuition;
    public string userId;


    public GoalTrackerPage(BudgetData budgetData, TuitionGoalData tuitionGoalData)
    {
        InitializeComponent();
        userId = FirebaseAuthManager.currentUser.UserId;
        _goalData = tuitionGoalData;
        _budgetData = budgetData;
        //budget = new Budget();
        tuition = new Tuitiongoal();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_goalData != null)
        {
            // display the due date 
            DueDate.Text = "Due Date: " + await GetDueDate();

            //calculate total progress
            Double progress = await CalculateTotalProgress();
            CirculartotalProgress.Progress = progress;

            //calculate monthly goal progress
            MonthlyProgress.Progress = await CalculateMonthlyProgress();
        }

    }

    /*Function to get the due date of the currently active tuition fee goal
     */
    public async Task<string> GetDueDate()
    {
        string dueDate = "";
        List<Tuitiongoal> goalList = await _goalData.GetTuitiongoal();


        List<Tuitiongoal> filterGoalList = goalList.Where(g => g.UserID == userId).ToList();

        //foreach (var g in filterGoalList)
        //{
        //    System.Diagnostics.Debug.WriteLine($" goalDate: {g.DueDate}");

        //}
        Tuitiongoal goal = filterGoalList.LastOrDefault();
        //System.Diagnostics.Debug.WriteLine($" TotalBal: {goal == null}");


        if (filterGoalList.Count > 0)
        {
            dueDate = goal.DueDate.ToShortDateString();
        }
        return dueDate;
    }

    /*Function to calculate progress
     */
    public async Task<double> CalculateTotalProgress()
    {
        decimal total = await _budgetData.GetTotalBalance(userId);
        double totalBalance = Convert.ToDouble(total);


        await _goalData.GetCurrentGoalAmount(tuition, userId);
        double goalAmount = Convert.ToDouble(tuition.Amount);

        double Progress = 0;
        try
        {
            if (goalAmount > 0)
            {
                Progress = totalBalance / goalAmount;
                if (Progress > 1)
                {
                    Progress = 1;

                }
            }
            if (Progress == 1)
            {
                //Show a notification when goal achieved
                DateTime NotifyTime = DateTime.Now;
                NotificationManager.SendNotification("CAMPUSCOIN", "You have achieved fee goal! Congratulations!", NotifyTime);
            }
            //if progress value is 0 set progress as 0 otherwise calculate the progress
            if (Progress > 0)
            {
                TotalProgresslbl.Text = Math.Round(Progress * 100).ToString() + "%";

            }
            else
            {
                TotalProgresslbl.Text = "0%";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error in Calculating Progress!", ex.Message, "OK");
        }
        return Progress;

    }

    /*Function to calculate monthly progress
     */
    public async Task<double> CalculateMonthlyProgress()
    {
        double Progress = 0;
        double monthlyGoalAmount = 0;
        double MonthlyBalance = 0;

        try
        {

            if (await _budgetData.GetMonthlyBalance(DateTime.Now.Month, userId) == "")
            {
                Progress = 0;
            }
            else
            {
                MonthlyBalance = Convert.ToDouble(await _budgetData.GetMonthlyBalance(DateTime.Now.Month, userId));
                List<Tuitiongoal> goalList = await _goalData.GetTuitiongoal();
                List<Tuitiongoal> filterGoalList = goalList.Where(g => g.UserID == userId).ToList();
                Tuitiongoal goal = filterGoalList.LastOrDefault();

                if (filterGoalList.Count > 0)
                {
                    //System.Diagnostics.Debug.WriteLine($"month goal: {goal.MonthlyGoal}");
                    monthlyGoalAmount = Convert.ToDouble(goal.MonthlyGoal);
                }

                if (monthlyGoalAmount > 0)
                {
                    Progress = MonthlyBalance / monthlyGoalAmount;
                    if (Progress > 1)
                    {
                        Progress = 1;

                    }
                }
            }
            //if progress value is 0 set progress as 0 otherwise calculate the progress
            if (Progress > 0)
            {
                Progreesslbl.Text = Math.Round(Progress * 100).ToString() + "%";

            }
            else
            {
                Progreesslbl.Text = "0%";
            }
            System.Diagnostics.Debug.WriteLine($"Progress: {Progress}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error in Calculating Progress!", ex.Message, "OK");
        }
        return Progress;


    }

}