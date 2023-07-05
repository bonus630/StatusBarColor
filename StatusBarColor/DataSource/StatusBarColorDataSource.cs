using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace StatusBarColor.DataSource
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class StatusBarColorDataSource : BaseDataSource
    {

        private string caption = "Enter Caption";
        private string icon = "guid://28ef6590-9aa2-493a-9ddc-dd9955ed5ac3";

        public StatusBarColorDataSource(DataSourceProxy proxy) : base(proxy)
        {

        }

        // You can change caption/icon dynamically setting a new value here 
        //or loading the value from resource specifying the id of the caption/icon 
        public string Caption
        {
            get { return caption; }
            set { caption = value; NotifyPropertyChanged(); }
        }
        public string Icon
        {
            get { return icon; }
            set { icon = value; NotifyPropertyChanged(); }
        }

        public void MenuItemCommand()
        {
            ControlUI.corelApp.MsgShow("Working");
        }
    }

}
