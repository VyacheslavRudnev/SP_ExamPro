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
            var projectDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var jsonFilePath = System.IO.Path.Combine(projectDirectory, "Assets\\ErrorMess.json");
            lottileObj.FileName = jsonFilePath;
            lottileObj.PlayAnimation();
        }

        private void lottileObj_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigatorObject.SwitchBack();
        }
    }
}
