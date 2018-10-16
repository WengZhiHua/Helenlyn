using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helenlyn.BoostrapUI.Helper
{

    public interface IDialogHelper
    {
        MessageBoxResult ShowMessage(string text, string title);

        MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button);
        MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner);
        MessageBoxResult ShowDialog(string title, UIElement content, MessageBoxButton? button, Window owner);
        MessageBoxResult ShowDialog(string title, UIElement content, List<Dialog.ButtonInfo> customButtons, Window owner, LanContext lanContext);
    }
    public interface IDialogResult
    {
        MessageBoxResult DialogResult { get; set; }
    }
    public enum LanContext
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        ///  主要
        /// </summary>        
        Primary = 1,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 2,

        /// <summary>
        /// 信息
        /// </summary>
        Info = 3,

        /// <summary>
        /// 警告
        /// </summary>
        Warning = 4,

        /// <summary>
        /// 危险
        /// </summary>
        Danger = 5
    }

    public static class DialogHelper
    {
        public static void SetDialgHelper(IDialogHelper helper)
        {
            instance = helper;
        }
        static IDialogHelper instance;
        public static MessageBoxResult ShowMessage(string text, string title = "")
        {
            return instance.ShowMessage(text, title);
        }
        public static MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner = null)
        {
            return instance.ShowMessage(text, title, button, owner);
        }
        /// <summary>
        /// 显示自定义按钮的消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <param name="customButtons"></param>
        /// <param name="owner"></param>
        /// <param name="lanContext"></param>
        /// <returns></returns>
        public static MessageBoxResult ShowMessage(string text, string title, List<Dialog.ButtonInfo> customButtons, Window owner = null, LanContext lanContext = LanContext.Primary)
        {
            var helper = instance as INuiDialogHelper;
            if (helper == null)
            {
                throw new NotSupportedException();
            }
            return helper.ShowMessage(text, title, customButtons, owner, lanContext);
        }
        /// <summary>
        /// 显示自定义内容的弹出窗口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="button">按钮组，若为空，则不显示任何按钮</param>
        /// <param name="owner"></param>
        /// <returns>根据点击的按钮返回。
        /// 若button为空，则尝试从content.DataContext.DialogResult中读取结果(要求content继承自FrameworkElement,content.DataText实现IDialogResult接口)
        /// 默认值为MessageBoxResult.None</returns>
        public static MessageBoxResult ShowDialog(string title, UIElement content, MessageBoxButton? button, Window owner = null)
        {
            var result = instance.ShowDialog(title, content, button, owner);
            if (result == MessageBoxResult.None)
            {
                return TryGetDialogResult(content);
            }
            return result;
        }
        private static MessageBoxResult TryGetDialogResult(UIElement content)
        {
            var result = ((content as FrameworkElement)?.DataContext as IDialogResult)?.DialogResult;
            if (result != null && result != MessageBoxResult.None)
                return result.Value;
            return MessageBoxResult.None;
        }
        /// <summary>
        /// 显示自定义内容,自定义按钮的弹出窗口
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="customButtons"></param>
        /// <param name="owner"></param>
        /// <param name="lanContext"></param>
        /// <returns>根据点击的按钮返回。
        /// 若未通过按钮关闭，则尝试从content.DataContext.DialogResult中读取结果(要求content继承自FrameworkElement,content.DataText实现IDialogResult接口)
        /// 默认值为MessageBoxResult.None</returns>
        public static MessageBoxResult ShowDialog(string title, UIElement content, List<Dialog.ButtonInfo> customButtons, Window owner = null, LanContext lanContext = LanContext.Primary)
        {
            var result = instance.ShowDialog(title, content, customButtons, owner, lanContext);
            if (result != MessageBoxResult.None)
            {
                return result;
            }
            return TryGetDialogResult(content);
        }
        public static MessageBoxResult ShowDialog(DialogOptions options)
        {
            var helper = instance as INuiDialogHelper;
            if (helper == null)
            {
                throw new NotSupportedException();
            }
            MessageBoxResult result = helper.ShowDialog(options);
            if (result != MessageBoxResult.None)
            {
                return result;
            }
            return TryGetDialogResult(options?.Content);
        }
        //public static MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Grid grid, LanContext lanContext)
        //{
        //    var helper = instance as INuiDialogHelper;
        //    if(helper == null)
        //    {
        //        throw new NotSupportedException();
        //    }
        //    return helper.ShowMessage(text, title, button, grid, lanContext);
        //}
        public static MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner, LanContext lanContext)
        {
            var helper = instance as INuiDialogHelper;
            if (helper == null)
            {
                throw new NotSupportedException();
            }
            return helper.ShowMessage(text, title, button, owner, lanContext);
        }
        /// <summary>
        /// 注册对话框元素与ViewModel，注册后可调用CloseByViewModel通过ViewModel关闭所在窗口
        /// </summary>
        /// <param name="content"></param>
        /// <param name="viewModel"></param>
        public static void RegisterClosingByViewModel(UIElement content, object viewModel)
        {
            if (viewModel == null || content == null) throw new ArgumentNullException();
            ViewModelViewDict.Add(viewModel, content);
        }
        /// <summary>
        /// 注册对话框元素（要求DataContext非空）,注册后可调用CloseByViewModel通过要求DataContext关闭所在窗口
        /// </summary>
        /// <param name="content"></param>
        public static void RegisterClosingByDataContext(FrameworkElement content)
        {
            if (content == null || content.DataContext == null) throw new ArgumentNullException();
            ViewModelViewDict[content.DataContext] = content;
        }
        /// <summary>
        /// 通过已注册的ViewModel关闭关联的ui所在对话框
        /// </summary>
        /// <param name="viewModel"></param>
        public static void CloseByViewModel(object viewModel)
        {
            if (ViewModelViewDict.ContainsKey(viewModel))
            {
                var window = Window.GetWindow(ViewModelViewDict[viewModel]);
                if (window != null)
                {

                    window.Close();
                }
                if (window?.IsActive != true)
                {
                    if (window.Owner != null)
                        window.Owner.Activate();
                    ViewModelViewDict.Remove(viewModel);
                }
            }
        }
        /// <summary>
        /// 通过已注册的ViewModel关闭关联的ui所在对话框，并设置ViewModel中的DialogResult
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="dialogResult"></param>
        public static void CloseByViewModel(IDialogResult viewModel, MessageBoxResult dialogResult)
        {
            if (viewModel == null) throw new ArgumentNullException();
            viewModel.DialogResult = dialogResult;
            CloseByViewModel(viewModel);
        }
        private static Dictionary<object, UIElement> ViewModelViewDict = new Dictionary<object, UIElement>();

        public static void ShowMessage(object p, string v, MessageBoxButton oK)
        {
            throw new NotImplementedException();
        }
    }

    public interface INuiDialogHelper : IDialogHelper
    {
        MessageBoxResult ShowDialog(DialogOptions options);
        MessageBoxResult ShowMessage(string text, string title, List<Dialog.ButtonInfo> customButtons, Window owner, LanContext lanContext);
        //MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Grid grid, LanContext lanContext);
        MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner, LanContext lanContext);

    }
    public class NuiDialogHelper : INuiDialogHelper
    {
        LanContext TransformEnum(Nd.BootstrapUI.Model.Enum.LanContext context)
        {
            switch (context)
            {
                case BootstrapUI.Model.Enum.LanContext.Default:
                    return LanContext.Default;
                case BootstrapUI.Model.Enum.LanContext.Primary:
                    return LanContext.Primary;
                case BootstrapUI.Model.Enum.LanContext.Success:
                    return LanContext.Success;
                case BootstrapUI.Model.Enum.LanContext.Info:
                    return LanContext.Info;
                case BootstrapUI.Model.Enum.LanContext.Warning:
                    return LanContext.Warning;
                case BootstrapUI.Model.Enum.LanContext.Danger:
                    return LanContext.Danger;
                default:
                    throw new NotSupportedException();
            }
        }
        Nd.BootstrapUI.Model.Enum.LanContext TransformEnum(LanContext context)
        {
            switch (context)
            {
                case LanContext.Default:
                    return Nd.BootstrapUI.Model.Enum.LanContext.Default;
                case LanContext.Primary:
                    return Nd.BootstrapUI.Model.Enum.LanContext.Primary;
                case LanContext.Success:
                    return Nd.BootstrapUI.Model.Enum.LanContext.Success;
                case LanContext.Info:
                    return Nd.BootstrapUI.Model.Enum.LanContext.Info;
                case LanContext.Warning:
                    return Nd.BootstrapUI.Model.Enum.LanContext.Warning;
                case LanContext.Danger:
                    return Nd.BootstrapUI.Model.Enum.LanContext.Danger;
                default:
                    throw new NotSupportedException();
            }
        }
        public MessageBoxResult ShowMessage(string text, string title)
        {
            return ShowMessage(text, title, MessageBoxButton.OK);
        }
        public MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    Dialog.ShowMsg(title, text);
                    return MessageBoxResult.OK;
                    break;
                case MessageBoxButton.OKCancel:
                    return Dialog.ShowConfirmSync(title, text);
                    break;
                case MessageBoxButton.YesNo:
                    return Dialog.ShowConfirmSync(title, text, BootstrapUI.Model.Enum.LanContext.Primary, null, Dialog.DialogType.YESNO);
                    break;
                case MessageBoxButton.YesNoCancel:
                    {
                        return Dialog.ShowConfirmSync(title, text, BootstrapUI.Model.Enum.LanContext.Primary, null, Dialog.DialogType.YESNOCANCEL);
                    }
                default:
                    throw new NotSupportedException();
            }
        }
        public MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Grid grid, LanContext lanContext)
        {
            var context = TransformEnum(lanContext);
            switch (button)
            {
                case MessageBoxButton.OK:
                    Dialog.ShowMsg(title, text, context, grid);
                    return MessageBoxResult.OK;
                default:
                    throw new NotSupportedException();
            }
        }

        public MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner, LanContext lanContext)
        {
            var context = TransformEnum(lanContext);
            switch (button)
            {
                case MessageBoxButton.OK:
                    Dialog.ShowMsgSync(title, text, context, owner);
                    return MessageBoxResult.OK;
                default:
                    throw new NotSupportedException();
            }
        }

        public MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner)
        {

            switch (button)
            {
                case MessageBoxButton.OK:
                    return Dialog.ShowMsgSync(title, text, BootstrapUI.Model.Enum.LanContext.Primary, owner);
                    break;
                case MessageBoxButton.OKCancel:
                    return Dialog.ShowConfirmSync(title, text, BootstrapUI.Model.Enum.LanContext.Primary, owner);
                    break;
                case MessageBoxButton.YesNo:
                    return Dialog.ShowConfirmSync(title, text, BootstrapUI.Model.Enum.LanContext.Primary, owner, Dialog.DialogType.YESNO);
                    break;
                case MessageBoxButton.YesNoCancel:
                    return Dialog.ShowConfirmSync(title, text, BootstrapUI.Model.Enum.LanContext.Primary, owner, Dialog.DialogType.YESNOCANCEL);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public MessageBoxResult ShowDialog(string title, UIElement content, MessageBoxButton? button, Window owner = null)
        {
            if (button == null)
            {
                var result = Dialog.ShowCustomDialog(title, content, BootstrapUI.Model.Enum.LanContext.Primary, owner);
                return result;
            }
            switch (button.Value)
            {
                case MessageBoxButton.OK:
                    return Dialog.ShowCustomMsg(title, content, BootstrapUI.Model.Enum.LanContext.Primary, owner);
                case MessageBoxButton.OKCancel:
                    return Dialog.ShowCustomConfirm(title, content, BootstrapUI.Model.Enum.LanContext.Primary, owner);

                    break;
                case MessageBoxButton.YesNoCancel:
                    {
                        return Dialog.ShowCustomConfirm(title, content, BootstrapUI.Model.Enum.LanContext.Primary, owner, Dialog.DialogType.YESNOCANCEL);
                    }
                    break;
                case MessageBoxButton.YesNo:
                    return Dialog.ShowCustomConfirm(title, content, BootstrapUI.Model.Enum.LanContext.Primary, owner, Dialog.DialogType.YESNO);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public MessageBoxResult ShowMessage(string text, string title, List<Dialog.ButtonInfo> customButtons, Window owner, LanContext lanContext)
        {
            return Dialog.ShowConfirmSync(title, text, customButtons, TransformEnum(lanContext), owner);
        }

        public MessageBoxResult ShowDialog(string title, UIElement content, List<Dialog.ButtonInfo> customButtons, Window owner, LanContext lanContext)
        {
            return Dialog.ShowCustomDialog(title, content, customButtons, TransformEnum(lanContext), owner);
        }

        public MessageBoxResult ShowDialog(DialogOptions options)
        {
            return Dialog.ShowDialog(options);
        }
    }
}
