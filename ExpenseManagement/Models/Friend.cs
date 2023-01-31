using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    
    public class Friend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PaymentId { get; set; }
        public int? GroupId { get; set; }
        public double? Balance { get; set; }

        public int? currencyId { get; set; }
    }
}
