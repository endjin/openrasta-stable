namespace OpenRasta.DI.Ninject
{
    #region Using Directives

    using System;
    using System.Linq;

    using global::Ninject.Activation;
    using global::Ninject.Components;
    using global::Ninject.Planning.Directives;
    using global::Ninject.Selection.Heuristics;

    #endregion

    /// <summary>
    /// Scores constructors by either looking for the existence of an injection marker
    /// attribute, or by counting the number of parameters that can be injected.
    /// </summary>
    public class InjectableConstructorScorer : NinjectComponent, IConstructorScorer
    {
        /// <summary>
        /// Gets the score for the specified constructor. Looks at the number of "resolvable" arguments.
        /// </summary>
        /// <param name="context">The injection context.</param>
        /// <param name="directive">The constructor.</param>
        /// <returns>The constructor's score.</returns>
        public int Score(IContext context, ConstructorInjectionDirective directive)
        {
            if (directive.Constructor.HasAttribute(Settings.InjectAttribute))
            {
                return Int32.MaxValue;
            }

            var bindableParameters = from param in directive.Constructor.GetParameters()
                                     where !param.IsOut && !param.IsRetval && !param.IsOptional
                                           && param.ParameterType.IsBindable(context.Kernel)
                                     select param;

            return bindableParameters.Count();
        }
    }
}