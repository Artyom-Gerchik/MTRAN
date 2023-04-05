using SyntaxAnalyzer.Nodes;
using SemanticAnalyzer.Functional;

namespace LAB5.Functional;

public class Executor
{
    private AbstractNode Root { get; set; }
    private Dictionary<string, Dictionary<string, object>> VariableTables { get; set; } = new();
    private Semantic Semantic { get; set; }

    public Executor(AbstractNode root, Dictionary<string, Dictionary<string, string>> variableTables, Semantic semantic)
    {
        Root = root;

        foreach (var codeBlock in variableTables.Keys)
        {
            VariableTables.Add(codeBlock, new());

            foreach (var key in variableTables[codeBlock].Keys)
            {
                VariableTables[codeBlock].Add(key, new());
            }
        }

        Semantic = semantic;
    }

    public void Execute()
    {
    }

    public object? ExecuteNode(AbstractNode? abstractNode)
    {
        if (abstractNode == null)
        {
            return null;
        }

        if (abstractNode is StatementsNode statementsNode)
        {
            
        }

        return null;
    }
}