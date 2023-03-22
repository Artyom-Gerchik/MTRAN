using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class VariableTypeNode : AbstractNode
{
    public Token VariableType { get; set; }

    public VariableTypeNode(Token variableType)
    {
        VariableType = variableType;
    }
}