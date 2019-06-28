using Leobro.VideoStore;
using Leobro.VideoStore.Model;
using Leobro.VideoStore.Price;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Leobro.VideoStoreTest
{
    [TestClass]
    public class PricePolicyTest
    {
        private static readonly RentalTerms newTitleTerms = new RentalTerms()
        {
            TitleType = VideoTitle.TitleType.New,
            FlatPeriodDays = 0,
            FlatPeriodFee = 0,
            TrailingFee = 40,
            IsPaymentByPointsAllowed = true,
            RentalDayPriceInPoints = 25,
            PointsForRent = 2
        };

        private static readonly RentalTerms regularTitleTerms = new RentalTerms()
        {
            TitleType = VideoTitle.TitleType.Regular,
            FlatPeriodDays = 3,
            FlatPeriodFee = 30,
            TrailingFee = 30,
            IsPaymentByPointsAllowed = false,
            RentalDayPriceInPoints = 0,
            PointsForRent = 1
        };
        private static readonly RentalTerms oldTitleTerms = new RentalTerms()
        {
            TitleType = VideoTitle.TitleType.Old,
            FlatPeriodDays = 5,
            FlatPeriodFee = 30,
            TrailingFee = 30,
            IsPaymentByPointsAllowed = false,
            RentalDayPriceInPoints = 0,
            PointsForRent = 1
        };

        private List<RentalTerms> allTerms = new List<RentalTerms>() {
            newTitleTerms,
            regularTitleTerms,
            oldTitleTerms
        };

        private IPricePolicy policy;

        [TestInitialize]
        public void TestInitialize()
        {
            policy = new PricePolicy(allTerms);
        }

        [TestMethod]
        public void PriceForNewTitleIsCorrect()
        {
            int bonusPoints = 0;
            var titleType = VideoTitle.TitleType.New;
            int dayCount = 3;
            decimal expectedPrice = newTitleTerms.TrailingFee * dayCount;

            var terms = policy.CalculateRentalOptions(titleType, dayCount, bonusPoints);

            Assert.AreEqual(expectedPrice, terms.Price);
        }

        [TestMethod]
        public void PriceForRegularTitleIsCorrect()
        {
            int bonusPoints = 0;
            var titleType = VideoTitle.TitleType.Regular;
            RentalTerms terms = regularTitleTerms;

            int dayCount = terms.FlatPeriodDays - 1;
            decimal expectedPrice = terms.FlatPeriodFee;
            var rentalTerms = policy.CalculateRentalOptions(titleType, dayCount, bonusPoints);
            Assert.AreEqual(expectedPrice, rentalTerms.Price);

            dayCount = terms.FlatPeriodDays;
            expectedPrice = terms.FlatPeriodFee;
            rentalTerms = policy.CalculateRentalOptions(titleType, dayCount, bonusPoints);
            Assert.AreEqual(expectedPrice, rentalTerms.Price);

            dayCount = terms.FlatPeriodDays + 1;
            expectedPrice = terms.FlatPeriodFee + (dayCount - terms.FlatPeriodDays) * terms.TrailingFee;
            rentalTerms = policy.CalculateRentalOptions(titleType, dayCount, bonusPoints);
            Assert.AreEqual(expectedPrice, rentalTerms.Price);
        }

        [TestMethod]
        public void PriceForOldTitleIsCorrect()
        {
            int bonusPoints = 0;
            var titleType = VideoTitle.TitleType.Old;
            RentalTerms terms = regularTitleTerms;

            int dayCount = oldTitleTerms.FlatPeriodDays - 1;
            decimal expectedPrice = oldTitleTerms.FlatPeriodFee;
            var rentalTerms = policy.CalculateRentalOptions(titleType, dayCount, bonusPoints);
            Assert.AreEqual(expectedPrice, rentalTerms.Price);

            dayCount = oldTitleTerms.FlatPeriodDays;
            expectedPrice = oldTitleTerms.FlatPeriodFee;
            rentalTerms = policy.CalculateRentalOptions(titleType, dayCount, bonusPoints);
            Assert.AreEqual(expectedPrice, rentalTerms.Price);

            dayCount = oldTitleTerms.FlatPeriodDays + 1;
            expectedPrice = oldTitleTerms.FlatPeriodFee + (dayCount - oldTitleTerms.FlatPeriodDays) * oldTitleTerms.TrailingFee;
            rentalTerms = policy.CalculateRentalOptions(titleType, dayCount, bonusPoints);
            Assert.AreEqual(expectedPrice, rentalTerms.Price);
        }
    }
}
