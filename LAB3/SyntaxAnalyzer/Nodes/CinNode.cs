using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class CinNode : AbstractNode
{
    public List<AbstractNode> Parameters { get; set; }

    public CinNode(List<AbstractNode> parameters)
    {
        Parameters = parameters;
    }
}