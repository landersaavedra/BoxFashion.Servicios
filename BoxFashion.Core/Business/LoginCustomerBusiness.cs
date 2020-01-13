using BoxFashion.Core.Data;
using BoxFashion.Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Business
{
    public class LoginCustomerBusiness : IDisposable
    {
        private LoginCustomerData _loginCustomerData;

        public LoginCustomerBusiness()
        {
            _loginCustomerData = new LoginCustomerData();
        }

        public int InsertLoginCustomer(Security_LoginCustomer oLogin)
        {
            return _loginCustomerData.InsertLoginCustomer(oLogin);
        }

        public void Dispose()
        {
            if (_loginCustomerData != null)
            {
                _loginCustomerData.Dispose();
                _loginCustomerData = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
