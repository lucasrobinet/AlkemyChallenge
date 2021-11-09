using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AhoraSi.Models
{
    public class DataBaseContext : IdentityDbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        public DbSet<Character> Character { get; set; }
        public DbSet<MovieOrSerie> MovieOrSerie { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<CharacterOfShow> CharacterOfShow { get; set; }
        public DbSet<GenreOfShow> GenreOfShow { get; set; }
    }
}
