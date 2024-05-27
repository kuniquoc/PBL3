using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models.DestinationModels;
using DaNangTourism.Server.Models.ScheduleModels;
using MySqlConnector;
using System.Text;

namespace DaNangTourism.Server.Service
{
    public interface IScheduleService
    {
        ListSchedule GetListSchedule(int userId, ScheduleFilter scheduleFilter);
    }
    public class ScheduleService: IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        /// <summary>
        /// Get list schedule
        /// </summary>
        /// <param name="scheduleFilter"></param>
        /// <returns></returns>
        public ListSchedule GetListSchedule(int userId, ScheduleFilter scheduleFilter)
        {
            ListSchedule listSchedule = new ListSchedule();
            listSchedule.Limit = scheduleFilter.Limit;
            listSchedule.Page = scheduleFilter.Page;

            StringBuilder sql = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            sql.Append("SELECT ScheduleId, Status, Title, Description, Destinations, StartDate, TotalDays, TotalBudget, UpdatedAt FROM Schedules WHERE UserId = @userId");
            parameters.Add(new MySqlParameter("@userId", userId));

            // xử lý where
            if (!string.IsNullOrEmpty(scheduleFilter.Search))
            {
                sql.Append(" AND Title LIKE @search");
                parameters.Add(new MySqlParameter("@search", "%" + scheduleFilter.Search + "%"));
            }    
            
            if (scheduleFilter.Status != ScheduleStatus.all)
            {
                sql.Append(" AND Status = @status");
                parameters.Add(new MySqlParameter("@status", scheduleFilter.Status.ToString()));
            }

            // xử lý order by
            sql.Append(" ORDER BY " + scheduleFilter.SortBy + " " + scheduleFilter.SortType);

            // Lấy tổng kết quả có được
            StringBuilder countSql = new StringBuilder();
            countSql.Append("SELECT COUNT(*) FROM (" + sql + ") AS subquery");
            listSchedule.Total = _scheduleRepository.GetScheduleCount(countSql.ToString(), parameters.ToArray());

            // Xử lý trang hiển thị và số lượng tương ứng
            sql.Append(" LIMIT @limit OFFSET @offset");

            // kiểm tra limit
            listSchedule.Limit = scheduleFilter.Limit;
            parameters.Add(new MySqlParameter("@limit", scheduleFilter.Limit));

            // kiểm tra page
            listSchedule.Page = scheduleFilter.Page;
            parameters.Add(new MySqlParameter("@offset", (scheduleFilter.Page - 1) * scheduleFilter.Limit));

            listSchedule.Items = _scheduleRepository.GeListSchedule(sql.ToString(), parameters.ToArray()).ToList();

            return listSchedule;
        }
    }
}
