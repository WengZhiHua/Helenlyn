using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helenlyn.BoostrapUI.Model.Entity
{

    public class TreeItemModel : INotifyPropertyChanged
    {
        #region Properties

        private bool? _isChecked;
        private TreeItemModel _parent;
        private List<TreeItemModel> _children;
        private string _name;
        private string _key;
        private Boolean _isExpand;
        private bool? _isLeaf;//是否叶节点
        private Boolean _isEffectiveItem;//是否是有效搜索项（如果子项全部被选中，有效搜索项就是父亲项，不包含子项）

        public bool? IsLeaf
        {
            get { return _isLeaf; }
            set { _isLeaf = value; this.OnPropertyChanged("IsLeaf"); }
        }

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { this.SetIsChecked(value, true, true); this.SetEffectiveItem(); }
        }

        public bool IsEffectiveItem
        {
            get { return _isEffectiveItem; }
            set { _isEffectiveItem = value; this.OnPropertyChanged("IsEffectiveItem"); }
        }


        public Boolean IsExpand
        {
            get { return _isExpand; }
            set { _isExpand = value; }
        }

        public bool IsInitiallySelected
        {
            get; set;
        }

        public TreeItemModel Parent
        {
            get { return _parent; }
            private set { _parent = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public List<TreeItemModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        private void CheckChildren()
        {
            if (IsChecked == true || IsChecked == false)
            {
                if (Children != null && Children.Any())
                {
                    Children.ForEach((tim) =>
                    {
                        tim.IsChecked = IsChecked;
                    });
                }
            }
        }

        void SetEffectiveItem()
        {
            var top = this;
            while (top.Parent != null)
            {
                top = top.Parent;
            }
            //开始自顶而下去计算是否合理
            InitSubItems(top);
        }

        void InitSubItems(TreeItemModel top)
        {
            if (top.IsChecked == true)
            {
                top.IsEffectiveItem = true;
                if (!top.Children.Any()) top.IsLeaf = true;
                ClearSubItems(top);
            }
            else
            {
                top.IsEffectiveItem = false;
                if (top.Children.Any())
                {
                    top.Children.ForEach(delegate (TreeItemModel item) {
                        InitSubItems(item);
                    });
                }
            }
        }

        void ClearSubItems(TreeItemModel top)
        {
            top.Children.ForEach(delegate (TreeItemModel item) {
                item.IsEffectiveItem = false;
                ClearSubItems(item);
            });
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;
            _isChecked = value;
            if (this.Children != null && updateChildren && _isChecked.HasValue)
                this.Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));
            if (updateParent && _parent != null)
                _parent.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        public TreeItemModel(String key, String name, Boolean isExpand = false, Boolean isCheck = false)
        {
            this.Key = key;
            this.Name = name;
            this.IsExpand = isExpand;
            this.SetIsChecked(isCheck, false, false);
            this.Children = new List<TreeItemModel>();
        }

        public void Initialize()
        {
            foreach (TreeItemModel child in this.Children)
            {
                child.Parent = this;
                child.Initialize();
            }
        }

        public void IinitChecked()
        {
            foreach (TreeItemModel child in this.Children)
            {
                child.IsChecked = child.IsChecked;
                child.IinitChecked();
            }
        }

        void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
