using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions;

namespace WebApiMovieRest.Core.Tests.xUnitDataSources
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DecimalAttribute : DataAttribute
    {
        private readonly string _value;

        public DecimalAttribute(string decimalAsString)
        {
            _value = decimalAsString;
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
           yield return new object[] { Decimal.Parse(_value) };
        }
    }
}