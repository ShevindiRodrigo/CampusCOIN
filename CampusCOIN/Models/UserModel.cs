using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusCOIN.Models
{
    public class UserModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

    }
}
