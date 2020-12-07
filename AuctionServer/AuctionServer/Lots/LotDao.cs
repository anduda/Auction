using AuctionServer.Users;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Lots
{
    public class LotDao
    {
        IMongoDatabase db = null;
        IMongoCollection<Lot> lots = null;


        public LotDao(string ConnectionString)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            db = dbClient.GetDatabase("Lots");
            lots = db.GetCollection<Lot>("Lots");
        }

        public ObjectId CreateLot(string Name, string Owner, string Description)
        {
            Lot lot = new Lot(Name, Owner, Description);
            lots.InsertOne(lot);
            return lot.Id;
        }

        public Lot GetLot(ObjectId id)
        {
            return lots.Find((lot => lot.Id == id)).FirstOrDefault();
        }

        public List<Lot> GetAllUserLots(string login)
        {
            return lots.Find((lot => lot.Owner == login)).ToList();
        }

        public void UpdateOwner(ObjectId id, string login)
        {
            lots.UpdateOne((lot => lot.Id == id), (new UpdateDefinitionBuilder<Lot>()).Set<string>((lot1 => lot1.Owner), login));
        }
    }
}
