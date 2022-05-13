using RentalOffer.SolutionSelector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServiceWorkshop.SolutionSelectorTests
{
    [TestFixture]
    public class BestOfferTest
    {
        [Test]
        public void ChooseBest()
        {
            var offer1 = new Offer(100, 0.2);
            var offer2 = new Offer(50, 1);
            var offer3 = new Offer(1000, 1);
            var needId = Guid.NewGuid().ToString();
            var bestOffer = new BestOffer();
            Assert.AreEqual(offer1, bestOffer.ChooseBest(needId, offer1));
            Assert.AreEqual(offer2, bestOffer.ChooseBest(needId, offer2));
            Assert.AreEqual(offer2, bestOffer.ChooseBest(needId, offer1));

            var needId2 = Guid.NewGuid().ToString();
            Assert.AreEqual(offer2, bestOffer.ChooseBest(needId2, offer2));
            Assert.AreEqual(offer3, bestOffer.ChooseBest(needId2, offer3));
            Assert.AreEqual(offer2, bestOffer.ChooseBest(needId, offer1));
        }

        [Test]
        public void ChooseBestWhenNegative()
        {
            var offer1 = new Offer(-100, 0.2);
            var bestOffer = new BestOffer();
            Assert.AreEqual(BestOffer.NullValue, bestOffer.ChooseBest("123", offer1));
        }
    }
}
