using LexicalAnalyzer.Functional;
using SemanticAnalyzer.Functional;
using SyntaxAnalyzer;
using SyntaxAnalyzer.Nodes;
using LAB5.Functional;

namespace LAB5;

internal class Program
{
    static void Main()
    {
        var pathToFile = "test.cpp";

        using var reader = new StreamReader(pathToFile!);
        var codeText = reader.ReadToEnd();
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

        var semantic = new Semantic(root);

        semantic.CheckCode();

        var executor = new Executor(root, lexer.VariablesTables, semantic);
        executor.Execute();
    }
}