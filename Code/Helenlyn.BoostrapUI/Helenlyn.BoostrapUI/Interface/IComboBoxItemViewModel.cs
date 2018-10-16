using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helenlyn.BoostrapUI.Interface
{
    public interface IComboBoxItemViewModel
    {
        bool IsChecked { get; set; }
        string Key { get; set; }
        List<IComboBoxItemViewModel> SubList { get; set; }
        string Text { get; set; }
        string TextKey { get; set; }
        object Value { get; set; }

        void UpdateText();
    }
}
