using Microsoft.AspNetCore.Components;
using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using HogWildWeb.Components;
using BYSResults;
using System.Reflection;
using MudBlazor.Charts;

namespace HogWildWeb.Components.Pages.SamplePages
{
    public partial class CustomerList
    {
        #region Fields

        // The last name
        private string lastName = string.Empty;

        // The phone number
        private string phoneNumber = string.Empty;

        // Tells us if the search has been performed
        private bool noRecords;

        // The feedback message
        private string feedbackMessage = string.Empty;

        // The error message
        private string errorMessage = string.Empty;

        // has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        // has error
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage);

        // error details
        private List<string> errorDetails = new();
		#endregion

		#region Properties
		// Injects the CustomerService dependency.
		[Inject]
		protected CustomerService CustomerService { get; set; } = default!;

		// Injects the InvoiceService dependency.
		[Inject]
		protected InvoiceService InvoiceService { get; set; } = default!;

		// Injects the NavigationManager dependency.
		[Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        // Gets or sets the customers search view.
        protected List<CustomerSearchView> Customers { get; set; } = new();

        protected InvoiceView Invoice = default!;
		protected CustomerSearchView Customer = default!;
		#endregion

		#region Methods
		//  search for an existing customer
		private void Search()
        {
            try
            {
                // reset the no records flag
                noRecords = false;

                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = String.Empty;

                //  clear the customer list before we do our search
                Customers.Clear();

                if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phoneNumber))
                {
                    throw new ArgumentException("Please provide either a last name and/or phone number");
                }

                //  search for our customers

                Customers = CustomerService.GetCustomers(lastName, phoneNumber);
                if (Customers.Count > 0)
                {
                    feedbackMessage = "Search for customer(s) was successful";
                }
                else
                {
                    feedbackMessage = "No customers were found for your search criteria";
                    noRecords = true;
                }
            }
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }
                errorMessage = $"{errorMessage}Unable to search for customer";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        //  new customer
        private void New()
        {
            NavigationManager.NavigateTo("/SamplePages/CustomerEdit/0");
        }

        //  edit selected customer
        private void EditCustomer(int customerID)
        {
            NavigationManager.NavigateTo($"/SamplePages/CustomerEdit/{customerID}");
        }

        //  new invoice for selected customer
        private void NewInvoice(int customerID)
        {
			// clear previous error details and messages
			errorDetails.Clear();
			errorMessage = string.Empty;
			feedbackMessage = String.Empty;

			//Missing Invoice: No invoice was supply
			//InvoiceView invoiceView = null;

			//Missing Information: All rules except rules involving invoice lines
			InvoiceView invoiceView = new InvoiceView();
			//  update missing fields
			invoiceView.CustomerID = 1;
			invoiceView.EmployeeID = 1;
			List<InvoiceLineView> invoiceLineViews = new();

			//Missing Information: Missing part
			//InvoiceLineView invoiceLineView = new();
			//invoiceLineViews.Add(invoiceLineView);

			//Invalid Part price and quantity
			//InvoiceLineView invoiceLineView = new()
			//{
			//    PartID = 1,
			//    Price = -1,
			//    Quantity = 0
			//};
			//         InvoiceLineView invoiceLineView = new();
			//         invoiceLineView.PartID = 1;
			//         invoiceLineView.Price = -1;
			//invoiceLineView.Quantity = 0;

			//Duplicated Invoice Line Items
			invoiceView.InvoiceID = 1173;
			InvoiceLineView invoiceLineView = new()
			{
				PartID = 1,
				Price = 10,
				Quantity = 4
			};
			invoiceLineViews.Add(invoiceLineView);
			invoiceLineView = new()
			{
				PartID = 1,
				Price = 10,
				Quantity = 4
			};
			invoiceLineViews.Add(invoiceLineView);

			invoiceView.InvoiceLines = invoiceLineViews;

			Customer = Customers
                        .Where(c => c.CustomerID == customerID)
                        .FirstOrDefault();

			// wrap the service call in a try/catch to handle unexpected exceptions
			try
			{
				Result<InvoiceView> result = InvoiceService.AddEditInvoice(invoiceView);
				if (result.IsSuccess)
				{
					Invoice = result.Value;
				}
				else
				{
					errorMessage = $"Unable to create a new Invoice for customer {Customer.FirstName} {Customer.LastName}";
					errorDetails = GetErrorMessages(result.Errors.ToList());
				}
			}
			catch (Exception ex)
			{
				// capture any exception message for display
				errorMessage = ex.Message;
			}
		}

		#endregion

		//	This region includes support methods
		#region Support Method
		// Converts a list of error objects into their string representations.
		public static List<string> GetErrorMessages(List<BYSResults.Error> errorMessage)
		{
			// Initialize a new list to hold the extracted error messages
			List<string> errorList = new();

			// Iterate over each Error object in the incoming list
			foreach (var error in errorMessage)
			{
				// Convert the current Error to its string form and add it to errorList
				errorList.Add(error.ToString());
			}

			// Return the populated list of error message strings
			return errorList;
		}
		#endregion
	}
}
