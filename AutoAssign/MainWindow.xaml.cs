using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CsvHelper;
using Microsoft.Win32;
using NHotkey;
using NHotkey.Wpf;
using WindowsInput;
using WindowsInput.Native;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;

namespace AutoAssign;
public partial class MainWindow : FluentWindow
{
    private readonly InputSimulator _sim = new InputSimulator();
    private DataPersist _dataPersist = new DataPersist();
    private static readonly HttpClient client = new HttpClient();
    private string? currentVersion;
    private string? latestVersion;
    private List<dynamic> Records;
    private double myValue = 100f;
    private bool _isExporting = false;
    private int Tranq = 0;
    private bool isReady = false;

    public void SaveFiles()
    {
        _dataPersist.DelayTime = (int)myValue;
        _dataPersist.FormatChooserIndex = FormatChooser.SelectedIndex;
        _dataPersist.IdentificationIndex = ID_Email.SelectedIndex;
        _dataPersist.MoveMethodIndex = MoveMethod.SelectedIndex;
        _dataPersist.EmailDomain = EmailDomainTextBox.Text;
        BinaryStorage.Save(DataPersist.file, _dataPersist);
        Console.WriteLine("Saved");
    }

    public void LoadFiles()
    {
        if (!File.Exists(DataPersist.file)) return;
        DataPersist loaded = BinaryStorage.Load<DataPersist>(DataPersist.file);
        _dataPersist = loaded;
        Console.WriteLine($"Filepath: {loaded.FilePath} " +
                          $"Format: {loaded.FormatChooserIndex} " +
                          $"Identification: {loaded.IdentificationIndex} " +
                          $"Move Method: {loaded.MoveMethodIndex} " +
                          $" Delay: {loaded.DelayTime} " + 
                          $" Email Domain: {loaded.EmailDomain}");
        FormatChooser.SelectedIndex = loaded.FormatChooserIndex;
        ID_Email.SelectedIndex = loaded.IdentificationIndex;
        MoveMethod.SelectedIndex = loaded.MoveMethodIndex;
        myValue = loaded.DelayTime;
        EmailDomainTextBox.Text = loaded.EmailDomain;

        if (File.Exists(loaded.FilePath))
        {
            using var reader = new StreamReader(loaded.FilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var csvrecords = csv.GetRecords<dynamic>().ToList();
            if (csvrecords.Count == 0)
            {
                MessageBox.Show("No records found in the CSV file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                FileName.Text = "Please select a CSV file";
                return;
            }

            if (csvrecords[0].ID == null)
            {
                MessageBox.Show("No IDs where found in the CSV file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                FileName.Text = "Please select a CSV file";
                return;
            }
            
            if (csvrecords[0].Name == null)
            {
                MessageBox.Show("No names where found in the CSV file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                FileName.Text = "Please select a CSV file";
                return;
            }
            
            FileName.Text = loaded.FilePath;
            DataService.Instance.Records = csvrecords;
            Records = DataService.Instance.Records;
            
        }
        else
        {
            FileName.Text = "Please select a CSV file";
        }
    }
    
    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        SaveFiles();
    }

    public MainWindow()
    {
        InitializeComponent();
        currentVersion = "V" + Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion.Split('+')[0];
        CheckUpdates();
        HotkeyManager.Current.AddOrReplace("ExportData", Key.Q, ModifierKeys.Control | ModifierKeys.Shift, ExportData);
        LoadFiles();
        DelayTimeSlider.Value = myValue;
        if (MoveMethod.SelectedIndex == 2)
        { Tranq = 0; }
    }

    private void DelayTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (DelayValue != null)
        {
            myValue = e.NewValue;
            DelayValue.Text = $"Value: {myValue}";
        }
    }

    private async void ExportData(object sender, HotkeyEventArgs e)
    {
        if (_isExporting || !isReady) return;
        
        if (Records == null)
        {
            MessageBox.Show("No data to export", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            OpenCsvButton_Click(e, null);
            ExportData(e, null);
            return;
        }
        _isExporting = true;
        await Task.Delay((int)myValue);
        Console.WriteLine("Exporting data");
        switch (FormatChooser.SelectedIndex)
        {
            case 0:
                foreach (var record in Records)
                {
                    IdentificationEntry(e, null, record);
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                break;
            case 1:
                foreach (var record in Records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                    IdentificationEntry(e, null, record);
                }
                break;
            case 2:
                foreach (var record in Records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                foreach (var record in Records)
                {
                    IdentificationEntry(e, null, record);
                }
                break;
            case 3:
                foreach (var record in Records)
                {
                    IdentificationEntry(e, null, record);
                }
                foreach (var record in Records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                break;
            case 4:
                foreach (var record in Records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                break;
            case 5:
                foreach (var record in Records)
                {
                    IdentificationEntry(e, null, record);
                }
                break;
        }

        Tranq = 0;
        Console.WriteLine("Data Exported");
        _isExporting = false;
    }

    private void KeyAction(object sender, KeyEventArgs e)
    {
        if (MoveMethod.SelectedIndex == 2)
        {
            if (Tranq == 1)
                _sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            else
                _sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
            Tranq = (Tranq + 1) % 2;
            return;
        }
        
        if (MoveMethod.SelectedIndex == 1)
            _sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        else
            _sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
    }

    private void IdentificationEntry(object sender, SelectionChangedEventArgs e,dynamic record)
    {
        switch (ID_Email.SelectedIndex)
        {
            case 0:
                _sim.Keyboard.TextEntry(record.ID);
                KeyAction(e, null);
                break;
            case 1:
                _sim.Keyboard.TextEntry(record.Email);
                KeyAction(e, null);
                break;
            case 2:
                string email = record.Name.Split(' ')[0] + record.ID + "@" + EmailDomainTextBox.Text.TrimEnd();
                _sim.Keyboard.TextEntry(email);
                KeyAction(e, null);
                break;
        }
    }

    private void OpenCsvButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select a CSV file",
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() == true)
        {
            string filePath = dialog.FileName; 
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var csvrecords = csv.GetRecords<dynamic>().ToList();
            if (csvrecords.Count == 0)
            {
                MessageBox.Show("No records found in the CSV file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (csvrecords[0].ID == null)
            {
                MessageBox.Show("No IDs where found in the CSV file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (csvrecords[0].Name == null)
            {
                MessageBox.Show("No names where found in the CSV file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _dataPersist.FilePath = filePath;
            FileName.Text = filePath;
            DataService.Instance.Records = csvrecords;
            Records = DataService.Instance.Records;
        }
        
    }

    private void MoveMethod_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MoveMethod.SelectedIndex == 2)
        {
            int i = 0;
            if (FormatChooser.SelectedIndex > 1)
                FormatChooser.SelectedIndex = 1;
            foreach (var item in FormatChooser.Items)
            {
                ComboBoxItem comboItem = (ComboBoxItem)item;
                if (i > 1)
                {
                    comboItem.IsEnabled = false;
                }
                else
                {
                    comboItem.IsEnabled = true;
                }
                i++;
            }
        }
        else
        {
            foreach (var item in FormatChooser.Items)
            {
                ComboBoxItem comboItem = (ComboBoxItem)item;
                comboItem.IsEnabled = true;
            }
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        isReady = !isReady;
        ReadyBtn.Content = isReady ? "Ready" : "Not Ready";
        ReadyBtn.BorderBrush = isReady ? Brushes.Green : Brushes.Red;
        ReadyLabel.Text = isReady ? "Please press: Ctrl + Shift + Q" : "Press Ready to start";
    }

    private async void ID_Email_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await Task.Delay(1);
        if (ID_Email.SelectedIndex == 1 && Records[0].Email == null)
        {
            MessageBox.Show("No email column in the CSV file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ID_Email.SelectedIndex = 2;
        }
        EmailDomainBlock.Visibility = ID_Email.SelectedIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
    }

    private async void CheckUpdates()
    {
        try
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "AutoAssignApp");
            string url = $"https://api.github.com/repos/mohamedWF67/AutoAssign/releases/latest";
            var response = await client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);
            latestVersion = doc.RootElement.GetProperty("tag_name").GetString();
            if (latestVersion.Equals(currentVersion))
            {
                Console.WriteLine("Updated");
                VersionUpdateLink.Visibility = Visibility.Collapsed;
                VersionText.Text = $"Version: {currentVersion.TrimStart('V')} (Latest)";
            }
            else
            {
                VersionText.Text = $"Version: {currentVersion.TrimStart('V')} -> {latestVersion.TrimStart('V')}";
                Console.WriteLine($"Version {currentVersion} is outdated. it should be {latestVersion}");
                VersionUpdateLink.Visibility = Visibility.Visible;
            }
            Console.WriteLine($"Version : {latestVersion}");
        }
        catch (HttpRequestException ex)
        {
            if (ex.Message.Contains("403") || ex.Message.Contains("rate limit"))
            {
                MessageBox.Show("GitHub API rate limit exceeded. Please try again later.", "Rate Limit Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show($"Failed to check for updates: {ex.Message}", "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Console.WriteLine($"Update check failed: {ex.Message}");
        }        
        catch (JsonException ex)
        {
            MessageBox.Show("Failed to parse update information.", "Parse Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine($"JSON parse error: {ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private void CheckUpdates_OnClick(object sender, RoutedEventArgs e)
    {
        CheckUpdates();
    }

    private void VersionUpdateLink_OnClick(object sender, RoutedEventArgs e)
    {
        string url = $"https://github.com/mohamedWF67/AutoAssign/releases/download/{latestVersion}/AutoAssign.exe";
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}