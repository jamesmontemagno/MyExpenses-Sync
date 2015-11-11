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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;

namespace MyExpenses.Portable.ViewModels
{
  public class ExpensesViewModel : ViewModelBase
  {
    private IExpenseService expenseService;
    private IMessageDialog messageDialog;

    public ExpensesViewModel()
    {
      expenseService = ServiceContainer.Resolve<IExpenseService>();
      messageDialog = ServiceContainer.Resolve<IMessageDialog>();
      NeedsUpdate = true;
    }

    /// <summary>
    /// Gets or sets if an update is needed
    /// </summary>
    public bool NeedsUpdate { get; set; }


    /// <summary>
    /// Gets or sets if we have loaded alert
    /// </summary>
    public bool LoadedAlert { get; set; }



    private ObservableCollection<Expense> expenses = new ObservableCollection<Expense>();

    public ObservableCollection<Expense> Expenses
    {
      get { return expenses; }
      set { expenses = value; OnPropertyChanged("Expenses"); }
    }


    private async Task UpdateExpenses()
    {
      Expenses.Clear();
      NeedsUpdate = false;
      try
      {
        var exps = await expenseService.GetExpensesAsync();

        foreach (var expense in exps)
        {

          Expenses.Add(expense);
        }

      }
      catch (Exception exception)
      {
        Debug.WriteLine("Unable to query and gather expenses");
      }
      finally
      {
        IsBusy = false;
      }
    }

    private RelayCommand loadExpensesCommand;

    public ICommand LoadExpensesCommand
    {
      get { return loadExpensesCommand ?? (loadExpensesCommand = new RelayCommand(async () => await ExecuteLoadExpensesCommand())); }
    }

    public async Task ExecuteLoadExpensesCommand()
    {
      if (IsBusy)
        return;

      IsBusy = true;
      await UpdateExpenses();
    }

    private RelayCommand<Expense> deleteExpensesCommand;

    public ICommand DeleteExpenseCommand
    {
      get { return deleteExpensesCommand ?? (deleteExpensesCommand = new RelayCommand<Expense>(async (item) => await ExecuteDeleteExpenseCommand(item))); }
    }

    public async Task ExecuteDeleteExpenseCommand(Expense exp)
    {
      if (IsBusy)
        return;

      IsBusy = true;
      try
      {

        await expenseService.DeleteExpenseAsync(exp);
        await expenseService.SyncExpensesAsync();
        Expenses.Remove(Expenses.FirstOrDefault(ex => ex.Id == exp.Id));


      }
      catch (Exception ex)
      {
        Debug.WriteLine("Unable to delete expenses");
      }
      finally
      {
        IsBusy = false;
      }
    }

  }
}
