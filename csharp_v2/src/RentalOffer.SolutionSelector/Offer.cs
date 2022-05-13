
namespace RentalOffer.SolutionSelector
{
    public class Offer
    {

        private readonly double _expectedValue;

        public Offer(double value, double likelyhood)
        {
            _expectedValue = value * likelyhood;
        }

        public bool BetterThan(Offer other)
        {
            return _expectedValue > other._expectedValue;
        }

        public override string ToString() => $"Offer ({Math.Round(_expectedValue)})";
    }
}
