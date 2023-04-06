using SyntaxAnalyzer.Nodes;
using SemanticAnalyzer.Functional;

namespace LAB5.Functional;

public class Executor
{
    private AbstractNode Root { get; set; }
    private Dictionary<string, Dictionary<string, object>> VariableTables { get; set; } = new();
    private Semantic Semantic { get; set; }
    private string CodeBlock { get; set; }
    private int CodeDepthLevel { get; set; }
    private int CodeDepthParent { get; set; }
    private int CodeBlockIndex { get; set; }
    private bool NeedToExecute { get; set; }
    private bool FoundBreak { get; set; }
    private bool FoundDefault { get; set; }
    private bool InSwitch { get; set; }
    private object? SwitchValue { get; set; }
    private bool InFor { get; set; }
    private Dictionary<string, Dictionary<string, AbstractNode>> Functions { get; set; } = new();

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
        CodeBlock = "-1"; // would be needed for code expanding
        CodeDepthLevel = -1;
        CodeDepthParent = -1;
        CodeBlockIndex = -1;
        NeedToExecute = true;
        FoundBreak = false;
        FoundDefault = false;
        InSwitch = false;
        SwitchValue = null;
        InFor = false;
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
            if (!InFor)
            {
                IncreaseDepth();
            }
            else
            {
                InFor = false;
            }

            foreach (var node in statementsNode.Nodes)
            {
                if (!FoundBreak)
                {
                    WorkOnNode(node);
                }
            }

            DecreaseDepthOnlyForLevel();
        }

        if (abstractNode is KeyWordNode keyWordNode)
        {
            switch (keyWordNode.KeyWord.Identifier)
            {
                case "endl":
                    return "\n";
                case "break":
                    FoundBreak = true;
                    break;
                case "default":
                    if (InSwitch)
                    {
                        NeedToExecute = false;
                    }
                    else
                    {
                        FoundDefault = true;
                    }

                    return null;
            }
        }

        if (abstractNode is CoutNode coutNode && (NeedToExecute || FoundDefault))
        {
            foreach (var param in coutNode.Parameters)
            {
                var readyParam = WorkOnNode(param);

                if (readyParam is string paramAsSTR)
                {
                    readyParam = paramAsSTR.Replace("\"", "").Replace("\\n", "\n");
                }

                Console.Write(readyParam);
            }
        }

        if (abstractNode is CinNode cinNode && (NeedToExecute || FoundDefault))
        {
            foreach (var param in cinNode.Parameters)
            {
                var codeBlock = GetCodeBlock();

                if (param is VariableNode variableNode)
                {
                    while (codeBlock != "-1")
                    {
                        if (VariableTables[codeBlock].ContainsKey(variableNode.Variable.Identifier))
                        {
                            switch (VariableTables[codeBlock][variableNode.Variable.Identifier])
                            {
                                case "int":
                                    return int.Parse(Console.ReadLine()!);
                                case "float":
                                    return double.Parse(Console.ReadLine()!);
                                case "char":
                                    return char.Parse(Console.ReadLine()!);
                                case "bool":
                                    return bool.Parse(Console.ReadLine()!);
                                default:
                                    return Console.ReadLine();
                            }

                            break;
                        }
                        else
                        {
                            codeBlock = ModifyLocalCodeBlock(codeBlock);
                        }
                    }
                }
                
            }

            return null;
        }

        if (abstractNode is LiteralNode literalNode)
        {
            switch (literalNode.Literal.Type)
            {
                case "int literal":
                    return int.Parse(literalNode.Literal.Identifier);
                case "float literal":
                    return double.Parse(literalNode.Literal.Identifier);
                case "char literal":
                    return char.Parse(literalNode.Literal.Identifier.Replace("\'", ""));
                case "bool literal":
                    return bool.Parse(literalNode.Literal.Identifier);
                default:
                    return literalNode.Literal.Identifier;
            }
        }

        if (abstractNode is FunctionNode functionNode)
        {
            if (functionNode.Function.Identifier == "main")
            {
                WorkOnNode(functionNode.Body);
            }
            else
            {
                IncreaseDepth();
                Functions.Add(functionNode.Function.Identifier,
                    new Dictionary<string, AbstractNode> { { CodeBlock, functionNode.Body } });
                DecreaseDepth();

                ExecuteNode(functionNode.Body);
            }

            return null;
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

    private void IncreaseDepth()
    {
        CodeDepthLevel += 1;
        CodeDepthParent += 1;

        var block = CodeBlock.Split(":");
        block[0] = CodeDepthLevel.ToString();
        CodeBlock = "";
        CodeBlock += block[0];

        for (int index = 1; index < block.Length; index++)
        {
            CodeBlock += $":{block[index]}";
        }

        CodeBlock += $":{CodeDepthParent}";
    }

    private void DecreaseDepth()
    {
        CodeDepthLevel -= 1;
        CodeDepthParent -= 1;

        CodeBlock = CodeBlock.Remove(CodeBlock.Length - 2);
        var block = CodeDepthLevel.ToString();
        CodeBlock = "";
        CodeBlock += block[0];

        for (int index = 1; index < block.Length; index++)
        {
            CodeBlock += $":{block[index]}";
        }
    }

    private void DecreaseDepthOnlyForLevel()
    {
        CodeDepthLevel -= 1;

        CodeBlock = CodeBlock.Remove(CodeBlock.Length - 2);
        var block = CodeDepthLevel.ToString();
        CodeBlock = "";
        CodeBlock += block[0];

        for (int index = 1; index < block.Length; index++)
        {
            CodeBlock += $":{block[index]}";
        }
    }

    private string ModifyLocalCodeBlock(string CodeBlockToModify)
    {
        CodeBlockToModify = CodeBlockToModify.Remove(CodeBlockToModify.Length - 2);

        var block = CodeBlockToModify.Split(":");
        block[0] = (int.Parse(block[0]) - 1).ToString();
        CodeBlockToModify = "";
        CodeBlockToModify += block[0];

        for (int index = 1; index < block.Length; index++)
        {
            CodeBlockToModify += $":{block[index]}";
        }

        return CodeBlockToModify;
    }
}