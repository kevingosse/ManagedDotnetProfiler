namespace ProfilerLib
{
    public abstract class Unknown : IUnknown
    {
        private int _referenceCount;

        public abstract HResult QueryInterface(in Guid guid, out nint ptr);

        public int AddRef()
        {
            return Interlocked.Increment(ref _referenceCount);
        }

        public int Release()
        {
            var value = Interlocked.Decrement(ref _referenceCount);

            if (value == 0)
            {
                Dispose();
            }

            return value;
        }

        public virtual void Dispose()
        {
        }
    }
}
