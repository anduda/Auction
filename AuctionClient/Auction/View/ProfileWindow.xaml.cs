using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Auction.Server;
using Auction.ViewModel;

namespace Auction
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();
            this.DataContext = new ProfileViewModel(this);
        }

        public ProfileWindow(string login)
        {
            InitializeComponent();
            this.DataContext = new ProfileViewModel(this, login);
        }

        private void Lots_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as ProfileViewModel).OpenLotWindowCommand.Execute(Lots.SelectedItem);
        }
    }
}
