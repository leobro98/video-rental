using Leobro.VideoStore.Model;

namespace Leobro.VideoStore
{
    public interface IPricePolicy
    {
        RentalOptions GetRentalOptions(VideoTitle.TitleType titleType, int rentalDays, int bonusPoints);

        int CalculateBonus(VideoTitle.TitleType titleType, int rentalDays);
    }
}
