using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Users
{
    [DataContract]
    public class User
    {
        public User(string surname, string name, string secondName, string login, string password, string country)
        {
            Surname = surname;
            Name = name;
            SecondName = secondName;
            Login = login;
            Password = password;
            Country = country;
        }

        public ObjectId Id { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SecondName { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public double Balance { get; set; } = 0;
        [DataMember]
        public bool IsBlocked { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
    }
}