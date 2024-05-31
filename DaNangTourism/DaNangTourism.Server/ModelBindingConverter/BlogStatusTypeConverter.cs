using DaNangTourism.Server.Models;
using System.ComponentModel;
using System.Globalization;

namespace DaNangTourism.Server.ModelBindingConverter
{
    public class BlogStatusTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string && Enum.TryParse<BlogStatus>((string)value, true, out var status))
            {
                return status;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
