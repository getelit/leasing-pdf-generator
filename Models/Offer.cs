namespace LeasingOffers.Models
{
	public class Offer
	{
		public Offer()
		{
				
		}

		public int Id { get; set; }
		public string? CompanyName { get; set; }
        public string? Subject { get; set; }
		public string? PlCost { get; set; }
		public string? Advance { get; set; }
        public string? AdvanceInRub { get; set; }
        public string? Commission { get; set; }
        public string? Annuity { get; set; }
        public string? PaymentAmount { get; set; }
        public string? Redemption { get; set; }
        public string? Insurance { get; set; }

		public string? TotalSum { get; set; }

    }
}
