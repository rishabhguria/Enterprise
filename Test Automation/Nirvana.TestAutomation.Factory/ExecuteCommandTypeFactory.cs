using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Interfaces.Enums;
using Nirvana.TestAutomation.Utilities;

namespace Nirvana.TestAutomation.Factory
{
    public class ExecuteCommandTypeFactory
    {
        public static ICommandUtilities SetExecutionCommandType(CommandType type)
        {
            switch (type)
            {
                case  CommandType.SqlQuery: return new SqlUtilities();
                case CommandType.Bat: return new BatchCommandUtilities();
            }
            return null;
        }
    }
}
