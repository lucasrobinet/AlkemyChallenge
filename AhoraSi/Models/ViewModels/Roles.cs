using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AhoraSi.Models.ViewModels
{
    public class Roles
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public string RolName { get; set; }
    }
}
