using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class ForNode : AbstractNode
{
    public AbstractNode First { get; set; }
    public AbstractNode Second { get; set; }
    public AbstractNode Third { get; set; }
    public AbstractNode Body { get; set; }

    public ForNode(AbstractNode first, AbstractNode second, AbstractNode third, AbstractNode body)
    {
        First = first;
        Second = second;
        Third = third;
        Body = body;
    }
}