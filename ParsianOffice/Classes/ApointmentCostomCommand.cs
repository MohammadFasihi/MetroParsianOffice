using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParsianOffice.Classes
{
    public static class ApointmentCostomCommand
    {
        public static readonly RoutedUICommand Refresh = new RoutedUICommand
            (
                "Refresh",
                "Refresh",
                typeof(MainCutomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F5, ModifierKeys.None)
                }
            );

        public static readonly RoutedUICommand Search = new RoutedUICommand
            (
                "Search",
                "Search",
                typeof(MainCutomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Insert = new RoutedUICommand
            (
                "Insert",
                "Insert",
                typeof(MainCutomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Enter, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Delete = new RoutedUICommand
            (
                "Delete",
                "Delete",
                typeof(MainCutomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Delete, ModifierKeys.Shift)
                }
            );

        public static readonly RoutedUICommand Print = new RoutedUICommand
            (
                "Print",
                "Print",
                typeof(MainCutomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.P, ModifierKeys.Control)
                }
            );
    }
}
