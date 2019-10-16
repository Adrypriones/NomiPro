using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NomiProProject.ViewModel
{
    public class EmployeeInformationViewModel
    {
        [Display(Name = "Numero Documento")]
        public String Numero_Documento { get; set; }
        
        public string Nombre { get; set; }
        public string Apellido { get; set; }      
        public int Id_Empleado { get; set; }       

        [Display(Name = "Cargo")]
        public string Id_Cargo { get; set; }
        public IEnumerable<SelectListItem> Cargos { get; set; }

        [Display(Name = "Jornada")]
        public int Id_Jornada { get; set; }
        public IEnumerable<SelectListItem> Jornada { get; set; }

        [Display(Name = "Basico")]
        public decimal Salario_Basico { get; set; }
        [Display(Name = "Tipo Contrato")]
        public string TipoContrato { get; set; }

        [Display(Name = "Fecha Inicio")]
        public DateTime Fecha_Inicio { get; set; }

        [Display(Name = "Fecha Terminacion")]
        public DateTime Fecha_Terminacion { get; set; }


    }
}