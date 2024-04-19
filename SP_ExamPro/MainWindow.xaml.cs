using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SP_ExamPro
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string word = "Завдання";
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void bt_search_Click(object sender, RoutedEventArgs e)
        {
            
            string path = tbx_path.Text;
            string word = tbx_word.Text;         
            List<string> files = new List<string>();

            tb_data.Text = "Починаємо перевірку файлів...\n";
            pb.Value = 0;

            var progress = new Progress<int>(value =>
            {
                pb.Value = value;
            });

            await Task.Run(() =>
            {
                GetFoldersAsync(path, files);
                Dispatcher.Invoke(() => tb_data.Text += $"{0}: {files.Count} {Environment.NewLine}");
                Parallel.ForEach(files, (file) => ReadFile(file, word).Wait());
            });
        }

        private void UpdateStatus(string status)
        {
            Dispatcher.Invoke(() =>
            {
                tb_data.Text += status + "\n";
            });
        }
        private async Task ReadFile(string path, string word)
        {
            
            int count = 0;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string? text = await sr.ReadLineAsync();
                        //if (text != null && text.Contains(word)) - враховує регістр букв в слові
                        if (text != null && text.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) // без врахування регістру
                        {
                            count++;
                        }
                    }
                }
            }
            Dispatcher.Invoke(() => tb_data.Text += $"{System.IO.Path.GetFileName(path)}\t| {System.IO.Path.GetFullPath(path)}\t| {count}{Environment.NewLine}");
        }
        private async Task<string> GetFoldersAsync(string path, List<string> files)
        {
            try
            {
                StringBuilder filesList = new StringBuilder();

                foreach (var file in Directory.EnumerateFiles(path))
                {
                    files.Add(file);
                }

                foreach (var folder in Directory.EnumerateDirectories(path))
                {
                    GetFoldersAsync(folder, files);
                }

                return filesList.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка обробки каталогу {path}: {ex.Message}");
            }
        }
    }
}