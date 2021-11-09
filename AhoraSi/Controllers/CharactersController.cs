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

namespace AhoraSi.Controllers
{
    public class CharactersController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public CharactersController(DataBaseContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Characters
        [AllowAnonymous]
        [HttpGet]
        [Route("Characters")]
        public async Task<IActionResult> Index(string name, [FromQuery] int ? age, [FromQuery] int? movies)
        {


            ViewData["GetAllMovies"] = name;
            var query = from x in _context.Character select x;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));

                return View(await query.AsNoTracking().ToListAsync());
            }

            if (age != null)
              {
                  var character = await _context.Character
                  .FirstOrDefaultAsync(s => s.Age == age);

                  if (character == null)
                  {
                      return NotFound();
                  }

                  return View("GetByName", character);
              }

            if (movies != null)
            {

                return View(_context.Character.Where(s => s.CharacterOfShows.Any(x => x.MovieOrSerieId == movies)));
            }


            return View(await _context.Character.ToListAsync());
        }



        // GET: Characters/Details/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var character = await _context.Character
                .Include(x => x.CharacterOfShows)
                .ThenInclude(y => y.MovieOrSerie)
                .SingleOrDefaultAsync(m => m.Id == id);
                
                
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // GET: Characters/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var showList = (from MovieOrSerie in _context.MovieOrSerie
                             select new SelectListItem()
                             {
                                 Text = MovieOrSerie.Title,
                                 Value = MovieOrSerie.Id.ToString()
                             }).ToList();

            showList.Insert(0, new SelectListItem()
            {
                Text = "--- Movie or Serie ---",
                Value = string.Empty,
                Selected = false,
                Disabled = true
            });

            ViewBag.showList = showList;
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Amin")]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Age,Weight,History,SelectedShow")] Character character)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/images
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(character.ImageFile.FileName);
                string extension = Path.GetExtension(character.ImageFile.FileName);
                character.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    await character.ImageFile.CopyToAsync(fileStream);
                }

                foreach (var showIds in character.SelectedShow)
                {
                    var show = _context.MovieOrSerie.Find(showIds);
                    _context.CharacterOfShow.Add(new CharacterOfShow
                    {
                        MovieOrSerieId = show.Id,
                        CharacterId = character.Id,
                        Character = character,
                    });

                }
                //Insert record
                _context.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(character);
        }

        // GET: Characters/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Name,Age,Weight,History")] Character character)
        {
            if (id != character.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.Id))
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
            return View(character);
        }

        // GET: Characters/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Character
                .FirstOrDefaultAsync(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Character.FindAsync(id);

            //delete image from wwwRoot/Image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", character.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            //deleted the record
            _context.Character.Remove(character);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Character.Any(e => e.Id == id);
        }
    }
}