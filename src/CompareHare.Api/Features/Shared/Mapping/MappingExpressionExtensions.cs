using System.Linq;
using AutoMapper;

namespace CompareHare.Api.Shared.Mapping
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(
            this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }

        public static IMappingExpression<TSource, TDest> IgnoreTheAsymmetric<TSource, TDest>(
          this IMappingExpression<TSource, TDest> expression)
        {
            var sourceProperties = typeof(TSource).GetProperties().ToList();
            var destinationProperties = typeof(TDest).GetProperties().ToList();

            destinationProperties.ForEach(prop =>
              {
                  if (!sourceProperties.Any(x => x.Name == prop.Name))
                  {
                      expression.ForMember(prop.Name, opt => opt.Ignore());
                  }
              });

            return expression;
        }
    }
}
