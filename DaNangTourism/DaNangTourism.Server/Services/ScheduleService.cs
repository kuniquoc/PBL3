﻿using DaNangTourism.Server.DAL;
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
        int CreateSchedule(int userId, InputSchedule schedule);
        int CloneSchedule(int userId, int scheduleId);
        int AddScheduleDestination(AddScheduleDestinationModel scheduleDestination);
        void DeleteScheduleDestination(int userId, int scheduleDestinationId);
        void UpdateScheduleDestination(int userId, int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination);
        void UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule);
        void DeleteSchedule(int userId, int scheduleId);
    }
    public class ScheduleService : IScheduleService
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
            sql.Append("SELECT Schedules.schedule_id AS ScheduleId, status AS Status, title AS Title, description AS Description, MIN(Date) AS StartDate," +
                " IFNULL((DATEDIFF(MAX(Date), MIN(Date)) + 1), 0) AS TotalDays, IFNULL(SUM(Budget), 0) AS TotalBudget, updated_at AS UpdatedAt" +
                " FROM Schedules LEFT JOIN ScheduleDestinations ON Schedules.schedule_id = ScheduleDestinations.schedule_id" +
                " WHERE user_id = @userId");
            parameters.Add(new MySqlParameter("@userId", userId));

            // xử lý where
            if (!string.IsNullOrEmpty(scheduleFilter.Search))
            {
                sql.Append(" AND title LIKE @search");
                parameters.Add(new MySqlParameter("@search", "%" + scheduleFilter.Search + "%"));
            }

            if (scheduleFilter.Status != ScheduleStatus.all)
            {
                sql.Append(" AND status = @status");
                parameters.Add(new MySqlParameter("@status", scheduleFilter.Status.ToString()));
            }

            // Dùng group by để tính StartDate, TotalDays, TotalBudget
            sql.Append(" GROUP BY Schedules.schedule_id, status, title, description, updated_at");

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

            foreach (var item in listSchedule.Items)
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
            sql.Append("SELECT Schedules.schedule_id AS ScheduleId, title AS Title, description AS Description, IFNULL((DATEDIFF(MAX(Date), MIN(Date)) + 1), 0) AS TotalDays, IFNULL(SUM(Budget), 0) AS TotalBudget, full_name AS Creator" +
                " FROM Schedules LEFT JOIN ScheduleDestinations ON Schedules.schedule_id = ScheduleDestinations.schedule_id" +
                " LEFT JOIN Users ON Users.user_id = Schedules.user_id WHERE is_public = TRUE");

            // kiểm tra xem có đăng nhập chưa, nếu có thì không lấy những schedule của người đã đăng nhập
            if (userId != 0)
            {
                sql.Append(" AND Schedules.user_id != @userId");
                parameters.Add(new MySqlParameter("@userId", userId));
            }


            // xử lý where
            if (!string.IsNullOrEmpty(publicScheduleFilter.Search))
            {
                sql.Append(" AND title LIKE @search");
                parameters.Add(new MySqlParameter("@search", "%" + publicScheduleFilter.Search + "%"));
            }

            // Dùng group by để tính StartDate, TotalDays, TotalBudget
            sql.Append(" GROUP BY Schedules.schedule_id, title, description");

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
        public int CreateSchedule(int userId, InputSchedule schedule)
        {
            return _scheduleRepository.CreateSchedule(userId, schedule);
        }


        /// <summary>
        /// Clone schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="creator"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public int CloneSchedule(int userId, int scheduleId)
        {
            int newScheduleId = _scheduleRepository.CloneSchedule(userId, scheduleId);
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
            // Check time exist in schedule
            bool checkTime = _scheduleDestinationRepository.CheckTimeExist(scheduleDestination.ScheduleId, scheduleDestination.Date, scheduleDestination.ArrivalTime, scheduleDestination.LeaveTime);
            if (checkTime == false) throw new BadHttpRequestException("Time conflict with another destination in this schedule");

            return _scheduleDestinationRepository.AddScheduleDestination(scheduleDestination);
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
        public void UpdateScheduleDestination(int userId, int scheduleDestinationId, UpdateScheduleDestinationModel scheduleDestination)
        {
            int scheduleId = _scheduleDestinationRepository.GetScheduleId(scheduleDestinationId);
            if (scheduleId == 0) throw new Exception("Schedule destination not found");
            if (!_scheduleRepository.IsCreator(userId, scheduleId)) throw new UnauthorizedAccessException("You are not the creator of this schedule");

            bool checkTime = _scheduleDestinationRepository.CheckTimeExist(scheduleDestination.ScheduleId, scheduleDestination.Date, scheduleDestination.ArrivalTime, scheduleDestination.LeaveTime, scheduleDestinationId);
            if (checkTime == false) throw new BadHttpRequestException("Time conflict with another destination in this schedule");

            _scheduleDestinationRepository.UpdateScheduleDestination(scheduleDestinationId, scheduleDestination);
        }

        public void UpdateSchedule(int userId, int scheduleId, UpdateScheduleModel schedule)
        {
            if (!_scheduleRepository.IsCreator(userId, scheduleId)) throw new UnauthorizedAccessException("You are not the creator of this schedule");
            _scheduleRepository.UpdateSchedule(userId, scheduleId, schedule);
        }

        public void DeleteSchedule(int userId, int scheduleId)
        {
            if (!_scheduleRepository.IsCreator(userId, scheduleId)) throw new UnauthorizedAccessException("You are not the creator of this schedule");
            _scheduleRepository.DeleteSchedule(scheduleId);
        }
    }
}
