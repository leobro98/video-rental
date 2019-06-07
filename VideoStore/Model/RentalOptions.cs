namespace Leobro.VideoStore.Model
{
    public class RentalOptions
    {
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
