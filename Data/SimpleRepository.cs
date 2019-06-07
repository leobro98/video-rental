using System.Collections.Generic;
using System.Linq;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStore.Data
{
    public class SimpleRepository : IRepository
    {
        protected List<VideoTitle> titles;
        protected List<Casette> casettes;
        protected List<Customer> customers;
        protected List<Rental> rentals;

        public SimpleRepository()
        {
            casettes = new List<Casette>();
            titles = new List<VideoTitle>();
            customers = new List<Customer>();
            rentals = new List<Rental>();
        }

        public int AddTitle(VideoTitle title)
        {
            int id = GetNextTitleId();
            var persistedTitle = new VideoTitle(title, id);
            titles.Add(persistedTitle);
            return id;
        }

        public void RemoveTitle(int id)
        {
            titles.RemoveAll(x => x.Id == id);
        }

        public VideoTitle GetTitle(int id)
        {
            return titles
                .FirstOrDefault(x => x.Id == id);
        }

        public List<VideoTitle> FindTitle(string name, int year)
        {
            return titles
                .Where(x => x.Name == name && x.Year == year)
                .ToList();
        }

        public List<VideoTitle> GetAllTitles()
        {
            return titles;
        }

        public List<VideoTitle> GetAllTitlesOnShelf()
        {
            if (titles.Count == 0)
            {
                return titles;
            }
            return titles.Join(
                casettes.Where(casette =>
                    !rentals.Any(rental =>
                        rental.Casette.Id == casette.Id
                        && rental.IsActive)),
                title => title.Id,
                casette => casette.Title.Id,
                (title, casette) => title)
                .ToList();
        }

        public int AddCasette(VideoTitle title)
        {
            int id = GetNextCasetteId();
            var casette = new Casette(id, title);
            casettes.Add(casette);
            return id;
        }

        public void RemoveCasette(int id)
        {
            casettes.RemoveAll(x => x.Id == id);
        }

        public Casette GetCasette(int id)
        {
            return casettes
                .FirstOrDefault(x => x.Id == id);
        }

        public Casette GetCasetteOnShelfByTitle(int titleId)
        {
            return casettes
                .FirstOrDefault(casette => 
                    casette.Title.Id == titleId &&
                    !rentals.Any(rental =>
                        rental.Casette.Id == casette.Id
                        && rental.IsActive));
        }

        public List<Casette> GetAllCasettesByTitle(int titleId)
        {
            if (casettes.Count == 0)
            {
                return casettes;
            }
            return casettes
                .Where(x => x.Title.Id == titleId)
                .ToList();
        }

        public Customer GetCustomer(int id)
        {
            var customer = customers
                .FirstOrDefault(x => x.Id == id);

            if (customer == null)
            {
                throw new CustomerNotFoundException();
            }
            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            var foundCustomer = customers.FirstOrDefault(x => x.Id == customer.Id);

            if (foundCustomer == null)
            {
                throw new CustomerNotFoundException();
            }
            foundCustomer.BonusPoints = customer.BonusPoints;
        }

        public void SaveRental(Rental rental)
        {
            rentals.Add(rental);
        }

        public void ReturnCasette(int customerId, int casetteId)
        {
            rentals
                .First(x => x.Customer.Id == customerId &&
                    x.Casette.Id == casetteId)
                .IsActive = false;
        }

        public List<Rental> GetActiveRentals(int customerId)
        {
            if (rentals.Count == 0)
            {
                return rentals;
            }
            return rentals
                .Where(x => x.Customer.Id == customerId && x.IsActive)
                .ToList();
        }

        public List<Rental> GetAllActiveRentals()
        {
            return rentals
                .Where(x => x.IsActive)
                .ToList();
        }

        public int CreateCustomer()
        {
            int id = GetNextCustomerId();
            customers.Add(new Customer(id));
            return id;
        }

        private int GetNextCasetteId()
        {
            if (casettes.Count == 0)
            {
                return 1;
            }
            return casettes.Max(x => x.Id) + 1;
        }

        private int GetNextTitleId()
        {
            if (titles.Count == 0)
            {
                return 1;
            }
            return titles.Max(x => x.Id) + 1;
        }

        private int GetNextCustomerId()
        {
            if (customers.Count == 0)
            {
                return 1;
            }
            return customers.Max(x => x.Id) + 1;
        }
    }
}
