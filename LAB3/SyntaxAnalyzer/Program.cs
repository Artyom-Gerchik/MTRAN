using LexicalAnalyzer.Functional;
using SyntaxAnalyzer.Nodes;

namespace SyntaxAnalyzer;

internal class Program
{
    static void BeautifyByTabs(int amountOfTabs)
    {
        for (var i = 0; i < amountOfTabs; i++)
        {
            Console.Write("\t");
        }
    }

    static void PrintNode(AbstractNode? abstractNode, int depth = 0)
    {
        if (abstractNode == null)
        {
            return;
        }

        if (abstractNode is StatementsNode node)
        {
            foreach (var elem in node.Nodes)
            {
                PrintNode(elem, depth);
            }
        }

        if (abstractNode is FunctionNode functionNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine(functionNode.Function.Identifier);
            BeautifyByTabs(depth + 1);

            foreach (var elem in functionNode.Parameters)
            {
                Console.Write(elem.Identifier);
                Console.Write(" ");
            }

            Console.WriteLine();

            PrintNode(functionNode.Body, depth + 1);
        }

        if (abstractNode is WhileNode whileNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine("while");
            PrintNode(whileNode.Condition, depth + 1);
            PrintNode(whileNode.Body, depth + 1);
        }

        if (abstractNode is IfNode ifNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine("if");
            PrintNode(ifNode.Condition, depth + 1);
            PrintNode(ifNode.Body, depth + 1);
            PrintNode(ifNode.ElseBody, depth + 1);
        }

        if (abstractNode is CoutNode coutNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine("cout");

            foreach (var elem in coutNode.Parameters)
            {
                PrintNode(elem, depth + 1);
            }
        }

        if (abstractNode is CinNode cinNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine("cin");

            foreach (var elem in cinNode.Parameters)
            {
                PrintNode(elem, depth + 1);
            }
        }

        if (abstractNode is ForNode forNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine("for");

            PrintNode(forNode.First, depth + 1);
            PrintNode(forNode.Second, depth + 1);
            PrintNode(forNode.Third, depth + 1);
            PrintNode(forNode.Body, depth + 1);
        }

        if (abstractNode is FunctionExecutionNode functionExecutionNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine(functionExecutionNode.Function.Identifier);

            foreach (var elem in functionExecutionNode.Parameters)
            {
                PrintNode(elem, depth + 1);
            }
        }

        if (abstractNode is SwitchNode switchNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine("switch");
            BeautifyByTabs(depth);
            Console.WriteLine(switchNode.Variable.Identifier);
            PrintNode(switchNode.Body, depth + 1);
        }

        if (abstractNode is CaseNode caseNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine("case");
            BeautifyByTabs(depth);
            Console.WriteLine(caseNode.Literal.Identifier);
        }

        if (abstractNode is KeyWordNode keyWordNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine(keyWordNode.KeyWord.Identifier);
        }

        if (abstractNode is BinaryOperationNode binaryOperationNode)
        {
            PrintNode(binaryOperationNode.LeftNode, depth + 1);
            BeautifyByTabs(depth);
            Console.WriteLine(binaryOperationNode.Operator.Identifier);
            PrintNode(binaryOperationNode.RightNode, depth + 1);
        }

        if (abstractNode is UnaryOperationNode unaryOperationNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine(unaryOperationNode.Operator.Identifier);
            PrintNode(unaryOperationNode.Operand, depth + 1);
        }

        if (abstractNode is LiteralNode literalNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine(literalNode.Literal.Identifier);
        }

        if (abstractNode is VariableNode variableNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine(variableNode.Variable.Identifier);
        }

        if (abstractNode is VariableTypeNode variableTypeNode)
        {
            BeautifyByTabs(depth);
            Console.WriteLine(variableTypeNode.VariableType.Identifier);
        }

        Console.WriteLine();
    }

    static void Main()
    {
        var pathToFile = "test.cpp";

        using var reader = new StreamReader(pathToFile!);
        string codeText = reader.ReadToEnd();
        reader.Close();

        var lexer = new Lexer(pathToFile, codeText);

        lexer.GetTokens();

        if (lexer.IsError)
        {
            Console.WriteLine(lexer.ErrorMessage);
            return;
        }

        var parser = new Parser(lexer, lexer.Tokens);

        var root = parser.ParseCode();

        PrintNode(root);
    }
}