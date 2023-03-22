using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class CaseNode : AbstractNode
{
    public Token Literal { get; set; }

    public CaseNode(Token literal)
    {
        Literal = literal;
    }
}