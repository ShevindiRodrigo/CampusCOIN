using CampusCOIN.Data;
using CampusCOIN.Models;
using System.Collections.ObjectModel;

namespace CampusCOIN.Pages;

public partial class IncomeListPage : ContentPage
{
    private readonly IncomeData data;
    public string userID;

    // ObservableCollection to bind to the UI for displaying expenses
    public ObservableCollection<Income> IncomeList { get; set; }


    public IncomeListPage(IncomeData incomeData, string userId)
    {
        InitializeComponent();
        //if (incomeData == null)
        //{
        //    throw new ArgumentNullException(nameof(incomeData));
        //}

        data = incomeData;
        userID = userId;
        IncomeList = new ObservableCollection<Income>();


        BindingContext = data;

        GetIncomeData(userID);

    }

    public async void GetIncomeData(string userID)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"user: {userID}");

            //Retrieve the list of income from data service
            List<Income> list = await data.GetIncome(userID);
            //System.Diagnostics.Debug.WriteLine($"IncomeData is null: {list == null}");



            //Add each expenses to observableCollection
            foreach (var income in list)
            {

                IncomeList.Add(income);


            }
            // Bind the ObservableCollection to the CollectionView
            IncomeCollectionView.ItemsSource = IncomeList;
        }
        catch (Exception ex)
        {
            //display error message on unsuccessful retreival
            await DisplayAlert("Failed to Retrieve Income Data", ex.Message, "OK");
        }
    }

    public async void AddIncome_Clicked(object sender, EventArgs e)
    {
        IncomeData income = new IncomeData();
        await Navigation.PushAsync(new AddIncomePage(income, userID));

    }
}