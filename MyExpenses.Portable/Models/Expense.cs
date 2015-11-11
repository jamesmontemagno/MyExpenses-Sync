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

using MyExpenses.Portable.BusinessLayer.Contracts;
using Newtonsoft.Json;

namespace MyExpenses.Portable.Models
{
  public class Expense : BusinessEntityBase
  {
    public Expense()
    {
      Name = string.Empty;
      Notes = string.Empty;
      Due = DateTime.Now;
      Total = "0.00";
      Category = string.Empty;
      Billable = true;
      IsVisible = true;
      IsDirty = true;
       UserId = string.Empty;
    }

    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    public bool IsDirty { get; set; }

    public bool IsVisible { get; set; }

    public string Name { get; set; }
    public string Notes { get; set; }
    public DateTime Due { get; set; }

    public string Total { get; set; }

    public string Category { get; set; }
    public bool Billable { get; set; }

    [JsonIgnore]
    public string DueDateLongDisplay
    {
      get { return Due.ToLocalTime().ToString("D"); }
    }


    [JsonIgnore]
    public string TotalDisplay
    {
      get { return "$" + Total; }
    }

    [JsonIgnore]
    public string DueDateShortDisplay
    {
      get { return Due.ToLocalTime().ToString("d"); }
    }

    public Expense(Expense expense)
    {
      SyncProperties(expense);
    }

    public void SyncProperties(Expense expense)
    {
      this.Billable = expense.Billable;
      this.Category = expense.Category;
      this.Due = expense.Due;
      this.IsVisible = expense.IsVisible;
      this.Name = expense.Name;
      this.Notes = expense.Notes;
      this.Total = expense.Total;
      this.UserId = expense.UserId;
    }
  }
}
