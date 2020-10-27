using Models.JoggingModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IJoggingRepository
    {
        Task<IEnumerable<Jogging>> GetAllJoggings();
        Task<Jogging> GetJoggingById(int id);
        Task<IEnumerable<Jogging>> GetJoggingsByUserId(int userId);
        Task<IEnumerable<Jogging>> GetJoggingsByUsername(string userName);
        void CreateJogging(Jogging jogging);
        void UpdateJogging(Jogging jogging);
        void DeleteJogging(Jogging jogging);
        List<WeekSummary> GetWeeklyReports(IEnumerable<Jogging> joggings);
        void Save();


    }
}
