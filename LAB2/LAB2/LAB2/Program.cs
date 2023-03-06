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
            { "string", "variable type" }
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
            { "<iostream>", "key word" },
            { "<math.h>", "key word" },
            { "using", "key word" },
            { "namespace", "key word" },
            { "std", "key word" },
            { "break", "key word" },
            { "continue", "key word" },
            { "case", "key word" },
            { "switch", "key word" },
            { "cin", "key word" },
            { "cout", "key word" },
            { "default", "key word" },
            { "endl", "key word" }
        };
        var opertorsDict = new Dictionary<string, string>()
        {
            { "-", "operator" },
            { "+=", "operator" },
            { "=", "operator" },
            { "+", "operator" },
            { "==", "operator" },
            { "%", "operator" },
            { "*", "operator" }
        };

        var functionsDict = new Dictionary<string, string>()
        {
            { "main", "function name" },
            { "pow", "function name" }
        };

        var fileAsList = File.ReadAllLines("test.cpp");

        for (var i = 0; i < fileAsList.Length; i++) fileAsList[i] += "\n";

        //string fileText = File.ReadAllText("test.cpp");
        var counter = 0;
        //Console.WriteLine(fileText);

        var oneWord = "";
        var stringStart = false;
        var variable = false;

        var foundVarTypes = new List<string>();
        var foundKeywords = new List<string>();
        var foundOperators = new List<string>();
        var foundVars = new List<string>();
        var errors = new List<string>();

        for (var index = 0; index < fileAsList.Length; index++)
        {
            counter = 0;
            foreach (var character in fileAsList[index])
            {
                counter++;
                if (character == '"' && !stringStart)
                {
                    stringStart = true;
                }
                else if (character == '"' && stringStart && oneWord.Length > 1)
                {
                    oneWord = "";
                    stringStart = false;
                    continue;
                }

                if (variable)
                {
                    if (character == Convert.ToChar(32)) continue;

                    if (character == Convert.ToChar(","))
                    {
                        if (char.IsDigit(oneWord[0]))
                        {
                            Console.WriteLine(
                                $"{oneWord} LOOKS LIKE BULLSHIT, I THINK IT'S A LEXICAL ERROR ({index + 1},{counter})");
                            errors.Add(
                                $"{oneWord} LOOKS LIKE BULLSHIT, I THINK IT'S A LEXICAL ERROR ({index + 1},{counter})");
                        }
                        else
                        {
                            Console.WriteLine($"{oneWord} - VAR");
                            foundVars.Add($"{oneWord} - VAR");
                            oneWord = "";
                            continue;
                        }
                    }
                    else if (character == Convert.ToChar(";") || character == Convert.ToChar("="))
                    {
                        if (char.IsDigit(oneWord[0]))
                        {
                            Console.WriteLine(
                                $"{oneWord} LOOKS LIKE BULLSHIT, I THINK IT'S A LEXICAL ERROR ({index + 1},{counter})");
                            errors.Add(
                                $"{oneWord} LOOKS LIKE BULLSHIT, I THINK IT'S A LEXICAL ERROR ({index + 1},{counter})");
                            variable = false;
                            oneWord = "";
                            continue;
                        }
                        else
                        {
                            Console.WriteLine($"{oneWord} - VAR");
                            foundVars.Add($"{oneWord} - VAR");
                            oneWord = "";
                            variable = false;
                            continue;
                        }
                    }
                }


                oneWord += character;
                if (!stringStart)
                    if (character == Convert.ToChar("\n") ||
                        character == Convert.ToChar(32) ||
                        character == Convert.ToChar(33) ||
                        //character == Convert.ToChar(34) ||
                        // character == Convert.ToChar(35) ||
                        character == Convert.ToChar(36) ||
                        // character == Convert.ToChar(37) ||
                        character == Convert.ToChar(38) ||
                        character == Convert.ToChar(39) ||
                        character == Convert.ToChar(40) ||
                        character == Convert.ToChar(41) ||
                        // character == Convert.ToChar(42) ||
                        // character == Convert.ToChar(43) ||
                        character == Convert.ToChar(44) ||
                        // character == Convert.ToChar(45) ||
                        // character == Convert.ToChar(46) ||
                        character == Convert.ToChar(47) ||
                        character == Convert.ToChar(58) ||
                        character == Convert.ToChar(59) ||
                        // character == Convert.ToChar(60) ||
                        // character == Convert.ToChar(61) ||
                        // character == Convert.ToChar(62) ||
                        character == Convert.ToChar(63) ||
                        character == Convert.ToChar(64) ||
                        character == Convert.ToChar(91) ||
                        character == Convert.ToChar(92) ||
                        character == Convert.ToChar(93) ||
                        character == Convert.ToChar(94) ||
                        character == Convert.ToChar(95) ||
                        character == Convert.ToChar(96) ||
                        character == Convert.ToChar(123) ||
                        character == Convert.ToChar(124) ||
                        character == Convert.ToChar(125) ||
                        character == Convert.ToChar(126))
                    {
                        var lastCharacter = oneWord[oneWord.Length - 1];
                        oneWord = oneWord.Remove(oneWord.Length - 1);
                        if (oneWord.Length >= 1 && oneWord[0] == Convert.ToChar("\n")) oneWord = oneWord.Remove(0);

                        if (keywordsDict.ContainsKey(oneWord))
                        {
                            Console.WriteLine($"{oneWord} - {keywordsDict[oneWord]}");
                            foundKeywords.Add($"{oneWord} - {keywordsDict[oneWord]}");
                            oneWord = "";
                            continue;
                        }

                        if (varTypesDict.ContainsKey(oneWord))
                        {
                            Console.WriteLine($"{oneWord} - {varTypesDict[oneWord]}");
                            foundVarTypes.Add($"{oneWord} - {varTypesDict[oneWord]}");
                            oneWord = "";
                            variable = true;
                            continue;
                        }

                        if (opertorsDict.ContainsKey(oneWord))
                        {
                            Console.WriteLine($"{oneWord} - {opertorsDict[oneWord]}");
                            foundOperators.Add($"{oneWord} - {opertorsDict[oneWord]}");
                            oneWord = "";
                            continue;
                        }

                        if (functionsDict.ContainsKey(oneWord))
                        {
                            Console.WriteLine($"{oneWord} - {functionsDict[oneWord]}");
                            oneWord = "";
                            variable = false;
                            continue;
                        }

                        if (oneWord.Length > 1)
                        {
                            foreach (var key in keywordsDict.Keys)
                                if (oneWord.Contains(key))
                                {
                                    Console.WriteLine(
                                        $"LEXICAL ERROR {oneWord}, RIGHT ONE IS: {key} ({index + 1},{counter})");
                                    errors.Add($"LEXICAL ERROR {oneWord}, RIGHT ONE IS: {key} ({index + 1},{counter})");
                                    oneWord = "";
                                    break;
                                }

                            foreach (var key in varTypesDict.Keys)
                                if (oneWord.Contains(key))
                                {
                                    Console.WriteLine(
                                        $"LEXICAL ERROR {oneWord}, RIGHT ONE IS: {key} ({index + 1},{counter})");
                                    errors.Add($"LEXICAL ERROR {oneWord}, RIGHT ONE IS: {key} ({index + 1},{counter})");
                                    oneWord = "";
                                    break;
                                }

                            if (oneWord != ">>" && oneWord != "<<" && oneWord != Convert.ToString(32) &&
                                !int.TryParse(oneWord, out var a) && !float.TryParse(oneWord, out var b) &&
                                oneWord != Convert.ToString(""))
                            {
                                Console.WriteLine(
                                    $"{oneWord} LOOKS LIKE BULLSHIT, I THINK IT'S A LEXICAL ERROR ({index + 1},{counter})");
                                errors.Add(
                                    $"{oneWord} LOOKS LIKE BULLSHIT, I THINK IT'S A LEXICAL ERROR ({index + 1},{counter})");
                            }
                        }

                        oneWord = "";
                    }
            }
        }

        Console.WriteLine();

        foundVarTypes = foundVarTypes.Distinct().ToList();
        foundKeywords = foundKeywords.Distinct().ToList();
        foundOperators = foundOperators.Distinct().ToList();
        foundVars = foundVars.Distinct().ToList();
        errors = errors.Distinct().ToList();

        Console.WriteLine("------------------------------");

        Console.WriteLine();
        foreach (var element in foundVarTypes) Console.WriteLine(element);

        Console.WriteLine();

        Console.WriteLine();
        foreach (var element in foundKeywords) Console.WriteLine(element);

        Console.WriteLine();

        Console.WriteLine();
        foreach (var element in foundOperators) Console.WriteLine(element);

        Console.WriteLine();

        Console.WriteLine();
        foreach (var element in foundVars) Console.WriteLine(element);

        Console.WriteLine();

        Console.WriteLine();

        foreach (var element in errors) Console.WriteLine(element);

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }
}