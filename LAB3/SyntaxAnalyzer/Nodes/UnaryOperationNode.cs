using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class UnaryOperationNode : AbstractNode
{
    public Token Operator { get; set; }
    public AbstractNode Operand { get; set; }

    public UnaryOperationNode(Token @operator, AbstractNode operand)
    {
        Operator = @operator;
        Operand = operand;
    }
}