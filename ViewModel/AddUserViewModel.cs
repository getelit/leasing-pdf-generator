using LeasingOffers.Commands;
using LeasingOffers.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace LeasingOffers.ViewModel
{
	public class AddOfferViewModel
	{

		public ICommand AddOfferCommand { get; set; }
        public string? CompanyNameText { get; set; }
		public string? SubjectText { get; set; }
		public string? PlCostText { get; set; }
		public string? AdvanceText { get; set; }
		public string? AdvanceInRubText { get; set; }
		public string? CommissionText { get; set; }
		public string? AnnuityText { get; set; }
		public string? PaymentAmountText { get; set; }
		public string? RedemptionText { get; set; }
		public string? InsuranceText { get; set; }
		public string? TotalSumText { get; set; }

		public AddOfferViewModel()
		{
            AddOfferCommand = new RelayCommand(AddOffer, CanAddOffer);
        }

		private bool CanAddOffer(object obj)
		{
			return true;
		}

		private void AddOffer(object obj)
		{
            string[] fieldNames = { "Стоимость ПЛ", "Аванс %", "Аванс в руб.", "Комиссия", "Аннуитетный платёж",
                                    "Количество платежей", "Выкупной платёж", "Страхование", "Сумма договора лизинга" };

            string[] fieldValues = { PlCostText, AdvanceText, AdvanceInRubText, CommissionText,
                         AnnuityText, PaymentAmountText, RedemptionText,
                         InsuranceText, TotalSumText };


            for (int i = 0; i < fieldNames.Length; i++)
            {

                if (string.IsNullOrEmpty(fieldValues[i]))
                {
                    fieldValues[i] = "0.00";
                    continue;
                }

                if (!IsNumber(fieldValues[i]))
                {
                    MessageBox.Show($"В окно '{fieldNames[i]}' нужно ввести число!");
                    return;
                }
            }

            string connectionString = @"Data Source=..\..\..\Models\offers.db";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO offers " +
                    "(id, companyName, plCost, advancePayment, advancePaymentInRub, commission, subject, annuity, paymentAmount, redemption, insurance, totalSum) " +
                    "VALUES (@id, @companyName, @plCost, @advancePayment, @advancePaymentInRub, @commission, @subject, @annuity, @paymentAmount, @redemption, @insurance, @totalSum)";

                int id = GetId();
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@companyName", CompanyNameText);
                    command.Parameters.AddWithValue("@subject", SubjectText);
                    command.Parameters.AddWithValue("@plCost", fieldValues[0]);
                    command.Parameters.AddWithValue("@advancePayment", fieldValues[1]);
                    command.Parameters.AddWithValue("@advancePaymentInRub", fieldValues[2]);
                    command.Parameters.AddWithValue("@commission", fieldValues[3]);
                    command.Parameters.AddWithValue("@annuity", fieldValues[4]);
                    command.Parameters.AddWithValue("@paymentAmount", fieldValues[5]);
                    command.Parameters.AddWithValue("@redemption", fieldValues[6]);
                    command.Parameters.AddWithValue("@insurance", fieldValues[7]);
                    command.Parameters.AddWithValue("@totalSum", fieldValues[8]);

                    command.ExecuteNonQuery();
                }

                OfferManager.AddOffer(new Offer()
                {
                    Id = id,
                    CompanyName = CompanyNameText,
                    Subject = SubjectText,
                    PlCost = FormatNumber(fieldValues[0]),
                    Advance = FormatNumber(fieldValues[1]),
                    AdvanceInRub = FormatNumber(fieldValues[2]),
                    Commission = FormatNumber(fieldValues[3]),
                    Annuity = FormatNumber(fieldValues[4]),
                    PaymentAmount = FormatNumber(fieldValues[5]),
                    Redemption = FormatNumber(fieldValues[6]),
                    Insurance = FormatNumber(fieldValues[7]),
                    TotalSum = FormatNumber(fieldValues[8]),
                });

                connection.Close();
            }

        }

        private static bool IsNumber(string strNum)
        {
            if (string.IsNullOrEmpty(strNum))
            {
                Console.WriteLine("JONAS");
                return false;
            }

            if (strNum.Contains('.'))
            {
                strNum = strNum.Replace('.', ',');
            }

            if (strNum.Contains(' '))
            {
                strNum = Regex.Replace(strNum, @"\s", "");
            }

            return double.TryParse(strNum, out double num);
        }

        private string FormatNumber(string strNum)
        {
            if (strNum.Contains(','))
            {
                strNum = strNum.Replace(',', '.');
            }

            if (strNum.Contains(' '))
            {
                strNum = Regex.Replace(strNum, @"\s", "");
            }

            return strNum;
        }

        private int GetId()
        {
            int offersCount = OfferManager._DatabaseOffers.Count;
            int[] allID = new int[offersCount];
            int id = offersCount + 1;

            for (int i = 0; i < offersCount; i++)
            {
                allID[i] = OfferManager._DatabaseOffers[i].Id;
            }

            for (int i = 1; i <= offersCount; i++)
            {
                if (!allID.Contains(i))
                {
                    id = i;
                    break;
                }
            }

            return id;
        }

    }
}
