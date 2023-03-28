using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class VariableTypeNode : AbstractNode
{
    public Token VariableType { get; set; }

    public VariableTypeNode(Token variableType)
    {
        VariableType = variableType;
    }
}