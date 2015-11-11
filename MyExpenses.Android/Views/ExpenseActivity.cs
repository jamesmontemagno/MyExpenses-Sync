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
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Microsoft.WindowsAzure.MobileServices;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.Android.Views
{
  [Activity(Label = "New Expense", Icon = "@drawable/ic_launcher")]
  public class ExpenseActivity : Activity
  {
    private ExpenseViewModel viewModel;
    private EditText notes, name, total;
    private DatePicker date;
    private CheckBox billable;
    private Spinner category;
    private IMessageDialog dialog;
    protected async override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      SetContentView(Resource.Layout.view_expense);

      dialog = ServiceContainer.Resolve<IMessageDialog>();

      var id = Intent.GetStringExtra("ID");
      viewModel = ServiceContainer.Resolve<ExpenseViewModel>();
      await viewModel.Init(id);

      this.ActionBar.Title = viewModel.Title;
      viewModel.IsBusyChanged = (busy) =>
      {
        if (busy)
          AndHUD.Shared.Show(this, "Loading...");
        else
          AndHUD.Shared.Dismiss(this);
      };

      name = FindViewById<EditText>(Resource.Id.name);
      date = FindViewById<DatePicker>(Resource.Id.date);
      notes = FindViewById<EditText>(Resource.Id.notes);
      total = FindViewById<EditText>(Resource.Id.total);
      billable = FindViewById<CheckBox>(Resource.Id.billable);
      category = FindViewById<Spinner>(Resource.Id.category);
      category.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerDropDownItem, viewModel.Categories);
      category.SetSelection(viewModel.Categories.IndexOf(viewModel.Category));
      name.Text = viewModel.Name;
      date.DateTime = viewModel.Due;
      notes.Text = viewModel.Notes;
      total.Text = viewModel.Total;
      billable.Checked = viewModel.Billable;
    }

    protected override void OnStart()
    {
      base.OnStart();
      MyExpensesApplication.CurrentActivity = this;
    }

    public override bool OnCreateOptionsMenu(IMenu menu)
    {
      MenuInflater.Inflate(Resource.Menu.menu_expense, menu);
      return base.OnCreateOptionsMenu(menu);
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
      switch (item.ItemId)
      {
        case (Resource.Id.menu_save_expense):
          viewModel.Name = name.Text;
          viewModel.Billable = billable.Checked;
          viewModel.Due = date.DateTime;
          viewModel.Notes = notes.Text;
          viewModel.Total = total.Text;
          viewModel.Category = viewModel.Categories[category.SelectedItemPosition];
          Task.Run(async () =>
          {
              await viewModel.ExecuteSaveExpenseCommand();

              if (!viewModel.CanNavigate)
                return;

              Finish();
          });
          return true;
      }
      return base.OnOptionsItemSelected(item);
    }
  }
}