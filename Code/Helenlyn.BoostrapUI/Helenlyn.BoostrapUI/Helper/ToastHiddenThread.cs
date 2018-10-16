using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helenlyn.BoostrapUI.Helper
{
    /// <summary>
    /// Toast 自动隐藏线程
    /// </summary>
    public class ToastHiddenThread
    {
        /// <summary>
        /// 结束时间
        /// </summary>
        public event EventHandler<Toast> toastEnd;

        /// <summary>
        /// 毫秒数
        /// </summary>
        public Int32 ms { get; set; }

        /// <summary>
        /// Toast对象
        /// </summary>
        public Toast mt { get; set; }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_ms"></param>
        /// <param name="_mt"></param>
        public ToastHiddenThread(Int32 _ms, Toast _mt)
        {
            ms = _ms;
            mt = _mt;
        }

        /// <summary>
        /// 启动加载
        /// </summary>
        public void Load()
        {
            Thread t = new Thread(Start);
            t.Start();
        }

        /// <summary>
        /// 正式处理函数
        /// </summary>
        private void Start()
        {
            Thread.Sleep(ms);
            if (toastEnd != null)
            {
                toastEnd(this, mt);
            }
        }
    }
}
