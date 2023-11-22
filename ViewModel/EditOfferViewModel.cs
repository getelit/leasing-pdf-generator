using LeasingOffers.Models;
using System.Windows.Input;

namespace LeasingOffers.ViewModel
{
    public class EditOfferViewModel
    {

        public ICommand EditOfferCommand { get; set; }

        public Offer offer { get; set; }

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

        public EditOfferViewModel()
        {

        }

        private bool CanEditOffer(object obj)
        {
            return true;
        }

        private static bool IsNumber(string strNum)
        {
            if (string.IsNullOrEmpty(strNum))
            {
                return false;
            }

            if (strNum != null && strNum.Contains('.'))
            {
                strNum = strNum.Replace('.', ',');
            }
            return double.TryParse(strNum, out double num);
        }

    }
}