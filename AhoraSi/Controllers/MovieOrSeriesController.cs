using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AhoraSi.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using AhoraSi.Models.ViewModels;

namespace AhoraSi.Controllers
{
    public class MovieOrSeriesController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MovieOrSeriesController(DataBaseContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: MovieOrSeries
        [AllowAnonymous]
        [HttpGet]
        [Route("Movies")]
        public async Task<IActionResult> Index(string title, [FromQuery] string order, [FromQuery] int? genreId)
        {
            ViewData["GetAllMovies"] = title;
            var query = from x in _context.MovieOrSerie select x;
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(x => x.Title.Contains(title));

                return View(await query.AsNoTracking().ToListAsync());
            }


            if (genreId != null) 
            {

                return View(_context.MovieOrSerie.Where(s => s.GenreOfShows.Any(x => x.GenreId == genreId))); 
            }



            if (order == "asc" || order == "ASC")
            {
                var movie = _context.MovieOrSerie;
                           
                return View(_context.MovieOrSerie.OrderBy(x=>x.DateBirth));
            }

            if(order== "desc" || order == "DESC")
            {
                var movie = _context.MovieOrSerie;

                return View(_context.MovieOrSerie.OrderByDescending(x => x.DateBirth));
            }

            return View(await _context.MovieOrSerie.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> OrderMovies(MovieOrSerie model)
        {
            if (ModelState.IsValid)
            {
                var query = from x in _context.MovieOrSerie select x;
                if (model.Order == 1)
                {
                    query = _context.MovieOrSerie.OrderBy(x => x.DateBirth);

                    return View(await query.AsNoTracking().ToListAsync());
                }
                else
                {
                    query = _context.MovieOrSerie.OrderByDescending(x => x.DateBirth);

                    return View(await query.AsNoTracking().ToListAsync());

                }
            }

            return RedirectToAction("Index", "Home");
        }



        // GET: MovieOrSeries/Details/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieOrSerie = await _context.MovieOrSerie
                .Include(x => x.CharacterOfShows)
                    .ThenInclude(x1 => x1.Character)
                .Include(y=>y.GenreOfShows)
                    .ThenInclude(y1 => y1.Genre)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (movieOrSerie == null)
            {
                return NotFound();
            }

            return View(movieOrSerie);
        }

        // GET: MovieOrSeries/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            
            var genreList = (from Genre in _context.Genre
                             select new SelectListItem()
                             {
                                 Text = Genre.Name,
                                 Value = Genre.Id.ToString()
                             }).ToList();

            genreList.Insert(0, new SelectListItem()
            {
                Text = "--- Genres ---",
                Value = string.Empty,
                Selected = false,
                Disabled = true
            });

           ViewBag.genreList = genreList;
            return View();
        }

        // POST: MovieOrSeries/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Title,DateBirth,Valoration,SelectedGenres")] MovieOrSerie movieOrSerie)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(movieOrSerie.ImageFile.FileName);
                string extension = Path.GetExtension(movieOrSerie.ImageFile.FileName);
                movieOrSerie.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await movieOrSerie.ImageFile.CopyToAsync(fileStream);
                }

                foreach (var genreIds in movieOrSerie.SelectedGenres)
                {
                    var genre = _context.Genre.Find(genreIds);
                    _context.GenreOfShow.Add(new GenreOfShow
                    {
                        GenreId = genre.Id,
                        MovieOrSerieId = movieOrSerie.Id,
                        MovieOrSerie = movieOrSerie,
                    });
                  
                }

                //Verificar si esto es necesario
                _context.Add(movieOrSerie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            

            _context.Add(movieOrSerie);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

            return View(movieOrSerie);
        }

        // GET: MovieOrSeries/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieOrSerie = await _context.MovieOrSerie.FindAsync(id);
            if (movieOrSerie == null)
            {
                return NotFound();
            }
            return View(movieOrSerie);
        }

        // POST: MovieOrSeries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Title,DateBirth,Valoration")] MovieOrSerie movieOrSerie)
        {
            if (id != movieOrSerie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieOrSerie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieOrSerieExists(movieOrSerie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movieOrSerie);
        }

        // GET: MovieOrSeries/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieOrSerie = await _context.MovieOrSerie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieOrSerie == null)
            {
                return NotFound();
            }

            return View(movieOrSerie);
        }

        // POST: MovieOrSeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieOrSerie = await _context.MovieOrSerie.FindAsync(id);
            _context.MovieOrSerie.Remove(movieOrSerie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieOrSerieExists(int id)
        {
            return _context.MovieOrSerie.Any(e => e.Id == id);
        }
    }
}



/*
 * if (title != null)
            {
                var movie = await _context.MovieOrSerie
                .FirstOrDefaultAsync(s => s.Title == title);

                if (movie == null)
                {
                    return NotFound();
                }

                return View("GetByTitle", movie);
            }
*/