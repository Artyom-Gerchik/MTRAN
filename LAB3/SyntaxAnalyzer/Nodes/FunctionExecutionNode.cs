using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

public class FunctionExecutionNode : AbstractNode
{
    public Token Function { get; set; }
    public List<AbstractNode> Parameters { get; set; }

    public FunctionExecutionNode(Token function, List<AbstractNode> parameters)
    {
        Function = function;
        Parameters = parameters;
    }
}