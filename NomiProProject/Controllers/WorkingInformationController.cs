using NomiProProject.Models;
using NomiProProject.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NomiProProject.Controllers
{
    public class WorkingInformationController : Controller
    {
        private NomiProEntities db = new NomiProEntities();

        // GET: WorkingInformation
        public ActionResult Information(string numero_documento)
        {
            Empleado empleado = db.Empleadoes.FirstOrDefault(e => e.Numero_Documento == numero_documento);

            EmployeeInformationViewModel employeeInformationViewModel = new EmployeeInformationViewModel();

            List<SelectListItem> cargos = new List<SelectListItem>();
            List<SelectListItem> jornada = new List<SelectListItem>();

            var Cargos = db.Cargoes.ToList();

            Cargos.ForEach(c =>
            cargos.Add(new SelectListItem {
                Text = c.Descripción_Cargo,
                Value = c.ID_Cargo.ToString()
            }));

            var Jornada = db.Jornadas.ToList();

            Jornada.ForEach(j =>
            jornada.Add(new SelectListItem
            {
                Text = j.Nombre,
                Value = j.ID_Jornada.ToString()
            }));

            employeeInformationViewModel.Jornada = jornada;
            employeeInformationViewModel.Cargos = cargos;
            employeeInformationViewModel.Id_Empleado = empleado.ID_Empleado;
            employeeInformationViewModel.Numero_Documento = numero_documento;
            employeeInformationViewModel.Nombre = empleado.Nombre;
            employeeInformationViewModel.Apellido = empleado.Apellido;

            return View(employeeInformationViewModel);
        }

        public ActionResult Nombre(string Numero_Documento )
        {
           
            Empleado empleado = db.Empleadoes.Find(Numero_Documento);
            if (Numero_Documento == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // GET: WorkingInformation/Create
        public ActionResult Create()
        {
            ViewBag.ID_Nomina = new SelectList(db.Empleadoes, "ID_Empleado", "Documento");
            return View();
        }

        // POST: WorkingInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Nomina,Total_Horas_Laboradas,Extras_Diurnas,Extras_Nocturnas,Fecha_Inicial_Pago,Fecha_Final_Pago")] Nomina nomina)
        {
            if (ModelState.IsValid)
            {
                db.Nominas.Add(nomina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Nomina = new SelectList(db.Empleadoes, "ID_Empleado", "Documento", nomina.ID_Nomina);
            return View(nomina);
        }

        // GET: WorkingInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nomina nomina = db.Nominas.Find(id);
            if (nomina == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Nomina = new SelectList(db.Empleadoes, "ID_Empleado", "Documento", nomina.ID_Nomina);
            return View(nomina);
        }

        // POST: WorkingInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Nomina,Total_Horas_Laboradas,Extras_Diurnas,Extras_Nocturnas,Fecha_Inicial_Pago,Fecha_Final_Pago")] Nomina nomina)
        {
            EmployeeInformationViewModel employeeInformationViewModel = new EmployeeInformationViewModel();

            if (ModelState.IsValid)
            {
                db.Entry(nomina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Nomina = new SelectList(db.Empleadoes, "ID_Empleado", "Documento", nomina.ID_Nomina);
            return View(employeeInformationViewModel);
        }

        // GET: WorkingInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nomina nomina = db.Nominas.Find(id);
            if (nomina == null)
            {
                return HttpNotFound();
            }
            return View(nomina);
        }

        // POST: WorkingInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nomina nomina = db.Nominas.Find(id);
            db.Nominas.Remove(nomina);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Guardar([Bind(Include = "Salario_Basico,Nombre,TipoContrato,Fecha_Inicio,Id_Cargo,Id_jornada,Id_Empleado")] EmployeeInformationViewModel information)
        {
            if (ModelState.IsValid)
            {
                Cargo_Empleado cargo_Empleado = new Cargo_Empleado
                {
                    ID_Empleado = information.Id_Empleado,
                    ID_Cargo = int.Parse(information.Id_Cargo),
                    Fecha_Inicio = information.Fecha_Inicio,
                    Salario_Basico = information.Salario_Basico,
                    TipoContrato = information.TipoContrato,
                    ID_Jornada = information.Id_Jornada
                };

                db.Cargo_Empleado.Add(cargo_Empleado);
                db.SaveChanges();
                return RedirectToAction("SearchEmployee","Employee");
            }

            return View("Information");
        }
    }
}