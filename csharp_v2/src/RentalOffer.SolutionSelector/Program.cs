using System;

using MicroServiceWorkshop.RapidsRivers;
using MicroServiceWorkshop.RapidsRivers.RabbitMQ;
using Newtonsoft.Json.Linq;

namespace RentalOffer.SolutionSelector
{
    class SolutionSelector : River.IPacketListener
    {

        private static readonly BestOffer _bestOffer = new BestOffer();

        static void Main(string[] args)
        {
            string host = args[0];
            string port = args[1];

            var rapidsConnection = new RabbitMqRapids("selector_in_csharp", host, port);
            var river = new River(rapidsConnection);
            // See RiverTest for various functions River supports to aid in filtering, like:
            //river.RequireValue("key", "expected_value");  // Reject packet unless key exists and has expected value
            //river.Require("key1", "key2");       // Reject packet unless it has key1 and key2
            //river.Forbid("key1", "key2");        // Reject packet if it does have key1 or key2
            river.Require("offer");
            river.Forbid("is_champion");
            river.Register(new SolutionSelector());         // Hook up to the river to start receiving traffic
        }

        public void ProcessPacket(RapidsConnection connection, JObject jsonPacket, PacketProblems warnings)
        {
            Console.WriteLine("Received offer, selecting best offer");
            var needId = jsonPacket["need_id"].Value<string>();
            var offer = jsonPacket["offer"].Value<string>();
            var likelyhood = jsonPacket["likelyhood"]?.Value<double>();
            var value = jsonPacket["value"]?.Value<int>();
            Console.WriteLine($"Offer: {offer}, Id: {needId}, Likelyhood: {likelyhood}, value: {value}");
            var challenger = new Offer(value ?? 0, likelyhood ?? 0);
            var champion = _bestOffer.ChooseBest(needId, challenger, out var previousChampion);
            if (champion == challenger)
            {
                PublishResult(connection, jsonPacket, champion, previousChampion);
            } else
            {
                Console.WriteLine($"New {challenger} is NOT better than previous {previousChampion}");
            }
            Console.WriteLine();
        }

        private void PublishResult(RapidsConnection connection, JObject jsonPacket, 
            Offer champion, Offer challenger)
        {
            Console.WriteLine($"New {challenger} is better than previous {champion}");
            jsonPacket["is_champion"] = true; // Could be completely new packet
            connection.Publish(jsonPacket.ToString());
        }

        public void ProcessError(RapidsConnection connection, PacketProblems errors)
        {
            //Console.WriteLine(" [x] {0}", errors);
        }
    }
}
