using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions;

namespace WebApiMovieRest.Core.Tests.xUnitDataSources
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class StringOfLengthAttribute : DataAttribute
    {
        private readonly char _characterToRepeat;
        private readonly int _stringLength;

        public StringOfLengthAttribute(int stringLength, char characterToRepeat = '*')
        {
            _characterToRepeat = characterToRepeat;
            _stringLength = stringLength;
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            yield return new object[] { new string(_characterToRepeat, _stringLength) };
        }
    }
}