using Microsoft.EntityFrameworkCore;
using ParkAPI_1030.Data;
using ParkAPI_1030.Models;
using ParkAPI_1030.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI_1030.Repository
{
    public class TrailRepository : iTrailRepository
    {
        private readonly ApplicationDbContext _context;
        public TrailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateTrail(Trail trail)
        {
            _context.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _context.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _context.Trails.Find(trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _context.Trails.Include(t=>t.NationalPark).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _context.Trails.Include(t => t.NationalPark).Where(t => t.NationalParkId == nationalParkId).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool TrailExists(int trailId)
        {
            return _context.Trails.Any(t => t.Id == trailId);
        }

        public bool TrailExists(string trailName)
        {
            return _context.Trails.Any(t => t.Name == trailName);
        }

        public bool UpdateTrail(Trail trail)
        {
            _context.Trails.Update(trail);
            return Save();
        }
    }
}
