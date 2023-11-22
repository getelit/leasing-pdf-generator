using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;

namespace LeasingOffers.Models
{
    public class OfferManager
    {
        public static int SelectedItemsAmount { get; set; }


        public static ObservableCollection<Offer> SelectedItems = new ObservableCollection<Offer>();
        public static Offer? offer { get; set; }

		public static ObservableCollection<Offer> _DatabaseOffers = new ObservableCollection<Offer>();

		public static ObservableCollection<Offer> GetOffers()
		{
            //string connectionString = @"Data Source=C:\Users\Jendos\Desktop\Record-Book 5\Record Book MVVM\Models\offers.db";
            //string connectionString = @"Data Source=C:\Users\Jendos\Desktop\Record-Book 5\Record Book MVVM\Models\offers.db";
            string connectionString = @"Data Source=..\..\..\Models\offers.db";
            string query = "SELECT * FROM offers";


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        int idIndex = reader.GetOrdinal("Id");
                        int companyNameIndex = reader.GetOrdinal("CompanyName");
                        int subjectIndex = reader.GetOrdinal("Subject");
                        int plCostIndex = reader.GetOrdinal("PlCost");
                        int advancePaymentIndex = reader.GetOrdinal("AdvancePayment");
                        int advancePaymentInRubIndex = reader.GetOrdinal("AdvancePaymentInRub");
                        int commissionIndex = reader.GetOrdinal("Commission");
                        int annuityIndex = reader.GetOrdinal("Annuity");
                        int paymentAmountIndex = reader.GetOrdinal("PaymentAmount");
                        int redemptionIndex = reader.GetOrdinal("Redemption");
                        int insuranceIndex = reader.GetOrdinal("Insurance");
                        int totalSumIndex = reader.GetOrdinal("TotalSum");

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(idIndex);
                            string companyName = reader.GetString(companyNameIndex);
                            string subject = reader.GetString(subjectIndex);
                            string plCost = reader.GetString(plCostIndex);
                            string advancePayment = reader.GetString(advancePaymentIndex);
                            string advancePaymentInRub = reader.GetString(advancePaymentInRubIndex);
                            string commission = reader.GetString(commissionIndex);
                            string annuity = reader.GetString(annuityIndex);
                            string paymentAmount = reader.GetString(paymentAmountIndex);
                            string redemption = reader.GetString(redemptionIndex);
                            string insurance = reader.GetString(insuranceIndex);
                            string totalSum = reader.GetString(totalSumIndex);

                            Offer offer = new Offer
                            {
                                Id = id,
                                CompanyName = companyName,
                                Subject = subject,
                                PlCost = plCost,
                                Advance = advancePayment,
                                AdvanceInRub = advancePaymentInRub,
                                Commission = commission,
                                Annuity = annuity,
                                PaymentAmount = paymentAmount,
                                Redemption = redemption,
                                Insurance = insurance,
                                TotalSum = totalSum
                            };

                            AddOffer(offer);
                        }
                    }
                }
                connection.Close();
            }

            return _DatabaseOffers;

        }

        public static void AddOffer(Offer offer)
		{
            _DatabaseOffers.Add(offer);
		}

    }
}
