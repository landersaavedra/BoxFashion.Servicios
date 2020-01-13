using BoxFashion.Core.Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Data
{
    public abstract class BaseData : IDisposable
    {
        private PostGresDataProvider postGresData = null;
        protected PostGresDataProvider PostGresDataProvider
        {
            get { return postGresData; }
        }

        public BaseData()
        {
             postGresData = new PostGresDataProvider(Common.Configuracion.ConnectionString);
        }
        public void Dispose()
        {
            if (PostGresDataProvider != null)
            {
                PostGresDataProvider.Dispose();
                //PostGresDataProvider = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
