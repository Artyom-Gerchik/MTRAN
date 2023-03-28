using LexicalAnalyzer.Functional;
using SyntaxAnalyzer.Nodes;

namespace SemanticAnalyzer.Functional;

internal class Semantic
{
    private AbstractNode Root { get; set; }
    private Dictionary<string, List<Token>> Functions { get; set; } = new();

    public Semantic(AbstractNode root)
    {
        Root = root;
    }

    public void CheckCode()
    {
        CheckNodes(Root);
    }

    private void CheckNodes(AbstractNode? node)
    {
        if (node == null) return;

        if (node is StatementsNode statementsNode)
            foreach (var element in statementsNode.Nodes)
                CheckNodes(element);

        if (node is FunctionNode functionNode)
        {
            Functions.Add(functionNode.Function.Identifier, functionNode.Parameters);
            CheckNodes(functionNode.Body);
        }

        if (node is WhileNode whileNode)
        {
            CheckNodes(whileNode.Condition);
            CheckNodes(whileNode.Body);
        }

        if (node is CoutNode coutNode)
        {
            var parameters = coutNode.Parameters;

            foreach (var parameter in parameters) CheckNodes(parameter);
        }

        if (node is CinNode cinNode)
        {
            var parameters = cinNode.Parameters;
            foreach (var parameter in parameters)
                if (parameter is BinaryOperationNode binaryOperationNode &&
                    binaryOperationNode.Operator.Identifier == "[]")
                    CheckNodes(binaryOperationNode);
                else if (parameter is VariableNode variableNode)
                    CheckNodes(variableNode);
                else
                    throw new Exception("Need var as param for cin");
        }

        if (node is ForNode forNode)
        {
            CheckNodes(forNode.First);
            CheckNodes(forNode.Second);
            CheckNodes(forNode.Third);
            CheckNodes(forNode.Body);
        }

        if (node is FunctionExecutionNode functionExecutionNode)
        {
            var parameters = Functions[functionExecutionNode.Function.Identifier];
            var needParamsCount = parameters.Count;
            var executionParams = functionExecutionNode.Parameters;
            var gotParamsCount = executionParams.Count;

            if (needParamsCount != gotParamsCount)
                throw new Exception($"Need {needParamsCount} params, got {gotParamsCount}");

            for (var index = 0; index < needParamsCount; index++)
            {
                var returnType = GetReturnType(executionParams[index]);

                if (parameters[index].Type != returnType)
                    throw new Exception($"need {parameters[index].Type}, got {returnType}");
            }
        }

        if (node is SwitchNode switchNode)
        {
            if (switchNode.Variable.Type != "int" && switchNode.Variable.Type != "char" &&
                switchNode.Variable.Type != "bool")
                throw new Exception("var in switch must be int or char or bool");

            CheckNodes(switchNode.Body);
        }

        if (node is CaseNode caseNode)
            if (caseNode.Literal.Type != "int literal" && caseNode.Literal.Type != "char literal" &&
                caseNode.Literal.Type != "bool literal")
                throw new Exception("after switch need int or char or bool literal");

        if (node is KeyWordNode) return;

        if (node is BinaryOperationNode binaryOperationNode1)
        {
            var returnType1 = GetReturnType(binaryOperationNode1.LeftNode);
            var returnType2 = GetReturnType(binaryOperationNode1.RightNode);

            if (returnType1 != returnType2)
            {
                if ((returnType1 != "int" || returnType2 != "float") &&
                    (returnType1 != "int" || returnType2 != "char") &&
                    (returnType1 != "float" || returnType2 != "char") &&
                    (returnType1 != "float" || returnType2 != "int") &&
                    (returnType1 != "char" || returnType2 != "int") &&
                    (returnType1 != "char" || returnType2 != "float"))
                {
                    if (binaryOperationNode1.Operator.Identifier != "new" &&
                        binaryOperationNode1.Operator.Identifier != "[]")
                        throw new Exception(
                            $"not real to do operation {binaryOperationNode1.Operator.Identifier} for {returnType1} and {returnType2}");

                    if (returnType2 != "int" && returnType2 != "char")
                        throw new Exception(
                            $"not real to do operation {binaryOperationNode1.Operator.Identifier} for {returnType1} and {returnType2}");
                }
                else
                {
                    if (binaryOperationNode1.Operator.Identifier == "new" ||
                        binaryOperationNode1.Operator.Identifier == "[]")
                        if (returnType1 != "int" && returnType2 != "char")
                            throw new Exception(
                                $"not real to do operation {binaryOperationNode1.Operator.Identifier} for {returnType1} and {returnType2}");
                }
            }
        }

        if (node is UnaryOperationNode unaryOperationNode)
        {
            var returnType = GetReturnType(unaryOperationNode.Operand);

            if (returnType == "string" || returnType == "bool")
                throw new Exception(
                    $"not real to do operation {unaryOperationNode.Operator.Identifier} for {returnType}");
        }

        return;
    }

    private string GetReturnType(AbstractNode abstractNode)
    {
        if (abstractNode is BinaryOperationNode binaryOperationNode)
        {
            var returnType1 = GetReturnType(binaryOperationNode.LeftNode);
            var returnType2 = GetReturnType(binaryOperationNode.RightNode);

            if (returnType1 != returnType2)
            {
                if ((binaryOperationNode.Operator.Identifier == "new" ||
                     binaryOperationNode.Operator.Identifier == "[]") && returnType2 == "int")
                {
                    if (binaryOperationNode.Operator.Identifier == "new")
                        return GetReturnType(binaryOperationNode.LeftNode) + "#";

                    var returnType3 = GetReturnType(binaryOperationNode.LeftNode);

                    if (returnType3.EndsWith('#')) return returnType3.Remove(returnType3.Length - 1);

                    if (returnType3 == "string") return "char";

                    throw new Exception($"not real to do operation [] for {returnType3}");
                }

                if ((returnType1 != "int" || returnType2 != "float") &&
                    (returnType1 != "int" || returnType2 != "char") &&
                    (returnType1 != "float" || returnType2 != "char") &&
                    (returnType1 != "float" || returnType2 != "int") &&
                    (returnType1 != "char" || returnType2 != "int") &&
                    (returnType1 != "char" || returnType2 != "float"))
                    throw new Exception(
                        $"not real to do operation {binaryOperationNode.Operator.Identifier} for {returnType1} and {returnType2}");

                if (binaryOperationNode.Operator.Identifier == "+" || binaryOperationNode.Operator.Identifier == "-" ||
                    binaryOperationNode.Operator.Identifier == "*" || binaryOperationNode.Operator.Identifier == "/")
                {
                    if (returnType1 == "float" || returnType2 == "float") return "float";

                    return "int";
                }

                if (binaryOperationNode.Operator.Identifier == "==" ||
                    binaryOperationNode.Operator.Identifier == "!=" ||
                    binaryOperationNode.Operator.Identifier == "<" || binaryOperationNode.Operator.Identifier == ">")
                    return "int";

                return GetReturnType(binaryOperationNode.LeftNode);
            }

            if (binaryOperationNode.Operator.Identifier == "==" || binaryOperationNode.Operator.Identifier == "!=" ||
                binaryOperationNode.Operator.Identifier == "<" || binaryOperationNode.Operator.Identifier == ">")
                return "int";

            //check later
            if ((binaryOperationNode.Operator.Identifier == "new" ||
                 binaryOperationNode.Operator.Identifier == "[]") && returnType2 == "int")
            {
                if (binaryOperationNode.Operator.Identifier == "new")
                    return GetReturnType(binaryOperationNode.LeftNode) + "#";

                var returnType3 = GetReturnType(binaryOperationNode.LeftNode);

                if (returnType3.EndsWith('#')) return returnType3.Remove(returnType3.Length - 1);

                if (returnType3 == "string") return "char";

                throw new Exception($"not real to do operation [] for {returnType3}");
            }

            return returnType1;
        }

        if (abstractNode is UnaryOperationNode unaryOperationNode)
        {
            var returnType = GetReturnType(unaryOperationNode.Operand);

            if (returnType == "string" || returnType == "bool")
                throw new Exception($"not real to do {unaryOperationNode.Operator.Identifier} for {returnType}");

            return returnType;
        }

        if (abstractNode is IfNode ifNode)
        {
            CheckNodes(ifNode.Condition);
            CheckNodes(ifNode.Body);
            CheckNodes(ifNode.ElseBody);

            var returnType1 = GetReturnType(ifNode.Body);
            var returnType2 = GetReturnType(ifNode.ElseBody!);


            if (returnType1 != returnType2)
                if ((returnType1 != "int" || returnType2 != "float") &&
                    (returnType1 != "int" || returnType2 != "char") &&
                    (returnType1 != "float" || returnType2 != "char") &&
                    (returnType1 != "float" || returnType2 != "int") &&
                    (returnType1 != "char" || returnType2 != "int") &&
                    (returnType1 != "char" || returnType2 != "float"))
                    throw new Exception(
                        $"different return types : {returnType1} and {returnType2}");
        }


        if (abstractNode is VariableNode variableNode) return variableNode.Variable.Type;

        if (abstractNode is LiteralNode literalNode) return literalNode.Literal.Type.Split()[0];

        if (abstractNode is VariableTypeNode variableTypeNode) return variableTypeNode.VariableType.Identifier;

        return "nothing";
    }
}