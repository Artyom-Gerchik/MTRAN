using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class KeyWordNode : AbstractNode
{
    public Token KeyWord { get; set; }

    public KeyWordNode(Token keyWord)
    {
        KeyWord = keyWord;
    }
}