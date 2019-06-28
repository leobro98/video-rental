using Leobro.VideoStore.Model;

namespace Leobro.VideoStore.UI
{
    /// <summary>
    /// Manages the workflow for the rental of a video casette.
    /// </summary>
    internal class FilmRenter
    {
        private Store store;
        private bool payByPoints = false;
        private Customer customer;
        private VideoTitle film;
        private int dayCount;
        private Casette casette;
        private RentalOptions options;

        public FilmRenter(Store store)
        {
            this.store = store;
        }

        /// <summary>
        /// The method implements the whole workflow for the casette rental.
        /// </summary>
        /// <exception cref="CustomerNotFoundException">when the entered ID does not correspond to any registered customer.</exception>
        /// <exception cref="TitleNotFoundException">when the entered ID does not correspond to any registered video title.</exception>
        /// <exception cref="CasetteNotFoundException">when there is no casette available for the video title specified.</exception>
        public void Rent()
        {
            try
            {
                GetCustomer();
                GetFilm();
                GetDayCount();
                CalculateTerms();

                if (Interactor.RentFilm.GetConfirmationForTerms(casette, options))
                {
                    RentCasette();
                    ShowReceipt();
                }
            }
            catch (CustomerNotFoundException)
            {
                Interactor.RentFilm.ShowCustomerNotFound();
            }
            catch (TitleNotFoundException)
            {
                Interactor.RentFilm.ShowFilmNotFound();
            }
            catch (CasetteNotFoundException)
            {
                Interactor.RentFilm.ShowCasetteNotFound();
            }
        }

        private void GetCustomer()
        {
            // as customer doesn't have personal information, we can't search by it
            // let's suppose, we just know their ID
            int customerId = Interactor.RentFilm.GetCustomerId();
            customer = store.GetCustomer(customerId);
        }

        private void GetFilm()
        {
            // film ID can be seen from the list of films
            int filmId = Interactor.RentFilm.GetFilmId();
            film = store.GetTitle(filmId);
        }

        private void GetDayCount()
        {
            dayCount = Interactor.RentFilm.GetDays();
        }

        private void CalculateTerms()
        {
            casette = store.GetCasetteOnShelfByTitle(film.Id);
            options = store.GetRentalOptions(customer.Id, casette.Title.Type, dayCount);
        }

        private void RentCasette()
        {
            GetConfirmationForPoints();

            var rental = new Rental(customer, casette)
            {
                BonusPointsPayed = payByPoints ? options.PriceInPoints : 0,
                Price = payByPoints ? 0 : options.Price,
                RentalDays = dayCount
            };
            store.RentCasette(rental);
        }

        private void GetConfirmationForPoints()
        {
            if (options.IsPaymentByPointsPossible)
            {
                payByPoints = Interactor.RentFilm.GetConfirmationForPayingByPoints(options);
            }
        }

        private void ShowReceipt()
        {
            if (payByPoints)
            {
                Interactor.RentFilm.ShowReceiptWithPoints(casette, options, customer.BonusPoints);
            }
            else
            {
                Interactor.RentFilm.ShowReceiptWithMoney(casette, options);
            }
        }
    }
}
