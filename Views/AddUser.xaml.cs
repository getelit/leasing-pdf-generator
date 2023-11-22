using LeasingOffers.ViewModel;
using System.Windows;

namespace LeasingOffers.Views
{
	
	public partial class AddOffer : Window
	{
		public AddOffer()
		{
			InitializeComponent();
            AddOfferViewModel addOfferViewModel = new AddOfferViewModel();
			this.DataContext = addOfferViewModel;

		}

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            CompanyNameTextBox.Text = string.Empty;
            PlCostTextBox.Text = string.Empty;
            AdvanceTextBox.Text = string.Empty;
            AdvanceInRubTextBox.Text = string.Empty;
            CommissionTextBox.Text = string.Empty;
            SubjectTextBox.Text = string.Empty;
            AnnuityTextBox.Text = string.Empty;
            PaymentAmountTextBox.Text = string.Empty;
            RedemptionTextBox.Text = string.Empty;
            InsuranceTextBox.Text = string.Empty;
            TotalSumTextBox.Text = string.Empty;
        }
    }
}
