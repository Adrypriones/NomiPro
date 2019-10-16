
using System;

using System.ComponentModel.DataAnnotations;

namespace NomiProProject.ViewModel
{
    public class NominaViewModel
    {
        [Display(Name = "Total Horas Laboradas")]
        public int Total_Horas_Laboradas { get; set; }

        [Display(Name = "Extras Diurnas")]
        public int? Extras_Diurnas { get; set; }

        [Display(Name = "Extras Nocturnas")]
        public int? Extras_Nocturnas { get; set; }

        [Display(Name = "Fecha Pago Inicial")]
        public DateTime Fecha_Inicial_Pago { get; set; }

        [Display(Name = "Fecha Pago Final")]
        public DateTime Fecha_Final_Pago { get; set; }

        [Display(Name = "Aporte Salud")]
        public decimal Aporte_Salud { get; set; }

        [Display(Name = "Aporte Pension")]
        public decimal Aporte_Pension { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [Display(Name = "Numero Documento")]
        public string Numero_Documento { get; set; }
        public string Documento { get; set; }

        [Display(Name = "Salario Basico")]
        public decimal Salario_Basico { get; set; }
        public string Cargo { get; set; }

        [Display(Name = "Aux. Transporte")]
        public Int32 AuxilioTransporte { get; set; }
        public string Jornada { get; set; }
        public int ID_Empleado { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime Fecha_Inicio { get; set; }


    }
}