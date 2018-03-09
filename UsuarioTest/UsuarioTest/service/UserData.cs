using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using UsuarioTest.dbConection;

namespace UsuarioTest.service
{
    public class UserData
    {

        public DataTable getData()
        {
            DataTable data = new DataTable();
            ExecuteScript obje = new ExecuteScript();
            data = obje.executeQuertyAndReturnDatatable("sp_getAllUser");
            return data;
        }
    }
}