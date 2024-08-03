using CampusCOIN.Data;
using CampusCOIN.Models;

namespace CampusCOIN.Pages;

// Bind query property to handle Income data passed through navigation
[QueryProperty("Income", "Income")]
public partial class AddIncomePage : ContentPage
{
    private readonly IncomeData _data;
    private readonly BudgetData budgetData;
    private Income income;
    private Budget Budget;
    public string userID;

    public AddIncomePage(IncomeData incomeData, string userId)
	{
		InitializeComponent();
        _data = incomeData;
        userID = userId;
        budgetData = new BudgetData();
        income = new Income();
        Budget = new Budget();
        Date.Date = DateTime.Now.Date;
        BindingContext = income;

    }

    // Event handler for the Add button click
    public async void OnAddClicked(object sender, EventArgs e)
    {
        // Validate input fields
        if (string.IsNullOrWhiteSpace(income.Title) || string.IsNullOrEmpty(income.Category)
            || string.IsNullOrEmpty(income.Amount) || string.IsNullOrEmpty(income.Description))
        {
            await DisplayAlert("Cannot have empty space.Entry required!", "Please enter Income data", "OK!");
            return;
        }

        //await DisplayAlert("Details", Income.Title + Income.Category + Income.Date +
        //Income.Amount +Income.Receipt_path, "OK");
        //System.Diagnostics.Debug.WriteLine($"IncomeData is null: {_data == null}");
        //System.Diagnostics.Debug.WriteLine($"IncomeData is null: {income.Category}");
        //System.Diagnostics.Debug.WriteLine($"IncomeData is null: {income.Date}");

        income.UserID = userID;
        // Save the Income data
        //System.Diagnostics.Debug.WriteLine($"user: {income.UserID}");

        int id = await _data.SaveIncome(income);
        await DisplayAlert("YOU Have added", $"You have added Income successfully with ID: {id}", "OK!");
        //await Shell.Current.GoToAsync("//MainPage");

        // Reset the form for new entry
        income = new Income();
        BindingContext = income;

    }
}