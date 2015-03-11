using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using MCDomain.Model;

namespace MContracts.Classes.Filtering
{
    public static class FilterConditionResolver
    {

	    

 

        
        public static Expression GetCompareOperator(Expression left, Expression right, ConditionTypes ConditionType)
        {
            switch (ConditionType)
            {
                case ConditionTypes.Equal:
                    {
                        if (left.Type == typeof(Nullable<DateTime>))
                        {
                            left = Expression.Convert(left, typeof (DateTime));
                        }
                        return Expression.Equal(left, right);
                    }

                case ConditionTypes.Containing:
                    {
                        MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });


                        var someValue = Expression.Convert(right, typeof(string));
                        
                        var containsMethodExp = Expression.Call(left, contains, someValue);
                        
                        

                        return containsMethodExp;
                    }
                case ConditionTypes.NotContaining:
                    {

                        MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                        var someValue = Expression.Convert(right, typeof(string));
                        
                        var containsMethodExp = Expression.Not(Expression.Call(left, contains, someValue));
                        return containsMethodExp;     
                    }
                case ConditionTypes.LessThen:
                    {
                        if (left.Type == typeof (Nullable<DateTime>))
                        {
                            left = Expression.Convert(left, typeof (DateTime));
                        }
                        return Expression.LessThan(left, right);
                    }

                case ConditionTypes.GreaterThen:
                    {
                        if (left.Type == typeof (Nullable<DateTime>))
                        {
                            left = Expression.Convert(left, typeof (DateTime));
                        }
                        return Expression.GreaterThan(left, right);
                    }

                case ConditionTypes.LessOrEqualThen:
                    {
                        if (left.Type == typeof (Nullable<DateTime>))
                        {
                            left = Expression.Convert(left, typeof (DateTime));
                        }
                        return Expression.LessThanOrEqual(left, right);
                    }

                case ConditionTypes.GreaterOrEqualThen:
                    {
                        if (left.Type == typeof (Nullable<DateTime>))
                        {
                            left = Expression.Convert(left, typeof (DateTime));
                        }
                        return Expression.GreaterThanOrEqual(left, right);
                    }

                case ConditionTypes.NotEqual:
                    {
                        if (left.Type == typeof(Nullable<DateTime>))
                        {
                            left = Expression.Convert(left, typeof(DateTime));
                        }
                        return Expression.NotEqual(left, right);
                    }

                default:
                    throw new Exception("Данный оператор сравнения не реализован");
            }
        }
    }
}
