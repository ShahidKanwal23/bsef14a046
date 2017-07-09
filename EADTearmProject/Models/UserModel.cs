using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EADTearmProject.Models
{
    public class UserModel
    {
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public int user_type { get; set; }
    
    }
}