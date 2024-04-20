using SP_ExamPro.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SP_ExamPro.Pages
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ErrorScreen : UserControl
    {
        public ErrorScreen()
        {
            InitializeComponent();
            lottileObj.FileName = "C:\\Users\\Rudnev V\\source\\repos\\ADO.NET\\SP_ExamPro\\SP_ExamPro\\Assets\\ErrorMess.json";
            lottileObj.PlayAnimation();
        }

        private void lottileObj_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigatorObject.SwitchBack();
        }
    }
}
