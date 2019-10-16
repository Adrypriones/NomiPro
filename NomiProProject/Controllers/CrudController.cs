using System.Data.Entity;
using NomiProProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;



namespace NomiProProject.Controllers
{
    public class CrudController : Controller
    {
        private NomiProEntities db = new NomiProEntities();


        // GET: Crud/Details/5
        public ActionResult Details(int id)
        {            
            Empleado empleado = db.Empleadoes.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }


        public ActionResult Inactive(int id)
        {
            Empleado empleado = db.Empleadoes.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }


        // GET: Crud/Edit/5
        public ActionResult Edit(int id)
        {
            var empleado = db.Empleadoes.FirstOrDefault(e => e.ID_Empleado == id);
            return View(empleado);
        }

        // POST: Crud/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_Empleado, Documento, Numero_Documento, Nombre, Apellido, Genero, Estado")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListEmployee", "Employee");
            }

            return View(empleado);
        }




        // GET: Crud/Edit/5
        public ActionResult PositionEdit(int id)
        {
            var cargo = db.Cargoes.FirstOrDefault(e => e.ID_Cargo == id);
            return View(cargo);
        }

        // POST: Crud/Edit/5
        [HttpPost]
        public ActionResult PositionEdit([Bind(Include = "ID_Cargo, Descripción_Cargo, Rango_Minimo_Salario, Rango_Maximo_Salario")] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cargo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Position", "Position");
            }

            return View(cargo);
        }

    }
}
