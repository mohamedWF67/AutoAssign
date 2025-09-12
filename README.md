Based on the code structure and functionality, here's a comprehensive README file for the AutoAssign application:

# AutoAssign 📋

A powerful WPF application designed to automate data entry from CSV files into forms or spreadsheets. AutoAssign streamlines the process of filling out repetitive forms by automatically typing student information, IDs, emails, and names with customizable formatting options.

![AutoAssign Logo](AutoAssign/Assets/AutoAssignLogo.ico)

## ✨ Features

### 🔄 **Automated Data Entry**
- Import CSV files containing student/participant data
- Automatically fill forms with names, IDs, and email addresses
- Global hotkey support (`Ctrl + Shift + Q`) for seamless operation
- Configurable delay timing for different systems

### 📧 **Flexible Email Generation**
- Use existing email addresses from CSV
- Generate custom emails using format: `FirstName + ID + @domain`
- Configurable email domain (defaults to `bue.edu.eg`)
- Automatic fallback to custom generation when email column is missing

### 📊 **Multiple Fill Formats**
- **ID + Name**: Enter ID followed by name for each record
- **Name + ID**: Enter name followed by ID for each record
- **Names Then IDs**: Enter all names first, then all IDs
- **IDs Then Names**: Enter all IDs first, then all names
- **Names Only**: Enter only names
- **IDs Only**: Enter only IDs

### 🎯 **Target Application Support**
- **Forms**: Standard web forms and desktop applications
- **Excel**: Direct Excel spreadsheet filling
- **Excel Form**: Specialized Excel form handling with alternating Tab/Enter navigation

### 🛡️ **Data Validation**
- Comprehensive CSV validation
- Checks for required ID and Name columns
- Empty file detection
- User-friendly error messages

## 🚀 Getting Started

### Prerequisites
- Windows OS
- .NET 9.0 Runtime
- CSV file with at minimum `ID` and `Name` columns

### Installation
1. Download the latest release from the [Releases](../../releases) page
2. Extract the files to your desired location
3. Run `AutoAssign.exe`

### CSV File Format
Your CSV file should contain at minimum these columns:
```
ID,Name,Email
12345,John Doe,john.doe@example.com
67890,Jane Smith,jane.smith@example.com
```


**Note**: The `Email` column is optional. If not present, the application will use the Custom Email generation feature.

## 🎮 Usage

### Basic Setup
1. **Load CSV File**: Click "File input" to select your CSV file
2. **Choose Fill Style**: Select how you want data to be entered
3. **Select ID/Email Mode**: Choose between ID, Email, or Custom Email generation
4. **Configure Settings**: Adjust delay time and move method as needed
5. **Get Ready**: Click "Not Ready" button until it shows "Ready"

### Data Entry Process
1. Position your cursor in the target form/spreadsheet
2. Press `Ctrl + Shift + Q` to start automated entry
3. The application will automatically type the data according to your settings
4. Use the configurable delay to match your system's response time

### Settings Configuration

#### Delay Time
- Adjust the delay between keystrokes (0-1000ms)
- Higher values for slower systems or web forms
- Lower values for faster local applications

#### Move Methods
- **Forms**: Uses Tab key to move between fields
- **Excel**: Uses Enter key to move to next cell
- **Excel Form**: Alternates between Tab and Enter keys

## 🔧 Technical Details

### Built With
- **.NET 9.0** - Modern .NET framework
- **WPF** - Windows Presentation Foundation for UI
- **CsvHelper** - CSV file processing
- **WindowsInput** - Keyboard automation
- **NHotkey.Wpf** - Global hotkey support
- **MessagePack** - Data serialization

### Data Persistence
- Application settings are automatically saved to `Documents/AutoAssign/data.bin`
- Settings include file path, format preferences, and configuration options
- Settings are restored when the application restarts

## 📱 User Interface

The application features a clean, tabbed interface:

### Home Tab
- CSV file selection
- Fill style configuration  
- ID/Email mode selection
- Custom email domain input (when applicable)
- Ready status indicator

### Settings Tab
- Delay time adjustment with slider
- Move method selection
- Real-time setting updates

## 🔒 Privacy & Security

- All data processing is done locally on your machine
- No data is transmitted over the internet
- CSV files and settings are stored locally
- Application requires no special permissions beyond file access

## 🐛 Troubleshooting

### Common Issues

**CSV File Not Loading**
- Ensure your CSV has `ID` and `Name` columns
- Check that the file is not empty
- Verify the file is not locked by another application

**Automation Not Working**
- Make sure the application shows "Ready" status
- Verify the target application has focus
- Try increasing the delay time in settings

**Email Generation Issues**
- Check that names contain at least one word
- Verify the email domain format is correct
- Ensure the Custom Email option is selected

## 🤝 Contributing

We welcome contributions! Please feel free to submit issues, feature requests, or pull requests.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔄 Changelog

### Version 0.3.6 (Latest)
- ✅ Enhanced CSV validation and error handling
- 📧 Added custom email generation with configurable domains
- 🔧 Updated to .NET SDK 9.0
- 🎨 Improved UI with dynamic email domain visibility
- 🛡️ Better error recovery mechanisms

## 📞 Support

For support, please:
1. Check the [Issues](../../issues) page for existing solutions
2. Create a new issue with detailed information
3. Include your CSV file structure (remove sensitive data)

---

**AutoAssign** - Streamlining data entry, one CSV at a time! 🚀
