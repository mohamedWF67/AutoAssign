# AutoAssign 📋

A powerful WPF application designed to automate data entry from CSV files into forms or spreadsheets. AutoAssign streamlines the process of filling out repetitive forms by automatically typing student information, IDs, emails, and names with customizable formatting and arrangement options.

![AutoAssign Logo](AutoAssign/Assets/AutoAssignLogo.ico)

## ✨ Features

### 🔄 Automated Data Entry
- Import CSV files containing student/participant data
- Automatically fill forms with names, IDs, and email addresses
- Global hotkey support (`Ctrl + Shift + Q`) for seamless operation
- Configurable delay timing for different systems

### 📧 Flexible Email Generation
- Use existing email addresses from CSV
- Generate custom emails using format: `FirstName + ID + @domain`
- Configurable email domain (defaults to `bue.edu.eg`)
- Automatic fallback to custom generation when email column is missing

### 🧩 New Algorithm With Field Configuration (V0.4+)
- Configure up to three fields per entry: First, Second, Third
- Field options: None, Name, ID, Email, Custom Email
- Works with multiple target environments (Forms, Excel, Excel Form)

### 🧭 Arrangement Styles (V0.4.1)
- By Person: Enter selected fields for each person before moving to the next
- By Field: Enter the first field for all people, then the second, then the third

### 📊 Legacy Fill Formats (Old Algorithm)
- ID + Name
- Name + ID
- Names Then IDs
- IDs Then Names
- Names Only
- IDs Only

### 🎯 Target Application Support
- Forms: Standard web forms and desktop applications
- Excel: Direct Excel spreadsheet filling
- Excel Form: Specialized Excel form handling with alternating Tab/Enter navigation

### 🛡️ Data Validation
- Comprehensive CSV validation
- Checks for required ID and Name columns
- Empty file detection
- User-friendly error messages

### 🎨 Modern UI
- Clean, card-based interface for better organization (WPF-UI)
- Mica backdrop theme for a modern Windows look
- Improved layout and spacing for readability

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
ID,Name
12345,John Doe
67890,Jane Smith
```
Note: The `Email` column is optional. If not present, the application can generate emails using the Custom Email option.

## 🎮 Usage

### Basic Setup
1. Load CSV File: Click “File input” to select your CSV file
2. Choose Algorithm:
   - New Algorithm (recommended): flexible field configuration
   - Old Algorithm: legacy predefined formats
3. For New Algorithm:
   - Select Arrangement Style: By Person or By Field
   - Configure Fields: Set First, Second, Third from Name/ID/Email/Custom Email
4. For Old Algorithm:
   - Choose Fill Style from the predefined list
   - Select ID/Email Mode (ID, Email, or Custom Email)
5. Configure Settings: Adjust delay time and move method as needed
6. Get Ready: Click “Not Ready” until it shows “Ready”

### Data Entry Process
1. Position your cursor in the target form/spreadsheet
2. Press `Ctrl + Shift + Q` to start automated entry
3. Data is typed according to your algorithm, arrangement, and movement settings
4. Adjust delay as needed for your environment

###  Settings Configuration

#### Algorithm Selection (V0.4+)
- New Algorithm: Field-based and arrangement-aware
- Old Algorithm: Legacy formats for backward compatibility

#### Arrangement Styles (V0.4.1)
- By Person: e.g., Name → ID → Email for each person
- By Field: e.g., all Names, then all IDs, then all Emails

#### Delay Time
- Adjust the delay between keystrokes (0–1000ms)
- Higher values for slower systems or web forms
- Lower values for faster local applications

#### Move Methods
- Forms: Uses Tab to move between fields
- Excel: Uses Enter to move to the next cell
- Excel Form: Alternates between Tab and Enter

## 🔧 Technical Details

### Built With
- .NET 9.0
- WPF
- WPF-UI (modern controls and theming)
- CsvHelper (CSV processing)
- WindowsInput (keyboard automation)
- NHotkey.Wpf (global hotkeys)
- MessagePack (settings persistence)

### Data Persistence
- Settings saved to `Documents/AutoAssign/data.bin`
- Persisted options: file path, format preferences, identification mode, move method, delay, email domain, new algorithm field selections, and arrangement style
- Restored automatically on app startup

## 📱 User Interface

### Home Tab
- CSV file selection
- Fill style (Old Algorithm)
- ID/Email mode selection
- Email domain input (when Custom Email is selected)
- Arrangement style (New Algorithm)
- Field configuration (New Algorithm)
- Ready status indicator at the bottom

### Settings Tab
- Delay time slider
- Algorithm toggle (New vs Old)
- Move method selection
- Version display and update checker

## 🔒 Privacy & Security
- All processing is local
- No internet transfer of your CSV data
- Files and settings stored locally
- No elevated permissions required beyond file access

## 🐛 Troubleshooting

### Common Issues

CSV File Not Loading
- Ensure your CSV has `ID` and `Name` columns
- Check that the file is not empty
- Verify the file is not locked by another application

Automation Not Working
- Make sure the application shows “Ready”
- Verify the target application has focus
- Increase delay time if fields are being skipped

Email Generation Issues
- Ensure names contain at least a first name
- Verify the email domain is correct
- Select Custom Email if your CSV lacks an Email column

Field Configuration Issues
- Ensure at least one of the three fields is not “None”
- Try switching arrangement style to match your target app’s flow

## 🤝 Contributing
We welcome contributions! Please feel free to submit issues, feature requests, or pull requests.

## 📄 License
This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

## 🔄 Changelog

### Version 0.4.1 (Latest)
- Added Arrangement Styles: By Person and By Field
- Modernized UI with card-based layout and improved spacing
- Three-field configuration grid for the New Algorithm
- Persist arrangement style and field selections between sessions
- Optimized window dimensions for better content display

### Version 0.4.0
- Introduced New vs Old Algorithm toggle
- Enhanced export handling and readability
- Additional selection options for flexible output

### Version 0.3.6
- Enhanced CSV validation and error handling
- Added Custom Email generation with configurable domains
- Updated to .NET SDK 9.0
- Improved UI with dynamic email domain visibility

## 📞 Support
For support, please:
1. Check the [Issues](../../issues) page for existing solutions
2. Create a new issue with detailed information
3. Include your CSV file structure (remove sensitive data)

---
**AutoAssign** — Streamlining data entry, one CSV at a time! 🚀
