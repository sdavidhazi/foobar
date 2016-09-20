using System;
using System.Diagnostics;

namespace Impl.Test
{
    class TestActionScope : IDisposable
    {
        private readonly string _testAction;

        public DateTime StartTime { get; }

        public TimeSpan TimeElapsed => DateTime.UtcNow - StartTime;

        public TestActionScope(string testAction)
        {
            _testAction = testAction;
            StartTime = DateTime.UtcNow;
        }

        public void Dispose()
        {
            Console.WriteLine("{0} took {1} seconds in total.", _testAction, TimeElapsed.TotalSeconds);
        }
    }
}