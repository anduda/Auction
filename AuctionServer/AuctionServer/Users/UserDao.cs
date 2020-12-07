using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Users
{
    internal class UserDao
    {
        IMongoDatabase db = null;
        IMongoCollection<User> users = null;

        public UserDao(string ConnectionString)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            db = dbClient.GetDatabase("Auction");
            users = db.GetCollection<User>("Users");
            var options = new CreateIndexOptions() { Unique = true };
            var field = new StringFieldDefinition<User>("Login");
            var indexDefinition = new IndexKeysDefinitionBuilder<User>().Ascending(field);
            users.Indexes.CreateOne(indexDefinition, options);
        }

        public void AddUser(User user)
        {
            try
            {
                user.IsBlocked = true;
                user.IsAdmin = false;
                users.InsertOne(user);
            }
            catch
            {
                throw new FaultException<InvalidLoginException>(new InvalidLoginException());
            }
        }

        public User GetUser(string login)
        {
            try
            {
                return users.Find((user => user.Login == login)).First();
            }
            catch
            {
                throw new FaultException<UserNotFoundException>(new UserNotFoundException());
            }
        }

        public List<User> GetAllUsers()
        {
            return users.Find((_ => true)).ToList();
        }

        public bool GetUserStatus(string login)
        {
            return GetUser(login).IsBlocked;
        }

        public void ChangeUserStatus(string login)
        {
            bool IsBlocked = GetUserStatus(login);
            users.UpdateOne((user => user.Login == login), (new UpdateDefinitionBuilder<User>()).Set((user => user.IsBlocked), !IsBlocked));
        }


        public void UpdateUserBalance(double balance, string login)
        {
            users.UpdateOne((user => user.Login == login), (new UpdateDefinitionBuilder<User>()).Inc((user => user.Balance), balance));
        }

    }
}
