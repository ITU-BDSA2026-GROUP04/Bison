namespace HelpFunctions;
public static class HelpFunction {
    
    public static void resetDatabase(string type)
    {
        System.IO.File.Delete("./" + type + ".csv");
    }
}