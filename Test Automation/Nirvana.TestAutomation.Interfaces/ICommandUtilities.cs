using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Interfaces
{
    public interface ICommandUtilities
    {
        void ExecuteCommand<T>(T command);
        void ExecuteCommand<T>(T commamd,string masterDB);
  
    }
}
