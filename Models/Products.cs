using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetBoxApp.Models
{
    public class Products
    {
        public int ProductId { get; set; }

        public int? SellerId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? DateAdded { get; set; }

        public bool IsAvailable { get; set; }

    }
}
