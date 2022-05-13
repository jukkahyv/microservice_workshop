using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalOffer.SolutionSelector
{
    public class BestOffer
    {

        public static readonly Offer NullValue = new Offer(0, 0);

        private static Dictionary<string, Offer> _champions = new Dictionary<string, Offer>();

        public Offer ChooseBest(string needId, Offer challenger)
        {
            return ChooseBest(needId, challenger, out _);
        }

        public Offer ChooseBest(string needId, Offer challenger, out Offer previousChampion)
        {
            previousChampion = _champions.GetValueOrDefault(needId) ?? NullValue;
            if (challenger.BetterThan(previousChampion))
            {
                _champions[needId] = challenger;
                return challenger;
            }
            return previousChampion;
        }
    }
}
