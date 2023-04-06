using SyntaxAnalyzer.Nodes;
using SemanticAnalyzer.Functional;

namespace LAB5.Functional;

public class Executor
{
    private AbstractNode Root { get; set; }
    private Dictionary<string, Dictionary<string, object?>> VariableTables { get; set; } = new();
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

                if (param is BinaryOperationNode binaryOperationNode)
                {
                    var leftNode = binaryOperationNode.LeftNode as VariableNode;
                    var indexRightNodeToInsert = WorkOnNode(binaryOperationNode.RightNode) as int?;

                    while (codeBlock != "-1")
                    {
                        if (VariableTables[codeBlock].ContainsKey(leftNode!.Variable.Identifier))
                        {
                            break;
                        }
                        else
                        {
                            codeBlock = ModifyLocalCodeBlock(codeBlock);
                        }
                    }

                    var paramType = Semantic.GetReturnType(param);

                    switch (paramType)
                    {
                        case "int":
                            (VariableTables[codeBlock][leftNode!.Variable.Identifier] as List<int>)![
                                int.Parse(indexRightNodeToInsert.ToString()!)] = int.Parse(Console.ReadLine()!);
                            break;
                        case "float":
                            (VariableTables[codeBlock][leftNode!.Variable.Identifier] as List<double>)![
                                int.Parse(indexRightNodeToInsert.ToString()!)] = double.Parse(Console.ReadLine()!);
                            break;
                        case "char":
                            (VariableTables[codeBlock][leftNode!.Variable.Identifier] as List<char>)![
                                int.Parse(indexRightNodeToInsert.ToString()!)] = char.Parse(Console.ReadLine()!);
                            break;
                        case "bool":
                            (VariableTables[codeBlock][leftNode!.Variable.Identifier] as List<bool>)![
                                int.Parse(indexRightNodeToInsert.ToString()!)] = bool.Parse(Console.ReadLine()!);
                            break;
                        default:
                            (VariableTables[codeBlock][leftNode!.Variable.Identifier] as List<string>)![
                                int.Parse(indexRightNodeToInsert.ToString()!)] = Console.ReadLine()!;
                            break;
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

        if (abstractNode is FunctionExecutionNode functionExecutionNode)
        {
            var codeLevel = CodeDepthLevel;
            var codeParent = CodeDepthParent;
            var codeBlock = CodeBlock;

            foreach (var key in Functions[functionExecutionNode.Function.Identifier].Keys)
            {
                foreach (var body in Functions[functionExecutionNode.Function.Identifier].Values)
                {
                    var paramtrs = VariableTables[key].Keys.ToList();

                    for (int index = 1; index < functionExecutionNode.Parameters.Count; index++)
                    {
                        VariableTables[key][paramtrs[index]] = WorkOnNode(functionExecutionNode.Parameters[index]);
                    }

                    CodeBlock = key;
                    CodeDepthLevel = int.Parse(key.Split(":")[0]);
                    CodeDepthParent = int.Parse(key.Split(":")[^1]);
                    InFor = true;
                    WorkOnNode(body);
                }
            }

            CodeDepthLevel = codeLevel;
            CodeDepthParent = codeParent;
            CodeBlock = codeBlock;

            return null;
        }

        if (abstractNode is VariableNode varNode)
        {
            var codeBlock = GetCodeBlock();

            while (codeBlock != "-1")
            {
                if (VariableTables[codeBlock].ContainsKey(varNode.Variable.Identifier))
                {
                    return VariableTables[codeBlock][varNode.Variable.Identifier];
                }
                else
                {
                    ModifyLocalCodeBlock(codeBlock);
                }
            }
        }

        if (abstractNode is SwitchNode switchNode)
        {
            var codeBlock = GetCodeBlock();

            while (codeBlock != "-1")
            {
                if (VariableTables[codeBlock].ContainsKey(switchNode.Variable.Identifier))
                {
                    break;
                }
                else
                {
                    ModifyLocalCodeBlock(codeBlock);
                }
            }

            SwitchValue = VariableTables[codeBlock][switchNode.Variable.Identifier];
            WorkOnNode(switchNode.Body);
            NeedToExecute = true;
            FoundDefault = false;
            InSwitch = false;

            return null;
        }

        if (abstractNode is CaseNode caseNode)
        {
            NeedToExecute = false;

            if (caseNode.Literal.Identifier.Replace("\'", "") == SwitchValue?.ToString())
            {
                NeedToExecute = true;
                InSwitch = true;
            }

            return null;
        }

        if (abstractNode is WhileNode whileNode && (NeedToExecute || FoundDefault))
        {
            while (true)
            {
                var whileCondition = WorkOnNode(whileNode.Condition) as bool?;

                if (whileCondition != null)
                {
                    if (whileCondition == false)
                    {
                        break;
                    }
                }

                InFor = true;

                var saveCodeBlock = CodeBlock;
                var saveCodeLevel = CodeDepthLevel;
                var saveCodeParent = CodeDepthParent;

                WorkOnNode(whileNode.Body);

                CodeBlock = saveCodeBlock;
                CodeDepthLevel = saveCodeLevel;
                CodeDepthParent = saveCodeParent;
            }

            return null;
        }

        if (abstractNode is IfNode ifNode && (NeedToExecute || FoundDefault))
        {
            var ifCondition = WorkOnNode(ifNode.Condition) as bool?;

            object? ifResult;

            if (ifCondition == true)
            {
                ifResult = WorkOnNode(ifNode.Body);
            }
            else
            {
                ifResult = WorkOnNode(ifNode.ElseBody);
            }

            return ifResult;
        }

        if (abstractNode is ForNode forNode && (NeedToExecute || FoundDefault))
        {
            IncreaseDepth();

            WorkOnNode(forNode.First);

            while (true)
            {
                var forCondition = WorkOnNode(forNode.Second) as bool?;

                if (forCondition != null)
                {
                    if (forCondition == false || FoundBreak)
                    {
                        DecreaseDepthOnlyForLevel();
                        FoundBreak = false;
                        break;
                    }
                }

                InFor = true;

                var saveCodeBlock = CodeBlock;
                var saveCodeLevel = CodeDepthLevel;
                var saveCodeParent = CodeDepthParent;

                WorkOnNode(forNode.Body);

                CodeBlock = saveCodeBlock;
                CodeDepthLevel = saveCodeLevel;
                CodeDepthParent = saveCodeParent;

                WorkOnNode(forNode.Third);
            }

            CodeDepthParent -= 1;

            ExecuteNode(forNode);

            return null;
        }

        if (abstractNode is UnaryOperationNode unaryOperationNode && (NeedToExecute || FoundDefault))
        {
            if (unaryOperationNode.Operator.Identifier == "++")
            {
                var variable = unaryOperationNode.Operand as VariableNode;
                var codeBlock = GetCodeBlock();

                if (variable.Variable.Type == "int")
                {
                    VariableTables[codeBlock][variable.Variable.Identifier] =
                        (VariableTables[codeBlock][variable.Variable.Identifier] as int?)! + 1;
                }

                if (variable.Variable.Type == "float")
                {
                    VariableTables[codeBlock][variable.Variable.Identifier] =
                        (VariableTables[codeBlock][variable.Variable.Identifier] as double?)! + 1;
                }

                if (variable.Variable.Type == "char")
                {
                    VariableTables[codeBlock][variable.Variable.Identifier] =
                        (VariableTables[codeBlock][variable.Variable.Identifier] as char?)! + 1;
                }
            }

            if (unaryOperationNode.Operator.Identifier == "--")
            {
                var variable = unaryOperationNode.Operand as VariableNode;
                var codeBlock = GetCodeBlock();

                if (variable.Variable.Type == "int")
                {
                    VariableTables[codeBlock][variable.Variable.Identifier] =
                        (VariableTables[codeBlock][variable.Variable.Identifier] as int?)! - 1;
                }

                if (variable.Variable.Type == "float")
                {
                    VariableTables[codeBlock][variable.Variable.Identifier] =
                        (VariableTables[codeBlock][variable.Variable.Identifier] as double?)! - 1;
                }

                if (variable.Variable.Type == "char")
                {
                    VariableTables[codeBlock][variable.Variable.Identifier] =
                        (VariableTables[codeBlock][variable.Variable.Identifier] as char?)! - 1;
                }
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

        if (abstractNode is StatementsNode statementsNode)
        {
            IncreaseDepth();
            foreach (var node in statementsNode.Nodes)
            {
                ExecuteNode(node);
            }

            DecreaseDepthOnlyForLevel();
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
        var block = CodeBlock.Split(":");
        block[0] = CodeDepthLevel.ToString();
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
        var block = CodeBlock.Split(":");
        block[0] = CodeDepthLevel.ToString();
        CodeBlock = "";
        CodeBlock += block[0];

        for (int index = 1; index < block.Length; index++)
        {
            CodeBlock += $":{block[index]}";
        }
    }

    private string ModifyLocalCodeBlock(string codeBlockToModify)
    {
        codeBlockToModify = codeBlockToModify.Remove(codeBlockToModify.Length - 2);

        var block = codeBlockToModify.Split(":");
        block[0] = (int.Parse(block[0]) - 1).ToString();
        codeBlockToModify = "";
        codeBlockToModify += block[0];

        for (int index = 1; index < block.Length; index++)
        {
            codeBlockToModify += $":{block[index]}";
        }

        return codeBlockToModify;
    }
}