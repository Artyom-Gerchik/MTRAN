using LexicalAnalyzer.Functional;

namespace SyntaxAnalyzer.Nodes;

internal class FunctionNode : AbstractNode
{
    public Token Function { get; set; }
    public List<Token> Parameters { get; set; }
    public AbstractNode Body { get; set; }

    public FunctionNode(Token function, List<Token> parameters, AbstractNode body)
    {
        Function = function;
        Parameters = parameters;
        Body = body;
    }
}