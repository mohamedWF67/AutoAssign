namespace AutoAssign;

public class DataService
{
    private static DataService _instance;
    public static DataService Instance => _instance ??= new DataService();
    
    public List<dynamic> Records { get; set; }
}