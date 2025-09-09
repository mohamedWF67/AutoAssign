using System.Globalization;
using System.IO;
using System.Windows;
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
    private double myValue = 100f;
    
    public MainWindow()
    {
        InitializeComponent();
        HotkeyManager.Current.AddOrReplace("ExportData", Key.Q, ModifierKeys.Control | ModifierKeys.Shift, ExportData);
        MySlider.Value = myValue;
    }

    private void MySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (SliderValueLabel != null)
        {
            myValue = e.NewValue;
            SliderValueLabel.Content = $"Value: {myValue}";
        }
    }
    
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var records = DataService.Instance.Records;

        if (records == null)
        {
            MessageBox.Show("No data to export", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        for (int i = 0; i < records.Count; i++)
        {
            Console.WriteLine($"{records[i].Name}, {records[i].Age}");
        }
    }

    private async void ExportData(object sender, HotkeyEventArgs e)
    {
        var records = DataService.Instance.Records;
        
        if (records == null)
        {
            MessageBox.Show("No data to export", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            OpenCsvButton_Click(e, null);
            ExportData(e, null);
            return;
        }
        
        await Task.Delay((int)myValue);
        
        Console.WriteLine("Exporting data");
        switch (MyComboBox.SelectedIndex)
        {
            case 0:
                foreach (var record in records)
                {
                    _sim.Keyboard.TextEntry(record.ID);
                    KeyAction(e, null);
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                }
                break;
            case 1:
                foreach (var record in records)
                {
                    _sim.Keyboard.TextEntry(record.Name);
                    KeyAction(e, null);
                    _sim.Keyboard.TextEntry(record.ID);
                    KeyAction(e, null);
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
                    _sim.Keyboard.TextEntry(record.ID);
                    KeyAction(e, null);
                }
                break;
            case 3:
                foreach (var record in records)
                {
                    _sim.Keyboard.TextEntry(record.ID);
                    KeyAction(e, null);
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
                    _sim.Keyboard.TextEntry(record.ID);
                    KeyAction(e, null);
                }
                break;
        }
        Console.WriteLine("Data Exported");
    }

    private void KeyAction(object sender, KeyEventArgs e)
    {
        _sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
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
            FileName.Content = "Filename: " + filePath;
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            DataService.Instance.Records = csv.GetRecords<dynamic>().ToList();
        }
        
    }
}