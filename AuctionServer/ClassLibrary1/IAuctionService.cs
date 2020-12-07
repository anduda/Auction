using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ClassLibrary1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IAuctionService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IAuctionService
    {
        [OperationContract]
        void DoWork();
    }
}
