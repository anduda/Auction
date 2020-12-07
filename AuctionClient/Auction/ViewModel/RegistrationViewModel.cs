using Auction.Model;
using Auction.Server;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace Auction.ViewModel
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        public RegistrationModel Model { get; set; }
        private Window window { get; set; }

        public RegistrationViewModel(Window window)
        {
            Model = new RegistrationModel();
            this.window = window;
        }

        private RelayCommand registrationCommand;
        public RelayCommand RegistrationCommand
        {
            get
            {
                return registrationCommand ?? (registrationCommand = new RelayCommand(obj =>
                {
                    var psw = obj as PasswordBox;
                    try
                    {
                        Session.GetClient().Registration(Model.Login, psw.Password, Model.Name, Model.Surname, Model.SecondName, Model.Country);
                    }
                    catch (FaultException<InvalidLoginException> ex)
                    {
                        MessageBox.Show(ex.Detail.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    (new LoginWindow()).Show();
                    window.Close();
                }));
            }
        }

        public RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return backCommand ?? (backCommand = new RelayCommand(obj =>
                {

                    (new LoginWindow()).Show();
                    window.Close();
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
