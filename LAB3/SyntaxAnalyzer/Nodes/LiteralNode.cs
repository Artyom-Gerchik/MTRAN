using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class LiteralNode : AbstractNode
{
    public Token Literal { get; set; }

    public LiteralNode(Token literal)
    {
        Literal = literal;
    }
}