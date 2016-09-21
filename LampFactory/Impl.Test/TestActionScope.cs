using System;

namespace Impl.Test
{
    class TestActionScope : IDisposable
    {
        private readonly string _testAction;

        public DateTime StartTime { get; }

        public TimeSpan TimeElapsed => DateTime.UtcNow - StartTime;

        public TestActionScope(string testAction = "Test action")
        {
            _testAction = testAction;
            StartTime = DateTime.UtcNow;
        }
        
        public void Dispose()
        {
            Console.WriteLine("{0} took {1} msecs in total.", _testAction, TimeElapsed.TotalMilliseconds);
        }
    }
}