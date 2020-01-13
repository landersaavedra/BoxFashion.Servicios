using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Exception
{
    public class DataProviderApplicationException : ApplicationException
    {
        public DataProviderApplicationException(string pMessage)
          : base(pMessage)
        {
        }

        public DataProviderApplicationException(string pMessage, System.Exception pInnerException)
          : base(pMessage, pInnerException)
        {
        }
    }
}