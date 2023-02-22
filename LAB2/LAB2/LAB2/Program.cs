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

        string fileText = File.ReadAllText("test.cpp");
        Console.WriteLine(fileText);

        var oneWord = "";

        foreach (var character in fileText)
        {
            oneWord += character;
            if (character == Convert.ToChar(32) ||
                character == Convert.ToChar(33) ||
                character == Convert.ToChar(34) ||
                //character == Convert.ToChar(35) ||
                character == Convert.ToChar(36) ||
                character == Convert.ToChar(37) ||
                character == Convert.ToChar(38) ||
                character == Convert.ToChar(39) ||
                character == Convert.ToChar(40) ||
                character == Convert.ToChar(41) ||
                character == Convert.ToChar(42) ||
                character == Convert.ToChar(43) ||
                character == Convert.ToChar(44) ||
                character == Convert.ToChar(45) ||
                character == Convert.ToChar(46) ||
                character == Convert.ToChar(47) ||
                character == Convert.ToChar(58) ||
                character == Convert.ToChar(59) ||
                //character == Convert.ToChar(60) ||
                character == Convert.ToChar(61) ||
                //character == Convert.ToChar(62) ||
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
                if (oneWord.Length >= 1 && oneWord[0] == Convert.ToChar("\n"))
                {
                    oneWord = oneWord.Remove(0);
                }

                if (keywordsDict.ContainsKey(oneWord))
                {
                    Console.WriteLine($"{oneWord} - {keywordsDict[oneWord]}");
                    oneWord = "";
                    continue;
                }

                if (varTypesDict.ContainsKey(oneWord))
                {
                    Console.WriteLine($"{oneWord} - {varTypesDict[oneWord]}");
                    oneWord = "";
                    continue;
                }

                if (oneWord.Length > 1)
                {
                    foreach (var key in keywordsDict.Keys)
                    {
                        if (oneWord.Contains(key))
                        {
                            Console.WriteLine($"LEXICAL ERROR {oneWord}, RIGHT ONE IS: {key}");
                            oneWord = "";
                            break;
                        }
                    }

                    foreach (var key in varTypesDict.Keys)
                    {
                        if (oneWord.Contains(key))
                        {
                            Console.WriteLine($"LEXICAL ERROR {oneWord}, RIGHT ONE IS: {key}");
                            oneWord = "";
                            break;
                        }
                    }

                    //Console.WriteLine($"{oneWord} LOOKS LIKE BULLSHIT, I THINK IT'S A LEXICAL ERROR");
                }


                oneWord = "";
            }
        }
    }
}