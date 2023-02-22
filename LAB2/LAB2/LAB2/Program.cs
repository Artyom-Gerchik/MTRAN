namespace LAB2;

public static class Program
{
    public static void Main()
    {
        var varTypesDict = new Dictionary<string, string>()
        {
            { "int", "variable type" },
            { "float", "variable type" },
            { "char", "variable type" },
            { "string", "variable type" },
        };
        var keywordsDict = new Dictionary<string, string>()
        {
            { "const", "key word" },
            { "if", "key word" },
            { "else", "key word" },
            { "else if", "key word" },
            { "do", "key word" },
            { "while", "key word" },
            { "for", "key word" },
            { "return", "key word" },
            { "#include", "key word" },
            { "#pragma", "key word" },
            { "<iostream>", "key word" },
            { "<math.h>", "key word" },
            { "using", "key word" },
            { "namespace", "key word" },
            { "break", "key word" },
            { "continue", "key word" },
        };
        var specSymbolsDict = new Dictionary<string, string>()
        {
            { "<<", "spec symbol" },
            { ">>", "spec symbol" },
            { "{", "spec symbol" },
            { "}", "spec symbol" },
            { "[", "spec symbol" },
            { "]", "spec symbol" },
            { ";", "spec symbol" },
        };
    }
}