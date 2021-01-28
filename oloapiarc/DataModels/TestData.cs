using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oloapiarc.DataModels
{
    public class TestData
    {
        public static readonly Dictionary<string, string> PostTestData = new Dictionary<string, string>
        {
            { "title", "test" },
            { "body", "This is how we do it" },
            { "userId", "11" }
        };

        public static readonly Dictionary<string, string> PatchTestData = new Dictionary<string, string>
        {
            { "title", "Tale of Two Cities" },
        };
    }
}
