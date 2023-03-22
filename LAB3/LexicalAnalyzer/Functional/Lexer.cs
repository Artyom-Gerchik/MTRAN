using System.Text.RegularExpressions;

namespace LexicalAnalyzer.Functional;

public class Lexer
{
    public string Path { get; set; }
    public string Code { get; set; }
    public List<Token> Tokens { get; set; }
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; }
    public List<string> VariablesTypes { get; set; }
    public Dictionary<string, Dictionary<string, string>> VariablesTables { get; set; }
    public Dictionary<string, string> Literals { get; set; }
    public Dictionary<string, string> KeyWords { get; set; }
    public Dictionary<string, string> CurrentKeyWords { get; set; }
    public Dictionary<string, string> KeySymbols { get; set; }
    public Dictionary<string, string> CurrentKeySymbols { get; set; }
    public Dictionary<string, string> Operations { get; set; }
    public Dictionary<string, string> CurrentOperations { get; set; }

    public Lexer(string path, string code)
    {
        Path = path;
        Code = code;
        Tokens = new();
        ErrorMessage = string.Empty;
        Literals = new();
        CurrentKeyWords = new();
        CurrentKeySymbols = new();
        CurrentOperations = new();
        VariablesTables = new()
        {
            { "0:0", new Dictionary<string, string>() }
        };
        VariablesTypes = new() { "int", "float", "char", "string", "void", "bool" };
        KeyWords = new()
        {
            { "do", "key word" },
            { "while", "key word" },
            { "for", "key word" },
            { "if", "key word" },
            { "else", "key word" },
            { "switch", "key word" },
            { "case", "key word" },
            { "default", "key word" },
            { "break", "key word" },
            { "continue", "key word" },
            { "cout", "key word" },
            { "cin", "key word" },
            { "new", "key word" },
            { "endl", "key word" },
            { "true", "key word" },
            { "false", "key word" },
            { "#include", "key word" },
            { "<iostream>", "library" },
            { "<string>", "library" },
            { "using", "key word" },
            { "namespace", "key word" },
            { "std", "namespace" },
            { "return", "keyword" }
        };
        KeySymbols = new()
        {
            { "(", "key symbol" },
            { ")", "key symbol" },
            { "{", "key symbol" },
            { "}", "key symbol" },
            { "[", "key symbol" },
            { "]", "key symbol" },
            { ",", "key symbol" },
            { ":", "key symbol" },
            { ";", "key symbol" },
        };
        Operations = new()
        {
            { "=", "operation" },
            { "!", "operation" },
            { "<", "operation" },
            { ">", "operation" },
            { "+", "operation" },
            { "-", "operation" },
            { "*", "operation" },
            { "/", "operation" },
            { "?", "operation" },
        };
    }

    
}