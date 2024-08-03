using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Services;
using Plugin.LocalNotification;

namespace CampusCOIN.Pages;

public partial class SetBudgetPage : ContentPage
{
	private readonly BudgetData data;
    private readonly ExpenseData _expenseData;
    private readonly IncomeData _incomeData;
    private readonly TuitionGoalData tuitiongoalData;

    private readonly Budget budget;

	public string userId;

	public SetBudgetPage(BudgetData budgetData, string userID)
	{
		InitializeComponent();
		userId = userID;
		data = budgetData;
		budget = new Budget();
		_expenseData = new ExpenseData();
		_incomeData = new IncomeData();
		tuitiongoalData = new TuitionGoalData();

		BindingContext = budget;
	}

	//method to add new budget when set button clicked
	public async void OnSetClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(budget.Amount))
		{

			await DisplayAlert("Cannot have empty space.Entry required!", "Please enter budget amount", "OK!");
			return;
		}

		budget.Month = DateTime.Now.Month;
		budget.UserID = userId;

		await data.SetBudget(budget,userId);

       //show notification to add a budget
		DateTime NotifyTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
        NotificationManager.SendNotification("CAMPUSCOIN", "Its a new Month. Start traking your budget!", NotifyTime);


        await Navigation.PushAsync(new MainPage(_expenseData,_incomeData,data,tuitiongoalData));

    }
}