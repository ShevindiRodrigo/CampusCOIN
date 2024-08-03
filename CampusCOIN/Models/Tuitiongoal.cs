using Microsoft.VisualBasic;
using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CampusCOIN.Models
{
    public class Tuitiongoal :ObservableObject
    {

        public string userID;

        public string amount;

        public DateTime dueDate;

        public string monthlyGoal;
        
        public Tuitiongoal(){ 
            monthlyGoal = calculateMonthlyGoal();
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //public string UserID { get; set; }


        /*
         * Following code snippet on PropertyChanged functionality was inspired by the blog
         * article https://devcodef1.com/news/1036665/how-to-useonpropertychanged-net-maui
         *  and Maui.net Microsoft official documentation 
         *  https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/mvvm?view=net-maui-8.0 
         *  on July 04th 2024
         */ 
        public string UserID
        {
            get => userID;
            set
            {
                userID = value;
                monthlyGoal = calculateMonthlyGoal();
                OnPropertyChanged(nameof(amount));
                OnPropertyChanged(nameof(MonthlyGoal)); // Notify change in MonthlyGoal too
            }
        }

        public string Amount
        {
            get => amount;
            set
            {
                amount = value;
                monthlyGoal = calculateMonthlyGoal();
                OnPropertyChanged(nameof(amount));
                OnPropertyChanged(nameof(MonthlyGoal)); // Notify change in MonthlyGoal too
            }
        }

        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                monthlyGoal = calculateMonthlyGoal();
                OnPropertyChanged(nameof(dueDate));
                OnPropertyChanged(nameof(MonthlyGoal)); // Notify change in MonthlyGoal too
            }
        }

        public string MonthlyGoal
        {
            get => monthlyGoal;
        }

        //method to calculate monthly goal
        public string calculateMonthlyGoal()
        {
            var today = DateTime.Today;

            int monthCount = ((dueDate.Year - today.Year) * 12) + dueDate.Month - today.Month;



            if (monthCount > 0 && decimal.TryParse(amount, out var decimalAmount)) 
            { 
                monthlyGoal = Math.Round((decimalAmount/ monthCount)).ToString();
            }

            else
            {
                monthlyGoal = null;
            }
            return monthlyGoal;

        }


    }
}
