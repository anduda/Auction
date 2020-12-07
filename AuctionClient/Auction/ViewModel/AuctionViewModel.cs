using Auction.Model;
using Auction.Server;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Auction.ViewModel
{
    public class AuctionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Players { get; set; }
        private Window _window;
        DispatcherTimer timer;
        public Server.AuctionInfo Model {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged("Model");
            }
        }

        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }


        private ObjectId _id;

        public AuctionViewModel(Window window, ObjectId id)
        {
            Session.GetSession().OnBetEvent += Refresh;
            Session.GetSession().OnFinishEvent += Finish;
            Session.GetSession().OnEnterEvent += Enter;
            Session.GetSession().OnLeaveEvent += Leave;
            this._window = window;
            this._id = id;
            Model = Session.GetClient().EnterAuction(id );
            Players = new ObservableCollection<string>(Model.Players);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        private void Leave(object sender, SessionEventArgs e)
        {
            if(e.Id == _id)
            {
                var players = Session.GetClient().GetAuction(e.Id ).Players;
                foreach (var player in Players)
                {
                    if(!players.Contains(player))
                    {
                        Players.Remove(player);
                    }
                }
            }
        }

        private void Enter(object sender, SessionEventArgs e)
        {
            if(e.Id == _id)
            {
                var players = Session.GetClient().GetAuction(e.Id).Players;
                foreach(var player in players)
                {
                    if (!Players.Contains(player))
                        Application.Current.Dispatcher.Invoke(() => Players.Add(player));
                }
            }
        }

        private void Finish(object sender, SessionEventArgs e)
        {
            if(e.Id == _id)
            {
                MessageBox.Show($"Auction end. Winner {Model.Winner}");
                Application.Current.Dispatcher.Invoke(() => OpenMainMenuCommand.Execute(null));
            }
        }

        private void Refresh(object sender, SessionEventArgs e)
        {
            if(e.Id == _id)
            {
                Model = Session.GetClient().GetAuction(_id);
                Players = new ObservableCollection<string>(Model.Players);
            }
        }

        private RelayCommand betCommand;
        private AuctionInfo model;
        private string time;

        public RelayCommand BetCommand
        {
            get
            {
                return betCommand ?? (betCommand = new RelayCommand(obj =>
                {
                    var bet = obj as string;
                    Session.GetClient().Bet(_id, Convert.ToDouble(bet));
                }));
            }
        }

        public RelayCommand OpenMainMenuCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    (new MainMenu()).Show();
                    _window.Close();
                });
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var x = (Model.EndTime - DateTime.UtcNow);

            Time = Math.Floor(x.TotalHours) + x.ToString(@"\:mm\:ss");
            if (Time == "00:00:00")
            {
                timer.Stop();
                //TODO:Disable button
            }
        }

        private RelayCommand openProfileWindowCommand;
        public RelayCommand OpenProfileWindowCommand
        {
            get
            {
                return openProfileWindowCommand ?? (openProfileWindowCommand =  new RelayCommand(obj =>
                {
                    (new ProfileWindow(obj as string)).Show();
                }));
            }
        }

        private RelayCommand openLotWindowCommand;
        public RelayCommand OpenLotWindowCommand
        {
            get
            {
                return openLotWindowCommand ?? (openLotWindowCommand = new RelayCommand(obj =>
                {
                    if(obj is ObjectId id)
                        (new LotWindow(id)).Show();
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
