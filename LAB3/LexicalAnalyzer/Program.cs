﻿using LexicalAnalyzer.Functional;

namespace LexicalAnalyzer;

internal class Program
{
    private static void Main(string[] args)
    {
        var path = "test.cpp";

        using var reader = new StreamReader(path!);
        var codeText = reader.ReadToEnd();
        reader.Close();

        var lexer = new Lexer(path, codeText);

        lexer.GetTokens();

        if (lexer.IsError)
        {
            Console.WriteLine(lexer.ErrorMessage);
        }
        else
        {
            Console.WriteLine("\nVariables:");

            foreach (var elem in lexer.VariablesTables)
                //Console.WriteLine($"\t{elem.Key}"); // UNCOMMENT THIS TO SEE DEPTH LEVEL //

            foreach (var elem2 in elem.Value)
                Console.WriteLine($"\t{elem2.Key} : {elem2.Value}");

            Console.WriteLine("\nLiterals:");

            foreach (var elem in lexer.Literals) Console.WriteLine($"\t{elem.Key} : {elem.Value}");

            Console.WriteLine("\nKey words:");

            foreach (var elem in lexer.CurrentKeyWords) Console.WriteLine($"\t{elem.Key} : {elem.Value}");

            Console.WriteLine("\nKey symbols:");

            foreach (var elem in lexer.CurrentKeySymbols) Console.WriteLine($"\t{elem.Key} : {elem.Value}");

            Console.WriteLine("\nOperations:");

            foreach (var elem in lexer.CurrentOperations) Console.WriteLine($"\t{elem.Key} : {elem.Value}");

            Console.WriteLine("\nTokens:");

            foreach (var elem in lexer.Tokens) Console.WriteLine($"\t{elem.Identifier} {elem.Type}");
        }
    }
}