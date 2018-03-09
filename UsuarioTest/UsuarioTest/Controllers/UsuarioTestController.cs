using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsuarioTest.service;

namespace UsuarioTest.Controllers
{
    public class UsuarioTestController : Controller
    {
        // GET: UsuarioTest
        public ActionResult UsuarioTestMaintenance()
        {
            UserData data = new UserData();
            data.getData();
            return View();
        }
    }
}