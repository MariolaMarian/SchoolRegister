using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolRegister.Web.Extensions
{
    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }
        public ExpressionParameterReplacer(IList<ParameterExpression> srcParameters, IList<ParameterExpression> destParameters)
        {
            ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
            for(int i = 0; i!= srcParameters.Count && i!= destParameters.Count; i++)
            {
                ParameterReplacements.Add(srcParameters[i], destParameters[i]);
            }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;

            if(ParameterReplacements.TryGetValue(node, out replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }

    }
}
