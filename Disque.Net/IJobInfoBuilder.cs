namespace Disque.Net
{
    public interface IJobInfoBuilder
    {
        JobInfo BuildFrom(object[] o);
    }
}