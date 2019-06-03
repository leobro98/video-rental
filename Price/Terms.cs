using Leobro.VideoStore.Model;

namespace Leobro.VideoStore.Price
{
    public class Terms
    {
        public VideoTitle.TitleType TitleType { get; set; }

        /// <summary>
        /// Rent for the first n days costs some fixed price. This property is this number of days.
        /// </summary>
        public int FlatPeriodDays { get; set; }

        /// <summary>
        /// Rent for the first n days costs some fixed price. This property is this fixed price.
        /// </summary>
        public decimal FlatPeriodFee { get; set; }

        /// <summary>
        /// This is the price for every day out of the <see cref="FlatPeriodDays"/>.
        /// </summary>
        public decimal TrailingFee { get; set; }

        /// <summary>
        /// Shows if the rental of the title type can be payed by bonus points. 
        /// </summary>
        public bool IsPaymentByPointsAllowed { get; set; }

        /// <summary>
        /// Bonus points rewarded for a rental period (regardless of the time rented).
        /// </summary>
        public int PointsForRent { get; set; }

        /// <summary>
        /// If the payment by bonus points allowed for this title type, this is the price of one rental day in bonus points.
        /// </summary>
        public int RentalDayPriceInPoints { get; set; }
    }
}
