using CampusCOIN.Models;
using SQLite;

namespace CampusCOIN.Data
{
    public class ExpenseData : ObservableObject
    {
        private readonly SQLiteAsyncConnection connection;
        private readonly BudgetData budgetData;

        public ExpenseData()
        {
            //create new connection
            connection = new SQLiteAsyncConnection(Constants.DatabasePath);

            //add new expense table
            connection.CreateTableAsync<Expense>();
            budgetData = new BudgetData();

        }

        //save expense
        public async Task<int> SaveExpense(Expense expense)
        {
            try
            {
                await connection.InsertAsync(expense);
                await budgetData.UpdateBalances(expense.Date.Month, expense.UserID);


            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                await Shell.Current.DisplayAlert("Error Saving Expense", ex.Message, "Ok");
            }
            return expense.Id;
        }

        //delete expense
        public async Task DeleteExpenseData(Expense expense)
        {
            //await Init();
            try
            {
                await connection.DeleteAsync(expense);
                await Shell.Current.DisplayAlert("Deletion Successful!", "Your data was successfully deleted.", "Ok");

            }

            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Deletion Unsuccessful!", ex.Message, "Ok");

            }

        }

        //get expense data based on title category date and user id
        public async Task<List<Expense>> GetExpenseAsync(string title, string category, DateTime date, string userID)
        {
            //await Init();
            try
            {
                return await connection.Table<Expense>()
                .Where(e => e.Title == title && e.Category == category && e.Date == date && e.UserID == userID).ToListAsync();
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Failed To Retrieve Expense Data", ex.Message, "Ok");

            }

            return new List<Expense>();

        }

        //get all expenses
        public async Task<List<Expense>> GetExpenses(string userID)
        {
            try
            {
                return await connection.Table<Expense>().Where(e => e.UserID == userID).OrderByDescending(e => e.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed To Retrieve Expense Data", ex.Message, "Ok");

            }

            return new List<Expense>();

        }

        //update expense data
        public async Task UpdateExpenseData(Expense expense)
        {
            try
            {
                await connection.UpdateAsync(expense);
                await Shell.Current.DisplayAlert("Update Successful!", "Your data was successfully updated.", "Ok");

            }

            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Update Unsuccessful!", ex.Message, "Ok");

            }
        }

        //delete all expenses
        public async Task DeleteAllExpense()
        {
            try
            {
                await connection.DeleteAllAsync<Expense>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error Deleting Expense", ex.Message, "OK");
            }
        }
    }
}
