using System.Diagnostics;
using CuttingEdge.Conditions;

namespace WebApiMovieRest.Core.Domain
{
    [DebuggerDisplay("{Name}")]
    public class Genre : Entity
    {
        public string Name { get; set; }

        internal Genre()
        {
        }

        public Genre(string name)
        {
            Condition.Requires(name, "name").IsNotNullOrWhiteSpace().IsShorterOrEqual(64);
            Name = name;
        }
    }
}