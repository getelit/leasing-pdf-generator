using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using LeasingOffers.Commands;
using LeasingOffers.Models;
using LeasingOffers.Views;

namespace LeasingOffers.ViewModel
{
	public class MainViewModel
	{
		public ObservableCollection<Offer> Offers { get; set; }


		public ICommand ShowWindowCommand { get; set; }
		
        public ICommand EditOfferCommand { get; set; }

        private ICommand _SavePDFCommand;
        public ICommand SavePDFCommand
        {
            get { return _SavePDFCommand; }
            set
            {
                _SavePDFCommand = value;
                OnPropertyChanged(nameof(SavePDFCommand));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
		{
			Offers = OfferManager.GetOffers();
			ShowWindowCommand = new RelayCommand(ShowWindow, CanShowWindow);
			EditOfferCommand = new RelayCommand(ShowEditOfferWindow, CanShowWindow);
        }

        private bool CanShowWindow(object obj)
		{
			return true;
		}

		private void ShowWindow(object obj)
		{
			var mainWindow = obj as Window;

			AddOffer addOfferWin = new AddOffer();
			addOfferWin.Owner = mainWindow;
			addOfferWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addOfferWin.Title = "Добавление предложения";
			addOfferWin.Show();
		}

        private void ShowEditOfferWindow(object obj)
        {
            if (OfferManager.SelectedItemsAmount == 0)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (OfferManager.SelectedItemsAmount > 1)
            {
                MessageBox.Show("Одновременно можно изменять только 1 предложение!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editWindow = obj as Window;
            EditOffer editOfferWin = new EditOffer();
            editOfferWin.Owner = editWindow;
            editOfferWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editOfferWin.Title = "Изменение предложения";
            editOfferWin.Show();
        }
    }
}
