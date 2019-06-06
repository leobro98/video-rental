using System.Collections.Generic;
using System.Linq;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStore
{
    public class Store
    {
        private IRepository repository;
        private IPricePolicy pricePolicy;

        public Store(IRepository repository, IPricePolicy pricePolicy)
        {
            this.repository = repository;
            this.pricePolicy = pricePolicy;
        }

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

        public Casette GetCasetteOnShelfByTitle(int titleId)
        {
            return repository.GetCasetteOnShelfByTitle(titleId);
        }

        public VideoTitle FindTitle(string name, int year)
        {
            return repository.FindTitle(name, year).FirstOrDefault();
        }

        public void RemoveTitle(int id)
        {
            foreach (var casette in repository.GetAllCasettesByTitle(id))
            {
                repository.RemoveCasette(casette.Id);
            }
            repository.RemoveTitle(id);
        }

        public void ChangeTitleType(int titleId, VideoTitle.TitleType type)
        {
            repository.GetTitle(titleId).Type = type;
        }

        public List<VideoTitle> GetAllTitles()
        {
            return repository.GetAllTitles();
        }

        public void RentCasette(int casetteId, RentalOptions options, int customerId)
        {
            repository.SaveRental(new Rental(customerId, casetteId, options));

            var customer = repository.GetCustomer(customerId);
            int storedBonus = customer.BonusPoints;
            int bonusForRental = pricePolicy.CalculateBonus(options.TitleType, options.RentalDays);
            customer.BonusPoints = storedBonus + bonusForRental - options.BonusPointsPayed;

            repository.UpdateCustomer(customer);
        }

        public void ReturnCasette(int customerId, int casetteId)
        {
            repository.ReturnCasette(customerId, casetteId);
        }

        public List<VideoTitle> GetAllTitlesOnShelf()
        {
            return repository.GetAllTitlesOnShelf();
        }

        public int CreateCustomer()
        {
            return repository.CreateCustomer(new Customer());
        }

        public RentalOptions GetRentalOptions(int customerId, VideoTitle.TitleType titleType, int dayCount)
        {
            int bonusPoints = repository.GetCustomer(customerId).BonusPoints;
            return pricePolicy.GetRentalOptions(titleType, dayCount, bonusPoints);
        }

        public int GetCustomerBonusPoints(int customerId)
        {
            return repository.GetCustomer(customerId).BonusPoints;
        }

        public List<ActiveRental> GetActiveRentals(int customerId)
        {
            return repository.GetActiveRentals(customerId)
                .Select(x => new ActiveRental()
                {
                    CustomerId = x.CustomerId,
                    RentedCasette = repository.GetCasette(x.CasetteId),
                    Options = new RentalOptions(x.TitleType, x.RentalDays, 0, true, x.Price)
                })
                .ToList();
        }

        public List<ActiveRental> GetAllActiveRentals()
        {
            return repository.GetAllActiveRentals()
                .Select(x => new ActiveRental()
                {
                    CustomerId = x.CustomerId,
                    RentedCasette = repository.GetCasette(x.CasetteId),
                    Options = new RentalOptions(x.TitleType, x.RentalDays, 0, true, x.Price)
                })
                .ToList();
        }
    }
}
