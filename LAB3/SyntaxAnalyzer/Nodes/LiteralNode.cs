using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class LiteralNode : AbstractNode
{
    public Token Literal { get; set; }

    public LiteralNode(Token literal)
    {
        Literal = literal;
    }
}