using System;
using System.Collections.Generic;
using System.Linq;
using Leobro.VideoStore.Data;
using Leobro.VideoStore.Model;
using Leobro.VideoStore.Price;

namespace Leobro.VideoStore.UI
{
    /// <summary>
    /// <para>Main class of the application. Combines together other application components. Represents the user
    /// interface, simplest, but sufficient for the rental store management.</para>
    /// <para>Contains configuration for the <see cref="PricePolicy"/> plugin in the form of the <see cref="RentalTerms"/>
    /// list. In real-life application this configuration can come from an external configuration file or from a
    /// database. However, it is left hard-coded here for the sake of simplicity.</para>
    /// <para>The class contains only interaction with the <see cref="Store"/> component, all console output texts
    /// and formatting are delegated to the <see cref="Interactor"/> class.</para>
    /// <para>The repository can be pre-filled with some test data, if "test" command line argument is passed.</para>
    /// </summary>
    public class ConsoleUI
    {
        private static Store store;
        private static int customerId;

        private static List<RentalTerms> allTerms = new List<RentalTerms>() {
            new RentalTerms() {
                TitleType = VideoTitle.TitleType.New,
                FlatPeriodDays = 0,
                FlatPeriodFee = 0,
                TrailingFee = 40,
                IsPaymentByPointsAllowed = true,
                RentalDayPriceInPoints = 25,
                PointsForRent = 2
            },
            new RentalTerms()
            {
                TitleType = VideoTitle.TitleType.Regular,
                FlatPeriodDays = 3,
                FlatPeriodFee = 30,
                TrailingFee = 30,
                IsPaymentByPointsAllowed = false,
                RentalDayPriceInPoints = 0,
                PointsForRent = 1
            },
            new RentalTerms()
            {
                TitleType = VideoTitle.TitleType.Old,
                FlatPeriodDays = 5,
                FlatPeriodFee = 30,
                TrailingFee = 30,
                IsPaymentByPointsAllowed = false,
                RentalDayPriceInPoints = 0,
                PointsForRent = 1
            }
        };

        static void Main(string[] args)
        {
            InitializeStore();
            if (IsTestRun(args))
            {
                InitializeInventory();
            }

            string command;
            do
            {
                command = Interactor.GetMenuCommand();
                ExecuteCommand(command);
            } while (Interactor.IsFinished(command));
        }

        private static void InitializeStore()
        {
            var repository = new SimpleRepository();
            var policy = new PricePolicy(allTerms);
            store = new Store(repository, policy);
        }

        private static bool IsTestRun(string[] args)
        {
            return args.Any(x => x == "test");
        }

        private static void InitializeInventory()
        {
            customerId = store.CreateCustomer();
            var customer = store.GetCustomer(customerId);
            customer.BonusPoints = 100;

            // rented casettes
            VideoTitle title = new VideoTitle("Out of Africa", 1985, VideoTitle.TitleType.Old);
            store.AddCasette(title);
            RentTitle(store.FindTitle(title.Name, title.Year), 7);

            title = new VideoTitle("Spider-Man", 2002, VideoTitle.TitleType.Regular);
            store.AddCasette(title);
            RentTitle(store.FindTitle(title.Name, title.Year), 5);

            title = new VideoTitle("Spider-Man 3", 2007, VideoTitle.TitleType.Regular);
            store.AddCasette(title);
            RentTitle(store.FindTitle(title.Name, title.Year), 2);

            // available casettes
            title = new VideoTitle("Parallels", 2015, VideoTitle.TitleType.New);
            store.AddCasette(title);

            title = new VideoTitle("Casablanka", 1943, VideoTitle.TitleType.Old);
            store.AddCasette(title);

            title = new VideoTitle("District 9", 2009, VideoTitle.TitleType.Regular);
            store.AddCasette(title);

            title = new VideoTitle("Skin Trade", 2014, VideoTitle.TitleType.New);
            store.AddCasette(title);
        }

        private static void RentTitle(VideoTitle title, int days)
        {
            var customer = store.GetCustomer(customerId);
            var casette = store.GetCasetteOnShelfByTitle(title.Id);
            var options = store.GetRentalOptions(customerId, title.Type, days);
            var rental = new Rental(customer, casette)
            {
                RentalDays = days,
                Price = options.Price
            };
            store.RentCasette(rental);
        }

        private static void ExecuteCommand(string command)
        {
            switch (command)
            {
                case Interactor.ADD_FILM_CMD:
                    AddFilm();
                    break;
                case Interactor.ADD_CASETTE_CMD:
                    AddCasette();
                    break;
                case Interactor.REMOVE_FILM_CMD:
                    RemoveFilm();
                    break;
                case Interactor.CHANGE_TYPE_CMD:
                    ChangeFilmType();
                    break;
                case Interactor.ALL_FILMS_CMD:
                    ListAllFilms();
                    break;
                case Interactor.ALL_ACTIVE_RENTALS_CMD:
                    ListAllAciveRentals();
                    break;
                case Interactor.CREATE_CUSTOMER_CMD:
                    CreateCustomer();
                    break;
                case Interactor.ALL_CUSTOMER_RENTALS_CMD:
                    ListCustomerRentals();
                    break;
                case Interactor.ALL_FILMS_AVAILABLE_CMD:
                    ListFilmsAvailable();
                    break;
                case Interactor.RENT_FILM_CMD:
                    RentFilm();
                    break;
                case Interactor.RETURN_CASETTE_CMD:
                    ReturnCasette();
                    break;
                case Interactor.QUIT_CMD:
                    // do nothing, then exit the main menu loop
                    break;
            }
        }

        /// <summary>
        /// Adds a new video title to the repository.
        /// </summary>
        private static void AddFilm()
        {
            string name = Interactor.AddFilm.GetFilmName();
            int year = Interactor.AddFilm.GetYear();
            var title = new VideoTitle(name, year);

            int casetteCount = Interactor.AddFilm.GetCasetteCount();
            for (int i = 0; i < casetteCount; i++)
            {
                store.AddCasette(title);
            }
        }

        /// <summary>
        /// Adds an additional casette for an existing video title.
        /// </summary>
        private static void AddCasette()
        {

            string name = Interactor.FindFilm.GetFilmName(Interactor.ADD_CASETTE_HEADING);
            int year = Interactor.FindFilm.GetYear();
            var title = store.FindTitle(name, year);

            if (title == null)
            {
                Interactor.FindFilm.ShowFilmNotFound();
            }
            else
            {
                store.AddCasette(title);
            }
        }

        /// <summary>
        /// Removes a video title and all its casettes from the repository.
        /// </summary>
        private static void RemoveFilm()
        {
            string name = Interactor.FindFilm.GetFilmName(Interactor.REMOVE_FILM_HEADING);
            int year = Interactor.FindFilm.GetYear();
            var title = store.FindTitle(name, year);

            if (title == null)
            {
                Interactor.FindFilm.ShowFilmNotFound();
            }
            else
            {
                store.RemoveTitle(title.Id);
            }
        }

        /// <summary>
        /// Changes the type of a registered video title.
        /// </summary>
        private static void ChangeFilmType()
        {
            string name = Interactor.FindFilm.GetFilmName(Interactor.CHANGE_FILM_TYPE_HEADING);
            int year = Interactor.FindFilm.GetYear();
            var title = store.FindTitle(name, year);

            if (title == null)
            {
                Interactor.FindFilm.ShowFilmNotFound();
            }
            else
            {
                string type = Interactor.ChangeFilmType.GetFilmType();
                var titleType = (VideoTitle.TitleType) Enum.Parse(typeof(VideoTitle.TitleType), type, true);
                store.ChangeTitleType(title.Id, titleType);
            }
        }

        /// <summary>
        /// Displays the list of all registered films.
        /// </summary>
        private static void ListAllFilms()
        {
            var films = store.GetAllTitles();
            Interactor.FilmList.ShowList(films, Interactor.ALL_FILMS_HEADING);
        }

        /// <summary>
        /// Displays the list of all rented casettes.
        /// </summary>
        private static void ListAllAciveRentals()
        {
            var rentals = store.GetAllActiveRentals();
            Interactor.RentalList.ShowList(rentals, Interactor.ALL_ACTIVE_RENTALS_HEADING);
        }

        /// <summary>
        /// Creates a new customer of the rental store.
        /// </summary>
        private static void CreateCustomer()
        {
            // according to business requirements, customer doesn't have any personal information, their only attribute is ID
            int id = store.CreateCustomer();
            Interactor.CreateCustomer.ShowCustomer(id);
        }

        /// <summary>
        /// Displays the list of all casettes rented at the moment by the specified customer.
        /// </summary>
        private static void ListCustomerRentals()
        {
            var customerId = Interactor.ListCustomerRentals.GetCustomerId();
            var rentals = store.GetActiveRentals(customerId);
            Interactor.RentalList.ShowList(rentals, Interactor.ALL_CUSTOMER_RENTALS_HEADING);
        }

        /// <summary>
        /// Displays the list of all films available for rental, i.e. video titles for which there is at least
        /// one casette not rented at the moment.
        /// </summary>
        private static void ListFilmsAvailable()
        {
            var films = store.GetAllTitlesOnShelf();
            Interactor.FilmList.ShowList(films, Interactor.ALL_FILMS_AVAILABLE_HEADING);
        }

        /// <summary>
        /// Manages the whole workflow for a film rental. This is quite spacy workflow, that is why
        /// it is moved to a separate class.
        /// </summary>
        private static void RentFilm()
        {
            new FilmRenter(store).Rent();
        }

        /// <summary>
        /// Allows to accept a returned casette from a customer.
        /// </summary>
        private static void ReturnCasette()
        {
            // both IDs can be known from the list of rented casettes
            int customerId = Interactor.ReturnCasette.GetCustomerId();
            int casetteId = Interactor.ReturnCasette.GetCasetteId();
            store.ReturnCasette(customerId, casetteId);
            Interactor.ReturnCasette.ShowConfirmation();
        }
    }
}
