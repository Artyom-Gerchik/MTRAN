using LexicalAnalyzer.Functional;

namespace LexicalAnalyzer;

internal class Program
{
    static void Main(string[] args)
    {
        var path = "test.cpp";

        using var reader = new StreamReader(path!);
        string codeText = reader.ReadToEnd();
        reader.Close();

        var lexer = new Lexer(path, codeText);

       
    }
}