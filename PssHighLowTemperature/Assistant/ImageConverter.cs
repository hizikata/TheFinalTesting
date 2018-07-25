using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace PssHighLowTemperature.Assistant
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = (bool)value;
            if (flag == true)
            {
                return "/PssHighLowTemperature;component/Resources/Green.jpg";
            }
            else if(flag==false)
            {
                return "/PssHighLowTemperature;component/Resources/Red.jpg";
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
