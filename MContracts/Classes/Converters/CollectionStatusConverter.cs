using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    //public class CollectionStatusToVisibilityConverter:IValueConverter
    //{
    //    public static bool GetCollectionStatus(IEnumerable<Contractrepositoryview> collection)
    //    {
    //        if (collection == null) throw new ArgumentNullException("collection");
    //        return collection.All(x => x.Status);
    //    }

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (!(value is IEnumerable)) return value;
    //        var obj = (value as IEnumerable).Cast<Contractrepositoryview>();
    //        return GetCollectionStatus(obj).ToVisibility(Visibility.Collapsed, Visibility.Visible);
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}