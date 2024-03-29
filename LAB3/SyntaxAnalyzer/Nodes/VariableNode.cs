using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class VariableNode : AbstractNode
{
    public Token Variable { get; set; }

    public VariableNode(Token variable)
    {
        Variable = variable;
    }
}