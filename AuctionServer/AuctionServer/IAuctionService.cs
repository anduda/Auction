using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using AuctionServer.Users;
using AuctionServer.Auctions;
using AuctionServer.Lots;
using MongoDB.Bson;

namespace AuctionServer
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IAuectionService" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IAuctionServerCallback))]
    public interface IAuctionService
    {

        [OperationContract]
        [FaultContract(typeof(InvalidLoginException))]
        void Registration(string login, string password, string name, string surname, string secondName, string country);

        [OperationContract]
        [FaultContract(typeof(IServerException))]
        [FaultContract(typeof(UserIsBlockedException))]
        [FaultContract(typeof(WrongPasswordException))]
        bool Logon(string login, string password);

        [OperationContract]
        void Logout();

        [OperationContract]
        List<AuctionInfo> GetCurrentAuctions();
        
        [OperationContract]
        UserInfo GetUserByLogin(string login);

        [OperationContract]
        List<Lot> GetUserLots(string login);

        [OperationContract]
        void AddAuction(DateTime startTime, DateTime endTime, ObjectId lotId, double startPrice);

        [OperationContract]
        AuctionInfo GetAuction(ObjectId id);

        [OperationContract(IsOneWay = true)]
        void Bet(ObjectId id, double bet);

        [OperationContract]
        AuctionInfo EnterAuction(ObjectId id);

        [OperationContract]
        void Deposit(double bet);

        [OperationContract]
        Lot GetLotById(ObjectId id);

        [OperationContract]
        ObjectId CreateLot(string Name, string Description);

        [OperationContract]
        void ChangeUserStatus(string login);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        List<AuctionInfo> GetNewAuctions();

        [OperationContract]
        UserInfo GetUser();
    }
    
    public interface IAuctionServerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnBet(ObjectId id);

        [OperationContract(IsOneWay = true)]
        void OnStart(ObjectId id);

        [OperationContract(IsOneWay = true)]
        void OnFinish(ObjectId id);

        [OperationContract(IsOneWay = true)]
        void OnEnter(ObjectId id);

        [OperationContract(IsOneWay = true)]
        void OnLeave(ObjectId id);
    }
}
