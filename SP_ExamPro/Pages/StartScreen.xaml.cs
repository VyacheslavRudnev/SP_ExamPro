using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using SP_ExamPro.Navigator;

namespace SP_ExamPro.Pages;

/// <summary>
/// Interaction logic for StartScreen.xaml
/// </summary>
public partial class StartScreen : UserControl
{
    private int filesChecked = 0;
    private int filesCount = 0;
    private int totalMatches = 0;
    public StartScreen()
    {
        InitializeComponent();
    }

    private async void bt_search_Click(object sender, RoutedEventArgs e)
    {

        string path = tbx_path.Text;
        string word = tbx_word.Text;
        List<string> files = new List<string>();

        if (string.IsNullOrEmpty(tbx_word.Text) || string.IsNullOrWhiteSpace(tbx_path.Text))
        {
            NavigatorObject.oldPage = this.Clone();
            NavigatorObject.Switch(new ErrorScreen());
            return;
        }

        tb_data.Text = "Починаємо перевірку файлів...\n";
        pb.Value = 0;
        filesChecked = 0;
        totalMatches = 0;

        await GetFoldersAsync(path, files);
        filesCount = files.Count;
        pb.Maximum = 100;

        await Task.Run(() =>
        {
            Parallel.ForEach(files, (file) => ReadFile(file, word).Wait());
        });

        MessageBox.Show("Перевірку файлів завершено.\n" +
            $"Всього перевірено: {filesChecked} файлів.\n" +
            $"Знайдено: {totalMatches} співпадінь слова '{word}'.", "Результат пошуку", MessageBoxButton.OK,MessageBoxImage.Information);
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
                    if (text != null && text.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) // регістр бук не враховуєм
                    {
                        count++;
                        totalMatches++;
                    }
                }
            }
        }
        Dispatcher.Invoke(() => tb_data.Text += $"{System.IO.Path.GetFileName(path)}\t|\t{System.IO.Path.GetFullPath(path)}\t|\t{count}{Environment.NewLine}");

        filesChecked++;
        UpdateProgressBar();

    }
    private void UpdateProgressBar()
    {
        Dispatcher.Invoke(() =>
        {
            pb.Value = (int)((double)filesChecked / filesCount * 100);
        });
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
                await GetFoldersAsync(folder, files);
            }

            return filesList.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка обробки каталогу {path}: {ex.Message}");
        }
    }
    public StartScreen Clone()
    {
        return new StartScreen();
    }

    
}

