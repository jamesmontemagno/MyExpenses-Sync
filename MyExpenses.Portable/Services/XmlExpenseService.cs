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
using System.Threading.Tasks;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using Newtonsoft.Json;
using System.Linq;
using PCLStorage;

namespace MyExpenses.Portable.Services
{
  public class XmlExpenseService : IExpenseService
  {

    private readonly IMessageDialog dialog;

    public XmlExpenseService()
    {
    }


    public static Task<T> DeserializeObjectAsync<T>(string value)
    {
      return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));
    }

    public static T DeserializeObject<T>(string value)
    {
      return JsonConvert.DeserializeObject<T>(value);
    }

    List<Expense> Expenses = new List<Expense>();

    public Task<Expense> GetExpenseAsync(string id)
    {
      return Task.Run(()=>Expenses.FirstOrDefault(s => s.Id == id));
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync()
    {
      var rootFolder = FileSystem.Current.LocalStorage;
      var folder = await rootFolder.CreateFolderAsync(Folder,
          CreationCollisionOption.OpenIfExists);
      var file = await folder.CreateFileAsync(File,
          CreationCollisionOption.OpenIfExists);
      var json = await file.ReadAllTextAsync();
      if(!string.IsNullOrWhiteSpace(json))
        Expenses = DeserializeObject<List<Expense>>(json);

      return Expenses;
    }

    public Task SyncExpensesAsync()
    {
      return Task.Run(() => { });
    }

    public async Task<Expense> SaveExpenseAsync(Expense expense)
    {
      if(string.IsNullOrWhiteSpace(expense.Id))
      {
        expense.Id = DateTime.Now.ToString();
        Expenses.Add(expense);
      }
      else
      {
        var found = Expenses.FirstOrDefault(e => e.Id == expense.Id);
        if(found != null)
          found.SyncProperties(expense);
      }
      await Save();
      return expense;
    }

    public async Task<string> DeleteExpenseAsync(Expense expense)
    {
      var id = expense.Id;
      Expenses.Remove(expense);
      await Save();
      return id;
    }

    private string Folder = "Expenses";
    private string File = "expenses.json";

    private async Task Save()
    {
      var rootFolder = FileSystem.Current.LocalStorage;
      var folder = await rootFolder.CreateFolderAsync(Folder,
          CreationCollisionOption.OpenIfExists);
      var file = await folder.CreateFileAsync(File,
          CreationCollisionOption.ReplaceExisting);
      await file.WriteAllTextAsync(JsonConvert.SerializeObject(Expenses));
    }

    public string UserId
    {
      get { return string.Empty; }
    }

    public Task Init()
    {
      throw new NotImplementedException();
    }
  }
}

