using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Helenlyn.BoostrapUI.Helper
{
    /// <summary>
    /// 动态UI样式资源整合
    /// </summary>
    public class DynamicResources
    {
        public static Style btnPrimary = Application.Current.FindResource("btn-primary") as Style;
        public static Style btnDefault = Application.Current.FindResource("btn-default") as Style;
        public static Brush foregroundDanger = Application.Current.FindResource("Foreground-Danger") as Brush;
        public static Color? foregroundColorDanger = Application.Current.FindResource("DangerColor5") as Color?;

        public static Brush DefaultSelectd = Application.Current.FindResource("Foreground-Success") as Brush;
        public static Brush DefaultSelectd1 = Application.Current.FindResource("DefaultSelectd") as Brush;
        public static Color? foregroundSuccessColor = Application.Current.FindResource("SuccessColor5") as Color?;

        public static Brush foregroundDefault = Application.Current.FindResource("Background-Default1") as Brush;
        public static Color? foregroundDefaultColor = Application.Current.FindResource("DefaultColor1") as Color?;
        public static Brush foregroundPrimary1 = Application.Current.FindResource("Foreground-Primary1") as Brush;
    }
}
