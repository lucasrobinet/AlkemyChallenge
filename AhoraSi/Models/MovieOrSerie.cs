using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AhoraSi.Models
{
    public class MovieOrSerie
    {
        [Key]
        public int Id { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Title can't be empty")]
        [StringLength(60)]
        public string Title { get; set; }
        public DateTime DateBirth { get; set; }
        [Range(0,5)]
        [Required(ErrorMessage = "Valoration is required")]
        public int Valoration { get; set; }

        [NotMapped]
        public List<int> SelectedGenres { get; set; }

        public List<Genre> generos { get; set; }

        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile ImageFile { get; set; }
        public virtual ICollection<CharacterOfShow> CharacterOfShows { get; set; }
        public virtual ICollection<GenreOfShow> GenreOfShows { get; set; }

        public int Order { get; set; }

    }
}
