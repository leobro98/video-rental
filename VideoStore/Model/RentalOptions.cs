namespace Leobro.VideoStore.Model
{
    public class RentalOptions
    {
        /// <summary>
        /// Groups data that can be presented to a customer about the rental of a video title.
        /// Contains all possible options for the rental to let customer choose one.
        /// </summary>
        public VideoTitle.TitleType TitleType { get; private set; }
        public int RentalDays { get; private set; }
        public int PointsAvailable { get; private set; }
        public bool IsPaymentByPointsPossible { get; private set; }
        public decimal Price { get; private set; }
        public int PriceInPoints { get; set; }

        public RentalOptions(VideoTitle.TitleType titleType, int rentalDays, int pointsAvailable,
            bool isPaymentByPointsPossible, decimal price, int priceInPoints)
        {
            TitleType = titleType;
            RentalDays = rentalDays;
            PointsAvailable = pointsAvailable;
            IsPaymentByPointsPossible = isPaymentByPointsPossible;
            Price = price;
            PriceInPoints = priceInPoints;
        }
    }
}
