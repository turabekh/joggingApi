using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.JoggingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class JoggingRepository : IJoggingRepository
    {
        private DataContext _context;
        public JoggingRepository(DataContext context)
        {
            _context = context;
        }

        public void CreateJogging(Jogging jogging)
        {
            _context.Add(jogging);
        }

        public void DeleteJogging(Jogging jogging)
        {
            _context.Remove(jogging);
        }

        public async Task<IEnumerable<Jogging>> GetAllJoggings()
        {
            return await _context.Joggings
                .Include(j => j.User)
                .OrderBy(j => j.JoggingDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Jogging> GetJoggingById(int id)
        {
            return await _context.Joggings
                .Where(j => j.Id.Equals(id))
                .Include(j => j.User)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Jogging>> GetJoggingsByUserId(int userId)
        {
            return await _context.Joggings
                .Where(j => j.UserId.Equals(userId))
                .Include(j => j.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Jogging>> GetJoggingsByUsername(string userName)
        {
            return await _context.Joggings
                .Include(j => j.User)
                .Where(j => j.User.UserName == userName)
                .ToListAsync();
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateJogging(Jogging jogging)
        {
            _context.Update(jogging);
        }
    }
}
