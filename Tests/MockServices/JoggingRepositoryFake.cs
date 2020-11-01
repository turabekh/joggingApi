using Interfaces;
using Models.JoggingModels;
using Models.RequestParams;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.MockServices
{
    public class JoggingRepositoryFake : IJoggingRepository
    {
        private List<Jogging> _joggings;

        public JoggingRepositoryFake()
        {
            _joggings = Helper.GetJoggings();
        }

        public void CreateJogging(Jogging jogging)
        {
            jogging.Id = _joggings.LastOrDefault().Id + 1;
            _joggings.Add(jogging);
        }

        public void DeleteJogging(Jogging jogging)
        {
            _joggings.Remove(jogging);
        }

        public async Task<PagedList<Jogging>> GetAllJoggings(JoggingParameters joggingParameters)
        {
            return PagedList<Jogging>.ToPagedList(_joggings, 1, 10);
        }

        public async Task<Jogging> GetJoggingById(int id)
        {
            return _joggings.FirstOrDefault(j => j.Id == id);
        }
        public async Task<IEnumerable<Jogging>> GetJoggingsByUserId(int userId)
        {
            return _joggings.Where(j => j.UserId == userId);
        }

        public async Task<PagedList<Jogging>> GetJoggingsByUserId(int userId, JoggingParameters joggingParameters)
        {
            var userJoggings = _joggings.Where(j => j.UserId == userId);
            return PagedList<Jogging>.ToPagedList(userJoggings, 1, 10);
        }

        public async Task<PagedList<Jogging>> GetJoggingsByUsername(string userName, JoggingParameters joggingParameters)
        {
            var userJoggings = _joggings.Where(j => j.UserId == 2002);
            return PagedList<Jogging>.ToPagedList(userJoggings, 1, 10);
        }

        public PagedList<WeekSummary> GetWeeklyReports(IEnumerable<Jogging> joggings, ReportParameters reportParameters)
        {
            List<WeekSummary> weekSummaries = new List<WeekSummary>();
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var yearlyLookup = _joggings.ToLookup(j => j.JoggingDate.Year, j => j);
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

            return PagedList<WeekSummary>.ToPagedList(weekSummaries, reportParameters.PageNumber, reportParameters.PageSize);
        }

        public void Save()
        {
            Console.WriteLine("Saved");
        }

        public void UpdateJogging(Jogging jogging)
        {
            var joggingFromList = _joggings.Where(j => j.Id == jogging.Id).FirstOrDefault();
            joggingFromList = jogging;
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
