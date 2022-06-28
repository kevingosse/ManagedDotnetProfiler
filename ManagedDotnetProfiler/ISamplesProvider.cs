using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal interface ISamplesProvider
    {
        public IEnumerable<Sample> GetSamples();
    }
}
