namespace BPU
{
    public abstract class HostProvider
    {
        public HostProvider()
        {
        }

        public abstract Host GetHost();
    }
}