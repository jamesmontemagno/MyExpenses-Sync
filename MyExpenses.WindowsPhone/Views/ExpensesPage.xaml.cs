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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.WindowsAzure.MobileServices;
using MyExpenses.PlatformSpecific;
using MyExpenses.Portable.Helpers;
using MyExpenses.Portable.Models;
using MyExpenses.Portable.Services;
using MyExpenses.Portable.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyExpenses.WindowsPhone.Views
{
  public partial class ExpensesPage : PhoneApplicationPage
  {
    private ExpensesViewModel viewModel;
    private bool loaded;
    // Constructor
    public ExpensesPage()
    {
      InitializeComponent();

      // Set the data context of the LongListSelector control to the sample data
      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();
      DataContext = viewModel;
      this.Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (loaded)
        return;
      loaded = true;

      await Authenticate();
      await viewModel.ExecuteLoadExpensesCommand();
    }

    // Load data for the ViewModel Items
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
      if(viewModel.NeedsUpdate)
        viewModel.LoadExpensesCommand.Execute(null);
    }

    // Handle selection changed on LongListSelector
    private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // If selected item is null (no selection) do nothing
      if (MainLongListSelector.SelectedItem == null)
        return;

      if (viewModel.IsBusy)
        return;

      // Navigate to the new page
      NavigationService.Navigate(new Uri("/Views/ExpensePage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as Expense).Id, UriKind.Relative));

      // Reset selected item to null (no selection)
      MainLongListSelector.SelectedItem = null;
    }


    private void Delete_OnClick(object sender, RoutedEventArgs e)
    {
      if (viewModel.IsBusy)
        return;

      var menuItem = sender as MenuItem;

      if (menuItem != null)
      {
        var selected = menuItem.DataContext as Expense;
        if (selected == null)
          return;

        viewModel.DeleteExpenseCommand.Execute(selected);
      }
    }

    private void NewExpenseAppButton_OnClick(object sender, EventArgs e)
    {
      if (viewModel.IsBusy)
        return;
      // Navigate to the new page
      NavigationService.Navigate(new Uri("/Views/ExpensePage.xaml", UriKind.Relative));

    }

    private async void RefreshButton_OnClick(object sender, EventArgs e)
    {
      viewModel.LoadExpensesCommand.Execute(null); ;
    }


    private async System.Threading.Tasks.Task Authenticate()
    {

      return;


      var client = AzureExpenseService.Instance.MobileService;
      if (client == null)
        return;

      while (client.CurrentUser == null)
      {
        
        try
        {

          client.CurrentUser = await client
            .LoginAsync(MobileServiceAuthenticationProvider.Twitter);
     
        }
        catch (InvalidOperationException ex)
        {
          var message = "You must log in. Login Required";
          MessageBox.Show(message, "Login", MessageBoxButton.OK);
     
        }

      }
    }
  }
}