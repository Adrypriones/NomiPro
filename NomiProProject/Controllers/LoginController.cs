using NomiProProject.Models;
using System.Linq;
using System.Web.Mvc;



namespace NomiProProject.Controllers
{
    public class LoginController : Controller
    {
        private NomiProEntities db = new NomiProEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Login/ResetPassword
        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Save([Bind(Include = "Nombre,Contrasena")] Usuario registro)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(registro);
                db.SaveChanges();
                return RedirectToAction("Register");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Authenticate([Bind(Include = "Nombre,Contrasena")] Usuario usuario)
        {
            Usuario user = db.Usuarios.FirstOrDefault(u => u.Nombre == usuario.Nombre && u.Contrasena == usuario.Contrasena);

            if (user != null)
            {
                return RedirectToAction("Index", "Employee");                
            }
            else
            {
                return RedirectToAction("Register", "Login");
            }
        }
    }
}
