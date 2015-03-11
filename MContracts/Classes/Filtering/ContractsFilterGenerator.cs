using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MCDomain.Model;
using MContracts.ViewModel;

namespace MContracts.Classes.Filtering
{
    class ContractsFilterGenerator
    {
        public static readonly ContractsFilterGenerator Instance = new ContractsFilterGenerator();

        public Func<Contractdoc, bool> Price(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "P");
            var money = Expression.Property(param, "ContractMoney");
            var factor = Expression.Property(money, "Factor");
            var national = Expression.Property(factor, "National");
            var pricewithnds = Expression.Property(national, "WithNdsValue");

            Expression exp = FilterConditionResolver.GetCompareOperator(pricewithnds, Expression.Constant(Convert.ToDouble(volume)), ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);
            return l.Compile();
        }

        public Func<Contractdoc, bool> IsActive(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "IA");

            Expression left = Expression.Property(param, typeof(Contractdoc), "IsActive");
            Expression right = Expression.Constant((bool) volume);

            Expression exp = FilterConditionResolver.GetCompareOperator(left, right, ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);
            return l.Compile();
        }

        public Func<Contractdoc, bool> IsGeneral(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "IG");

            Expression left = Expression.Property(param, typeof(Contractdoc), "IsGeneral");
            Expression right = Expression.Constant((bool)volume);

            Expression exp = FilterConditionResolver.GetCompareOperator(left, right, ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);
            return l.Compile();
        }

        public Func<Contractdoc, bool> Appliedat(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "AD");

            Expression exp = FilterConditionResolver.GetCompareOperator(Expression.Property(param, "Appliedat"), Expression.Constant(Convert.ToDateTime(volume)), ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);
            return l.Compile();
        }

        public Func<Contractdoc, bool> Approvedat(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "AD");

            Expression exp = FilterConditionResolver.GetCompareOperator(Expression.Property(param, "Approvedat"), Expression.Constant(Convert.ToDateTime(volume)), ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);
            return l.Compile();
        }

        public Func<Contractdoc, bool> Num(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "CN");

            Expression exp = FilterConditionResolver.GetCompareOperator(Expression.Property(param, "Num"), Expression.Constant(Convert.ToString(volume)), ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);

            return l.Compile();
        }


        public Func<Contractdoc, bool> Subject(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "CS");

            Expression exp = FilterConditionResolver.GetCompareOperator(Expression.Property(param, "Subject"), Expression.Constant(Convert.ToString(volume)), ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);

            return l.Compile();
        }

        public Func<Contractdoc, bool> ContractType(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "CT");
            var type = Expression.Property(param, "Contracttype");
            var type_name = Expression.Property(type, "Name");

            Expression exp = FilterConditionResolver.GetCompareOperator(type_name, Expression.Constant(volume.ToString()), ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);
            return l.Compile();
        }

        public Func<Contractdoc, bool> ContractCondition(object volume, ConditionTypes ConditionType)
        {
            var param = Expression.Parameter(typeof(Contractdoc), "CT");
            var condition = Expression.Property(param, "Condition");

            Expression exp = FilterConditionResolver.GetCompareOperator(condition, Expression.Constant(volume), ConditionType);

            var l = Expression.Lambda<Func<Contractdoc, bool>>(exp, param);
            return l.Compile();
        }

    }
}
