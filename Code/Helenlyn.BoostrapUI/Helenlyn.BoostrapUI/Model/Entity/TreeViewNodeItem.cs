using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Helenlyn.BoostrapUI.Model.Entity
{
    public class TreeViewNodeItem : TreeView
    {
        /// <summary>
        /// 前缀图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 编辑图标
        /// </summary>
        public string EditIcon { get; set; }

        /// <summary>
        /// 显示文字
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 显示提示
        /// </summary>
        public string DisplayTip { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 子信息
        /// </summary>
        public List<TreeViewNodeItem> Children { get; set; }
        public TreeViewNodeItem()
        {
            Children = new List<TreeViewNodeItem>();
        }
    }
}
