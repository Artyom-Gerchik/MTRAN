using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class WhileNode : AbstractNode
{
    public AbstractNode Condition { get; set; }
    public AbstractNode Body { get; set; }

    public WhileNode(AbstractNode condition, AbstractNode body)
    {
        Condition = condition;
        Body = body;
    }
}