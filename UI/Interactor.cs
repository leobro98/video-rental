using Leobro.VideoStore.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Leobro.VideoStore.UI
{
    /// <summary>
    /// Encapsulates all formatting of UI.
    /// </summary>
    static class Interactor
    {
        public const string ADD_FILM_CMD = "1";
        public const string ADD_CASETTE_CMD = "2";
        public const string REMOVE_FILM_CMD = "3";
        public const string CHANGE_TYPE_CMD = "4";
        public const string ALL_FILMS_CMD = "5";
        public const string ALL_ACTIVE_RENTALS_CMD = "6";
        public const string CREATE_CUSTOMER_CMD = "7";
        public const string ALL_CUSTOMER_RENTALS_CMD = "8";
        public const string ALL_FILMS_AVAILABLE_CMD = "9";
        public const string RENT_FILM_CMD = "10";
        public const string RETURN_CASETTE_CMD = "11";
        public const string QUIT_CMD = "q";

        private const string ADD_FILM_HEADING = "ADDING FILM";
        public const string ADD_CASETTE_HEADING = "ADDING CASETTE";
        public const string REMOVE_FILM_HEADING = "REMOVING FILM";
        public const string CHANGE_FILM_TYPE_HEADING = "CHANGE FILM TYPE";
        public const string ALL_FILMS_HEADING = "ALL FILMS IN STORE";
        public const string ALL_ACTIVE_RENTALS_HEADING = "ALL ACTIVE RENTALS";
        private const string CREATE_CUSTOMER_HEADING = "CREATED CUSTOMER";
        public const string ALL_CUSTOMER_RENTALS_HEADING = "ALL CUSTOMER RENTALS";
        public const string ALL_FILMS_AVAILABLE_HEADING = "ALL FILMS AVAILABLE FOR RENT";
        private const string RENT_FILM_HEADING = "RENT A FILM";
        private const string RETURN_CASETTE_HEADING = "RETURN A CASETTE";

        private const string CONTINUE_PROMPT = "Press Enter to continue ";
        private const int MIN_YEAR = 1888;
        private const int MIN_CUSTOMER_ID = 1;
        private const int MAX_CUSTOMER_ID = 1000;
        private const int MIN_FILM_ID = 1;
        private const int MAX_FILM_ID = 1000;
        private const int MIN_DAYS = 1;
        private const int MAX_DAYS = 14;
        private const int MIN_CASETTE_ID = 1;
        private const int MAX_CASETTE_ID = 1000;

        private static List<string> allCommands = new List<string>() {
            ADD_FILM_CMD,
            ADD_CASETTE_CMD,
            REMOVE_FILM_CMD,
            CHANGE_TYPE_CMD,
            ALL_FILMS_CMD,
            ALL_ACTIVE_RENTALS_CMD,
            ALL_CUSTOMER_RENTALS_CMD,
            CREATE_CUSTOMER_CMD,
            ALL_CUSTOMER_RENTALS_CMD,
            ALL_FILMS_AVAILABLE_CMD,
            RENT_FILM_CMD,
            RETURN_CASETTE_CMD,
            QUIT_CMD
        };

        /// <summary>
        /// Shows main menu and reads a command from the user input, ignores all other input.
        /// </summary>
        /// <returns>The entered command.</returns>
        internal static string GetMenuCommand()
        {
            string input;

            do
            {
                ShowMenu();
                input = ReadCommand().Trim();
            } while (!IsCommand(input));

            return input;
        }

        private static void ShowMenu() {
            Console.Clear();
            Console.WriteLine("Following commands are available:");
            Console.WriteLine();
            Console.WriteLine("Inventory");
            Console.WriteLine("    " + ADD_FILM_CMD + " Add film");
            Console.WriteLine("    " + ADD_CASETTE_CMD + " Add casette");
            Console.WriteLine("    " + REMOVE_FILM_CMD + " Remove film");
            Console.WriteLine("    " + CHANGE_TYPE_CMD + " Change type of film");
            Console.WriteLine("    " + ALL_FILMS_CMD + " All films");
            Console.WriteLine("    " + ALL_ACTIVE_RENTALS_CMD + " All active rentals");
            Console.WriteLine("Service");
            Console.WriteLine("    " + CREATE_CUSTOMER_CMD + " Create customer");
            Console.WriteLine("    " + ALL_CUSTOMER_RENTALS_CMD + " All customer rentals");
            Console.WriteLine("    " + ALL_FILMS_AVAILABLE_CMD + " All available films");
            Console.WriteLine("   " + RENT_FILM_CMD + " Rent film");
            Console.WriteLine("   " + RETURN_CASETTE_CMD + " Return casette");
            Console.WriteLine();
            Console.WriteLine("    " + QUIT_CMD + " Quit");
            Console.WriteLine();
        }

        private static string ReadCommand()
        {
            return GetInput("Choose a command: ");
        }

        /// <summary>
        /// Determines if the user input is one of the menu commands.
        /// </summary>
        /// <param name="input">text entered by user.</param>
        /// <returns><c>true</c> if the entered text is one of the commands; otherwise, <c>false</c>.</returns>
        private static bool IsCommand(string input)
        {
            return allCommands
                .Select(x => x.ToLower())
                .Contains(input.ToLower());
        }

        /// <summary>
        /// Determines if the command is Quit command.
        /// </summary>
        /// <param name="command">one of menu commands.</param>
        /// <returns><c>true</c> if the entered command is quit; otherwise, <c>false</c>.</returns>
        internal static bool IsFinished(string command)
        {
            return command.ToLower() != QUIT_CMD;
        }

        private static string GetInput(string message) {
            Console.Write(message);
            string input = Console.ReadLine();
            Console.WriteLine();
            return input;
        }

        private static bool GetConfirmation(string message, string confirmation) {
            Console.Write(message);
            string input = Console.ReadLine();
            Console.WriteLine();

            if (input.ToLower() == confirmation.ToLower()) {
                return true;
            }
            return false;
        }

        private static int ReadFilmYear(string message)
        {
            int year = ReadInteger("Film year: ", MIN_YEAR, DateTime.Now.Year);
            Console.WriteLine();
            return year;
        }

        private static int ReadInteger(string message, int min, int max)
        {
            int input;
            do
            {
                input = ReadInteger(message);
            } while (input < min || input > max);

            return input;
        }

        private static int ReadInteger(string message)
        {
            Console.Write(message);
            bool success = false;
            int parsed = 0;

            while (!success)
            {
                string entered = Console.ReadLine();
                success = int.TryParse(entered, out parsed);
                if (!success)
                {
                    Console.Write(message);
                }
            }

            return parsed;
        }

        private static void ClearAndWrite(string heading)
        {
            Console.Clear();
            Console.WriteLine(heading);
            Console.WriteLine();
        }

        private static void ShowError(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Pause();
        }

        private static void Pause()
        {
            Console.Write(CONTINUE_PROMPT);
            Console.ReadLine();
        }

        internal static class AddFilm
        {

            internal static string GetFilmName()
            {
                ClearAndWrite(ADD_FILM_HEADING);
                return GetInput("Film name: ");
            }

            internal static int GetYear()
            {
                Console.WriteLine();
                return ReadFilmYear("Film year: ");
            }

            internal static int GetCasetteCount()
            {
                Console.WriteLine();
                return ReadInteger("Number of casettes for this film: ", 1, 99);
            }
        }

        internal static class FindFilm
        {
            internal static string GetFilmName(string heading)
            {
                ClearAndWrite(heading);
                return GetInput("Film name: ");
            }

            internal static int GetYear()
            {
                Console.WriteLine();
                return ReadFilmYear("Film year: ");
            }

            internal static void ShowFilmNotFound()
            {
                ShowError("This film is not found in the store.");
            }
        }

        internal static class ChangeFilmType
        {
            internal static string GetFilmType()
            {
                Console.WriteLine();
                string input;

                do
                {
                    input = GetInput("New film type (new/regular/old): ");
                } while (!IsTitleType(input));

                return input;
            }

            private static bool IsTitleType(string input)
            {
                return Enum.GetNames(typeof(VideoTitle.TitleType))
                    .Any(x => x.Equals(input, StringComparison.OrdinalIgnoreCase));
            }
        }

        internal static class FilmList
        {
            internal static void ShowList(List<VideoTitle> films, string heading)
            {
                ClearAndWrite(heading);
                int maxLength = films.Count > 0
                    ? films.Max(x => x.Name.Length)
                    : 0;

                Console.WriteLine(" {0}  {1,-" + maxLength + "}  {2}  {3,-7}", "ID", "Name", "Year", "Type");
                Console.WriteLine("-----" + new string('-', maxLength) + "--------" + "-------");

                foreach (var title in films)
                {
                    Console.WriteLine("{0,3}  {1,-" + maxLength + "}  {2}  {3}", title.Id, title.Name, title.Year, title.Type);
                }

                Console.WriteLine();
                Pause();
            }
        }

        internal static class RentalList
        {
            internal static void ShowList(List<Rental> rentals, string heading)
            {
                ClearAndWrite(heading);
                int maxNameLength = rentals.Count > 0
                    ? rentals.Max(x => x.Casette.Title.Name.Length)
                    : 0;
                int maxTypeLength = Enum.GetNames(typeof(VideoTitle.TitleType)).Max(x => x.Length);
                decimal total = 0;

                Console.WriteLine("{0,6}  {1,7}  {2,-" + maxNameLength + "}  {3,-"
                    + maxTypeLength + "} {4,7}  {5,7}", "Cas.ID", "Cust.ID", "Name", "Type", "Days", "Price");
                Console.WriteLine("-----------------" + new string('-', maxNameLength) + "--"
                    + new string('-', maxTypeLength) + "-----------------");

                foreach (var rental in rentals)
                {
                    Console.WriteLine("{0,6}  {1,7}  {2,-" + maxNameLength + "}  {3,-"
                        + maxTypeLength + "} {4,2} days  {5,3} Eur",
                        rental.Casette.Id, rental.Customer.Id, rental.Casette.Title.Name, rental.Casette.Title.Type,
                        rental.RentalDays, rental.Price);

                    total += rental.Price;
                }

                Console.WriteLine("---------------------");
                Console.WriteLine("Total price: {0,4} Eur", total);

                Console.WriteLine();
                Pause();
            }
        }

        internal static class CreateCustomer
        {

            internal static void ShowCustomer(int customerId)
            {
                ClearAndWrite(CREATE_CUSTOMER_HEADING);
                Console.WriteLine("Created customer ID: {0}", customerId);
                Console.WriteLine();
                Pause();
            }
        }

        internal static class ListCustomerRentals
        {
            internal static int GetCustomerId()
            {
                ClearAndWrite(ALL_CUSTOMER_RENTALS_HEADING);
                return ReadInteger("Customer ID: ", MIN_CUSTOMER_ID, MAX_CUSTOMER_ID);
            }

        }

        internal static class RentFilm
        {
            internal static int GetCustomerId()
            {
                ClearAndWrite(RENT_FILM_HEADING);
                return ReadInteger("Customer ID: ", MIN_CUSTOMER_ID, MAX_CUSTOMER_ID);
            }

            internal static int GetFilmId()
            {
                Console.WriteLine();
                return ReadInteger("Film ID: ", MIN_FILM_ID, MAX_FILM_ID);
            }

            internal static int GetDays()
            {
                Console.WriteLine();
                return ReadInteger("Number of days for rent: ", MIN_DAYS, MAX_DAYS);
            }

            internal static bool GetConfirmationForTerms(Casette casette, RentalOptions options)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"You have selected:\n" +
                    $"{casette.Title.Name}, {casette.Title.Year} ({options.TitleType}) {options.RentalDays} days ---> {options.Price} Eur");
                Console.WriteLine();
                return GetConfirmation("Agree to rent? (y/n) ", "y");
            }

            internal static bool GetConfirmationForPayingByPoints(RentalOptions options)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"You have {options.PointsAvailable} bonus points accumulated.\n" +
                    $"You can pay {options.PriceInPoints} points for this rental.");
                Console.WriteLine();
                return GetConfirmation("Pay by points? (y/n) ", "y");
            }

            internal static void ShowReceiptWithMoney(Casette casette, RentalOptions options)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Rented:\n" +
                    $"{casette.Title.Name}, {casette.Title.Year} ({casette.Title.Type}) {options.RentalDays} days {options.Price} Eur");
                Console.WriteLine();
                Pause();
            }

            internal static void ShowReceiptWithPoints(Casette casette, RentalOptions options, int bonusPointsRemaining)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Rented:\n" +
                    $"{casette.Title.Name}, {casette.Title.Year} ({casette.Title.Type}) {options.RentalDays} days (Paid with {options.PriceInPoints} bonus points)\n" +
                    $"Remaining bonus points: {bonusPointsRemaining}");
                Console.WriteLine();
                Pause();
            }

            internal static void ShowCustomerNotFound()
            {
                ShowError("This customer is not found in the store.");
            }

            internal static void ShowFilmNotFound()
            {
                ShowError("This film is not found in the store.");
            }

            internal static void ShowCasetteNotFound()
            {
                ShowError("There is no available casette for this film.");
            }
        }

        internal static class ReturnCasette
        {
            internal static int GetCustomerId()
            {
                ClearAndWrite(RETURN_CASETTE_HEADING);
                return ReadInteger("Customer ID: ", MIN_CUSTOMER_ID, MAX_CUSTOMER_ID);
            }

            internal static int GetCasetteId()
            {
                Console.WriteLine();
                return ReadInteger("Casette ID: ", MIN_CASETTE_ID, MAX_CASETTE_ID);
            }

            internal static void ShowConfirmation()
            {
                Console.WriteLine();
                Console.WriteLine("The casette is returned");
                Console.WriteLine();
                Pause();
            }
        }
    }
}
