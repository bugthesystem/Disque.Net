namespace Disque.Net
{
    public interface IQstatBuilder
    {
        Qstat BuildFrom(object[] o);
    }
}