using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BirdwatcherWebsite.Models;

namespace BirdwatcherWebsite.Data
{
    public class BirdwatcherWebsiteContext : DbContext
    {
        public BirdwatcherWebsiteContext (DbContextOptions<BirdwatcherWebsiteContext> options)
            : base(options)
        {
        }

        public DbSet<BirdwatcherWebsite.Models.Picture> Picture { get; set; } = default!;
    }
}
