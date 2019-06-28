using Leobro.VideoStore.Model;

namespace Leobro.VideoStore
{
    /// <summary>
    /// The store's policy regarding the price for the rental and bonus points usage.
    /// </summary>
    public interface IPricePolicy
    {
        /// <summary>
        /// Caclulates the price for the rental and different options for the payment.
        /// </summary>
        /// <param name="titleType">the type of the video title.</param>
        /// <param name="rentalDays">the number of days for which the film is rented.</param>
        /// <param name="bonusPoints">the number of bonus points the customer has.</param>
        /// <returns>The price for the rental and payment options.</returns>
        RentalOptions CalculateRentalOptions(VideoTitle.TitleType titleType, int rentalDays, int bonusPoints);

        /// <summary>
        /// Calculates the number of bonus points gained by the customer for the current rental.
        /// </summary>
        /// <param name="titleType">the type of the video title.</param>
        /// <param name="rentalDays">the number of days for which the film is rented.</param>
        /// <returns>The number of bonus points for the current rental.</returns>
        int CalculateBonus(VideoTitle.TitleType titleType, int rentalDays);
    }
}
