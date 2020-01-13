using System;
using System.Configuration;


namespace BoxFashion.Core.Common
{
    public static class Utility 
    {
        public static string GetKey(string key)
        {
            if (System.Configuration.ConfigurationManager.AppSettings[key] == null)
            {
                throw new ArgumentNullException(" La LLave " + key + " no fue encontrado en el archivo de configuracion");
            }
            return @ConfigurationManager.AppSettings[key];
        }
    }
}
