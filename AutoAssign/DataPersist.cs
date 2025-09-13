using System.IO;
using MessagePack;

namespace AutoAssign;
[MessagePackObject]
public class DataPersist
{
    public static String file { get; set; }
    [Key(0)]
    public String FilePath { get; set; }
    [Key(1)]
    public int FormatChooserIndex { get; set; }
    [Key(2)]
    public int IdentificationIndex { get; set; }
    [Key(3)]
    public int MoveMethodIndex { get; set; }
    [Key(4)]
    public int DelayTime { get; set; }
    [Key(5)]
    public string EmailDomain { get; set; }

    static DataPersist()
    {
        // Get the user's Documents folder
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Create "AutoAssign" subfolder inside Documents
        string folder = Path.Combine(documents, "AutoAssign");
        Directory.CreateDirectory(folder); // ensures it exists

        // Full path to data.bin
        file = Path.Combine(folder, "data.bin");
    }
}