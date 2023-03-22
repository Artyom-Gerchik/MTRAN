using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class VariableNode : AbstractNode
{
    public Token Variable { get; set; }

    public VariableNode(Token variable)
    {
        Variable = variable;
    }
}