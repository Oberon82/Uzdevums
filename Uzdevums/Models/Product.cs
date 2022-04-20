using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Uzdevums.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Nosaukums")]
        [Required(ErrorMessage = "Nav norādīts nosaukums")]
        [MaxLength(250)]
        public string Name { get; set; }
        
        [Display(Name = "Vienību skaits")]
        [Required(ErrorMessage = "Nav norādīts vienību skaits")]
        public int NumberOfUnits { get; set; }
        
        [Display(Name = "Cena par vienu vienību")]
        [Required(ErrorMessage = "Nav norādīta vienas vienības cena")]
        public decimal PricePerUnit { get; set; }
    }
}
