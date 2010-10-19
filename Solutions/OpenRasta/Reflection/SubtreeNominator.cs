namespace OpenRasta.Reflection
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    #endregion

    /// <summary>
    /// Performs bottom-up analysis to determine which nodes can possibly
    /// be part of an evaluated sub-tree.
    /// </summary>
    public class SubtreeNominator : ExpressionVisitor
    {
        private readonly Func<Expression, bool> canBeEvaluated;
        private HashSet<Expression> candidates;
        private bool cannotBeEvaluated;

        internal SubtreeNominator(Func<Expression, bool> canBeEvaluated)
        {
            this.canBeEvaluated = canBeEvaluated;
        }

        internal HashSet<Expression> Nominate(Expression expression)
        {
            this.candidates = new HashSet<Expression>();
            this.Visit(expression);
            return this.candidates;
        }

        protected override Expression Visit(Expression expression)
        {
            if (expression != null)
            {
                bool saveCannotBeEvaluated = this.cannotBeEvaluated;

                this.cannotBeEvaluated = false;

                base.Visit(expression);

                if (!this.cannotBeEvaluated)
                {
                    if (this.canBeEvaluated(expression))
                    {
                        this.candidates.Add(expression);
                    }
                    else
                    {
                        this.cannotBeEvaluated = true;
                    }
                }

                this.cannotBeEvaluated |= saveCannotBeEvaluated;
            }

            return expression;
        }
    }
}