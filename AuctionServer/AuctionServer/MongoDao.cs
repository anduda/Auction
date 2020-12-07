using AuctionServer.Auctions;
using AuctionServer.Lots;
using AuctionServer.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer
{
    internal class MongoDao
    {
        public static UserDao Users = new UserDao("mongodb://localhost:27017");
        public static AuctionDao Auctions = new AuctionDao("mongodb://localhost:27017");
        public static LotDao Lots = new LotDao("mongodb://localhost:27017");
    }
}
