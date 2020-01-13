using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Entidades
{
    public class Security_LoginPartner
    {
        public String password { get; set; }
        public DateTime? last_login { get; set; }
        public Boolean is_superuser { get; set; }
        public String loginPartner { get; set; }
        public String firtsName { get; set; }
        public String secondName { get; set; }
        public String lastdName { get; set; }
        public String fullName { get; set; }
        public String email { get; set; }
        public String number_identification { get; set; }
        public Boolean is_staff { get; set; }
        public Boolean is_active { get; set; }
        public DateTime? date_joined { get; set; }

    }
}
