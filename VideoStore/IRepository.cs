using System.Collections.Generic;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStore
{
    /// <summary>
    /// Repository to store video rental data.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Adds a video title to the repository.
        /// </summary>
        /// <param name="title">The title to be added.</param>
        /// <returns>ID of the title added.</returns>
        int AddTitle(VideoTitle title);

        /// <summary>
        /// Removes a video title from the repository by its ID.
        /// </summary>
        /// <param name="id">ID of the title to be removed.</param>
        void RemoveTitle(int id);

        /// <summary>
        /// Retrieves the video title by its ID.
        /// </summary>
        /// <param name="id">ID of the title to get.</param>
        /// <returns>Video title with the given ID.</returns>
        /// <exception cref="TitleNotFoundException">If the title with this ID does not exist.</exception>
        VideoTitle GetTitle(int id);

        /// <summary>
        /// Searches for the video title by its name and release year.
        /// </summary>
        /// <param name="name">The name of the video title.</param>
        /// <param name="year">The year of release of the video title.</param>
        /// <returns>List of video titles conforming the search criteria.</returns>
        List<VideoTitle> FindTitle(string name, int year);

        /// <summary>
        /// Returns all video titles stored in the repository.
        /// </summary>
        /// <returns>List of all video titles.</returns>
        List<VideoTitle> GetAllTitles();

        /// <summary>
        /// Returns all video titles for which there is at least one casette available for rent.
        /// </summary>
        /// <returns>List of all video titles which can be rented at the moment.</returns>
        List<VideoTitle> GetAllTitlesOnShelf();

        /// <summary>
        /// Adds a new casette to the shelf.
        /// </summary>
        /// <param name="title">The title of the movie on the casette.</param>
        /// <returns>ID of the new casette.</returns>
        int AddCasette(VideoTitle storedTitle);

        /// <summary>
        /// Removes casette from the store.
        /// </summary>
        /// <param name="id">Casette ID.</param>
        void RemoveCasette(int id);

        /// <summary>
        /// Retrieves the casette with the given ID.
        /// </summary>
        /// <param name="id">Casette ID.</param>
        /// <returns>The casette with the given ID.</returns>
        Casette GetCasette(int id);

        /// <summary>
        /// Searches for casette with the given title available for rent.
        /// </summary>
        /// <param name="titleId">ID of the video title.</param>
        /// <returns>First found casette or <c>null</c> if nothing is found.</c></returns>
        /// <exception cref="CasetteNotFoundException">If no casette available for rent of this title is found.</exception>
        Casette GetCasetteOnShelfByTitle(int titleId);

        /// <summary>
        /// Returns all casettes having the given title.
        /// </summary>
        /// <param name="titleId">ID of the video title.</param>
        /// <returns>List of all casettes having the same title.</returns>
        List<Casette> GetAllCasettesByTitle(int titleId);

        /// <summary>
        /// Creates a new customer to the system.
        /// </summary>
        /// <returns>ID of the new created customer.</returns>
        int CreateCustomer();

        /// <summary>
        /// Retrieves the customer by their ID.
        /// </summary>
        /// <param name="id">ID of the customer.</param>
        /// <returns>The customer with the given ID.</returns>
        /// <exception cref="CustomerNotFoundException">If the customer with this ID does not exist.</exception>
        Customer GetCustomer(int id);

        /// <summary>
        /// Saves the new data for the customer.
        /// </summary>
        /// <param name="customer">The customer object with all data needed for saving.</param>
        /// <exception cref="CustomerNotFoundException">If the customer with this ID does not exist.</exception>
        void UpdateCustomer(Customer customer);

        /// <summary>
        /// Records a rental of the video casette by the customer.
        /// </summary>
        /// <param name="rental">Rental object with all data about the rental.</param>
        void SaveRental(Rental rental);

        /// <summary>
        /// Records that the customer has returned the video casette.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="casetteId">ID of the casette.</param>
        void ReturnCasette(int customerId, int casetteId);

        /// <summary>
        /// Returns the information about all casettes rented by the customer.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <returns>List of all rentals of the given customer.</returns>
        List<Rental> GetActiveRentals(int customerId);

        /// <summary>
        /// Returns the information about all casettes rented currently.
        /// </summary>
        /// <returns>List of all currently rented casettes and the rental terms.</returns>
        List<Rental> GetAllActiveRentals();
    }
}
