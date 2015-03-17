using System;
using System.Linq.Expressions;
using Shouldly;
using Xunit;

namespace ExpressionToString.Tests
{
    public class ExpressionStringBuilderTests
    {
        [Fact]
        public void VerifyProperties()
        {
            var nullable = (int?) null;
            var foo = new Foo();
            VerifyExpression(() => Foo, "Foo");
            VerifyExpression(() => nullable.Value, "nullable.Value");
            VerifyExpression(() => foo.Bar.Baz, "foo.Bar.Baz");
        }

        [Fact]
        public void VerifyFormulas()
        {
            var notifies = new Totals
            {
                TaxPercentage = 20
            };
            var foo = new Foo();
            VerifyExpression(() => (int)(notifies.SubTotal * (1m + (notifies.TaxPercentage / 100m))), "(notifies.SubTotal * (1 + (notifies.TaxPercentage / 100)))");
            VerifyExpression(() => InstanceMethodCall(), "InstanceMethodCall()");
            VerifyExpression(() => foo.InstanceMethod(), "foo.InstanceMethod()");
            VerifyExpression(() => foo.InstanceMethod(true), "foo.InstanceMethod(true)");
            VerifyExpression(() => StaticMethod(notifies.SubTotal), "StaticMethod(notifies.SubTotal)");
            VerifyExpressionWithTruncate(() => StaticMethodLotsOfArguments(notifies.TaxPercentage, notifies.TaxPercentage, notifies.TaxPercentage, notifies.TaxPercentage), "StaticMethodLotsOfArguments(...)");
            VerifyExpression(() => StaticMethodLotsOfArguments(notifies.TaxPercentage, notifies.TaxPercentage, notifies.TaxPercentage, notifies.TaxPercentage), "StaticMethodLotsOfArguments(notifies.TaxPercentage, notifies.TaxPercentage, notifies.TaxPercentage, notifies.TaxPercentage)");
        }

        static void StaticMethod(int subTotal)
        {
        }

        static void StaticMethodLotsOfArguments(int a, int b, int c, int d)
        {
        }

        [Fact]
        public void VerifyAction()
        {
            Action action = () => { };
            VerifyExpression(() => action(), "action()");
        }

        object InstanceMethodCall()
        {
            return null;
        }

        void VerifyExpression<T>(Expression<Func<T>> func, string expression)
        {
            ExpressionStringBuilder.ToString(func).ShouldBe(expression);
        }

        private void VerifyExpressionWithTruncate(Expression<Action> func, string expression)
        {
            ExpressionStringBuilder.ToString(func, trimLongArgumentList: true).ShouldBe(expression);
        }

        void VerifyExpression(Expression<Action> func, string expression)
        {
            ExpressionStringBuilder.ToString(func).ShouldBe(expression);
        }

        public object Foo { get; set; }
    }
}
