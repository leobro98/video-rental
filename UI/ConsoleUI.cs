using System;
using System.Collections.Generic;
using System.Linq;
using Leobro.VideoStore.Data;
using Leobro.VideoStore.Model;
using Leobro.VideoStore.Price;

namespace Leobro.VideoStore.UI
{
    public class ConsoleUI
    {
        private static Store store;
        private static int customerId;

        private static List<Terms> allTerms = new List<Terms>() {
            new Terms() {
                TitleType = VideoTitle.TitleType.New,
                FlatPeriodDays = 0,
                FlatPeriodFee = 0,
                TrailingFee = 40,
                IsPaymentByPointsAllowed = true,
                RentalDayPriceInPoints = 25,
                PointsForRent = 2
            },
            new Terms()
            {
                TitleType = VideoTitle.TitleType.Regular,
                FlatPeriodDays = 3,
                FlatPeriodFee = 30,
                TrailingFee = 30,
                IsPaymentByPointsAllowed = false,
                RentalDayPriceInPoints = 0,
                PointsForRent = 1
            },
            new Terms()
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

            ShowGreeting();

            var titles = store.GetAllTitlesOnShelf();
            while (titles.Count > 0)
            {
                ListTitles(titles);
                bool wasRented = MakeDeal();
                if (wasRented)
                {
                    ShowActiveRentals();
                }
                titles = store.GetAllTitlesOnShelf();
            }
            ShowGoodBye();
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

            var title = new VideoTitle("Out of Africa", 1985, VideoTitle.TitleType.Old);
            RentTitle(title, 7);
            title = new VideoTitle("Spider-Man", 2002, VideoTitle.TitleType.Regular);
            RentTitle(title, 5);
            title = new VideoTitle("Spider-Man 3", 2007, VideoTitle.TitleType.Regular);
            RentTitle(title, 2);

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
            int id = store.AddCasette(title);
            var terms = store.GetRentalTerms(customerId, title.Type, days);
            store.RentCasette(id, terms, customerId);
        }

        private static void ShowGreeting()
        {
            Console.WriteLine("Welcome to our video store! Your customer ID is {0}", customerId);
        }

        private static void ListTitles(List<VideoTitle> titles)
        {
            Console.WriteLine("We have the following movies available for rental:");
            Console.WriteLine();
            foreach (var title in titles)
            {
                Console.WriteLine("({0}) {1}, {2}", title.Id, title.Name, title.Year);
            }
            Console.WriteLine();
        }

        private static bool MakeDeal()
        {
            int titleId = ReadInteger("Please enter the movie ID to rent and press Enter (or bare Enter to exit):");
            int dayCount = ReadInteger("Enter the number of days and press Enter:");
            if (dayCount <= 0)
            {
                Console.WriteLine("Wrong number");
                return false;
            }

            var casette = store.GetCasetteOnShelfByTitle(titleId);
            var terms = store.GetRentalTerms(customerId, casette.Title.Type, dayCount);
            Console.WriteLine("You have selected: {0}, {1} ({2}) {3} days {4} Eur",
                casette.Title.Name, casette.Title.Year, terms.TitleType, terms.RentalDays, terms.Price);
            Console.WriteLine("Press y to accept the terms:");
            string decision = Console.ReadLine();
            Console.WriteLine();
            
            bool success = false;
            if (decision.ToLower() == "y")
            {
                success = true;
                store.RentCasette(casette.Id, terms, customerId);
            }
            return success;
        }

        private static void ShowActiveRentals()
        {
            Console.WriteLine("You are currently renting:");
            Console.WriteLine();
            foreach (var rental in store.GetActiveRentals(customerId))
            {
                Console.WriteLine("{0} ({1}) {2} days {3} Eur",
                    rental.RentedCasette.Title.Name, rental.RentedCasette.Title.Type, rental.Terms.RentalDays, rental.Terms.Price);
            }
            Console.WriteLine();
        }

        private static int ReadInteger(string prompt)
        {
            Console.WriteLine(prompt);
            bool success = false;
            int parsed = 0;

            while (!success)
            {
                string entered = Console.ReadLine();
                if (entered == string.Empty)
                {
                    Environment.Exit(1);
                }
                success = int.TryParse(entered, out parsed);
                if (!success)
                {
                    Console.WriteLine("Incorrect number, please repeat (press Enter to exit):");
                }
            }
            return parsed;
        }

        private static void ShowGoodBye()
        {
            Console.WriteLine();
            Console.WriteLine("No more movies. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
