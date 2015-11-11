
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using MyExpenses.Portable.ViewModels;
using MyExpenses.Portable.Helpers;
using MyExpenses.iOS.Views;
using System.Threading.Tasks;
using MyExpenses.PlatformSpecific;
using Microsoft.WindowsAzure.MobileServices;

namespace MyExpenses.iOS
{
  public partial class ExpenseListViewController : UITableViewController
  {
    static bool UserInterfaceIdiomIsPhone
    {
      get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
    }

    private ExpensesViewModel viewModel;
    public ExpenseListViewController(IntPtr handle)
      : base(handle)
    {
    }

    public async override void ViewDidLoad()
    {
      base.ViewDidLoad();
      NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
      viewModel = ServiceContainer.Resolve<ExpensesViewModel>();

      TableView.Source = new ExpenseSource(this, viewModel);
	  viewModel.IsBusyChanged = (busy) =>
		{
			if (busy)
				RefreshControl.BeginRefreshing();
			else
				RefreshControl.EndRefreshing();
		};


	  RefreshControl = new UIRefreshControl();
	  RefreshControl.ValueChanged += async (sender, args) =>
		{
			if (viewModel.IsBusy)
				return;

			await viewModel.ExecuteLoadExpensesCommand();
			TableView.ReloadData();
		};

	  ButtonAdd.Clicked += (sender, args) =>
		{
			NavigationController.PushViewController(new ExpenseViewController(null), true);
		};

	 
    }

    async public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

	  if (viewModel.NeedsUpdate)
      {
		await Authenticate();
        await viewModel.ExecuteLoadExpensesCommand();
        TableView.ReloadData();
      }
    }

	/// <summary>
	/// Authenticate the azure client with twitter authentication.
	/// </summary>
	/// <returns></returns>
	private async Task Authenticate()
	{
		//comment out to turn on authenticate
		return;
		var client = AzureExpenseService.Instance.MobileService;
		if (client == null)
			return;

		while (client.CurrentUser == null)
		{
			try
			{
				await client.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter);
			}
			catch (InvalidOperationException ex)
			{
				var message = "You must log in. Login Required";
			}
		}
	}



    public class ExpenseSource : UITableViewSource
    {

      ExpenseListViewController vc;
      ExpensesViewModel viewModel;
      public ExpenseSource(ExpenseListViewController vc, ExpensesViewModel viewModel)
      {
        this.vc = vc;
        this.viewModel = viewModel;
      }

      public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
      {
        return UITableViewCellEditingStyle.Delete;
      }

      public async override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
      {
        var expense = viewModel.Expenses[indexPath.Row];
        await viewModel.ExecuteDeleteExpenseCommand(expense);
        tableView.ReloadData();
      }

      public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
      {
        if (viewModel.IsBusy)
          return;

        var expense = viewModel.Expenses[indexPath.Row];

        vc.NavigationController.PushViewController(new ExpenseViewController(expense), true);
      }
      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
      {
        var cell = tableView.DequeueReusableCell("ExpenseCell");


        var expense = viewModel.Expenses[indexPath.Row];

        cell.TextLabel.Text = expense.Name;
        cell.DetailTextLabel.Text = expense.TotalDisplay;

        return cell;
      }

      public override nint RowsInSection(UITableView tableview, nint section)
      {
        return viewModel.Expenses.Count;
      }

    }

  }
}