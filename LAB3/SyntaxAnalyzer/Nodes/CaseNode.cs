using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class CaseNode : AbstractNode
{
    public Token Literal { get; set; }

    public CaseNode(Token literal)
    {
        Literal = literal;
    }
}