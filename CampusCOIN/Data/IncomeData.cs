using SQLite;
using CampusCOIN.Models;

namespace CampusCOIN.Data
{
    public class IncomeData:ObservableObject 
    {

        private readonly SQLiteAsyncConnection connection;
        private readonly BudgetData budgetData;

        public IncomeData()
        {
            //create new SQLite database connection
            connection = new SQLiteAsyncConnection(Constants.DatabasePath);

            //create income table
            connection.CreateTableAsync<Income>();
            budgetData = new BudgetData();

        }

        //save new income data
        public async Task<int> SaveIncome(Income income)
        {
            try
            {
                await connection.InsertAsync(income);
                await budgetData.UpdateBalances(income.Date.Month, income.UserID);


            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                await Shell.Current.DisplayAlert("Error Saving Income", ex.Message, "Ok");
            }
            return income.Id;
        }

        //delete an income data
        public async Task DeleteIncomeData(Income income)
        {
            //await Init();
            await connection.DeleteAsync(income);
        }

        //get income data by title, category, date and userid
        public async Task<List<Income>> GetIncomeByFields(string title, string category, DateTime date,string userID)
        {
            //await Init();
            return await connection.Table<Income>()
                .Where(i => i.Title == title && i.Category == category && i.Date == date && i.UserID == userID).ToListAsync();
        }

        //get income data where give user id exists
        public async Task<List<Income>> GetIncome(string userID)
        {
            try
            {
                return await connection.Table<Income>().Where(i => i.UserID == userID).OrderByDescending(e => e.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed To Retrieve Income Data", ex.Message, "Ok");

            }

            return new List<Income>();

        }

        //delete all income data
        public async Task DeleteAllIncome()
        {
            try
            {
                await connection.DeleteAllAsync<Income>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error Deleting Income", ex.Message, "OK");
            }
        }

    }
}
