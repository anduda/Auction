using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using Auction.Server;
using MongoDB.Bson;

namespace Auction
{

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class Session : IAuctionServiceCallback
    {
        private Session() { }

        private static Session Instance;

        public static AuctionServiceClient GetClient()
        {
            if(Instance == null)
            {
                Instance = new Session();
                Instance.client = new AuctionServiceClient(new InstanceContext(Instance));
            }
            return Instance.client;
        }

        public static Session GetSession()
        {
            if (Instance == null)
            {
                Instance = new Session();
                Instance.client = new AuctionServiceClient(new InstanceContext(Instance));
            }
            return Instance;
        }
        


        public AuctionServiceClient client;

        public EventHandler<SessionEventArgs> OnBetEvent;
        public EventHandler<SessionEventArgs> OnStartEvent;
        public EventHandler<SessionEventArgs> OnEnterEvent;
        public EventHandler<SessionEventArgs> OnFinishEvent;
        public EventHandler<SessionEventArgs> OnLeaveEvent;

        public void OnBet(ObjectId id)
        {
            OnBetEvent.Invoke(this, new SessionEventArgs(id));
        }

        public void OnEnter(ObjectId id)
        {
            OnEnterEvent.Invoke(this, new SessionEventArgs(id));
        }

        public void OnFinish(ObjectId id)
        {
            OnFinishEvent.Invoke(this, new SessionEventArgs(id));
        }

        public void OnLeave(ObjectId id)
        {
            OnLeaveEvent.Invoke(this, new SessionEventArgs(id));
        }

        public void OnStart(ObjectId id)
        {
            OnStartEvent.Invoke(this, new SessionEventArgs(id));
        }
        
    }

    public class SessionEventArgs : EventArgs
    {
        public ObjectId Id;
        public SessionEventArgs(ObjectId id) : base()
        {
            this.Id = id;
        }
    }
}
