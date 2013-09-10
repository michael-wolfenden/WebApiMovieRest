using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions;

namespace WebApiMovieRest.Core.Tests.xUnitDataSources
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NullEmptyAndWhitespaceAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            yield return new object[] { null };
            yield return new object[] { "" };
            yield return new object[] { "\t\t" };
        }
    }
}