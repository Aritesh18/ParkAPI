using Microsoft.EntityFrameworkCore;
using ParkAPI_1030.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI_1030.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet <Trail>Trails { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
