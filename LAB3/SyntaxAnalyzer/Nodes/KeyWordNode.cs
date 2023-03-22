using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class KeyWordNode : AbstractNode
{
    public Token KeyWord { get; set; }

    public KeyWordNode(Token keyWord)
    {
        KeyWord = keyWord;
    }
}