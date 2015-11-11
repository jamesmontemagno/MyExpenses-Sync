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

using Android.App;
using Android.Views;
using Android.Widget;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.ViewModels;

namespace MyExpenses.Android.Adapters
{
  public class ExpenseWrapper : Java.Lang.Object
  {
    public TextView Name { get; set; }
    public TextView DueDate { get; set; }
    public TextView Total { get; set; }
  }
  public class ExpenseAdapter : BaseAdapter<Expense>
  {
    private ExpensesViewModel viewModel;
    private Activity context;
    public ExpenseAdapter(Activity context, ExpensesViewModel viewModel)
    {
      this.viewModel = viewModel;
      this.context = context;
    }
    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      ExpenseWrapper wrapper = null;
      var view = convertView;
      if (convertView == null)
      {
        view = context.LayoutInflater.Inflate(Resource.Layout.item_expense, null);
        wrapper = new ExpenseWrapper();
        wrapper.Name = view.FindViewById<TextView>(Resource.Id.name);
        wrapper.DueDate = view.FindViewById<TextView>(Resource.Id.due);
        wrapper.Total = view.FindViewById<TextView>(Resource.Id.total);
        view.Tag = wrapper;
      }
      else
      {
        wrapper = convertView.Tag as ExpenseWrapper;
      }

      var expense = viewModel.Expenses[position];
      wrapper.Name.Text = expense.Name;
      wrapper.DueDate.Text = expense.DueDateLongDisplay;
      wrapper.Total.Text = expense.TotalDisplay;

      return view;
    }

    public override Expense this[int position]
    {
      get { return viewModel.Expenses[position]; }
    }

    public override int Count
    {
      get { return viewModel.Expenses.Count; }
    }

    public override long GetItemId(int position)
    {
      return position;
    }
  }
}