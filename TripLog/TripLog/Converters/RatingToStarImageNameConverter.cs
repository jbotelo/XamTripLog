using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TripLog.Converters
{
    class RatingToStarImageNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is int rating)
            {
                if (rating <= 1)
                {
                    return "star_1";
                }

                if (rating >= 5)
                {
                    return "stars_5";
                }

                return "stars_" + rating;
            }

            return value;
        }


        // This converter is only used to convert the value when displaying it.
        // The value is never changed in the UI
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
