using CampusCOIN.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusCOIN.Data
{
    public class TuitionGoalData
    {
        private readonly SQLiteAsyncConnection connection;
        private readonly BudgetData budgetData;

        public TuitionGoalData()
        {
            //if connection null create new SQLite database connection
            if (connection == null)
            {
                connection = new SQLiteAsyncConnection(Constants.DatabasePath);
            }

            //create tuition goal table
            connection.CreateTableAsync<Tuitiongoal>().Wait();
            budgetData = new BudgetData();

        }

        //add a new tuition goal
        public async Task<int> AddGoal(Tuitiongoal tuitiongoal)
        {
            try
            {
                var userId = tuitiongoal.UserID;
                //Ensure that there should be one tuition goal exits in the database
                //Check for number of tuitiongoals exist in Tuitiongoal table
                List<Tuitiongoal> list = await GetTuitiongoal();
                List<Tuitiongoal> filterList = list.Where(t => t.UserID == userId).ToList();

                if(filterList == null || filterList.Count == 0)
                {
                    await connection.InsertAsync(tuitiongoal);
                }
                else
                {
                    await DeleteAllTuitiongoals();
                    await connection.InsertAsync(tuitiongoal);
                    await Shell.Current.DisplayAlert("You Have added Goal Successfully!", $"You have set a budget successfully!", "OK!");

                }

            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                await Shell.Current.DisplayAlert("Error Adding Tuition Fee Goal", ex.Message, "Ok");
            }
            return tuitiongoal.Id;
        }

        //Get all the Tuitiongoals exits in the table
        public async Task<List<Tuitiongoal>> GetTuitiongoal(){
            try
            {
                return await connection.Table<Tuitiongoal>().ToListAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed To Retrieve Tuitiongoal Data", ex.Message, "Ok");

            }

            return new List<Tuitiongoal>();
        }

        //Delete all goals
        public async Task DeleteAllTuitiongoals()
        {
            try
            {
                await connection.DeleteAllAsync<Tuitiongoal>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error Deleting Goal", ex.Message, "OK");
            }
        }


     
        //calculate current goal amount
        public async Task GetCurrentGoalAmount(Tuitiongoal tuitiongoal, string userId)
        {
            try
            {
                //await SetMonthlyBalance(budget,month);
                List<Tuitiongoal> goalList = await GetTuitiongoal();
                List<Tuitiongoal> filterList = goalList.Where(t => t.UserID == userId).ToList();

                Tuitiongoal goal = filterList.FirstOrDefault();
                if (filterList.Count>0)
                {
                    tuitiongoal.Amount = goal.Amount;
                    //System.Diagnostics.Debug.WriteLine($" Total Balance: {tuitiongoal.Amount}");

                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed To Retrieve Budget Data", ex.Message, "Ok");

            }
        }

        

    }
}
