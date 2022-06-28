using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSearchService.Database
{
    /// <summary>
    /// DB Context class
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        /// <summary>
        /// FlightDetails Entity
        /// </summary>
        public DbSet<FlightDetails> FlightDetails { get; set; }
    }
}
