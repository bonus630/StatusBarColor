
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using corel = Corel.Interop.VGCore;

namespace StatusBarColor
{
    public partial class ControlUI : UserControl
    {
        public static corel.Application corelApp;

        private corel.Color currentColor;


        public ControlUI(object app)
        {

            try
            {
              
                corelApp = app as corel.Application;
                corelApp.SelectionChange += CorelApp_SelectionChange;
                currentColor = corelApp.CreateCMYKColor(0,0,0,0);
                //var dsf = new DataSource.DataSourceFactory();
                //dsf.AddDataSource("StatusBarColorDS", typeof(DataSource.StatusBarColorDataSource));
                //dsf.Register(); 
                InitializeComponent();

            }
            catch
            {
                global::System.Windows.MessageBox.Show("VGCore Erro");
            }

        }

        private void CorelApp_SelectionChange()
        {
            if (corelApp.ActiveShape.Fill.Type != corel.cdrFillType.cdrUniformFill)
                return;
            currentColor = corelApp.ActiveShape.Fill.UniformColor;

            ntb_C.Value = currentColor.CMYKCyan;
            ntb_Y.Value = currentColor.CMYKYellow;
            ntb_M.Value = currentColor.CMYKMagenta;
            ntb_K.Value = currentColor.CMYKBlack;

        }

        private void ntb_C_ValueChangedEvent(double obj)
        {
            currentColor.CMYKCyan = (int)obj;
            if(corelApp.ActiveShape!=null)
                corelApp.ActiveShape.Fill.UniformColor = currentColor;
        }

        private void ntb_Y_ValueChangedEvent(double obj)
        {
            currentColor.CMYKYellow = (int)obj;
            if (corelApp.ActiveShape != null)
                corelApp.ActiveShape.Fill.UniformColor = currentColor;
        }

        private void ntb_M_ValueChangedEvent(double obj)
        {
            currentColor.CMYKMagenta = (int)obj;
            if (corelApp.ActiveShape != null)
                corelApp.ActiveShape.Fill.UniformColor = currentColor;
        }

        private void ntb_K_ValueChangedEvent(double obj)
        {
            currentColor.CMYKBlack = (int)obj;
            if (corelApp.ActiveShape != null)
                corelApp.ActiveShape.Fill.UniformColor = currentColor;
        }
    }
}
