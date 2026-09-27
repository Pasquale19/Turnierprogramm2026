
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;


namespace Turnierprogramm2.Utilities
{
    public class ScaleConverter : MarkupExtension, IValueConverter
    {

        private static ScaleConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //double _scale = System.Convert.ToDouble(parameter);
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new ScaleConverter());
        }

    }

    /// <summary>
    /// nearer Information:   https://stackoverflow.com/questions/7000819/binding-a-buttons-visibility-to-a-bool-value-in-viewmodel
    /// </summary>
    public class BoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Boolean && (bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed; //Hidden reserves the Space Collapsed doesnt render the control
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility && (Visibility)value == Visibility.Visible) return true;
            else return false;
        }

    }
    public class VisToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility && (Visibility)value == Visibility.Visible) return true;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Boolean && (bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed; //Hidden reserves the Space Collapsed doesnt render the control
        }
        #endregion
    }
    /// <summary>
    /// source: https://www.rhyous.com/2011/02/22/binding-visibility-to-a-bool-value-in-wpf/
    /// </summary>
    public class BoolToVisOrHidConverter : IValueConverter
    {
        #region Constructors
        /// <summary>
        /// The default constructor
        /// </summary>
        public BoolToVisOrHidConverter() { }
        #endregion

        #region Properties
        public bool Collapse { get; set; }
        #endregion

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;
            if (bValue)
            {
                return Visibility.Visible;
            }
            else
            {
                if (Collapse)
                    return Visibility.Collapsed;
                else
                    return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return true;
            else
                return false;
        }
        #endregion
    }

    public class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }
    public class BooleanInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = true;
            if ((bool)value == true) res = false;


            return res;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

    }

    public class AddValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) + System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) - System.Convert.ToDouble(parameter);
        }

    }

    /// <summary>
    /// source: https://stackoverflow.com/questions/20707160/data-binding-int-property-to-enum-in-wpf
    /// </summary>
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            Enum enumValue = default(Enum);
            if (parameter is Type)
            {
                enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());
            }
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                returnValue = (int)Enum.Parse((Type)parameter, value.ToString());
            }
            return returnValue;
        }
    }
    /// <summary>
    /// https://stackoverflow.com/questions/20707160/data-binding-int-property-to-enum-in-wpf
    /// </summary>
    public sealed class BidirectionalEnumAndNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (targetType.IsEnum)
            {
                // convert int to enum
                //int val=(int) Enum.ToObject(targetType, value);
                return Enum.ToObject(targetType, value);
            }

            if (value.GetType().IsEnum)
            {
                // convert enum to int
                return System.Convert.ChangeType(
                    value,
                    Enum.GetUnderlyingType(value.GetType()));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // perform the same conversion in both directions
            return Convert(value, targetType, parameter, culture);
        }
    }

}
