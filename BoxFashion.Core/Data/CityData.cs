using BoxFashion.Core.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Data
{
    public class CityData : BaseData
    {

        public Collection<City> GetCity()
        {
            StringBuilder oQuery = null;
            Collection<City> oCity = null;
            DataSet odataSet = null;
            Hashtable oParametro = null;

            try
            {
                oQuery = new StringBuilder();
                oParametro = new Hashtable();

                oQuery.Append("SELECT * FROM OOZ.TB_CITY");
                odataSet = base.PostGresDataProvider.RecordSet(oQuery.ToString());
                oCity = this.TraerCiudades(odataSet);

                return oCity;

            }

            finally
            {
                oQuery = null;
                oCity = null;
                odataSet = null;

            }

        }

        private Collection<City> TraerCiudades(DataSet data)
        {
            Collection<City> oCity = null;
            City eCity = null;
            DataRow linea = null;

            try
            {
                oCity = new Collection<City>();
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    linea = data.Tables[0].Rows[i];
                    eCity = new City();

                    eCity.nidcity = int.Parse(linea["nidcity"].ToString());
                    eCity.nidcountry = int.Parse(linea["nidcountry"].ToString());
                    eCity.nidstate = int.Parse(linea["nidstate"].ToString());
                    eCity.scity = linea["scity"].ToString();

                    oCity.Add(eCity);

                }

                return oCity;
            }
            finally
            {
                oCity = null;
                linea = null;
                eCity = null;
            }
        }

    }
}
