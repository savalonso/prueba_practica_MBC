using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsuarioTest.Models
{
    public class User
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}