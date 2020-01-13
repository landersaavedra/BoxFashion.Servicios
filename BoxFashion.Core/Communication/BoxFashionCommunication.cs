using System;
using System.Collections.ObjectModel;
using System.Text;
using System.ServiceModel;
using BoxFashion.Core.Entidades;
using BoxFashion.Core.Communication.IContrato;

namespace BoxFashion.Core.Communication
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    public class BoxFashionCommunication : IContrato.IBoxFashionCommunication 
    {
        public int InsertLoginPartner(Security_LoginPartner oLogin)
        {
            Business.LoginPartnerBusiness objLogin = null;
            try
            {
                objLogin = new Business.LoginPartnerBusiness();
                return objLogin.InsertLoginPartner(oLogin);
            }
            finally
            {
                if (objLogin != null)
                {
                    objLogin.Dispose();
                    objLogin = null;
                }
            }
        }

        Collection<City> IBoxFashionCommunication.GetCity()
        {
            Bussines.CityBusiness objCitys = null;
            try
            {
                objCitys = new Bussines.CityBusiness();
                return objCitys.GetCity();
            }
            finally
            {
                if(objCitys != null)
                {
                    objCitys.Dispose();
                    objCitys = null;
                }
            }
        }

      
        int IBoxFashionCommunication.InsertLoginCustomer(Security_LoginCustomer oLogin)
        {
            Business.LoginCustomerBusiness objLogin = null;
            try
            {
                objLogin = new Business.LoginCustomerBusiness();
                return objLogin.InsertLoginCustomer(oLogin);
            }
            finally
            {
                if (objLogin != null)
                {
                    objLogin.Dispose();
                    objLogin = null;
                }
            }
        }
    }
}
