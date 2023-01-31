using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Models;

namespace ExpenseManagement.Models
{
    public class ExpenseManagerContext : DbContext
    {

        public ExpenseManagerContext(DbContextOptions<ExpenseManagerContext> options)
            : base(options)
        {
        }

        public DbSet<ExpenseManagement.Models.Friend> Friend { get; set; }

        public DbSet<ExpenseManagement.Models.Expense> Expense { get; set; }

        public DbSet<ExpenseManagement.Models.Currency> Currency { get; set; }

        public DbSet<ExpenseManagement.Models.Payment> Payment { get; set; }

        public DbSet<ExpenseManagement.Models.Group> Group { get; set; }
    }
}
