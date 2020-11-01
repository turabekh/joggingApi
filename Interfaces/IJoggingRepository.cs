using Models.JoggingModels;
using Models.RequestParams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IJoggingRepository
    {
        Task<PagedList<Jogging>> GetAllJoggings(JoggingParameters joggingParameters);
        Task<Jogging> GetJoggingById(int id);
        Task<PagedList<Jogging>> GetJoggingsByUserId(int userId, JoggingParameters joggingParameters);
        Task<IEnumerable<Jogging>> GetJoggingsByUserId(int userId);
        Task<PagedList<Jogging>> GetJoggingsByUsername(string userName, JoggingParameters joggingParameters);
        void CreateJogging(Jogging jogging);
        void UpdateJogging(Jogging jogging);
        void DeleteJogging(Jogging jogging);
        PagedList<WeekSummary> GetWeeklyReports(IEnumerable<Jogging> joggings, ReportParameters reportParameters);
        void Save();


    }
}
