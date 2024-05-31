using DaNangTourism.Server.Models;
using System.ComponentModel;
using System.Globalization;

namespace DaNangTourism.Server.ModelBindingConverter
{
    public class BlogTypeTypeConverter : TypeConverter
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
            if (value is string && Enum.TryParse<BlogType>((string)value, true, out var type))
            {
                return type;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
