using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Auctions
{
    [DataContract]
    public class AuctionInfo
    {
        public AuctionInfo(Auction auction, Lot lot)
        {
            Id = auction.Id;
            Lot = lot;
            CurrentPrice = auction.CurrentPrice;
            StartPrice = auction.StartPrice;
            StartTime = auction.StartTime;
            EndTime = auction.EndTime;
            Owner = auction.Owner;
            Winner = auction.Winner;
            Players = auction.Players;

        }

        [DataMember]
        public ObjectId Id { get; set; }
        [DataMember]
        public Lot Lot { get; set; }
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
