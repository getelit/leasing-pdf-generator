using LeasingOffers.Models;
using LeasingOffers.ViewModel;
using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;

namespace LeasingOffers.Views
{
    public partial class EditOffer : Window
    {
        public EditOffer()
        {
            InitializeComponent();
        }

        public void SetFields(object sender, RoutedEventArgs e)
        {
            Offer offer = OfferManager.offer;

            CompanyNameTextBox.Text = offer.CompanyName;
            PlCostTextBox.Text = offer.PlCost;
            AdvanceTextBox.Text = offer.Advance;
            AdvanceInRubTextBox.Text = offer.AdvanceInRub;
            CommissionTextBox.Text = offer.Commission;
            SubjectTextBox.Text = offer.Subject;
            AnnuityTextBox.Text = offer.Annuity;
            PaymentAmountTextBox.Text = offer.PaymentAmount;
            RedemptionTextBox.Text = offer.Redemption;
            InsuranceTextBox.Text = offer.Insurance;
            TotalSumTextBox.Text = offer.TotalSum;
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

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            string[] fieldNames = { "Стоимость ПЛ", "Аванс %", "Аванс в руб.", "Комиссия", "Аннуитетный платёж",
                                    "Количество платежей", "Выкупной платёж", "Страхование", "Сумма договора лизинга" };

            string[] fieldValues = { PlCostTextBox.Text, AdvanceTextBox.Text, AdvanceInRubTextBox.Text, CommissionTextBox.Text,
                                     AnnuityTextBox.Text, PaymentAmountTextBox.Text, RedemptionTextBox.Text,
                                     InsuranceTextBox.Text, TotalSumTextBox.Text };

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

            Offer offer = OfferManager.offer;
            string connectionString = @"Data Source=..\..\..\Models\offers.db";

            string query = "UPDATE offers SET companyName = @companyName, subject = @subject, plCost = @plCost, " +
                   "advancePayment = @advancePayment, advancePaymentInRub = @advancePaymentInRub, " +
                   "commission = @commission, annuity = @annuity, paymentAmount = @paymentAmount, " +
                   "redemption = @redemption, insurance = @insurance, totalSum = @totalSum " +
                   "WHERE id = " + offer.Id;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@companyName", CompanyNameTextBox.Text);
                    command.Parameters.AddWithValue("@subject", SubjectTextBox.Text);
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

                connection.Close();
            }

            OfferManager._DatabaseOffers.Clear();
            OfferManager.GetOffers();
            Close();
        }

        private static bool IsNumber(string strNum)
        {
            if (string.IsNullOrEmpty(strNum))
            {
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
    }
}