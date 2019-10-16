using System;
using System.ComponentModel.DataAnnotations;

namespace NomiProProject.ViewModel
{
    public class SearchEmployeeViewModel
    {
        [Display(Name = "Numero Documento")]
        public string Numero_Documento { get; set; }
       
    }
}