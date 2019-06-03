using Leobro.VideoStore;
using Leobro.VideoStore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Leobro.VideoStoreTest
{
    [TestClass]
    public class RepositoryTest
    {
        private RepositorySpy repo;
        private TestHelper helper;

        [TestInitialize]
        public void TestInitialize()
        {
            repo = new RepositorySpy();
            helper = new TestHelper(repo);
        }

        [TestMethod]
        public void When_AddTitle_Then_TitleExists()
        {
            var title = helper.GetBrandNewTitle();
            repo.AddTitle(title);

            Assert.AreEqual(title, repo.Titles[0]);
        }

        [TestMethod]
        public void When_RemoveTitle_Then_TitleIsRemoved()
        {
            var title = helper.GetBrandNewTitle();
            repo.Titles.Add(title);

            repo.RemoveTitle(title.Id);

            Assert.AreEqual(0, repo.Titles.Count);
        }

        [TestMethod]
        public void When_GetTitle_Then_TitleIsReturned()
        {
            var title = helper.GetBrandNewTitle();
            repo.Titles.Add(title);

            var gotTitle = repo.GetTitle(title.Id);

            Assert.AreEqual(title.Id, gotTitle.Id);
        }

        [TestMethod]
        public void When_FindTitle_Then_TitleIsFound()
        {
            var title = helper.GetBrandNewTitle();
            repo.Titles.Add(title);

            var foundTitles = repo.FindTitle(title.Name, title.Year);

            Assert.AreEqual(1, foundTitles.Count);
            Assert.AreEqual(foundTitles[0].Id, title.Id);
        }

        [TestMethod]
        public void When_GetAllTitles_Then_AllTypesAreListed()
        {
            var expectedTitles = new List<VideoTitle>();
            expectedTitles.Add(helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.OnShelf));
            expectedTitles.Add(helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.OnShelf));
            expectedTitles.Add(helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.Rented));

            var titles = repo.GetAllTitles();

            CollectionAssert.AreEqual(expectedTitles, titles);
        }

        [TestMethod]
        public void When_GetAllTitlesOnShelf_Then_ReturnsOnlyThatHasAnyCasetteOnShelf()
        {
            var expectedTitles = new List<VideoTitle>();
            expectedTitles.Add(helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.OnShelf));
            expectedTitles.Add(helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.OnShelf));

            var title = helper.GetBrandNewTitle();
            repo.Titles.Add(title);
            var casette = new Casette(repo.Casettes.Count + 1, title);
            repo.Casettes.Add(casette);
            repo.Rentals.Add(new Rental(1, casette.Id, CreateEmptyOptions()));

            var titles = repo.GetAllTitlesOnShelf();

            CollectionAssert.AreEqual(expectedTitles, titles);
        }

        [TestMethod]
        public void When_AddCasette_Then_CasetteWithCorrectStatusIsAdded()
        {
            var title = helper.GetBrandNewTitle();

            int idOnShelf = repo.AddCasette(title, Casette.CasetteStatus.OnShelf);
            int idRented = repo.AddCasette(title, Casette.CasetteStatus.Rented);

            Assert.AreEqual(Casette.CasetteStatus.OnShelf, repo.Casettes.Find(x => x.Id == idOnShelf).Status);
            Assert.AreEqual(Casette.CasetteStatus.Rented, repo.Casettes.Find(x => x.Id == idRented).Status);
        }

        [TestMethod]
        public void When_RemoveCasette_Then_CasetteIsRemoved()
        {
            var casette = new Casette(repo.Casettes.Count + 1, helper.GetBrandNewTitle());
            repo.Casettes.Add(casette);

            repo.RemoveCasette(casette.Id);

            Assert.AreEqual(0, repo.Casettes.Count);
        }

        [TestMethod]
        public void When_GetCasette_Then_CorrectCasetteIsReturned()
        {
            var casette = new Casette(repo.Casettes.Count + 1, helper.GetBrandNewTitle());
            repo.Casettes.Add(casette);

            var found = repo.GetCasette(casette.Id);

            Assert.AreEqual(casette.Id, found.Id);
        }

        [TestMethod]
        public void When_GetCasetteOnShelfByTitle_Then_CasetteWithCorrectTitleAndStatusReturned()
        {
            var wrongTitle = helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.OnShelf);
            var title = helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.Rented);
            repo.Casettes.Add(CreateOnShelfCasette(title));

            var casette = repo.GetCasetteOnShelfByTitle(title.Id);

            Assert.AreEqual(title.Id, casette.Title.Id);
            Assert.AreEqual(Casette.CasetteStatus.OnShelf, casette.Status);
        }

        [TestMethod]
        public void When_GetAllCasettesByTitle_Then_ReturnedOnlyWithTheTitle()
        {
            var wrongTitle = helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.OnShelf);
            var title = helper.AddNewTestTitleWithOneCasette(Casette.CasetteStatus.Rented);
            repo.Casettes.Add(CreateOnShelfCasette(title));

            var casettes = repo.GetAllCasettesByTitle(title.Id);

            Assert.AreEqual(2, casettes.Count);
            Assert.AreSame(title, casettes[0].Title);
            Assert.AreSame(title, casettes[1].Title);
        }

        [TestMethod]
        public void When_ChangeCasetteStatus_Then_HitsCorrectCasette()
        {
            var title = helper.GetBrandNewTitle();
            repo.Casettes.Add(CreateOnShelfCasette(title));
            var casette = CreateOnShelfCasette(title);
            repo.Casettes.Add(casette);

            repo.ChangeCasetteStatus(casette.Id, Casette.CasetteStatus.Rented);

            Assert.AreEqual(Casette.CasetteStatus.Rented, casette.Status);
        }

        [TestMethod]
        public void When_GetCustomer_Then_ReturnCorrectCustomer()
        {
            repo.Customers.Add(new Customer(repo.Customers.Count + 1));
            var customer = new Customer(repo.Customers.Count + 1);
            repo.Customers.Add(customer);

            var foundCustomer = repo.GetCustomer(customer.Id);

            Assert.AreEqual(customer.Id, foundCustomer.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(CustomerNotFoundException))]
        public void When_GetCustomer_And_CustomerNotFound_Then_ThrowsException()
        {
            var customer = new Customer(repo.Customers.Count + 1);
            repo.Customers.Add(customer);

            repo.GetCustomer(customer.Id + 1);
        }

        [TestMethod]
        public void When_UpdateCustomer_Then_BonusPointsAreUpdated()
        {
            repo.Customers.Add(new Customer(1));
            var points = 42;
            var customer = new Customer(1);
            customer.BonusPoints = points;

            repo.UpdateCustomer(customer);

            Assert.AreEqual(points, repo.Customers[0].BonusPoints);
        }

        [TestMethod]
        public void When_SaveRental_Then_RentalIsSaved()
        {
            Rental rental = CreateCustomerAndRental();

            repo.SaveRental(rental);

            Assert.AreSame(rental, repo.Rentals[0]);
        }

        [TestMethod]
        public void When_ReturnCasette_Then_RentalIsNoMoreActive()
        {
            Rental rental = CreateCustomerAndRental();
            repo.Rentals.Add(rental);

            repo.ReturnCasette(rental.CustomerId, rental.CasetteId);

            Assert.AreEqual(0, repo.Rentals.Count(
                x => x.CustomerId == rental.CustomerId
                && x.CasetteId == rental.CasetteId
                && x.IsActive == true));
        }

        [TestMethod]
        public void When_GetActiveRentals_Then_OnlyActiveAndOnlyForCustomerReturned()
        {
            Rental irrelevantRental = CreateCustomerAndRental();
            repo.Rentals.Add(irrelevantRental);
            Rental rental1 = CreateCustomerAndRental();
            repo.Rentals.Add(rental1);
            int customerId = rental1.CustomerId;
            var rental2 = new Rental(customerId, rental1.CasetteId + 1, CreateEmptyOptions());
            rental2.IsActive = false;
            repo.Rentals.Add(rental2);

            var rentals = repo.GetActiveRentals(customerId);

            Assert.AreEqual(1, rentals.Count);
            Assert.AreEqual(customerId, rentals[0].CustomerId);
        }

        [TestMethod]
        public void When_GetAllActiveRentals_Then_AllActiveReturned()
        {
            Rental rental1 = CreateCustomerAndRental();
            repo.Rentals.Add(rental1);
            Rental rental2 = CreateCustomerAndRental();
            repo.Rentals.Add(rental2);
            Rental rental3 = new Rental(rental2.CustomerId, rental2.CasetteId + 1, CreateEmptyOptions());
            rental3.IsActive = false;
            repo.Rentals.Add(rental3);

            var rentals = repo.GetAllActiveRentals();

            Assert.AreEqual(2, rentals.Count);
        }

        [TestMethod]
        public void When_CreateCustomer_Then_CustomerExists()
        {
            Customer customer = new Customer(repo.Customers.Count + 1);
            repo.CreateCustomer(customer);

            Assert.AreEqual(1, repo.Customers.Count);
            Assert.AreEqual(customer.Id, repo.Customers[0].Id);
        }

        private Casette CreateOnShelfCasette(VideoTitle title)
        {
            Casette casette = new Casette(repo.Casettes.Count + 1, title);
            casette.Status = Casette.CasetteStatus.OnShelf;
            return casette;
        }

        private Rental CreateCustomerAndRental()
        {
            var customer = new Customer(repo.Customers.Count + 1);
            repo.Customers.Add(customer);
            var rental = new Rental(customer.Id, 1, CreateEmptyOptions());
            return rental;
        }

        private static RentalOptions CreateEmptyOptions()
        {
            return new RentalOptions(VideoTitle.TitleType.New, 1, 0, false, 0);
        }
    }
}
