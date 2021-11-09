using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AhoraSi.Models
{
    public class CharacterOfShow
    {
        [Key]
        public int Id { get; set; }
        public int CharacterId { get; set; }

        [ForeignKey(nameof(CharacterId))]
        public virtual Character Character { get; set; }

        public int MovieOrSerieId { get; set; }

        [ForeignKey(nameof(MovieOrSerieId))]
        public virtual MovieOrSerie MovieOrSerie { get; set; }

    }
}