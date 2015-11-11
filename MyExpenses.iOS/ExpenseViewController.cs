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
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using BigTed;
using Microsoft.WindowsAzure.MobileServices;
using MonoTouch.Dialog;
using UIKit;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.iOS.Views
{
  public class ExpenseViewController : DialogViewController
  {
    private EntryElement name, total,notes;
    private CheckboxElement billable;
    private DateElement due;
    private RadioGroup categories;
    private ExpenseViewModel viewModel;
    private Expense expense;
    private IMessageDialog dialog;

    public ExpenseViewController(Expense expense) : base(UITableViewStyle.Plain, null, true)
    {
      this.expense = expense;
      dialog = ServiceContainer.Resolve<IMessageDialog>();
      viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
      viewModel.Init(this.expense);

      viewModel.IsBusyChanged = (busy) =>
      {
        if (busy)
          BTProgressHUD.Show("Saving...");
        else
          BTProgressHUD.Dismiss();
      };
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
      var title = expense == null ? "New Expense" : "Edit Expense";
      this.Root = new RootElement(title)
      {
        new Section("Expense Details")
        {
          (name = new EntryElement("Name", "Expense Name", string.Empty)),
          (total = new EntryElement("Total", "1.00", string.Empty){KeyboardType = UIKeyboardType.DecimalPad}),
          (billable = new CheckboxElement("Billable", true)),
          new RootElement("Category", categories = new RadioGroup("category", 0))
          {
            new Section()
            {
              from category in viewModel.Categories
                select (Element) new RadioElement(category)
            }
          },
          (due = new DateElement("Due Date", DateTime.Now))  
        },
        new Section("Notes")
        {
          (notes = new EntryElement(string.Empty, "Enter expense notes.", string.Empty))
        } 

      };

      billable.Value = viewModel.Billable;
      name.Value = viewModel.Name;
      total.Value = viewModel.Total;
      notes.Caption = viewModel.Notes;
      categories.Selected = viewModel.Categories.IndexOf(viewModel.Category);
      due.DateValue = viewModel.Due;

      this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Save, async delegate
      {
        viewModel.Category = viewModel.Categories[categories.Selected];
        viewModel.Name = name.Value;
        viewModel.Billable = billable.Value;
        viewModel.Due = due.DateValue;
        viewModel.Notes = notes.Caption;
        viewModel.Total = total.Value;

        await viewModel.ExecuteSaveExpenseCommand();
        if (!viewModel.CanNavigate)
          return;
        NavigationController.PopToRootViewController(true);
        
      });
    }
  }
}