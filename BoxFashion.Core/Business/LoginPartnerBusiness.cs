using BoxFashion.Core.Data;
using BoxFashion.Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Business
{
    public class LoginPartnerBusiness: IDisposable
    {
        private LoginPartnerData _loginPartnerData;

        public LoginPartnerBusiness()
        {
            _loginPartnerData = new LoginPartnerData();
        }

        public int InsertLoginPartner(Security_LoginPartner oLogin)
        {
            return _loginPartnerData.InsertLoginPartner(oLogin);
        }

        public void Dispose()
        {
            if (_loginPartnerData != null)
            {
                _loginPartnerData.Dispose();
                _loginPartnerData = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
