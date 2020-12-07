using Auction.Model;
using Auction.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Bson;
using Auction.View;

namespace Auction.ViewModel
{
    public class MainMenuViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Server.AuctionInfo> Auctions { get; set; }
        private Window window { get; set; }

        public MainMenuViewModel(Window window)
        {
            this.window = window;
            Auctions = new ObservableCollection<AuctionInfo>(Session.GetClient().GetCurrentAuctions());
            Session.GetSession().OnStartEvent += AddAuction;
            Session.GetSession().OnFinishEvent += RemoveAuction;
        }

        private void RemoveAuction(object sender, SessionEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => Auctions.Remove(Auctions.FirstOrDefault(x => x.Id == e.Id)));
        }

        private void AddAuction(object sender, SessionEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => Auctions.Add(Session.GetClient().GetAuction(e.Id )));
        }
        
        public RelayCommand OpenAuctionWindowCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var auction = obj as Server.AuctionInfo;
                    (new AuctionWindow(auction.Id)).Show();
                    window.Close();
                });
            }
        }

        public RelayCommand OpenProfileWindowCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    (new ProfileWindow()).Show();
                    window.Close();
                });
            }
        }

        public RelayCommand OpenAddAuctionWindowCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    (new AddAuctionWindow()).Show();
                    window.Close();
                });
            }
        }

        public RelayCommand OpenAdminWindowCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    (new AdminWindow()).Show();
                    window.Close();
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
