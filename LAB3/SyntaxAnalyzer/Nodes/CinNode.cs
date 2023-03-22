using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class CinNode
{
    public List<AbstractNode> Parameters { get; set; }

    public CinNode(List<AbstractNode> parameters)
    {
        Parameters = parameters;
    }
}