using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.JoggingModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            List<WeekSummary> weekSummaries = new List<WeekSummary>();
            if (joggings.Count() < 1)
            {
                return weekSummaries;
            }
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var yearlyLookup = joggings.ToLookup(j => j.JoggingDate.Year, j => j);
            int i = 1;
            foreach (var gr in yearlyLookup.OrderBy(y => y.Key))
            {
                var weeklyLookup = gr.ToLookup(g => myCal.GetWeekOfYear(g.JoggingDate, myCWR, myFirstDOW), g => g);
                foreach (var week in weeklyLookup.OrderBy(w => w.Key))
                {
                    var weekSummary = GetWeekSummary(week.ToList());
                    weekSummary.WeekNumber = i;
                    weekSummaries.Add(weekSummary);
                    i++;
                }
            }
            return weekSummaries;

        }

        private WeekSummary GetWeekSummary(List<Jogging> joggings)
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
                AvgTimeInMinutes = avgTime,
                AvgDistanceInMeters = avgDistance,
                AvgSpeedMeterPerMinute = avgSpeed,
                WeekDates = dateList
            };
        }
    }

}
