using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AhoraSi.Models.ViewModels
{
    public class EditRoles
    {
        public string Id { get; set; }
        public EditRoles()
        {
            Users = new List<string>();
        }

        [Required]
        public string RolName { get; set; }
        public List<string> Users { get; set; }
    }
}
