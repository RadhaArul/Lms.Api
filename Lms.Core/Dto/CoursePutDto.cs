using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    public class CoursePutDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Length of the Title can't be more than 20")]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
    }
}
