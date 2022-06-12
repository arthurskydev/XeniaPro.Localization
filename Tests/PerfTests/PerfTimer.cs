using System.Diagnostics;

namespace PerfTests;

public struct PerfInfo
{
    public TimeSpan ElapsedTime { get; set; }
}

public class PerfTimer<T> where T : Delegate
{
    private readonly Stopwatch _stopwatch = new();
    private readonly T _delegate;
    private readonly object[] _args;

    public PerfTimer(T @delegate, params object[] args)
    {
        _delegate = @delegate;
        _args = args;
    }

    public PerfInfo PerformTiming()
    {
        _stopwatch.Start();
        _delegate.DynamicInvoke(_args);
        _stopwatch.Stop();
        return new PerfInfo
        {
            ElapsedTime = _stopwatch.Elapsed
        };
    }
}