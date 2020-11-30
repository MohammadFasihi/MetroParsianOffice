using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ParsianOffice.Classes
{
    public static class MainCutomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Exit",
                "Exit",
                typeof(MainCutomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.End, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NewIssue = new RoutedUICommand
            (
                "NewIssue",
                "NewIssue",
                typeof(MainCutomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.N, ModifierKeys.Control)
                }
            );

        public static void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }
    }
}
