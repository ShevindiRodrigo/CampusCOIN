using CampusCOIN.Data;
using CampusCOIN.Models;
using System.Collections.ObjectModel;

namespace CampusCOIN.Pages;

public partial class ExpenseListPage : ContentPage
{
    private readonly ExpenseData data;

    // ObservableCollection to bind to the UI for displaying expenses
    public ObservableCollection<Expense> ExpensesList { get; set; }
    public string userId;


    public ExpenseListPage(ExpenseData expenseData, string userID)
    {
        InitializeComponent();
        data = expenseData;
        userId = userID;
        ExpensesList = new ObservableCollection<Expense>();


        BindingContext = data;


    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetExpenseData();

    }

    public async Task GetExpenseData()
    {
        try
        {

            //Retrieve the list of expenses from data service
            List<Expense> list = await data.GetExpenses(userId);

            //Add each expenses to observableCollection
            foreach (var expense in list)
            {

                ExpensesList.Add(expense);

            }
            // Bind the ObservableCollection to the CollectionView
            ExpensesCollectionView.ItemsSource = ExpensesList;


        }
        catch (Exception ex)
        {
            //display error message on unsuccessful retreival
            await DisplayAlert("Failed to Retrieve Expense Data", ex.Message, "OK");
        }
    }

    public async void AddExpense_Clicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new AddExpensePage(data,userId));
    }
}