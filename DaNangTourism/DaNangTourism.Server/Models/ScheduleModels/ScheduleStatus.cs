using System.ComponentModel;
using DaNangTourism.Server.ModelBindingConverter;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.Models.ScheduleModels
{
    [TypeConverter(typeof(ScheduleStatusTypeConverter))]
    public enum ScheduleStatus
    {
        all = 0,
        planning = 1,
        ongoing = 2,
        completed = 3,
        canceled = 4,
    }
}
