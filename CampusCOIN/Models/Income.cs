using SQLite;

namespace CampusCOIN.Models
{
    public class Income
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserID { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

    }
}
