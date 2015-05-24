namespace Disque.Net
{
    public enum Keywords
    {
        FROM,
        REPLICATE,
        DELAY,
        RETRY,
        TTL,
        MAXLEN,
        ASYNC,
        TIMEOUT,
        COUNT
    }

    public enum Commands
    {
        ADDJOB,
        GETJOB,
        ACKJOB,
        INFO,
        QLEN,
        QPEEK,
        DELJOB,
        DEQUEUE,
        ENQUEUE,
        FASTACK,
        SHOW,
        PING,
        WORKING
    }
}