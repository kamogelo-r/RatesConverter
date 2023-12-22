using System;
using Microsoft.EntityFrameworkCore;

namespace RatesConverter.Models
{
    public class RatesDbContext:DbContext
    {
        public DbSet<Rate>? Rates { get; set; }

        public RatesDbContext(DbContextOptions<RatesDbContext> options) : base (options) { }
    }
}

