using MessagePack;

namespace AutoAssign;
[MessagePackObject]
public class DataPersist
{
    public static String file { get; set; } = "data.bin";
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
}