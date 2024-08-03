using CampusCOIN.Data;
using CampusCOIN.Models;

namespace CampusCOIN.Pages;

public partial class ReceiptViewPage : ContentPage
{
	private readonly ExpenseData data; 
	private Expense expense;

	public ReceiptViewPage(Expense expenseData)
	{
		InitializeComponent();
		expense = expenseData;
		data = new ExpenseData();

        //display the expense data
        GetExpenseDataByCriteria();
	}

    //Function to retrieve expense data based on criteria and display it
    public async void GetExpenseDataByCriteria()
	{
        string title = expense.Title;
        string category = expense.Category;
        DateTime date = expense.Date;
		string userId = expense.UserID;
		

        List<Expense> list = await data.GetExpenseAsync(title, category, date,userId);

		if (list.Count == 0)
		{
			await DisplayAlert("Cannot Find Expense Data", "Expense data is not available.Please try another Search!.", "OK");
		}

		Title.Text = title;
		Category.Text = category;
		Date.Text = date.ToShortDateString();


        foreach (var exp in list)
        {

            Amount.Text = exp.Amount;
			Description.Text = exp.Description;
			ReceiptImage.Source = exp.Receipt_path;

			//Set values to expense instance
			expense.Receipt_path = exp.Receipt_path;
			expense.Amount = exp.Amount;
			expense.Description = exp.Description;
			expense.Id = exp.Id;

        }
    }

    //Click event for edit expense
    public async void ExpenseEdit_Clicked(object sender, EventArgs e)
	{

		await Navigation.PushAsync(new EditExpensePage(expense));

	}

	//Click event to delete expense
	public async void ExpenseDelete_Clicked(object sender, EventArgs e)
	{
		await data.DeleteExpenseData(expense);
		await Navigation.PopAsync();
	}
}