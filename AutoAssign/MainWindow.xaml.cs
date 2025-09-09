using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CsvHelper;
using Microsoft.Win32;
using NHotkey;
using NHotkey.Wpf;
using WindowsInput;
using WindowsInput.Native;

namespace AutoAssign;
public partial class MainWindow : Window
{
    private readonly InputSimulator _sim = new InputSimulator();
    private DataPersist _dataPersist = new DataPersist();
    private List<dynamic> Records;
    private double myValue = 100f;
    private bool _isExporting = false;

    public void SaveFiles()
    {
        _dataPersist.DelayTime = (int)myValue;
        _dataPersist.FormatChooserIndex = FormatChooser.SelectedIndex;
        _dataPersist.IdentificationIndex = ID_Email.SelectedIndex;
        _dataPersist.MoveMethodIndex = MoveMethod.SelectedIndex;
        BinaryStorage.Save(DataPersist.file, _dataPersist);
        Console.WriteLine("Saved");
    }

    public void LoadFiles()
    {
        DataPersist loaded = BinaryStorage.Load<DataPersist>(DataPersist.file);
        _dataPersist = loaded;
        Console.WriteLine($"Filepath: {loaded.FilePath} " +
                          $"Format: {loaded.FormatChooserIndex}" +
                          $"Identification: {loaded.IdentificationIndex}" +
                          $"Move Method: {loaded.MoveMethodIndex}" +
                          $" Delay: {loaded.DelayTime}");
        FormatChooser.SelectedIndex = loaded.FormatChooserIndex;
        ID_Email.SelectedIndex = loaded.IdentificationIndex;
        MoveMethod.SelectedIndex = loaded.MoveMethodIndex;
        myValue = loaded.DelayTime;
        
        FileName.Content = "Filename: " + loaded.FilePath;
        using var reader = new StreamReader(loaded.FilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        DataService.Instance.Records = csv.GetRecords<dynamic>().ToList();
        Records = DataService.Instance.Records;
    }
    
    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        SaveFiles();
    }

    public MainWindow()
    {
        InitializeComponent();
        HotkeyManager.Current.AddOrReplace("ExportData", Key.Q, ModifierKeys.Control | ModifierKeys.Shift, ExportData);
        LoadFiles();
        DelayTimeSlider.Value = myValue;
    }

    private void DelayTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (DelayValue != null)
        {
            myValue = e.NewValue;
            DelayValue.Text = $"Value: {myValue}";
        }
    }
    
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        SaveFiles();
    }

    private async void ExportData(object sender, HotkeyEventArgs e)
    {
        if (_isExporting) return;
        var records = DataService.Instance.Records;
        
        if (records == null)
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
                foreach (var record in records)
                {
                    IdentificationEntry(e, null, record);
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                break;
            case 1:
                foreach (var record in records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                    IdentificationEntry(e, null, record);
                }
                break;
            case 2:
                foreach (var record in records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                foreach (var record in records)
                {
                    IdentificationEntry(e, null, record);
                }
                break;
            case 3:
                foreach (var record in records)
                {
                    IdentificationEntry(e, null, record);
                }
                foreach (var record in records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                break;
            case 4:
                foreach (var record in records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                break;
            case 5:
                foreach (var record in records)
                {
                    IdentificationEntry(e, null, record);
                }
                break;
        }
        Console.WriteLine("Data Exported");
        _isExporting = false;
    }

    private void KeyAction(object sender, KeyEventArgs e)
    {
        if (MoveMethod.SelectedIndex == 0)
            _sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
        else
            _sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
    }

    private void IdentificationEntry(object sender, SelectionChangedEventArgs e,dynamic record)
    {
        if (ID_Email.SelectedIndex == 0)
        {
            _sim.Keyboard.TextEntry(record.ID);
            KeyAction(e, null);
        }
        else
        {
            if (record.Email != null)
                _sim.Keyboard.TextEntry(record.Email);
            else
                _sim.Keyboard.TextEntry(record.Name + record.ID);
            KeyAction(e, null);
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
            _dataPersist.FilePath = filePath;
            FileName.Content = "Filename: " + filePath;
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            DataService.Instance.Records = csv.GetRecords<dynamic>().ToList();
            Records = DataService.Instance.Records;
        }
        
    }
}