using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Models.ScheduleModels;
using MySqlConnector;
using System.Text;

namespace DaNangTourism.Server.Services
{
    public interface IScheduleService
    {
        ListSchedule GetListSchedule(int userId, ScheduleFilter scheduleFilter);
        PublicSchedule GetPublicSchedule(PublicScheduleFilter publicScheduleFilter, int userId = 0);
        ScheduleDetail? GetScheduleDetail(int userId, int scheduleId);
        int CreateSchedule(int userId, string creator, InputSchedule schedule);
        int CloneSchedule(int userId, string creator, int scheduleId);
        int AddScheduleDestination(AddScheduleDestinationModel scheduleDestination);    
        void DeleteScheduleDestination(int userId, int scheduleDestinationId);
        AddScheduleDestinationModel UpdateScheduleDestination(int userId, int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination);
        UpdateScheduleModel UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule);
    }
    public class ScheduleService: IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IScheduleDestinationRepository _scheduleDestinationRepository;
        private readonly IDestinationRepository _destinationRepository;
        public ScheduleService(IScheduleRepository scheduleRepository, IScheduleDestinationRepository scheduleDestinationRepository, IDestinationRepository destinationRepository)
        {
            _scheduleRepository = scheduleRepository;
            _scheduleDestinationRepository = scheduleDestinationRepository;
            _destinationRepository = destinationRepository;
        }

        /// <summary>
        /// Get list schedule
        /// </summary>
        /// <param name="scheduleFilter"></param>
        /// <returns></returns>
        public ListSchedule GetListSchedule(int userId, ScheduleFilter scheduleFilter)
        {
            ListSchedule listSchedule = new ListSchedule();

            StringBuilder sql = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            sql.Append("SELECT ScheduleId, Status, Title, Description, StartDate, TotalDays, TotalBudget, UpdatedAt FROM Schedules WHERE UserId = @userId");
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

            foreach (var item in  listSchedule.Items)
            {
                item.Destinations = _scheduleDestinationRepository.GetListDesNameByScheduleId(item.Id);
            }

            return listSchedule;
        }

        /// <summary>
        /// Get public schedule
        /// </summary>
        /// <param name="publicScheduleFilter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PublicSchedule GetPublicSchedule(PublicScheduleFilter publicScheduleFilter, int userId = 0)
        {
            PublicSchedule publicSchedule = new PublicSchedule();

            StringBuilder sql = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            sql.Append("SELECT ScheduleId, Title, Description, TotalDays, TotalBudget, Creator FROM Schedules WHERE IsPublic = TRUE");

            // kiểm tra xem có đăng nhập chưa, nếu có thì không lấy những schedule của người đã đăng nhập
            if (userId != 0)
            {
                sql.Append(" AND UserId != @userId");
                parameters.Add(new MySqlParameter("@userId", userId));
            }


            // xử lý where
            if (!string.IsNullOrEmpty(publicScheduleFilter.Search))
            {
                sql.Append(" AND Title LIKE @search");
                parameters.Add(new MySqlParameter("@search", "%" + publicScheduleFilter.Search + "%"));
            }

            // xử lý order by
            sql.Append(" ORDER BY " + publicScheduleFilter.SortBy + " " + publicScheduleFilter.SortType);

            // Lấy tổng kết quả có được
            StringBuilder countSql = new StringBuilder();
            countSql.Append("SELECT COUNT(*) FROM (" + sql + ") AS subquery");
            publicSchedule.Total = _scheduleRepository.GetScheduleCount(countSql.ToString(), parameters.ToArray());

            // Xử lý trang hiển thị và số lượng tương ứng
            sql.Append(" LIMIT @limit OFFSET @offset");

            // kiểm tra limit
            publicSchedule.Limit = publicScheduleFilter.Limit;
            parameters.Add(new MySqlParameter("@limit", publicScheduleFilter.Limit));

            // kiểm tra page
            publicSchedule.Page = publicScheduleFilter.Page;
            parameters.Add(new MySqlParameter("@offset", (publicScheduleFilter.Page - 1) * publicScheduleFilter.Limit));

            publicSchedule.Items = _scheduleRepository.GetPublicSchedule(sql.ToString(), parameters.ToArray()).ToList();

            foreach (var item in publicSchedule.Items)
            {
                item.Destinations = _scheduleDestinationRepository.GetListDesNameByScheduleId(item.Id);
            }

            return publicSchedule;
        }

        /// <summary>
        /// Get schedule detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public ScheduleDetail? GetScheduleDetail(int userId, int scheduleId)
        {
            ScheduleDetail? scheduleDetail = _scheduleRepository.GetScheduleDetail(userId, scheduleId);
            if (scheduleDetail == null) return null;
            scheduleDetail.Days = _scheduleDestinationRepository.GetScheduleDay(scheduleId).ToList();
            return scheduleDetail;
        }

        /// <summary>
        /// Create schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public int CreateSchedule(int userId, string creator, InputSchedule schedule)
        {
            return _scheduleRepository.CreateSchedule(userId, creator, schedule);
        }


        /// <summary>
        /// Clone schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="creator"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public int CloneSchedule(int userId, string creator, int scheduleId)
        {
            int newScheduleId = _scheduleRepository.CloneSchedule(userId, creator, scheduleId);
            _scheduleDestinationRepository.CloneScheduleDestination(scheduleId, newScheduleId);
            return newScheduleId;
        }

        /// <summary>
        /// Add schedule destination
        /// </summary>
        /// <param name="scheduleDestination"></param>
        /// <returns></returns>
        public int AddScheduleDestination(AddScheduleDestinationModel scheduleDestination)
        {
            // Get Name by DestinationId
            string name = _destinationRepository.GetName(scheduleDestination.DestinationId);
            // Get Address by DestinationId
            string address = _destinationRepository.GetAddress(scheduleDestination.DestinationId);

            return _scheduleDestinationRepository.AddScheduleDestination(scheduleDestination, name, address);
        }

        /// <summary>
        /// Delete schedule destination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scheduleDestinationId"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void DeleteScheduleDestination(int userId, int scheduleDestinationId)
        {
            int scheduleId = _scheduleDestinationRepository.GetScheduleId(scheduleDestinationId);
            if (!_scheduleRepository.IsCreator(userId, scheduleId)) throw new UnauthorizedAccessException("You are not the creator of this schedule");

            _scheduleDestinationRepository.DeleteScheduleDestination(scheduleDestinationId);
        }

        /// <summary>
        /// Update schedule destination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scheduleDestinationId"></param>
        /// <param name="scheduleDestination"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public AddScheduleDestinationModel UpdateScheduleDestination(int userId, int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination)
        {
            int scheduleId = _scheduleDestinationRepository.GetScheduleId(scheduleDestinationId);
            if (scheduleId == 0) throw new Exception("Schedule destination not found");
            if (!_scheduleRepository.IsCreator(userId, scheduleId)) throw new UnauthorizedAccessException("You are not the creator of this schedule");

            return _scheduleDestinationRepository.UpdateScheduleDestination(scheduleDestinationId, scheduleDestination);
        }

        public UpdateScheduleModel UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule)
        {
            if (!_scheduleRepository.IsCreator(userId, scheduleId)) throw new UnauthorizedAccessException("You are not the creator of this schedule");
            return _scheduleRepository.UpdateSchedule(userId, scheduleId, schedule);
        }
    }
}
