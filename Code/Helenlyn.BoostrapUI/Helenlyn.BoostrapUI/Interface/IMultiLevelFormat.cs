using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helenlyn.BoostrapUI.Interface
{
    public interface IMultiLevelFormat
    {
        string Key { get; set; }
        object Value { get; set; }
        string Text { get; set; }
        string TextKey { get; set; }
        List<IMultiLevelFormat> Children { get; set; }
        void UpdateText();
    }
}
