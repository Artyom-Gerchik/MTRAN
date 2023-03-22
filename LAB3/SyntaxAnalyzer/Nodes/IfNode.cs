using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class IfNode : AbstractNode
{
    public AbstractNode Condition { get; set; }
    public AbstractNode Body { get; set; }
    public AbstractNode? ElseBody { get; set; }

    public IfNode(AbstractNode condition, AbstractNode body, AbstractNode? elseBody)
    {
        Condition = condition;
        Body = body;
        ElseBody = elseBody;
    }
}