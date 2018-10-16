using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helenlyn.BoostrapUI.Model.Entity
{
    /// <summary>
    /// 复选框信息
    /// </summary>
    public class ComboBoxItemModel
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 文本信息
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// 值信息
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// 目标对象信息
        /// </summary>
        public Object Tag { get; set; }
    }
}
