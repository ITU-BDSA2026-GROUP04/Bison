namespace HelpFunctions;
public static class HelpFunction {
    
    public static void resetDatabases()
    {
        System.IO.File.Delete("./observations.csv");
        System.IO.File.Delete("./proposals.csv");
        System.IO.File.Delete("./comments.csv");
    }
}