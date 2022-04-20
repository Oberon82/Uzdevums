using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uzdevums.Models
{
    public class ChangeLog
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UserUserId { get; set; }

        public int ProductId { get; set; }

        public string ChangeText { get; set; }
    }
}
