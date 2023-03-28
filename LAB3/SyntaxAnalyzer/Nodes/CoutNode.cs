using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class CoutNode : AbstractNode
{
    public List<AbstractNode> Parameters { get; set; }

    public CoutNode(List<AbstractNode> parameters)
    {
        Parameters = parameters;
    }
}