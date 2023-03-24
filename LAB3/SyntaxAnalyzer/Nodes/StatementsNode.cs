using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class StatementsNode : AbstractNode // THIS NODE CONTAINS CODE STRINGS HE IS ROOT
{
    public List<AbstractNode> Nodes { get; set; } = new();

    public void AddNode(AbstractNode node)
    {
        Nodes.Add(node);
    }
}