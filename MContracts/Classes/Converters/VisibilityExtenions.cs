using System.Windows;

namespace MContracts.Classes.Converters
{
    public static class VisibilityExtenions
    {
        public static bool ToBoolean(this Visibility source, Visibility trueVisibility=Visibility.Visible)
        {
            return BoolToVisibilityConverter.VisibilityToBoolean(source, trueVisibility);
        }

        public static Visibility ToVisibility(this bool source, Visibility trueVisibility = Visibility.Visible, Visibility falseVisibility = Visibility.Collapsed)
        {
            return BoolToVisibilityConverter.BoolToVisibility(source, trueVisibility, falseVisibility);
        }

        public static Visibility Or(this Visibility source, Visibility other, Visibility trueVisibility = Visibility.Visible, Visibility falseVisibility = Visibility.Collapsed)
        {
            return (source.ToBoolean(trueVisibility) || other.ToBoolean(trueVisibility)).ToVisibility(trueVisibility,
                                                                                                      falseVisibility);
        }

        public static Visibility And(this Visibility source, Visibility other, Visibility trueVisibility = Visibility.Visible, Visibility falseVisibility = Visibility.Collapsed)
        {
            return (source.ToBoolean(trueVisibility) && other.ToBoolean(trueVisibility)).ToVisibility(trueVisibility,
                                                                                                      falseVisibility);
        }
    }
}