using NomiProProject.Models;
using System.Linq;
using System.Web.Mvc;
using NomiProProject.ViewModel;



namespace NomiProProject.Controllers
{
    public class EmployeeController : Controller
    {
        private NomiProEntities db = new NomiProEntities();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crear([Bind(Include = "Documento,Numero_Documento,Nombre,Apellido,Genero,Estado")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Empleadoes.Add(empleado);
                db.SaveChanges();
                return RedirectToAction("NewEmployee");
            }

            return View(empleado);
        }

        public ActionResult NewEmployee()
        {
            return View();
        }



        // GET: Employee/Details/5
        public ActionResult ListEmployee()
        {
            var a = db.Empleadoes.ToList();
            return View(a);
        }

        // GET: Employee/Create
        public ActionResult SearchEmployee()
        {
            if (TempData["Error"] != null && !string.IsNullOrEmpty(TempData["Error"].ToString()))
            {
                ViewBag.Error = TempData["Error"].ToString();
            }

            return View();
        }       
              

        public ActionResult Buscar([Bind(Include = "Numero_Documento")] SearchEmployeeViewModel searchEmployeeViewModel)
        {
            Empleado empleado = db.Empleadoes.FirstOrDefault(e => e.Numero_Documento == searchEmployeeViewModel.Numero_Documento);
            if (empleado == null)
            {
                TempData["Error"] = "EMPLEADO NO ENCONTRADO"; 

                return RedirectToAction("SearchEmployee");
            }

            return RedirectToAction("Information", "WorkingInformation", new { searchEmployeeViewModel.Numero_Documento });
        }


    }
}
