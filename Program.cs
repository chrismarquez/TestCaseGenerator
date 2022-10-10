// See https://aka.ms/new-console-template for more information

class Program
{
    static void Main()
    {
        var combinations = new List<Tuple<string, List<string>>>()
        {
            Tuple.Create("a", new List<string>() { "off", "onload" }),
            Tuple.Create("js", new List<string>() { "off", "prefetch", "preload" })
        };
        var prog = new Program();
        var result = prog.GeneratePermSequence(combinations);
        result.ForEach(setting =>
        {
            Console.WriteLine("=== Setting ===");
            Console.WriteLine(string.Join(Environment.NewLine, setting));
        });
    }

    public List<Dictionary<string, string>> GeneratePermSequence(List<Tuple<string, List<string>>> parameters)
    {
        if (!parameters.Any())
        {
            return new List<Dictionary<string, string>>();
        }

        var head = parameters.First();
        var (name, values) = head;
        var tail = parameters.GetRange(1, parameters.Count - 1);
        var tailSequence = GeneratePermSequence(tail);
        var headSequence = values.Select(value =>
            new Dictionary<string, string>() { { name, value } }
        );
        if (!tailSequence.Any())
        {
            return headSequence.ToList();
        }

        var result = new List<Dictionary<string, string>>();
        foreach (var headDict in headSequence)
        {
            foreach (var tailDict in tailSequence)
            {
                var merge = new Dictionary<string, string>();
                headDict.ToList().ForEach(x => merge.Add(x.Key, x.Value));
                tailDict.ToList().ForEach(x => merge.Add(x.Key, x.Value));
                result.Add(merge);
            }
        }

        return result;
    }

}