using LexicalAnalyzer.Functional;
using SemanticAnalyzer.Functional;
using SyntaxAnalyzer;

namespace SemanticAnalyzer;

internal class Program
{
    private static void Main()
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
    }
}