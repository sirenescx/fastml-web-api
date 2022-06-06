namespace Fast.ML.Extensions;

public static class CancellationTokenExtensions
{
    public static CancellationTokenSource LinkWithTimeout(
        this CancellationToken token, 
        int timeoutMilliseconds)
    {
        var linked = CancellationTokenSource.CreateLinkedTokenSource(token);
        linked.CancelAfter(timeoutMilliseconds);

        return linked;
    }
}