using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using SP_ExamPro.Navigator;
using SP_ExamPro.Pages;

namespace SP_ExamPro;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        NavigatorObject.pageSwitcher = this;
        NavigatorObject.Switch(new StartScreen());
    }
    public Action? CloseAction { get; set; }

    public void Navigate(UserControl nextPage)
    {
        this.Content = nextPage;
    }

    public void Navigate(UserControl nextPage, object state)
    {
        this.Content = nextPage;
        INavigator? s = nextPage as INavigator;

        if (s != null)
            s.UtilizeState(state);
        else
            throw new ArgumentException("NextPage is not INavigator! " + nextPage.Name.ToString());
    }
}