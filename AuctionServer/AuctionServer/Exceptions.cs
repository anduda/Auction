using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer
{
    [DataContract]
    public abstract class IServerException
    {
        [DataMember]
        public virtual string Message { get; set; }
    }

    [DataContract]
    public class InvalidLoginException : IServerException
    {
        private string _message;
        [DataMember]
        public override string Message { get { return _message; } set { _message = value; } }
        public InvalidLoginException()
        {
            _message = "User with the same login has already exist";
        }
    }
    

    [DataContract]
    public class UserNotFoundException : IServerException
    {
        private string _message;
        [DataMember]
        public override string Message { get { return _message; } set { _message = value; } }
        public UserNotFoundException()
        {
            _message = "User not found";
        }
    }

    [DataContract]
    public class UserIsBlockedException : IServerException
    {
        private string _message;
        [DataMember]
        public override string Message { get { return _message; } set { _message = value; } }
        public UserIsBlockedException()
        {
            _message = "User is blocked";
        }
    }

    [DataContract]
    public class WrongPasswordException : IServerException
    {
        private string _message;
        [DataMember]
        public override string Message { get { return _message; } set { _message = value; } }
        public WrongPasswordException()
        {
            _message = "Wrong password";
        }
    }

    [DataContract]
    public class InvalidTimeException
    {
        public InvalidTimeException()
        {
            
        }
        [DataMember]
        public  string Message => "You can not bet because of invalid time";
    }

    [DataContract]
    public class InvalidBetException
    {
        public InvalidBetException()
        {

        }
        [DataMember]
        public  string Message => "Bet price is lower than current";
    }

    [DataContract]
    public class InvalidSessionException
    {
        public InvalidSessionException()
        {

        }
        [DataMember]
        public  string Message => "Your session has been expired" ;
    }
}
