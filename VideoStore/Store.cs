using System.Collections.Generic;
using System.Linq;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStore
{
    /// <summary>
    /// The core business logic component of the video rental store. Uses repository to store or retrieve data.
    /// Uses price policy to calculate the price for the rental or bonus points.
    /// </summary>
    public class Store
    {
        private IRepository repository;
        private IPricePolicy pricePolicy;

        /// <summary>
        /// Creates an instance of the store having references to the repository and price policy.
        /// </summary>
        /// <param name="repository">an instance of the repository.</param>
        /// <param name="pricePolicy">an instance of the price policy.</param>
        public Store(IRepository repository, IPricePolicy pricePolicy)
        {
            this.repository = repository;
            this.pricePolicy = pricePolicy;
        }

        /// <summary>
        /// Adds casette of a certain title to the repository.
        /// </summary>
        /// <param name="title">the title of the casette.</param>
        /// <returns>ID of the added casette.</returns>
        public int AddCasette(VideoTitle title)
        {
            var storedTitle = FindOrCreateTitle(title);
            return repository.AddCasette(storedTitle);
        }

        private VideoTitle FindOrCreateTitle(VideoTitle soughtTitle)
        {
            List<VideoTitle> titles = repository.FindTitle(soughtTitle.Name, soughtTitle.Year);

            if (titles.Count == 0)
            {
                var title = new VideoTitle(soughtTitle.Name, soughtTitle.Year, soughtTitle.Type);
                int id = repository.AddTitle(title);
                title = repository.GetTitle(id);
                return title;
            }
            return titles.First();
        }

        /// <summary>
        /// Find an available (not rented) casette of the title.
        /// </summary>
        /// <param name="titleId">ID of the video title.</param>
        /// <returns>Found casette.</returns>
        /// <exception cref="CasetteNotFoundException">if there is no available casette with this title.</exception>
        public Casette GetCasetteOnShelfByTitle(int titleId)
        {
            return repository.GetCasetteOnShelfByTitle(titleId);
        }

        /// <summary>
        /// Finds the video title.
        /// </summary>
        /// <param name="name">the name of the title (case insensitive).</param>
        /// <param name="year">the year of the title.</param>
        /// <returns>The found video title or <c>null</c> if not found.</returns>
        public VideoTitle FindTitle(string name, int year)
        {
            return repository.FindTitle(name, year).FirstOrDefault();
        }

        /// <summary>
        /// Returns the title by its ID.
        /// </summary>
        /// <param name="id">ID of the title.</param>
        /// <returns>The title with this ID.</returns>
        /// <exception cref="TitleNotFoundException">if there is no title with this ID.</exception>
        public VideoTitle GetTitle(int id)
        {
            return repository.GetTitle(id);
        }

        /// <summary>
        /// Removes the title from the repository.
        /// </summary>
        /// <param name="id">ID of the title.</param>
        public void RemoveTitle(int id)
        {
            foreach (var casette in repository.GetAllCasettesByTitle(id))
            {
                repository.RemoveCasette(casette.Id);
            }
            repository.RemoveTitle(id);
        }

        /// <summary>
        /// Changes the type of the title.
        /// </summary>
        /// <param name="titleId">ID of the title.</param>
        /// <param name="type">The new type for the title.</param>
        public void ChangeTitleType(int titleId, VideoTitle.TitleType type)
        {
            repository.GetTitle(titleId).Type = type;
        }

        /// <summary>
        /// Gets the list of all registered video titles.
        /// </summary>
        /// <returns>The list of all video titles.</returns>
        public List<VideoTitle> GetAllTitles()
        {
            return repository.GetAllTitles();
        }

        /// <summary>
        /// Registers the rental of the casette. Adds bonus points for the rental to the customer.
        /// </summary>
        /// <param name="rental">The rental data.</param>
        public void RentCasette(Rental rental)
        {
            repository.SaveRental(rental);

            var customer = rental.Customer;
            int storedBonus = customer.BonusPoints;
            int bonusForRental = pricePolicy.CalculateBonus(rental.Casette.Title.Type, rental.RentalDays);
            customer.BonusPoints = storedBonus + bonusForRental - rental.BonusPointsPayed;

            repository.UpdateCustomer(customer);
        }

        /// <summary>
        /// Registers the end of the rental of the casette and its returning to the store.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="casetteId">ID of the returning casette.</param>
        public void ReturnCasette(int customerId, int casetteId)
        {
            repository.ReturnCasette(customerId, casetteId);
        }

        /// <summary>
        /// Returns all viteo title available for rental (the titles for which there is at least one casette in the store).
        /// </summary>
        /// <returns>All video titles available for rental.</returns>
        public List<VideoTitle> GetAllTitlesOnShelf()
        {
            return repository.GetAllTitlesOnShelf();
        }

        /// <summary>
        /// Creates a new customer of the video rental store.
        /// </summary>
        /// <returns>ID of the created customer.</returns>
        public int CreateCustomer()
        {
            return repository.CreateCustomer();
        }

        /// <summary>
        /// Return the options for the rental of the film - price and possibility to pay with bonus points.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="titleType">the type of the film.</param>
        /// <param name="dayCount">the number of days to rent.</param>
        /// <returns>Rental options for the customer and the video title.</returns>
        public RentalOptions GetRentalOptions(int customerId, VideoTitle.TitleType titleType, int dayCount)
        {
            int bonusPoints = repository.GetCustomer(customerId).BonusPoints;
            return pricePolicy.CalculateRentalOptions(titleType, dayCount, bonusPoints);
        }

        /// <summary>
        /// Returns the customer by their ID.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <returns>The customer having this ID.</returns>
        public Customer GetCustomer(int customerId)
        {
            return repository.GetCustomer(customerId);
        }

        /// <summary>
        /// Returns the list of the casettes rented currently by the customer.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <returns>The list of rented casettes.</returns>
        public List<Rental> GetActiveRentals(int customerId)
        {
            return repository.GetActiveRentals(customerId);
        }

        /// <summary>
        /// Returns the list of all casettes currently rented.
        /// </summary>
        /// <returns>The list of all rented casettes.</returns>
        public List<Rental> GetAllActiveRentals()
        {
            return repository.GetAllActiveRentals();
        }
    }
}
