using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Tests.SampleUsage
{
    public class SampleType
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<int> ChildIds { get; set; }
    }
}
