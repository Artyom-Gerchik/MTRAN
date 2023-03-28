using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class SwitchNode : AbstractNode
{
    public Token Variable { get; set; }
    public AbstractNode Body { get; set; }

    public SwitchNode(Token variable, AbstractNode body)
    {
        Variable = variable;
        Body = body;
    }
}