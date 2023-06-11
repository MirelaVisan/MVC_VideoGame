using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_VideoGames.Data;
using MVC_VideoGames.Models;

namespace MVC_VideoGames.Controllers
{
    public class VideoGamesController : Controller
    {
        private readonly MVC_VideoGamesContext _context;

        public VideoGamesController(MVC_VideoGamesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = sortOrder == "title_desc" ? "title" : "title_desc";
            ViewData["ReleaseDateSortParm"] = sortOrder == "release_date" ? "release_date_desc" : "release_date";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var games = from s in _context.VideoGame
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title":
                    games = games.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    games = games.OrderByDescending(s => s.Title);
                    break;
                case "price":
                    games = games.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    games = games.OrderByDescending(s => s.Price);
                    break;
                case "release_date":
                    games = games.OrderBy(s => s.ReleaseDate);
                    break;
                case "release_date_desc":
                    games = games.OrderByDescending(s => s.ReleaseDate);
                    break;
                default:
                    games = games.OrderBy(s => s.Rating);
                    break;
            }
            int pageSize = 3;

            return View(await PaginatedList<VideoGame>.CreateAsync(games.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGame
                .Include(v => v.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (videoGame == null)
            {
                return NotFound();
            }

            return View(videoGame);
        }


        // GET: VideoGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VideoGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating,IsMultiplayer")] VideoGame videoGame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoGame);
        }

        // GET: VideoGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VideoGame == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGame.FindAsync(id);
            if (videoGame == null)
            {
                return NotFound();
            }
            return View(videoGame);
        }

        // POST: VideoGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating,IsMultiplayer")] VideoGame videoGame)
        {
            if (id != videoGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGameExists(videoGame.Id))
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
            return View(videoGame);
        }

        // GET: VideoGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VideoGame == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoGame == null)
            {
                return NotFound();
            }

            return View(videoGame);
        }

        // POST: VideoGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VideoGame == null)
            {
                return Problem("Entity set 'MVC_VideoGamesContext.VideoGame'  is null.");
            }
            var videoGame = await _context.VideoGame.FindAsync(id);
            if (videoGame != null)
            {
                _context.VideoGame.Remove(videoGame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoGameExists(int id)
        {
          return (_context.VideoGame?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int videoGameId, [Bind("Rating,Content,UserName")] Review review)
        {
            var videoGame = await _context.VideoGame
                .Include(v => v.Reviews)
                .FirstOrDefaultAsync(v => v.Id == videoGameId);

            if (videoGame == null)
            {
                return NotFound();
            }

            try
            {
                videoGame.Reviews.Add(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.FirstOrDefault();
                if (entry != null)
                {
                    await entry.ReloadAsync();
                    videoGame.Reviews.Add(review);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = videoGameId });
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", new { id = videoGameId });
        }



    }
}
