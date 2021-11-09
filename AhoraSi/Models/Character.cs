using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AhoraSi.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage ="Name can't be empty")]
        [StringLength(60)]
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public string History { get; set; }

        [NotMapped]
        public List<int> SelectedShow { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; }

        // public List<MovieOrSerie> MovieOrSeries { get; set; }

        public virtual ICollection<CharacterOfShow> CharacterOfShows { get; set; }

    }
}

//@Html.DisplayNameFor(model => model.ImageFile)