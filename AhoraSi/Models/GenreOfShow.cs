using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AhoraSi.Models
{
    public class GenreOfShow
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MovieOrSerie")]
        public int MovieOrSerieId { get; set; }

        public virtual MovieOrSerie MovieOrSerie { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
