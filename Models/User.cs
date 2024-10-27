using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetBoxApp.Models
{
    public class User
    {
        // המחלקה חייבת להיות זהה למחלקה המוצגת בDTO SERVER 
        public int UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string UserType { get; set; } = null!;

      
    }
}
