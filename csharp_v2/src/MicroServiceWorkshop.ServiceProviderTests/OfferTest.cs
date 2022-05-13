using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using RentalOffer.SolutionSelector;

namespace MicroServiceWorkshop.Tests.SolutionSelector
{
    [TestFixture]
    public class OfferTest
    {
        [Test] public void BetterThan()
        {
            Assert.True(new Offer(100, 1).BetterThan(new Offer(50, 1)));
            Assert.True(new Offer(100, 1).BetterThan(new Offer(200, 0.1)));
            Assert.True(new Offer(100, 1).BetterThan(new Offer(100, 0)));
        }
    }
}
