


namespace BoxFashion.Core.Common
{
    public static class Configuracion
    {
        public static string ConnectionString
        {
            get
            {
                return Common.Utility.GetKey("DefaultConnection");
            }
        }
    }
}
