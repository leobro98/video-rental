using Leobro.VideoStore;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStoreTest
{
    /// <summary>
    /// Returns predefined values from the methods.
    /// </summary>
    public class PricePolicyStub : IPricePolicy
    {
        public const int BonusPoints = 3;

        public RentalOptions CalculateRentalOptions(VideoTitle.TitleType titleType, int rentalDays, int bonusPoints)
        {
            return new RentalOptions(titleType, rentalDays, bonusPoints, false, 0, 0);
        }

        public int CalculateBonus(VideoTitle.TitleType titleType, int rentalDays)
        {
            return BonusPoints;
        }
    }
}
