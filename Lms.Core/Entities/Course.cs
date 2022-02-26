using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Length of the Title can't be more than 30")]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        public ICollection<Module> Modules { get; set; }

    }
}
