using LeasingOffers.Commands;
using LeasingOffers.Models;
using LeasingOffers.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;


namespace LeasingOffers.Views
{
	public partial class Main : Window
	{
		public Main()
		{
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainViewModel mainViewModel = new MainViewModel();
			this.DataContext = mainViewModel;
            mainViewModel.SavePDFCommand = new RelayCommand(ExecuteSavePDFCommand);
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			OfferList.Items.Filter = FilterMethod;
		}

		private bool FilterMethod(object obj)
		{
			var Offer = (Offer)obj;

			return Offer.CompanyName.Contains(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase);

		}

        private void ExecuteSavePDFCommand(object parameter)
        {
            if (OfferList.SelectedItems.Count > 5)
            {
                MessageBox.Show("Больше пяти предложений нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if (OfferList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                GetSelectedItems();
                GetSelectedItemsAmount();

                var physicalPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                PDFGenerator pdfGenerator = new PDFGenerator();
                PDFGenerator.Comment = expertCommentTextBox.Text;
                pdfGenerator.GeneratePDF(physicalPath);
            }
        }

        private void DeleteSelected(object sender, RoutedEventArgs e)
        {
            GetSelectedItems();
            List<Offer> offersToRemove = new List<Offer>();

            foreach (Offer offerInSelected in OfferManager.SelectedItems)
            {
                foreach (Offer offerInList in OfferManager._DatabaseOffers)
                {
                    if (offerInList.Id == offerInSelected.Id)
                    {
                        offersToRemove.Add(offerInList);
                    }
                }
            }

            string connectionString = @"Data Source=..\..\..\Models\offers.db";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (Offer offerToRemove in offersToRemove)
                {
                    string deleteQuery = "DELETE FROM offers WHERE id = " + offerToRemove.Id;

                    using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }

            OfferManager._DatabaseOffers.Clear();
            OfferManager.GetOffers();
        }

        private void ChangeSelected(object sender, RoutedEventArgs e)
        {
            OfferManager.SelectedItemsAmount = OfferList.SelectedItems.Count;
            OfferManager.offer = (Offer)OfferList.SelectedItem;
        }

        private void GetSelectedItemsAmount()
        {
            OfferManager.SelectedItemsAmount = OfferList.SelectedItems.Count;
        }

        private void GetSelectedItems()
        {
            ObservableCollection<Offer> selectedItems = new ObservableCollection<Offer>();

            foreach (Offer offer in OfferList.SelectedItems)
            {
                selectedItems.Add(offer);
            }

            OfferManager.SelectedItems = selectedItems;
        }

    }
}
