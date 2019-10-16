using NomiProProject.Models;
using NomiProProject.Shared.Enums;
using NomiProProject.ViewModel;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;
using System.Reflection;

namespace NomiProProject.Controllers
{

    public class RosterController : Controller
    {
        private NomiProEntities db = new NomiProEntities();


        public ActionResult Index(int id)
        {
            Empleado empleado = db.Empleadoes.Find(id);

            if (empleado == null)
            {
                return HttpNotFound();
            }

            NominaViewModel nomina = new NominaViewModel();
            var cargoEmpleado = empleado.Cargo_Empleado.FirstOrDefault();

            nomina.Nombre = empleado.Nombre;
            nomina.Apellido = empleado.Apellido;
            nomina.Numero_Documento = empleado.Numero_Documento;
            nomina.Salario_Basico = cargoEmpleado.Salario_Basico;
            nomina.Cargo = cargoEmpleado.Cargo.Descripción_Cargo;
            nomina.Jornada = cargoEmpleado.Jornada.Nombre;            
            nomina.ID_Empleado = empleado.ID_Empleado;
            

            if (cargoEmpleado.TipoContrato == ContractType.ContratoIndefinido.ToString())
            {
                nomina.Aporte_Salud = nomina.Salario_Basico * (decimal)0.04;
                nomina.Aporte_Pension = nomina.Salario_Basico * (decimal)0.04;
            }

            if (nomina.Salario_Basico <= 1656232)
            {
                nomina.AuxilioTransporte += 97032;
            }

            if (TempData["Error"] != null && !string.IsNullOrEmpty(TempData["Error"].ToString()))
            {
                ViewBag.Error = TempData["Error"].ToString();
            }

            return View(nomina);
        }

        public ActionResult Guardar([Bind(Include = "Total_Horas_Laboradas,Extras_Diurnas,Extras_Nocturnas,Fecha_Inicial_Pago, Fecha_Final_Pago,ID_Empleado,Aporte_Salud,Aporte_Pension")] NominaViewModel infonomina)
        {
            if (ModelState.IsValid)
            {
                var fecha_inicio_empleado = db.Empleadoes.FirstOrDefault(e => e.ID_Empleado == infonomina.ID_Empleado).Cargo_Empleado.FirstOrDefault().Fecha_Inicio;

                Nomina nomina = new Nomina
                {
                    Total_Horas_Laboradas = infonomina.Total_Horas_Laboradas,
                    Extras_Diurnas = infonomina.Extras_Diurnas,
                    Extras_Nocturnas = infonomina.Extras_Nocturnas,
                    Fecha_Inicial_Pago = infonomina.Fecha_Inicial_Pago,
                    Fecha_Final_Pago = infonomina.Fecha_Final_Pago,
                    ID_Empleado = infonomina.ID_Empleado,
                    Aporte_Salud = infonomina.Aporte_Salud,
                    Aporte_Pension = infonomina.Aporte_Pension
                };

                if(fecha_inicio_empleado > infonomina.Fecha_Inicial_Pago)
                {
                    TempData["Error"] = "FUERA DEL RANGO DE PAGO";

                    return RedirectToAction("Index", new { id = infonomina.ID_Empleado });
                }

                db.Nominas.Add(nomina);
                db.SaveChanges();

                return RedirectToAction("ListEmployee", "Employee");
            }           

            return View("ListEmployee", "Employee");
        }

        private string MapPath(string seedFile)
        {
            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath; //was AbsolutePath but didn't work with spaces according to comments
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }

        public ActionResult Imprimir(int id)
        {
            var empleado = db.Empleadoes.FirstOrDefault(e => e.ID_Empleado == id);
            var cargoEmpleado = empleado.Cargo_Empleado.FirstOrDefault();
            var nomina = empleado.Nominas.FirstOrDefault();

            decimal recargo = 0;
            decimal auxilioTransporte = 0;

            if (cargoEmpleado.ID_Jornada == (int)WorkingDay.Night)
            {
                recargo = CalcularRecargoNocturno(cargoEmpleado.Salario_Basico, nomina.Extras_Nocturnas);
            }
            

            if (cargoEmpleado.Salario_Basico <= 1656232)
            {
                auxilioTransporte += 97032;
            }

            var horasextrasdiurnas = CalcularExtrasDiurnas(cargoEmpleado.Salario_Basico, nomina.Extras_Diurnas);

            var TotalDevengado = cargoEmpleado.Salario_Basico + recargo + horasextrasdiurnas + auxilioTransporte
                - empleado.Nominas.FirstOrDefault().Aporte_Salud - empleado.Nominas.FirstOrDefault().Aporte_Pension;

            

            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfWriter.GetInstance(doc, new FileStream($"{MapPath("~/Pdf")}/hola.pdf", FileMode.Create));
            doc.Open();

            Paragraph title = new Paragraph();
            title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLUE);
            title.Add("NomiProSA");
            doc.Add(title); doc.Add(new Paragraph("NIT: 900565456-9"));
            doc.Add(new Paragraph("CORREO:  nomiprosa@gmail.com"));
            doc.Add(new Paragraph("Direccion:  Calle 8 sur # 78-67"));
            doc.Add(new Paragraph("-------------------------------------------------------------------------------------------------------------------------------"));

            doc.Add(new Paragraph($"Empleado: {empleado.Apellido} {empleado.Nombre}"));
            doc.Add(new Paragraph("Documento de Identidad:" + empleado.Numero_Documento));
            doc.Add(new Paragraph("Básico:" + empleado.Cargo_Empleado.FirstOrDefault().Salario_Basico));
            doc.Add(new Paragraph("Cargo:" + cargoEmpleado.Cargo.Descripción_Cargo));
            doc.Add(new Paragraph("Cargo:" + cargoEmpleado.TipoContrato));
            doc.Add(new Paragraph("Jornada:" + cargoEmpleado.Jornada.Nombre));
            doc.Add(new Paragraph("Periodo de Liquidacion Inicial:" + empleado.Nominas.LastOrDefault().Fecha_Inicial_Pago));
            doc.Add(new Paragraph("Periodo de Liquidacion Final:" + empleado.Nominas.LastOrDefault().Fecha_Final_Pago));

            doc.Add(new Paragraph("-------------------------------------------------------------------------------------------------------------------------------"));

            
            doc.Add(new Paragraph("Recibo de pago"));
            doc.Add(new Paragraph("========================================================================="));


            PdfPTable table = new PdfPTable(9);

            table.AddCell("Basico");
            table.AddCell("Total Horas");
            table.AddCell("Auxi.Transporte");
            table.AddCell("Salud");
            table.AddCell("Pension");
            table.AddCell("Extras Dia");
            table.AddCell("Extras Noche");
            table.AddCell("Recargo");
            table.AddCell("Neto");






            table.AddCell("" + empleado.Cargo_Empleado.FirstOrDefault().Salario_Basico);
            table.AddCell("" + empleado.Nominas.LastOrDefault().Total_Horas_Laboradas);
            table.AddCell("" + auxilioTransporte);
            table.AddCell("" + nomina.Aporte_Salud);
            table.AddCell("" + nomina.Aporte_Pension);
            table.AddCell("" + nomina.Extras_Diurnas);
            table.AddCell("" + nomina.Extras_Nocturnas);
            table.AddCell("" + recargo);
            table.AddCell("" + TotalDevengado);

           
            doc.Add(table);
            doc.Close();




            // db.Nominas.Add(imprimir);
            //  db.SaveChanges();
            //  return RedirectToAction("Index", "Roster");

            //byte[] fileBytes = System.IO.File.ReadAllBytes($"{MapPath("~/Pdf")}/hola.pdf");
            //string fileName = "myfile.pdf";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            return Redirect("/Pdf/hola.pdf");
        }


        private decimal CalcularRecargoNocturno(decimal Salario_Basico, int? Horas_Extras = 0)
        {
            decimal vhora = Salario_Basico / 240;
            var recarnoc = vhora * (decimal)0.35;
            var extrat = vhora * (decimal)0.75;
            
            return recarnoc + (extrat * (Horas_Extras != null ? (int)Horas_Extras : 0));
        }

        private decimal CalcularExtrasDiurnas(decimal Salario_Basico, int? Extras_Diurnas = 0)
        {
            decimal vhora = Salario_Basico / 240;
            var extraDia = vhora * (decimal)0.25;
            var totalExtra  = extraDia * (Extras_Diurnas != null ? (int)Extras_Diurnas : 0);

            return totalExtra;
        }
    }
}
