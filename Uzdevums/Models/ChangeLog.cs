using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Uzdevums.Models
{
    public class ChangeLog
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }

#nullable enable
        public string? OldValue { get; set; }

#nullable enable
        public string? NewValue { get; set; }
    }
}
