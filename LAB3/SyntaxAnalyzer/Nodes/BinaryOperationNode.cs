using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class BinaryOperationNode : AbstractNode
{
    public Token Operator { get; set; }
    public AbstractNode LeftNode { get; set; }
    public AbstractNode RightNode { get; set; }

    public BinaryOperationNode(Token @operator, AbstractNode leftNode, AbstractNode rightNode)
    {
        Operator = @operator;
        LeftNode = leftNode;
        RightNode = rightNode;
    }
}