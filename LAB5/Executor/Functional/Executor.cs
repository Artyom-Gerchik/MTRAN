using SyntaxAnalyzer.Nodes;
using SemanticAnalyzer.Functional;

namespace LAB5.Functional;

public class Executor
{
    private AbstractNode Root { get; set; }
    private Dictionary<string, Dictionary<string, object>> VariableTables { get; set; } = new();
    private Semantic Semantic { get; set; }
    private string CodeBlock { get; set; }

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
        CodeBlock = "NONE";
    }

    public void Execute()
    {
        WorkOnNode(Root);
    }

    public object? WorkOnNode(AbstractNode? abstractNode)
    {
        if (abstractNode == null)
        {
            return null;
        }

        if (abstractNode is StatementsNode statementsNode)
        {
        }

        if (abstractNode is VariableTypeNode)
        {
            return null;
        }

        return null;
    }

    public string GetCodeBlock()
    {
        return CodeBlock;
    }

    public void ExecuteNode(AbstractNode? abstractNode)
    {
        if (abstractNode == null)
        {
            return;
        }

        if (abstractNode is StatementsNode)
        {
            //TODO
        }

        if (abstractNode is ForNode forNode)
        {
            ExecuteNode(forNode.Body);
        }

        if (abstractNode is IfNode ifNode)
        {
            ExecuteNode(ifNode.Body);
            ExecuteNode(ifNode.ElseBody);
        }

        if (abstractNode is WhileNode whileNode)
        {
            ExecuteNode(whileNode.Body);
        }
    }
}