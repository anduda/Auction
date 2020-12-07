using Auction.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Model
{
    public class RegistrationModel
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Login { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
    }
}
