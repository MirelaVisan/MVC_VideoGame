using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_VideoGames.Models;

namespace MVC_VideoGames.Data
{
    public class MVC_VideoGamesContext : DbContext
    {
        public MVC_VideoGamesContext (DbContextOptions<MVC_VideoGamesContext> options)
            : base(options)
        {
        }

        public DbSet<MVC_VideoGames.Models.VideoGame> VideoGame { get; set; } = default!;
        public DbSet<MVC_VideoGames.Models.VideoGame> Review { get; set; } = default!;
    }
}
