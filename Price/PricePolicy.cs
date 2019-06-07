using System.Collections.Generic;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStore.Price
{
    public class PricePolicy : IPricePolicy
    {
        private List<RentalTerms> allTerms;

        public PricePolicy(List<RentalTerms> allTerms)
        {
            this.allTerms = allTerms;
        }

        public RentalOptions GetRentalOptions(VideoTitle.TitleType titleType, int rentalDays, int bonusPoints)
        {
            var terms = allTerms.Find(x => x.TitleType == titleType);
            bool isPaymentByPointsPossible = IsPaymentByPointsPossible(rentalDays, bonusPoints, terms);
            decimal price = terms.FlatPeriodFee + GetTrailingDaysPrice(rentalDays, terms);
            int priceInPoints = GetPriceInPoints(rentalDays, terms);

            return new RentalOptions(titleType, rentalDays, bonusPoints, isPaymentByPointsPossible, price, priceInPoints);
        }

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
