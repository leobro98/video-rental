
namespace Leobro.VideoStore.Model
{
    public class RentalOptions
    {
        public VideoTitle.TitleType TitleType { get; private set; }
        public int RentalDays { get; private set; }
        public int BonusPointsAvailable { get; private set; }
        public bool IsPaymentByPointsPossible { get; private set; }
        public decimal Price { get; private set; }
        public int BonusPointsPayed { get; set; }

        public RentalOptions(VideoTitle.TitleType titleType, int rentalDays, int bonusPointsAvailable,
            bool isPaymentByPointsPossible, decimal price)
        {
            TitleType = titleType;
            RentalDays = rentalDays;
            BonusPointsAvailable = bonusPointsAvailable;
            IsPaymentByPointsPossible = isPaymentByPointsPossible;
            Price = price;
        }
    }
}
