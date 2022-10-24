// See https://aka.ms/new-console-template for more information

using HtmlAgilityPack;

enum Optimization
{
    Off,
    Prefetch,
    Preload,
    Preconnect
}

class ExperimentSetting
{
    public Optimization img { get; set; }
    public Optimization a { get; set; }
    public Optimization link { get; set; }
    public Optimization script { get; set; }
}

static class Extensions
{
    private static HtmlNode ApplyOptimization(this HtmlNode node)
    {
        return node;
    }
    
    public static HtmlNode ApplyOptimizations(this HtmlNode root, List<Dictionary<string, Optimization>> optimizations)
    {
        return root;
    }
}

class Program
{
    
    static void Main()
    {
        var combinations = new List<(string, List<Optimization>)>()
        {
            ("img", new List<Optimization>() { Optimization.Off, Optimization.Prefetch, Optimization.Preload }),
            ("a", new List<Optimization>() { Optimization.Off, Optimization.Preconnect }),
            ("link", new List<Optimization>() { Optimization.Off, Optimization.Prefetch, Optimization.Preload }),
            ("script", new List<Optimization>() {Optimization.Off, Optimization.Prefetch, Optimization.Preload }),
        };
        var prog = new Program();
        var optimizations = prog.GeneratePermSequence(combinations);
        optimizations.ForEach(setting =>
        {
            Console.WriteLine("=== Setting ===");
            Console.WriteLine(string.Join(Environment.NewLine, setting));
        });
        var document = prog.GetPage("https://www.google.com").Result;
        var modifiedDoc = document.ApplyOptimizations(optimizations);
    }

    private List<Dictionary<string, T>> GeneratePermSequence<T>(List<(string, List<T>)> parameters)
    {
        if (!parameters.Any())
        {
            return new List<Dictionary<string, T>>();
        }

        var head = parameters.First();
        var (name, values) = head;
        var tail = parameters.GetRange(1, parameters.Count - 1);
        var tailSequence = GeneratePermSequence(tail);
        var headSequence = values.Select(value =>
            new Dictionary<string, T>() { { name, value } }
        );
        if (!tailSequence.Any())
        {
            return headSequence.ToList();
        }

        var result = new List<Dictionary<string, T>>();
        foreach (var headDict in headSequence)
        {
            foreach (var tailDict in tailSequence)
            {
                var merge = new Dictionary<string, T>();
                headDict.ToList().ForEach(x => merge.Add(x.Key, x.Value));
                tailDict.ToList().ForEach(x => merge.Add(x.Key, x.Value));
                result.Add(merge);
            }
        }

        return result;
    }

    private async Task<HtmlNode> GetPage(string url)
    {
        HtmlDocument htmlSnippet = new HtmlDocument();
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
        var content = await client.GetStringAsync(url);
        htmlSnippet.LoadHtml(content);
        return htmlSnippet.DocumentNode;
    }

}