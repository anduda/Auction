using Auction.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Auction.ViewModel
{
    public class AddAuctionViewModel : INotifyPropertyChanged
    {
        private Window window { get; set; }
        public ObservableCollection<Server.Lot> Lots { get; set; }

        public AddAuctionViewModel(Window window)
        {
            this.window = window;
            //Lots = new ObservableCollection<Server.Lot>(Session.GetClient().GetUserLots(Session.GetSession().session.Login));
        }

        public RelayCommand AddAuctionCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var auction = obj as Tuple<DateTime, DateTime, double, Server.Lot>;
                    Session.GetClient().AddAuctionAsync(auction.Item1.AddSeconds(-1 * auction.Item1.Second), auction.Item2.AddSeconds(-1 * auction.Item2.Second), auction.Item4.Id, auction.Item3 );
                    OpenMainMenuCommand.Execute(null);
                });
            }
        }

        public RelayCommand OpenMainMenuCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    (new MainMenu()).Show();
                    window.Close();
                });
            }
        }

        public RelayCommand OpenLotWindowCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var lot = obj as Server.Lot;
                    (new LotWindow(lot.Id)).Show();
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
