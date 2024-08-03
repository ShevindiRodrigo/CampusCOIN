using SQLite;

namespace CampusCOIN.Models
{
    public class Budget
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserID { get; set; }

        public string Amount { get; set; }

        public int Month { get; set; }

        public string MonthlyBalance { get; set; }

        public string TotalCurrentBalance { get; set; }

       
    }
}
