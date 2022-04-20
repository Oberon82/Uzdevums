using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Uzdevums.Models
{
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        [MaxLength(24)]
        public string Salt { get; set; }

        [MaxLength(64)]
        public string PasswordHash { get; set; }
    }
}
