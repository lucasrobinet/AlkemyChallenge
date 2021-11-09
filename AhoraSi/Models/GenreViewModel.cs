using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AhoraSi.Models
{
    public class GenreViewModel
    {
        [DisplayName("Genre")]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> Genress { get; set; }
    }
}
