using BoxFashion.Core.Data;
using BoxFashion.Core.Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Bussines
{
    public class CityBusiness : IDisposable
    {
        private CityData _city = null;

        public CityBusiness()
        {
            _city = new CityData();
        }

        public Collection<City> GetCity()
        {
            return _city.GetCity();
        }

        public void Dispose()
        {
            if (_city != null)
            {
                _city.Dispose();
                _city = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}

