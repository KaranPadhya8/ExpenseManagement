using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Models;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections;

namespace ExpenseManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Expenses")]
    public class ExpensesController : Controller
    {
        private readonly ExpenseManagerContext _context;

        public ExpensesController(ExpenseManagerContext context)
        {
            _context = context;
        }

        // GET: api/Expenses
        [HttpGet]
        public IEnumerable<Expense> GetExpense()
        {
            var expense = _context.Expense
                .Select(e => new Expense
                {
                    ExpenseId = e.ExpenseId,
                    Description = e.Description,
                    Cost = e.Cost,
                    CurrencyId = e.CurrencyId,
                    PaidByFriendId = e.PaidByFriendId,
                    GroupId = e.GroupId,
                });
            return expense.ToList();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var expense = _context.Expense
                .Where(e => e.ExpenseId == id)
                .Select(e => new Expense
                {
                    ExpenseId = e.ExpenseId,
                    Description = e.Description,
                    Cost = e.Cost,
                    CurrencyId = e.CurrencyId,
                    PaidByFriendId = e.PaidByFriendId,
                    GroupId = e.GroupId,
                });


            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense([FromRoute] int id, [FromBody] Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Expenses
        [HttpPost]
        public async Task<IActionResult> PostExpense([FromBody] Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            switch (expense.CurrencyId)
            {
                case 2:
                    expense.Cost *= 0.75;
                    break;
                case 3:
                    expense.Cost *= 0.053;
                    break;
            }
            List<ExpenseManagement.Models.Friend> query = new();
            
                query = _context.Friend
                                  .Where(s => s.GroupId == expense.GroupId).ToList();

                var IndividualShare = expense.Cost / query.Count;

            for (int i = 1; i <= query.Count; i++)
            {

                if (query.Any(cus => cus.FriendId == i))
                { 

                    if (i == expense.PaidByFriendId)
                    {

                        var result = (from f in _context.Friend
                                      where f.FriendId == expense.PaidByFriendId
                                      select f).SingleOrDefault();

                        result.Balance += expense.Cost - IndividualShare;
                    }
                    else 
                    {
                        var result2 = (from f in _context.Friend
                                       where f.FriendId == i
                                       select f).SingleOrDefault();

                        result2.Balance += -IndividualShare;
                    }
                } 
                    
            }
            _context.SaveChanges();

            _context.Expense.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetExpense", new { id = expense.ExpenseId }, expense);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expense.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok(expense);
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expense.Any(e => e.ExpenseId == id);
        }
    }
}