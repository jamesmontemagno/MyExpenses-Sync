//The MIT License (MIT)
//Copyright (c) 2015 Xamarin
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//    FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//    COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//    IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//    CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections;
using MyExpenses.Portable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExpenses.Portable.Models;

namespace MyExpenses.Tests.Helpers
{
  public class ExpenseServiceMock : IExpenseService
  {
    public List<Expense> Expenses = new List<Expense>(); 
    public Task<Portable.Models.Expense> GetExpense(int id)
    {
      return Task.Run(()=>Expenses.FirstOrDefault(e => e.ID == id));
    }

    public Task<IEnumerable<Portable.Models.Expense>> GetExpenses()
    {
      return Task.Run(() => (IEnumerable<Expense>)Expenses);
    }

    public async Task<Portable.Models.Expense> SaveExpense(Portable.Models.Expense expense)
    {

      var ex = await GetExpense(expense.ID);
      if (ex == null)
      {
        expense.ID = Expenses.Count;
        Expenses.Add(expense);
      }
      else
      {
        Expenses.Remove(ex);
        Expenses.Add(expense);
      }
      return expense;
    }

    public async Task<int> DeleteExpense(int id)
    {
      var ex = await GetExpense(id);
      if (ex != null)
      {
        Expenses.Remove(ex);
      }

      return 0;
    }


    public Task<Alert> GetExpenseAlert()
    {
      throw new NotImplementedException();
    }
  }
}
