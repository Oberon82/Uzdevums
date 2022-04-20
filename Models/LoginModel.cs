using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Uzdevums.Models
{
    public class LoginModel
    {

        [Display(Name = "Lietotājs")]
        [Required(ErrorMessage = "Nav norādīts lietotājs")]
        [MaxLength(150)] 
        public string Name { get; set; }

        [Display(Name = "Parole")]
        [Required(ErrorMessage = "Nav norādīta parole")]
        [MaxLength(150)]
        public string Password { get; set; }
    }
}
