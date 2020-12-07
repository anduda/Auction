using AuctionServer.Users;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Auctions
{
    internal class AuctionDao
    {
        IMongoDatabase db = null;
        IMongoCollection<Auction> currentAuctions = null;
        IMongoCollection<Auction> newAuctions = null;
        IMongoCollection<Auction> oldAuctions = null;


        public AuctionDao(string ConnectionString)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            db = dbClient.GetDatabase("Auctions");
            currentAuctions = db.GetCollection<Auction>("CurrentAuctions");
            newAuctions = db.GetCollection<Auction>("NewAuctions");
            oldAuctions = db.GetCollection<Auction>("OldAuctions");
        }

        public bool Bet(ObjectId id, string user, double bet)
        {
            Auction auction = GetAuction(id);
            if(auction.StartTime.ToUniversalTime() < DateTime.UtcNow && auction.EndTime.ToUniversalTime() > DateTime.UtcNow && bet > auction.CurrentPrice)
            {
                MongoDao.Users.UpdateUserBalance(auction.CurrentPrice, auction.Winner);
                auction.Winner = user;
                auction.CurrentPrice = bet;
                currentAuctions.FindOneAndReplace((a => a.Id == id), auction);
                return true;
            }
            return false;

        }

        public ObjectId CreateAuction(ObjectId lot, double startPrice, DateTime startTime, DateTime endTime, string owner)
        {
            Auction auction = new Auction(lot, startPrice, startTime, endTime, owner);
            newAuctions.InsertOne(auction);
            return auction.Id;
        }

        public Auction EnterAuction(ObjectId id, string user)
        {
            //return currentAuctions.FindOneAndUpdate()
            var option = new FindOneAndUpdateOptions<Auction> { ReturnDocument = ReturnDocument.After };
            return currentAuctions.FindOneAndUpdate<Auction>((auction => auction.Id == id), (new UpdateDefinitionBuilder<Auction>()).AddToSet((a => a.Players), user), option);
        }

        public Auction GetAuction(ObjectId id)
        {
            var result = currentAuctions.Find((auction => auction.Id == id)).FirstOrDefault();
            if(result == null)
            {
                result = newAuctions.Find((auction => auction.Id == id)).FirstOrDefault();
            }
            return result;
        }

        public void LeaveAuction(ObjectId id, string user)
        {
            currentAuctions.FindOneAndUpdate((auction => auction.Id == id), (new UpdateDefinitionBuilder<Auction>()).Pull((a => a.Players), user));
        }
        

        public List<Auction> GetCurrentAuctions()
        {
            return currentAuctions.Find((_ => true)).ToList();
        }

        public void RemoveToCurrent(ObjectId id)
        {
            var result = newAuctions.FindOneAndDelete((auction => auction.Id == id));
            currentAuctions.InsertOne(result);
        }

        public void RemoveToOld(ObjectId id)
        {
            var result = currentAuctions.FindOneAndDelete((auction => auction.Id == id));
            oldAuctions.InsertOne(result);
        }

        public List<Auction> GetNewAuctions()
        {
            return newAuctions.Find((_ => true)).ToList();
        }

        public List<Auction> GetOldAuctions()
        {
            return oldAuctions.Find((_ => true)).ToList();
        }

        public void RemoveOldAuction(ObjectId id)
        {
            oldAuctions.DeleteOne((auction => auction.Id == id));
        }

        public List<string> GetAuctionPlayers(ObjectId id)
        {
            return currentAuctions.Find((auction => auction.Id == id)).FirstOrDefault()?.Players.ToList();
        }
    }
}
