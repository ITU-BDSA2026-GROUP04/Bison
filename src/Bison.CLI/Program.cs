

using System.Net.Http.Headers;
using System.Security.Claims;
using CLIapp;

public class Program 
{   
    static Tree _tree = new Tree();
    public static async Task<int> Main(string[] args)
    {   
        //create taxonomy hiearchy
        buildTree();

        //Make client for our web-database on localhost
        HttpClient client = makeClient();
        return CLIapp.CLIapp.buildRoot(client, args, _tree);
        
        
    }

    static void buildTree()
    {
        _tree.create();
    
    }

    public static Tree getTree()
    {
        return _tree;
    
    }
    

    static HttpClient makeClient()
    {
        var baseURL = "http://localhost:5094"; //this is set in the Bison.CSVDBService --> properties --> launchsettings
        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
        return client;
    }
}



    
















//Location command


