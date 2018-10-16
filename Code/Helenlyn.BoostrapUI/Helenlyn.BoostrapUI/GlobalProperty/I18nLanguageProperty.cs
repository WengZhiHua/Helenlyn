using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helenlyn.BoostrapUI.GlobalProperty
{
    /// <summary>
    /// 国际化属性设置
    /// </summary>
    public class I18nLanguageProperty
    {
        public static DependencyProperty I18nLangProperty = DependencyProperty.RegisterAttached(
            "I18nLang", typeof(string), typeof(I18nLanguageProperty),
            new PropertyMetadata(""));
        public static string GetI18nLang(DependencyObject obj)
        {
            return (string)obj.GetValue(I18nLangProperty);
        }
        public static void SetI18nLang(DependencyObject obj, string value)
        {
            obj.SetValue(I18nLangProperty, value);
        }
    }
}
