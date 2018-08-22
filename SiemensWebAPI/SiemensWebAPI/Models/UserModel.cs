using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models
{
    public class UserModel
    {
        public String ID { get; set; }
        public String Username { get
            {
                return Username;
            } }
        public String Type {
            get
            {
                return Type;
            }}
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String CNP { get; set; }
        public String Password {
            get
            {
                return Password;
            }
        }
    }
}