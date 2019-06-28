using System.Collections.Generic;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStore.Price
{
    /// <summary>
    /// <para>The store's policy regarding the price for the rental and bonus points usage. Configured with 3
    /// <see cref="RentalTerms"/> for 3 different video title types passed from the main application module.
    /// </para>
    /// <para>Assumptions:</para>
    /// <list type="bullet">
    ///     <item>The price for the first part of a rental period is fixed ("flat period").</item>
    ///     <item>The price for days overflowing the "flat period" is calculated by multiplying the day price
    ///         by the day count.</item>
    ///     <item>Bonus points awarded for the rental depend on the video title type and rental day count.</item>
    ///     <item>Every rental day costs equal number of bonus points.</item>
    ///     <item>Only the whole rental period can be payed by points.</item>
    ///     <item>There is no business rules for delayed return of a video casette.</item>
    /// </list>
    /// </summary>
    public class PricePolicy : IPricePolicy
    {
        private List<RentalTerms> allTerms;

        public PricePolicy(List<RentalTerms> allTerms)
        {
            this.allTerms = allTerms;
        }

        public RentalOptions CalculateRentalOptions(VideoTitle.TitleType titleType, int rentalDays, int bonusPoints)
        {
            var terms = allTerms.Find(x => x.TitleType == titleType);
            bool isPaymentByPointsPossible = IsPaymentByPointsPossible(rentalDays, bonusPoints, terms);
            decimal price = terms.FlatPeriodFee + GetTrailingDaysPrice(rentalDays, terms);
            int priceInPoints = GetPriceInPoints(rentalDays, terms);

            return new RentalOptions(titleType, rentalDays, bonusPoints, isPaymentByPointsPossible, price, priceInPoints);
        }

        /// <summary>
        /// Calculates the price for the rental period after the "flat period" - period with a fixed price
        /// independent on the number of days in the period. The price for the "trailing days" depends on the
        /// day count.
        /// </summary>
        /// <param name="rentalDays">the whole duration of the rental period in days,</param>
        /// <param name="terms">the set of rental terms for the chosen video title type.</param>
        /// <returns>The price for the "trailing days" of the rental period.</returns>
        private decimal GetTrailingDaysPrice(int rentalDays, RentalTerms terms)
        {
            if (rentalDays > terms.FlatPeriodDays)
            {
                return (rentalDays - terms.FlatPeriodDays) * terms.TrailingFee;
            }
            return 0;
        }

        private bool IsPaymentByPointsPossible(int rentalDays, int bonusPoints, RentalTerms terms)
        {
            int priceInPoints = GetPriceInPoints(rentalDays, terms);
            return terms.IsPaymentByPointsAllowed && (bonusPoints >= priceInPoints);
        }

        private static int GetPriceInPoints(int rentalDays, RentalTerms terms)
        {
            return rentalDays * terms.RentalDayPriceInPoints;
        }

        public int CalculateBonus(VideoTitle.TitleType titleType, int rentalDays)
        {
            var terms = allTerms.Find(x => x.TitleType == titleType);
            return terms.PointsForRent;
        }
    }
}
