using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AuctionServer.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuctionServer
{
    [DataContract]
    public class Lot
    {
        public Lot(string name, string owner, string description)
        {
            Name = name;
            Owner = owner;
            Description = description;
        }


        [DataMember]
        [BsonId]
        public ObjectId Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Owner { get; set; }
    }
}
