using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Pages;
using CampusCOIN.Services;

namespace CampusCOIN;

public partial class ExpenseSearchPage : ContentPage
{

    private readonly ExpenseData data;
    private Expense Expense;
    public string userId;


    public ExpenseSearchPage(ExpenseData expenseData)
	{
		InitializeComponent();
        userId = FirebaseAuthManager.currentUser.UserId;
        Date.Date = DateTime.Now;
        data = expenseData;
        Expense = new Expense();
        BindingContext = Expense;
    }

    /*Click event to search and retrive data based on title, category and date
     */
    public async void ExpenseSearch_Clicked(object sender, EventArgs e)
    {
        string title = Title.Text;
        string category = Category.Text;
        DateTime date = Date.Date;

        if (title.Equals("") || category.Equals("") || date.Equals(""))
        {
            await DisplayAlert("Entries Required!", "Please Fill All the Entries.", "OK!");
        }

        Expense.UserID = userId;
        Expense.Title = title;
        Expense.Category = category;
        Expense.Date = date;

        await Shell.Current.Navigation.PushAsync(new ReceiptViewPage(Expense));

    }

    //Click event to clear search details 
    public void Clear_Clicked(object sender, EventArgs e)
    {
        Title.Text = null;
        Category.Text = null;
        Date.Date = DateTime.Today;

    }
}