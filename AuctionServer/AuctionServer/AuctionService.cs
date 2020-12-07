using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AuctionServer.Users;
using AuctionServer.Auctions;
using System.Threading.Tasks;
using AuctionServer.Lots;
using MongoDB.Bson;
using System.Threading;

namespace AuctionServer
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "AuectionService" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AuctionService : IAuctionService
    {
        UserDao userDao = MongoDao.Users;
        AuctionDao auctionDao = MongoDao.Auctions;
        LotDao lotDao = MongoDao.Lots;
        Dictionary<string, Session> sessions;


        
        public AuctionService()
        {
            sessions = new Dictionary<string, Session>();
            NewAuctionsLoop();
            CurrentAuctionsLoop();
            OldAuctionsLoop();
        }

        #region Auction
        public void AddAuction(DateTime startTime, DateTime endTime, ObjectId lotId, double startPrice)
        {
            auctionDao.CreateAuction(lotId, startPrice, startTime, endTime, GetUser().Login);
            lotDao.UpdateOwner(lotId, "");
        }

        public AuctionInfo EnterAuction(ObjectId id)
        {
            var auction = auctionDao.EnterAuction(id, GetUser().Login);
            var info = new AuctionInfo(auction, lotDao.GetLot(auction.Lot));
            NotifyAllAsync(sessions.Where(s => auction.Players.Contains(s.Value.Login)).Select(s => s.Value.Callback).ToList(), OnEnter, id);
            return info;
        }

        public void LeaveAuction(ObjectId id)
        {
            auctionDao.LeaveAuction(id, GetUser().Login);
            NotifyAllAsync(sessions.Where(s => auctionDao.GetAuction(id).Players.Contains(s.Value.Login)).Select(s => s.Value.Callback).ToList(), OnLeave, id);

        }

        public List<AuctionInfo> GetCurrentAuctions()
        {
            return auctionDao.GetCurrentAuctions().Select(auction => new AuctionInfo(auction, lotDao.GetLot(auction.Lot))).ToList();
        }

        public void Bet(ObjectId id, double bet)
        {
            auctionDao.Bet(id, GetUser().Login, bet);
            var players = auctionDao.GetAuctionPlayers(id);
            NotifyAllAsync(sessions.Where(s => players.Contains(s.Value.Login)).Select(s => s.Value.Callback).ToList(), OnBet, id);
        }

        public AuctionInfo GetAuction(ObjectId id)
        {
            var auction = auctionDao.GetAuction(id);
            var lot = lotDao.GetLot(auction.Lot);
            return new AuctionInfo(auction, lot);
        }

        public List<AuctionInfo> GetNewAuctions()
        {
            return auctionDao.GetNewAuctions().Select(auction => new AuctionInfo(auction, lotDao.GetLot(auction.Lot))).ToList();
        }
        #endregion

        #region Lot

        public Lot GetLotById(ObjectId id)
        {
            return lotDao.GetLot(id);
        }

        public List<Lot> GetUserLots(string login)
        {
            return lotDao.GetAllUserLots(login);
        }

        public ObjectId CreateLot(string Name, string Description)
        {
            return lotDao.CreateLot(Name, GetUser().Login, Description);
        }

        #endregion

        #region Connection

        public bool Logon(string login, string password)
        {
            var user = userDao.GetUser(login);
            if(user.Password == password)
            {
                if (!user.IsBlocked)
                {
                    sessions.Add(OperationContext.Current.SessionId, new Session(user.Login, user.IsAdmin, OperationContext.Current));
                    return true;
                }
                //throw new FaultException<IServerException>(new UserIsBlockedException());
            }
            //throw new FaultException<IServerException>(new WrongPasswordException());
            return false;
        }

        public void Registration(string login, string password, string name, string surname, string secondName, string country)
        {
            User user = new User(surname, name, secondName, login, password, country);
            userDao.AddUser(user);
            //return Logon(login, password);
        }

        public void Logout()
        {
            sessions.Remove(OperationContext.Current.SessionId);
        }

        #endregion

        #region User

        public UserInfo GetUserByLogin(string login)
        {
            if(GetSession().IsAdmin || GetUser().Login == login)
            {

            }
            return new UserInfo(userDao.GetUser(login));
        }
        

        public void Deposit(double bet)
        {
            userDao.UpdateUserBalance(bet, GetUser().Login);
            //GetUserBySession(session).Balance += bet;
        }

        public UserInfo GetUser()
        {
            return GetUserByLogin(GetSession().Login);
        }
        #endregion

        #region Admin
        public void ChangeUserStatus(string login)
        {
            if(GetSession().IsAdmin)
                userDao.ChangeUserStatus(login);
        }

        public List<User> GetAllUsers()
        {
            if(GetSession().IsAdmin)
            {
                return userDao.GetAllUsers();
            }
            return null;
        }

        #endregion

        #region Callback

        private void NotifyAllAsync(List<OperationContext> callbacks, Action<List<OperationContext>> action)
        {
            Task.Factory.StartNew(() => action.Invoke(callbacks));
        }

        private void NotifyAllAsync(List<OperationContext> callbacks, Action<List<OperationContext>, ObjectId> action, ObjectId id)
        {
            Task.Factory.StartNew(() => action.Invoke(callbacks, id));
        }

        private void OnStart(List<OperationContext> callbacks, ObjectId id)
        {
            if (sessions == null)
                return;
            foreach (var callback in callbacks)
            {
                try
                {
                    callback.GetCallbackChannel<IAuctionServerCallback>().OnStart(id);
                }
                catch
                {
                    sessions.Remove(callback.SessionId);
                }
            }
        }

        private void OnBet(List<OperationContext> callbacks, ObjectId id)
        {
            if (sessions == null)
                return;
            foreach (var callback in callbacks)
            {
                try
                {
                    callback.GetCallbackChannel<IAuctionServerCallback>().OnBet(id);
                }
                catch
                {
                    sessions.Remove(callback.SessionId);
                }
            }
        }

        private void OnFinish(List<OperationContext> callbacks, ObjectId id)
        {
            if (sessions == null)
                return;
            foreach (var callback in callbacks)
            {
                try
                {
                    callback.GetCallbackChannel<IAuctionServerCallback>().OnFinish(id);
                }
                catch
                {
                    sessions.Remove(callback.SessionId);
                }
            }
        }

        private void OnEnter(List<OperationContext> callbacks, ObjectId id)
        {
            if (sessions == null)
                return;
            foreach (var callback in callbacks)
            {
                try
                {
                    callback.GetCallbackChannel<IAuctionServerCallback>().OnEnter(id);
                }
                catch
                {
                    sessions.Remove(callback.SessionId);
                }
            }
        }

        private void OnLeave(List<OperationContext> callbacks, ObjectId id)
        {
            if (sessions == null)
                return;
            foreach (var callback in callbacks)
            {
                try
                {
                    callback.GetCallbackChannel<IAuctionServerCallback>().OnStart(id);
                }
                catch
                {
                    sessions.Remove(callback.SessionId);
                }
            }
        }
        #endregion

        #region BackgroundJob

        private void NewAuctionsLoop()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var auction in auctionDao.GetNewAuctions())
                    {
                        if (auction.StartTime <= DateTime.UtcNow)
                        {
                            auctionDao.RemoveToCurrent(auction.Id);
                            NotifyAllAsync(sessions.Values.Select(x => x.Callback).ToList(), OnStart, auction.Id);
                            Thread.Sleep(200);
                        }
                    }
                }
            });
        }

        private void CurrentAuctionsLoop()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var auction in auctionDao.GetCurrentAuctions())
                    {
                        if (auction.EndTime <= DateTime.UtcNow)
                        {
                            auctionDao.RemoveToOld(auction.Id);
                            NotifyAllAsync(sessions.Values.Select(x => x.Callback).ToList(), OnFinish, auction.Id);
                            Thread.Sleep(50);
                        }
                    }
                }
            });
        }

        private void OldAuctionsLoop()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var auction in auctionDao.GetOldAuctions())
                    {
                        if (string.IsNullOrEmpty(auction.Winner))
                        {
                            lotDao.UpdateOwner(auction.Lot, auction.Owner);
                        }
                        else
                        {
                            lotDao.UpdateOwner(auction.Lot, auction.Winner);
                        }
                        auctionDao.RemoveOldAuction(auction.Id);
                    }
                }
            });
        }

        #endregion

        #region Private
        private Session GetSession()
        {
            sessions.TryGetValue(OperationContext.Current.SessionId, out Session user);
            if (user == null)
            {
                throw new Exception();
            }
            return user;
        }

        private class Session
        {
            public string Login { get; private set; }
            public bool IsAdmin { get; private set; }
            public OperationContext Callback {get; private set;}

            public Session(string login, bool isAdmin, OperationContext context)
            {
                this.Login = login;
                this.IsAdmin = isAdmin;
                this.Callback = context;
            }
        }

        #endregion
    }
}
