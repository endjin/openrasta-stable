namespace OpenRasta.Reflection
{
    using System.Linq.Expressions;
    
    public static class ExpressionExtensions
    {
        public static T ResolveReferences<T>(this T expression) where T : Expression
        {
            return (T)new SubtreeEvaluator(new SubtreeNominator(e => e.NodeType != ExpressionType.Parameter).Nominate(expression)).Eval(expression);
        }
    }
}