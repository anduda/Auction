using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AuctionServer.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuctionServer.Auctions
{
    [DataContract]
    public class Auction
    {
        public Auction(ObjectId lot, double startPrice, DateTime startTime, DateTime endTime, string owner)
        {
            Lot = lot;
            CurrentPrice = startPrice;
            StartPrice = startPrice;
            StartTime = startTime;
            EndTime = endTime;
            Owner = owner;
            Players = new HashSet<string>();
        }

        [BsonId]
        [DataMember]
        public ObjectId Id { get; set; }
        [DataMember]
        public ObjectId Lot { get; set; }
        [DataMember]
        public double CurrentPrice { get; set; }
        [DataMember]
        public double StartPrice { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public string Owner { get; set; }
        [DataMember]
        public string Winner { get; set; }
        [DataMember]
        public HashSet<string> Players { get; set; }
    }
}
