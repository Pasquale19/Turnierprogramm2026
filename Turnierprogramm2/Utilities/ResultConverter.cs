using System;
using System.Globalization;
using System.Windows.Data;

namespace Turnierprogramm2.Utilities
{
    public class ResultConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Int32.TryParse(values[0].ToString(), out int pkt1);
            Int32.TryParse(values[1].ToString(), out int pkt2);
            return $"{pkt1} : {pkt2}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
