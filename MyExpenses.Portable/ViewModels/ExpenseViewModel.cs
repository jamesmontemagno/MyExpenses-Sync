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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;

namespace MyExpenses.Portable.ViewModels
{
  public class ExpenseViewModel : ViewModelBase
  {
    public ExpenseViewModel()
    {
      expenseService = ServiceContainer.Resolve<IExpenseService>();
    }

    public bool CanNavigate { get; set; }

    private IExpenseService expenseService;
    private IMessageDialog dialog;

    public ExpenseViewModel(IExpenseService expenseService)
    {
      this.expenseService = expenseService;
      Title = "New Expense";
    }

    private Expense currentExpense;
    public async Task Init(string id)
    {
      if (!string.IsNullOrWhiteSpace(id))
        currentExpense = await expenseService.GetExpenseAsync(id);
      else
        currentExpense = null;
      Init();
    }

    public void Init(Expense expense)
    {
      currentExpense = expense;
      Init();
    }

    private void Init()
    {
      dialog = ServiceContainer.Resolve<IMessageDialog>();
      CanNavigate = true;
      if (currentExpense == null)
      {
        Name = string.Empty;
        Billable = true;
        Due = DateTime.Now;
        Notes = string.Empty;
        Total = string.Empty;
        Category = Categories[0];
        Title = "New Expense";
        return;
      }

      Name = currentExpense.Name;
      Notes = currentExpense.Notes;
      Due = currentExpense.Due;
      Billable = currentExpense.Billable;
      Total = currentExpense.Total;
      Category = currentExpense.Category;
      Title = "Edit Expense";
    }

    private string title = string.Empty;
    public string Title
    {
      get { return title; }
      set { title = value; OnPropertyChanged("Title"); }
    }

    private string name = string.Empty;
    public string Name
    {
      get { return name; }
      set { name = value; OnPropertyChanged("Name"); }
    }

    private string notes = string.Empty;
    public string Notes
    {
      get { return notes; }
      set { notes = value; OnPropertyChanged("Notes"); }
    }

    private DateTime due = DateTime.Now;
    public DateTime Due
    {
      get { return due; }
      set { due = value; OnPropertyChanged("Due"); }
    }

    private string category = categories[0];
    public string Category
    {
      get { return category; }
      set { category = value; OnPropertyChanged("Category"); }
    }

    private string total = "0.00";
    public string Total
    {
      get { return total; }
      set { total = value; OnPropertyChanged("Total"); }
    }

    private bool billable = true;

    public bool Billable
    {
      get { return billable; }
      set { billable = value; OnPropertyChanged("Billable"); }
    }

    private static List<string> categories = new List<string>
        {
          "Uncategorized",
          "Entertainment",
          "Fuel/Milage",
          "Lodging",
          "Meals",
          "Other",
          "Phone",
          "Transportation"
        };

    public List<string> Categories
    {
      get { return categories; }
    }

    private RelayCommand saveExpenseCommand;

    public ICommand SaveExpenseCommand
    {
      get { return saveExpenseCommand ?? (saveExpenseCommand = new RelayCommand(async () => await ExecuteSaveExpenseCommand())); }
    }

    public async Task ExecuteSaveExpenseCommand()
    {
      if (IsBusy)
        return;

      CanNavigate = false;
      if (currentExpense == null)
        currentExpense = new Expense();

      currentExpense.Billable = Billable;
      currentExpense.Category = Category;
      currentExpense.Due = Due.ToUniversalTime();
      currentExpense.Name = Name;
      currentExpense.Notes = Notes;
      currentExpense.Total = Total;
      try
      {
        IsBusy = true;
        await expenseService.SaveExpenseAsync(currentExpense);
        await expenseService.SyncExpensesAsync();
        ServiceContainer.Resolve<ExpensesViewModel>().NeedsUpdate = true;
        CanNavigate = true;
      }
      catch (Exception ex)
      {
        
        
      }
      finally
      {
        IsBusy = false;
      }
    }
  }
}
