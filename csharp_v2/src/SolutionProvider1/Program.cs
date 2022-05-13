using System;
using System.Globalization;
using MicroServiceWorkshop.RapidsRivers;
using MicroServiceWorkshop.RapidsRivers.RabbitMQ;
using Newtonsoft.Json.Linq;

namespace RentalOffer.Provider1
{
    class SolutionProvider2 : River.IPacketListener
    {
        static void Main(string[] args)
        {
            string host = args[0];
            string port = args[1];

            var rapidsConnection = new RabbitMqRapids("provider_in_csharp", host, port);
            var river = new River(rapidsConnection);
            // See RiverTest for various functions River supports to aid in filtering, like:
            //river.RequireValue("key", "expected_value");  // Reject packet unless key exists and has expected value
            //river.Require("key1", "key2");       // Reject packet unless it has key1 and key2
            //river.Forbid("key1", "key2");        // Reject packet if it does have key1 or key2
            river.RequireValue("need", "car_rental_offer");
            river.Forbid("offer");
            river.Register(new SolutionProvider2());         // Hook up to the river to start receiving traffic
        }

        public void ProcessPacket(RapidsConnection connection, JObject jsonPacket, PacketProblems warnings)
        {
            var id = jsonPacket["need_id"].Value<string>();
            jsonPacket["offer"] = "bmw, double points";
            var likelyhood = new Random().NextDouble();
            jsonPacket["likelyhood"] = likelyhood;
            jsonPacket["value"] = new Random().Next(-100, 200);
            Console.WriteLine($"Received request for car rental offer ID {id}, making an offer with likelyhood {likelyhood}");
            connection.Publish(jsonPacket.ToString());

        }

        public void ProcessError(RapidsConnection connection, PacketProblems errors)
        {
            //Console.WriteLine(" [x] {0}", errors);
        }

        /*private static string OfferPacket()
        {
            var likelyhood = new Random().NextDouble();
            return
                "{\"offer\":\"bmw, double points\", \"likelyhood\":\"" + likelyhood.ToString(CultureInfo.GetCultureInfo("en-US")) + "\", \"value\":\"300\"}";
        }*/
    }
}
