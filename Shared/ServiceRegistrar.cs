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
using System.IO;
using System.Linq;
using System.Text;

using MyExpenses.PlatformSpecific;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Interfaces;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;
using Newtonsoft.Json;
#if __IOS__
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
#elif __ANDROID__
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
#elif WINDOWS_PHONE

#endif

namespace MyExpenses.Helpers
{
  public static class ServiceRegistrar
  {
    public static void Startup()
    {
#if __ANDROID__
      CurrentPlatform.Init();
#elif __IOS__
      CurrentPlatform.Init();
      SQLitePCL.CurrentPlatform.Init();
#endif

      var expenseService = new XmlExpenseService();
      //var expenseService = new AzureExpenseService();
      //expenseService.Init().Wait();

      ServiceContainer.Register<IMessageDialog>(() => new MessageDialog());
      ServiceContainer.Register<IExpenseService>(() => expenseService);
      ServiceContainer.Register<ExpensesViewModel>();
      ServiceContainer.Register<ExpenseViewModel>();
    }
  }
}