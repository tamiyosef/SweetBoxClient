using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetBoxApp.Models
{
    public class Sellers
    {
        public int SellerId { get; set; }

        public string BusinessName { get; set; } = null!;

        public string BusinessAddress { get; set; } = null!;

        public string? BusinessPhone { get; set; }

        public string? Description { get; set; }

        public string? ProfilePicture { get; set; }

    }
}
