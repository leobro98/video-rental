using System.Collections.Generic;
using Leobro.VideoStore;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStoreTest
{
    class RepositoryMock : IRepository
    {
        public Casette LastAddedCasette { get; set; }

        public int AddCasette(VideoTitle storedTitle, Casette.CasetteStatus status)
        {
            LastAddedCasette = new Casette(0, storedTitle);
            return 0;
        }

        public List<VideoTitle> FindTitle(string name, int year)
        {
            var titles = new List<VideoTitle>();
            titles.Add(new VideoTitle(name, year));
            return titles;
        }

        public VideoTitle GetTitle(int titleId)
        {
            return null;
        }

        public int AddTitle(VideoTitle title)
        {
            return 0;
        }

        public Casette GetCasetteOnShelfByTitle(int titleId)
        {
            return null;
        }

        public List<Casette> GetAllCasettesByTitle(int titleId)
        {
            return null;
        }

        public void RemoveCasette(int casetteId) { }

        public void RemoveTitle(int titleId) { }

        public List<VideoTitle> GetAllTitles()
        {
            return null;
        }

        public void ChangeCasetteStatus(int id, Casette.CasetteStatus casetteState) { }

        public List<VideoTitle> GetAllTitlesOnShelf()
        {
            return null;
        }

        public int CustomerBonusPoints { get; set; }

        public Customer GetCustomer(int id)
        {
            return new Customer(0, CustomerBonusPoints);
        }

        public void UpdateCustomer(Customer customer)
        { }

        public void SaveRental(Rental rental)
        { }

        public Casette GetCasette(int casetteId)
        {
            return null;
        }

        public List<Rental> GetActiveRentals(int customerId)
        {
            return null;
        }

        public int CreateCustomer(Customer customer)
        {
            return 0;
        }

        public void ReturnCasette(int customerId, int casetteId)
        { }

        public List<Rental> GetAllActiveRentals()
        {
            return null;
        }
    }
}
