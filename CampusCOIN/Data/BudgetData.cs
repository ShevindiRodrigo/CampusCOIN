using CampusCOIN.Models;
using Firebase.Auth;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace CampusCOIN.Data
{
    public class BudgetData: ObservableObject
    {
        private readonly SQLiteAsyncConnection connection;


        public BudgetData()
        {
            //if connection null create new SQLite database connection
            if (connection == null) 
            {
                connection = new SQLiteAsyncConnection(Constants.DatabasePath);
            }
            //create budget table
            connection.CreateTableAsync<Budget>();

        }

        //set new budget
        public async Task<int> SetBudget(Budget budget, string userId)
        {
            try
            {
                List<Budget> list = await GetBudget();

                //list all budgets for the current month
                List<Budget> filterlist = list.Where(b => b.Month.Equals(DateTime.Now.Month)).ToList();
                int Counter = 0;

                //System.Diagnostics.Debug.WriteLine($"No of budgets: {filterlist.Count}");
                foreach (var  filter in filterlist)
                {
                    if (filter.UserID == userId)
                    {
                        Counter += 1;
                    }

                }
                //if counter is 0 -> add new budget instance
                if (Counter == 0)
                {
                    //intial monthly balance is budget amount
                    budget.MonthlyBalance = budget.Amount;

                    //intitial total current balance is monthly balance
                    budget.TotalCurrentBalance = budget.MonthlyBalance;

                    //System.Diagnostics.Debug.WriteLine($"id: {budget.Id}");
                    //System.Diagnostics.Debug.WriteLine($"id: {budget.UserID}");
                    //System.Diagnostics.Debug.WriteLine($"amount: {budget.Amount}");
                    //System.Diagnostics.Debug.WriteLine($"month: {budget.Month}");
                    //System.Diagnostics.Debug.WriteLine($"monthBal: {budget.MonthlyBalance}");
                    //System.Diagnostics.Debug.WriteLine($"totBal: {budget.TotalCurrentBalance}");

                    await connection.InsertAsync(budget);
                    return budget.Id;

                }
                else
                {
                    await Shell.Current.DisplayAlert("Budget Aready Exists!", "You have already set a budget for current month.", "OK");
                }

            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message
                await Shell.Current.DisplayAlert("You have alreday set a budget", ex.Message, "Ok");
            }
            return budget.Id;
        }

        //get total income for the give  user
        public async Task<decimal> GetTotalIncome(int month, string userID)
        {

            decimal totalIncome = 0;
            int Year = DateTime.Now.Year;

            try
            {
                //get all income data for the user id and month
                List<Income> list = await connection.Table<Income>().ToListAsync();
                List<Income> filterList = list.Where(i => i.UserID == userID).ToList();

                if (filterList.Count > 0)
                {
                    foreach (var income in filterList)
                    {
                        if (income.Date.Month.Equals(month) && income.Date.Year.Equals(Year))
                        {
                            totalIncome += Decimal.Parse(income.Amount);
                            //System.Diagnostics.Debug.WriteLine($"Total Income: {income.Amount}");
                        }

                    }
                }
                else
                {
                    totalIncome = 0;
                }

                
            }
            catch (Exception ex) 
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");

            }



            return totalIncome;
        }

        //get all expenses for the given user and month
        public async Task<decimal> GetTotalExpense(int month, string userID)
        {
            decimal totalExpense = 0;
            int Year = DateTime.Now.Year;

            try
            {
                List<Expense> list = await connection.Table<Expense>().ToListAsync();
                List<Expense> filterList = list.Where(i => i.UserID == userID).ToList();
                // List<Expense> filterList = list.Where(e => e.Date.Month.Equals(month) && e.Date.Year.Equals(Year)).ToList();
                if (filterList.Count > 0)
                {
                    foreach (var expense in filterList)
                    {
                        if (expense.Date.Month.Equals(month) && expense.Date.Year.Equals(Year))
                        {
                            totalExpense += Decimal.Parse(expense.Amount);
                            //System.Diagnostics.Debug.WriteLine($"Total expense: {expense.Amount}");
                        }
                    }
                }
                else
                {
                    totalExpense = 0;
                }
                


            }
            catch (Exception ex) 
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            return totalExpense;

        }


        /*
         * Set monthly budget by calculating total expenses and incomes
         */
        public async Task SetMonthlyBalance(Budget budget, int Month)
        {
            var userID = budget.UserID;
            decimal totalIncome = await GetTotalIncome(Month, userID);

            decimal totalExpense = await GetTotalExpense(Month, userID);

            if(budget.Amount != null)
            {
                Decimal.TryParse(budget.MonthlyBalance, out var decMonthlyBalance);
                Decimal.TryParse(budget.Amount, out var decAmount);

                //claculate monthly balance for the given month
                decMonthlyBalance = decAmount + totalIncome - totalExpense;

                budget.MonthlyBalance = decMonthlyBalance.ToString();
                
                //await connection.UpdateAsync(budget);
                //System.Diagnostics.Debug.WriteLine($"Monthly Balance: {decMonthlyBalance}");

            }


        }

        //calculate total balance
        public async Task<decimal> GetTotalBalance(string userID)
        {
            decimal totalBalance = 0;

            try
            {
                //retrive all budget for given user
                List<Budget> list = await GetBudget();
                List<Budget> filterList = list.Where(b => b.UserID == userID).ToList();

                if(filterList.Count > 0)
                {
                    foreach (Budget b in filterList)
                    {
                        //for each budget data calculate monthly balance
                        await SetMonthlyBalance(b, b.Month);
                        //System.Diagnostics.Debug.WriteLine($"month bal: {b.MonthlyBalance}");

                        Decimal.TryParse(b.MonthlyBalance, out var decimalMonthlyBalance);
                        //add each monthly balance in the list to total balance
                        totalBalance += decimalMonthlyBalance;
                    }

                }
                else
                {
                    totalBalance = 0;
                }


                System.Diagnostics.Debug.WriteLine($"Total Balance: {totalBalance}");


            }
            catch (Exception ex) 
            {
                await Shell.Current.DisplayAlert("Error Data", ex.Message, "Ok");
            }

            return totalBalance;
        }

        //get all budget
        public async Task<List<Budget>> GetBudget()
        {
            try
            {
                return await connection.Table<Budget>().OrderByDescending(e => e.Month).ToListAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed To Retrieve Budget Data", ex.Message, "Ok");

            }

            return new List<Budget>();

        }

        //get monthly budget
        public async Task<string> GetMonthlyBalance(int month,string userID)
        {
            string monthlyBalance = "";

            try
            {
                List <Budget> list = await GetBudget();
                List<Budget> filterList = list.Where(b=> b.UserID == userID).ToList();

                if (filterList.Count > 0)
                {
                    foreach (Budget b in filterList)
                    {
                        if (b.Month.Equals(month))
                        {
                            await SetMonthlyBalance(b, month);
                            monthlyBalance = b.MonthlyBalance;
                        }
                    }
                }
                else
                {
                    monthlyBalance = "";
                }
                
                System.Diagnostics.Debug.WriteLine($" MonthlyBal: {monthlyBalance}");

                                      


            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed To Retrieve Budget Data", ex.Message, "Ok");

            }
            return monthlyBalance;

        }

        //get current total balalnce
        public async Task GetCurrentTotalBalance(int month, Budget budget,string userID)
        {
            try 
            {
                //await SetMonthlyBalance(budget,month);
                List<Budget> budgetList = await GetBudget();
                if(budgetList.Count>0)
                {
                    foreach (var b in budgetList)
                    {
                        if (b.UserID == userID && b.Month.Equals(month))
                        {
                            budget.TotalCurrentBalance = b.TotalCurrentBalance;
                            //System.Diagnostics.Debug.WriteLine($" Total Balance: {budget.TotalCurrentBalance}");
                        }
                    }
                }
                

                //Budget monthlyBudget = budgetList.FirstOrDefault(b => b.Month.Equals(month));
                //if (monthlyBudget != null)
                //{
                //    budget.TotalCurrentBalance = monthlyBudget.TotalCurrentBalance;                    
                //    System.Diagnostics.Debug.WriteLine($" Total Balance: {budget.TotalCurrentBalance}");
                                      
                //}

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed To Retrieve Budget Data", ex.Message, "Ok");

            }
        }

        //update the budget data with balances
        public async Task UpdateBalances(int Month, string userID)
        {
            try
            {
                List<Budget> budgetList = await GetBudget();

                if (budgetList != null)
                {
                    foreach (var budget in budgetList)
                    {
                        if (budget.UserID == userID && budget.Month.Equals(Month))
                        {
                            await SetMonthlyBalance(budget, Month);
                            decimal balance = await GetTotalBalance(userID);
                            budget.TotalCurrentBalance = balance.ToString();

                            await connection.UpdateAsync(budget);
                            System.Diagnostics.Debug.WriteLine($" Monthly budget: {budget.MonthlyBalance}");
                            System.Diagnostics.Debug.WriteLine($" Monthly budget: {budget.UserID}");
                            System.Diagnostics.Debug.WriteLine($" Monthly budget: {budget.Amount}");
                            System.Diagnostics.Debug.WriteLine($" Monthly budget: {budget.TotalCurrentBalance}");

                        }
                    }
                }

                //Budget currentMonthBudget = budgetList.FirstOrDefault(b => b.Month.Equals(Month));
                //if (currentMonthBudget != null)
                //{
                //    await SetMonthlyBalance(currentMonthBudget, Month);
                //    System.Diagnostics.Debug.WriteLine($" Monthly budget: {currentMonthBudget.MonthlyBalance}");
                //    System.Diagnostics.Debug.WriteLine($" Monthly budget: {currentMonthBudget.UserID}");
                //    System.Diagnostics.Debug.WriteLine($" Monthly budget: {currentMonthBudget.Amount}");
                //    System.Diagnostics.Debug.WriteLine($" Monthly budget: {currentMonthBudget.TotalCurrentBalance}");
                //    await GetTotalBalance(currentMonthBudget);
                //    await connection.UpdateAsync(currentMonthBudget);

                //}

            }
            catch (Exception ex) 
            {
                await Shell.Current.DisplayAlert("Failed To Update Balances", ex.Message, "Ok");

            }

        }

        //delete all budget
        public async Task DeleteAllBudgets()
        {
            try
            {
                await connection.DeleteAllAsync<Budget>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error Deleting Budgets", ex.Message, "OK");
            }
        }



    }
}
