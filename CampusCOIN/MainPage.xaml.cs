using CampusCOIN.Data;
using CampusCOIN.Models;
using CampusCOIN.Pages;
using CampusCOIN.Services;

namespace CampusCOIN
{
    public partial class MainPage : ContentPage
    {
        private readonly ExpenseData _expenseData;
        private readonly IncomeData _incomeData;
        private readonly BudgetData _budgetData;
        private readonly TuitionGoalData tuitiongoalData;
        private readonly UserData _userData;

        public readonly Budget _budget;
        private readonly Tuitiongoal _tuitiongoal;

        public string userID;

        public MainPage(ExpenseData expenseData, IncomeData incomeData, BudgetData budgetData, TuitionGoalData tuitionGoalData)
        {
            InitializeComponent();

            if (FirebaseAuthManager.currentUser == null)
            {
                Navigation.PushAsync(new SignUpPage());
            }
            _expenseData = expenseData;
            _incomeData = incomeData;
            _budgetData = budgetData;
            tuitiongoalData = tuitionGoalData;
            _budget = new Budget();
            _tuitiongoal = new Tuitiongoal();
            _userData = new UserData();

            if (FirebaseAuthManager.currentUser != null)
            {
                userID = FirebaseAuthManager.currentUser.UserId;

            }

            //get the current month
            Month.Text = DateTime.Now.ToString("MMMM").ToUpperInvariant();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //display current userName
            userID = FirebaseAuthManager.currentUser.UserId;
            HelloLabel.Text = "Hello " + await GetUserName() + " !";

            //update uI based on monthly, total budget and month
            await UpdateUIBasedOnBudget();

            //calculate progress of the goal
            Double progress = await CalculateProgress();
            tuitiongoalProgress.Progress = progress;


            //get expense total and income total for the current month
            GetTotalExpense(DateTime.Now.Month, Expenselbl);
            GetTotalIncome(DateTime.Now.Month, Incomelbl);
        }

        /* Function to update UI of the set budget frame and Total monthly budget balnace based on
         * specific critera
         */
        private async Task UpdateUIBasedOnBudget()
        {

            _budget.MonthlyBalance = await _budgetData.GetMonthlyBalance(DateTime.Now.Month, userID);
            decimal currentTotalBalance = await _budgetData.GetTotalBalance(userID);
            _budget.TotalCurrentBalance = currentTotalBalance.ToString();

            List<Budget> list = await _budgetData.GetBudget();
            //budget list for current user
            List<Budget> filterList = list.Where(b => b.UserID == userID).ToList();

            //get the budget list for the current month
            List<Budget> budgetCurrentMonth = filterList.Where(b => b.Month.Equals(DateTime.Now.Month)).ToList();



            if (filterList.Count > 0)// Check if the current user budget list null
            {
                if (budgetCurrentMonth.Count > 0) //check if the budget list for current month null
                {
                    TotalMonthlyBalance.IsVisible = true;
                    CurrentBalance.IsVisible = true;
                    SetBudgetFrame.IsVisible = false;

                    MonthlyBalance.Text = "$ " + _budget.MonthlyBalance;
                    TotalBalancelbl.Text = "BALANCE: $ " + _budget.TotalCurrentBalance.ToString();
                }
                else
                {
                    TotalMonthlyBalance.IsVisible = false;
                    SetBudgetFrame.IsVisible = true;
                    CurrentBalance.IsVisible = true;
                    TotalBalancelbl.Text = "BALANCE: $ " + _budget.TotalCurrentBalance.ToString();


                }
            }
            else
            {
                TotalMonthlyBalance.IsVisible = false;
                SetBudgetFrame.IsVisible = true;
                CurrentBalance.IsVisible = true;
                TotalBalancelbl.Text = "BALANCE: $ " + _budget.TotalCurrentBalance.ToString();


            }
        }

        // Tap event on expense frame
        public async void OnTapViewExpenses(object sender, TappedEventArgs t)
        {
            //Navigate to ExpenseListPage to view recent expense transaction
            await Navigation.PushAsync(new ExpenseListPage(_expenseData, userID));
        }

        // Tap event on income frame
        public async void OnTapViewIncome(object sender, TappedEventArgs t)
        {
            //Navigate to IncomeListPage to view recent expense transaction
            await Navigation.PushAsync(new IncomeListPage(_incomeData, userID));
        }

        //Tap event on set budget
        public async void OnTapSetBudget(object sender, TappedEventArgs t)
        {
            await Navigation.PushAsync(new SetBudgetPage(_budgetData, userID));
        }


        /* Function to calculate total expenses for the current month
         */
        public async void GetTotalExpense(int Month, Label label)
        {
            decimal total = 0;
            List<Expense> list = await _expenseData.GetExpenses(userID);

            if (list.Count != 0)
            {
                foreach (var expense in list)
                {
                    if (expense.Date.Month.Equals(Month) && expense.Date.Year.Equals(DateTime.Now.Year))
                    {
                        total += Decimal.Parse(expense.Amount);
                    }
                }
            }

            label.Text = total.ToString();

            System.Diagnostics.Debug.WriteLine($"ExpenseData is null: {total}");

        }


        /* Function to calculate tota income expenses for the current month
         */
        public async void GetTotalIncome(int Month, Label label)
        {
            decimal total = 0;
            List<Income> list = await _incomeData.GetIncome(userID);

            if (list.Count != 0)
            {

                foreach (var income in list)
                {
                    if (income.Date.Month.Equals(Month) && income.Date.Year.Equals(DateTime.Now.Year))
                    {

                        total += Decimal.Parse(income.Amount);
                    }
                }

            }
            label.Text = total.ToString();
        }


        /* Function to calculate total progress of the tuition fee goal
         */
        public async Task<double> CalculateProgress()
        {
            double Progress = 0;
            double totalBalance = Convert.ToDouble(_budget.TotalCurrentBalance);

            await tuitiongoalData.GetCurrentGoalAmount(_tuitiongoal, userID);
            double goalAmount = Convert.ToDouble(_tuitiongoal.Amount);

            try
            {

                if (goalAmount > 0 && totalBalance != 0)
                {
                    Progress = totalBalance / goalAmount;
                    if (Progress > 1)
                    {
                        Progress = 1;
                    }
                }
                Progreesslbl.Text = Math.Round(Progress * 100).ToString() + "%";
                System.Diagnostics.Debug.WriteLine($"Progress: {Progress}");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error in Calculating Progress!", ex.Message, "OK");
            }
            return Progress;

        }


        /* Function to get the current user name
         */
        public async Task<string> GetUserName()
        {
            var email = FirebaseAuthManager.currentUser.Email;
            var userName = "";
            List<UserModel> list = await _userData.GetUserAsync(email);

            if (list != null)
            {
                foreach (UserModel user in list)
                {
                    userName = user.Name;
                }
            }
            return userName;

        }


    }

}
