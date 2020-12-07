using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Users
{
    [DataContract]
    public class UserInfo
    {
        public UserInfo(User user)
        {
            Surname = user.Surname;
            Name = user.Name;
            SecondName = user.SecondName;
            Login = user.Login;
            Country = user.Country;
            Balance = user.Balance;
        }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SecondName { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public double Balance { get; set; }
    }
}
