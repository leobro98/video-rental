using System.Collections.Generic;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStore.Price
{
    public class PricePolicy : IPricePolicy
    {
        private List<Terms> allTerms;

        public PricePolicy(List<Terms> allTerms)
        {
            this.allTerms = allTerms;
        }

        public RentalOptions GetRentalOptions(VideoTitle.TitleType titleType, int rentalDays, int bonusPoints)
        {
            var terms = allTerms.Find(x => x.TitleType == titleType);
            bool isPaymentByPointsPossible = IsPaymentByPointsPossible(rentalDays, bonusPoints, terms);
            decimal price = terms.FlatPeriodFee + GetTrailingDaysPrice(rentalDays, terms);

            return new RentalOptions(titleType, rentalDays, bonusPoints, isPaymentByPointsPossible, price);
        }

        private decimal GetTrailingDaysPrice(int rentalDays, Terms terms)
        {
            if (rentalDays > terms.FlatPeriodDays)
            {
                return (rentalDays - terms.FlatPeriodDays) * terms.TrailingFee;
            }
            return 0;
        }

        private bool IsPaymentByPointsPossible(int rentalDays, int bonusPoints, Terms terms)
        {
            int rentalPriceInPoints = rentalDays * terms.RentalDayPriceInPoints;

            return terms.IsPaymentByPointsAllowed && (bonusPoints >= rentalPriceInPoints);
        }

        public int CalculateBonus(VideoTitle.TitleType titleType, int rentalDays)
        {
            var terms = allTerms.Find(x => x.TitleType == titleType);
            return terms.PointsForRent;
        }
    }
}
