using BoxFashion.Core.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Data
{
    public class LoginPartnerData:BaseData
    {
        public int InsertLoginPartner(Security_LoginPartner oLogin)
        {
            StringBuilder oQuery = null;
            Hashtable oParam = null;
            DataSet oAccesodataset = null;

            try
            {
                oQuery = new StringBuilder();
                oParam = new Hashtable();
                oAccesodataset = new DataSet();

                oQuery.Append("call sp_insert_loginpartner(:password,:last_login,:is_superuser,:loginPartner,:firtsName,:secondName,:lastdName,:fullName,:email,:number_identification,:is_staff,:is_active,:date_joined)");


                if(oLogin.password != null)
                {
                    byte[] encryptado = System.Text.Encoding.Unicode.GetBytes(oLogin.password);
                    oLogin.password = Convert.ToBase64String(encryptado);
                    oParam.Add(":password", oLogin.password);
                }

                
                if (oLogin.last_login != DateTime.MinValue)
                {
                    
                    oParam.Add(":last_login", oLogin.last_login);
                }
                else
                {
                    oParam.Add(":last_login", DBNull.Value);
                }

                oParam.Add(":is_superuser", oLogin.is_superuser);

                if (oLogin.loginPartner != null)
                {
                    oParam.Add(":loginPartner", oLogin.loginPartner);
                }

                oParam.Add(":firtsName", oLogin.firtsName);
                oParam.Add(":secondName", oLogin.secondName);
                oParam.Add(":lastdName", oLogin.lastdName);
                oParam.Add(":fullName", oLogin.fullName);
                oParam.Add(":email", oLogin.email);
                oParam.Add(":number_identification", oLogin.number_identification);
                oParam.Add(":is_staff", oLogin.is_staff);
                oParam.Add(":is_active", oLogin.is_active);

                if (oLogin.date_joined != DateTime.MinValue)
                {
                    oParam.Add(":date_joined", oLogin.date_joined);
                }
                else
                {
                    oParam.Add(":date_joined", DBNull.Value);
                }

                oAccesodataset = base.PostGresDataProvider.RecordSet(oQuery.ToString(), oParam);

                return int.MinValue;

            }
            finally
            {
                if (oQuery != null)
                {
                    oQuery = null;
                }
                if (oParam != null)
                {
                    oParam = null;
                }
                if (oAccesodataset != null)
                {
                    oAccesodataset.Dispose();
                    oAccesodataset = null;
                }
            }
        }
    }
}
