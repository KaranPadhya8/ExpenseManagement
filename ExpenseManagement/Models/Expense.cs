using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }

        public string? Description { get; set; }

        public double Cost { get; set; }
        public int CurrencyId { get; set; }
        public int PaidByFriendId { get; set; }
        public int GroupId { get; set; }

    }
}
