using Auction.Model;
using Auction.Server;
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
    public class AdminViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Auctions { get; set; }
        private Window window { get; set; }

        public AdminViewModel(Window window)
        {
            this.window = window;
            Auctions = new ObservableCollection<User>(Session.GetClient().GetAllUsers());
        }


        private RelayCommand openProfileWindowCommand;
        public RelayCommand OpenProfileWindowCommand
        {
            get
            {
                return openProfileWindowCommand ?? (openProfileWindowCommand = new RelayCommand(obj =>
                {
                    (new ProfileWindow((obj as User).Login)).Show();
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
