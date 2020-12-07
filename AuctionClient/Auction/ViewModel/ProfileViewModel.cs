using Auction.Model;
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

namespace Auction.ViewModel
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Server.Lot> Lots { get; set; }
        private Window window { get; set; }
        public Server.UserInfo Model { get; set; }
        public Visibility IsVisible { get; set; }

        public ProfileViewModel(Window window)
        {
            IsVisible = Visibility.Visible;
            this.window = window;
            //Lots = new ObservableCollection<Server.Lot>(Session.GetClient().GetUserLots(Session.GetSession().session.Login));
            Model = Session.GetClient().GetUser();
        }

        public ProfileViewModel(Window window, string login)
        {
            IsVisible = Visibility.Hidden;
            this.window = window;
            Lots = new ObservableCollection<Server.Lot>(Session.GetClient().GetUserLots(login));
            Model = Session.GetClient().GetUserByLogin(login);
        }

        public RelayCommand OpenLotWindowCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (obj is Server.Lot lot)
                    {
                        (new LotWindow(lot.Id)).Show();
                    }
                    else
                    {
                        (new LotWindow()).Show();
                        window.Close();
                    }
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

        public RelayCommand OpenDepositDialogCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    (new DepositDialog()).Show();
                    window.Close();
                });
            }
        }

        private RelayCommand changeStatusCommand;
        public RelayCommand ChangeStatusCommand
        {
            get
            {
                return changeStatusCommand ?? (changeStatusCommand = new RelayCommand(obj =>
                {
                    Session.GetClient().ChangeUserStatus(Model.Login);
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
