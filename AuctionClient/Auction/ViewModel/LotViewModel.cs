using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Auction.Model;
using Microsoft.Win32;
using MongoDB.Bson;

namespace Auction.ViewModel
{
    public class LotViewModel : INotifyPropertyChanged
    {
        private Window _window;
        private ObjectId id;
        private BitmapImage imageSource;

        public LotViewModel(Window window, ObjectId id)
        {
            this._window = window;
            this.id = id;
        }

        public LotViewModel(Window window)
        {
            this._window = window;
        }

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        private RelayCommand loadImageCommand;
        public RelayCommand LoadImageCommand
        {
            get
            {
                return loadImageCommand ?? (loadImageCommand = new RelayCommand(obj =>
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.Title = "Select a picture";
                    op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                      "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                      "Portable Network Graphic (*.png)|*.png";
                    if (op.ShowDialog() == true)
                    {
                        ImageSource = new BitmapImage(new Uri(op.FileName));
                    }
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
