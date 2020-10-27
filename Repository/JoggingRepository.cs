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


        public List<WeekSummary> GetWeeklyReports(IEnumerable<Jogging> joggings)
        {
            joggings = joggings.OrderBy(j => j.JoggingDate);
            if (joggings.Count() < 1)
            {
                return new List<WeekSummary>();
            }
            var temp_date = (int)joggings.FirstOrDefault().JoggingDate.DayOfWeek;
            var temp_list = new List<Jogging>();
            var resultList = new List<WeekSummary>();
            int i = 1;
            foreach(var j in joggings)
            {
                if ((int)j.JoggingDate.DayOfWeek >= temp_date)
                {
                    temp_list.Add(j);
                }
                else
                {
                    resultList.Add(GetWeekSummary(temp_list, i));
                    i++;
                    temp_list.Clear();
                    temp_list.Add(j);
                    temp_date = (int)j.JoggingDate.DayOfWeek;
                }
            }
            if (temp_list.Count() > 0)
            {
                resultList.Add(GetWeekSummary(temp_list, i));
            }
            return resultList.OrderBy(r => r.WeekNumber).ToList();

        }

        private WeekSummary GetWeekSummary(List<Jogging> joggings, int weekNumber)
        {
            if (joggings.Count() < 1)
            {
                return null;
            }
            var userId = joggings.FirstOrDefault().UserId;
            var avgTime = joggings.Average(j => j.JoggingDurationInMinutes);
            var avgDistance = joggings.Average(j => j.DistanceInMeters);
            var avgSpeed = avgDistance / avgTime;
            var dateList = joggings.Select(j => j.JoggingDate).ToList();
            return new WeekSummary
            {
                UserId = userId,
                WeekNumber = weekNumber,
                AvgTimeInMinutes = avgTime,
                AvgDistanceInMeters = avgDistance,
                AvgSpeedMeterPerMinute = avgSpeed, 
                WeekDates = dateList

            };
        }
    }

}
