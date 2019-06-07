using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Leobro.VideoStore;
using Leobro.VideoStore.Model;

namespace Leobro.VideoStoreTest
{
    [TestClass]
    public class VideoStoreTest
    {
        private RepositorySpy repo;
        private Store store;
        private TestHelper helper;

        [TestInitialize]
        public void TestInitialize()
        {
            repo = new RepositorySpy();
            store = new Store(repo, new PricePolicyStub());
            helper = new TestHelper(repo);
        }

        [TestMethod]
        public void AddedCasetteShouldBeInRepository()
        {
            var title = helper.GetBrandNewTitle();
            repo.Titles.Add(title);

            store.AddCasette(title);

            Assert.AreEqual(title.Name, repo.Casettes[0].Title.Name);
            Assert.AreEqual(title.Year, repo.Casettes[0].Title.Year);
        }

        [TestMethod]
        public void AddingCasseteWithNewTitleShouldAddTitle()
        {
            var title = helper.GetBrandNewTitle();

            store.AddCasette(title);

            Assert.AreEqual(title.Name, repo.Titles[0].Name);
            Assert.AreEqual(title.Year, repo.Titles[0].Year);
        }

        [TestMethod]
        public void AddedTitleShouldBeNewRelease()
        {
            var title = helper.GetBrandNewTitle();

            store.AddCasette(title);

            Assert.AreEqual(VideoTitle.TitleType.New, repo.Titles[0].Type);
        }

        [TestMethod]
        public void RemovedTitleShouldNotBeInRepository()
        {
            var title = helper.AddNewTestTitleWithOneCasette();
            repo.Casettes.Add(new Casette(2, title));

            store.RemoveTitle(title.Id);

            Assert.AreEqual(0, repo.Titles.Count);
            Assert.AreEqual(0, repo.Casettes.Count);
        }

        [TestMethod]
        public void TitleTypeCanBeChanged()
        {
            var title = helper.AddNewTestTitleWithOneCasette();

            store.ChangeTitleType(title.Id, VideoTitle.TitleType.Old);

            Assert.AreEqual(VideoTitle.TitleType.Old, title.Type);
        }

        [TestMethod]
        public void CustomerCanRentCasette()
        {
            helper.AddNewTestTitleWithOneCasette();
            int casetteId = repo.Casettes[0].Id;
            int customerId = 1;
            Customer customer = new Customer(customerId);
            repo.Customers.Add(customer);

            store.RentCasette(new Rental(customer, repo.Casettes[0])
            {
                RentalDays = 0,
                Price = 0,
                BonusPointsPayed = 0
            });

            Assert.AreEqual(1, repo.Rentals.Count);
            Assert.AreEqual(casetteId, repo.Rentals[0].Casette.Id);
            Assert.AreEqual(customerId, repo.Rentals[0].Customer.Id);
            Assert.AreEqual(PricePolicyStub.BonusPoints, repo.Customers[0].BonusPoints);
        }

        [TestMethod]
        public void CustomerCanReturnCasette()
        {
            helper.AddNewTestTitleWithOneCasette();
            int casetteId = repo.Casettes[0].Id;
            int customerId = 1;
            Customer customer = new Customer(customerId);
            repo.Customers.Add(customer);
            repo.Rentals.Add(helper.CreateRental(customer.Id, repo.Casettes[0]));

            store.ReturnCasette(customerId, casetteId);

            Assert.AreEqual(0, repo.Rentals.Count(x => x.Customer.Id == customerId && x.IsActive));
            Assert.AreEqual(0, repo.Rentals.Count(x => x.Casette.Id == casetteId && x.IsActive));
        }

        [TestMethod]
        public void CustomerCanReviewTheirActiveRentals()
        {
            int customerId = 1;
            Customer customer = new Customer(customerId);
            repo.Customers.Add(customer);

            string name1 = "Matrix 11";
            AddTitleAndRentCasette(customer, name1, 2015, VideoTitle.TitleType.New);

            string name2 = "Spider Man";
            AddTitleAndRentCasette(customer, name2, 2000, VideoTitle.TitleType.Regular);

            string name3 = "Out of Africa";
            AddTitleAndRentCasette(customer, name3, 1975, VideoTitle.TitleType.Old);

            var rentals = store.GetActiveRentals(customerId);

            Assert.AreEqual(3, rentals.Count);
            Assert.IsTrue(rentals.Any(x => x.Casette.Title.Name == name1));
            Assert.IsTrue(rentals.Any(x => x.Casette.Title.Name == name2));
            Assert.IsTrue(rentals.Any(x => x.Casette.Title.Name == name3));
        }

        private void AddTitleAndRentCasette(Customer customer, string name, int year, VideoTitle.TitleType titleType)
        {
            var title = AddTitle(name, year, titleType);
            int casetteId = AddCasette(title);
            AddRental(customer, title, casetteId);
        }

        private VideoTitle AddTitle(string name, int year, VideoTitle.TitleType titleType)
        {
            var title = new VideoTitle(name, year, repo.Titles.Count + 1);
            title.Type = titleType;
            repo.Titles.Add(title);
            return title;
        }

        private int AddCasette(VideoTitle title)
        {
            int casetteId = repo.Casettes.Count + 1;
            repo.Casettes.Add(new Casette(casetteId, title));
            return casetteId;
        }

        private void AddRental(Customer customer, VideoTitle title, int casetteId)
        {
            var casette = new Casette(casetteId, title);
            repo.Rentals.Add(new Rental(customer, casette));
        }
    }
}
