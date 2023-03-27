namespace ProfilerLib
{
    public abstract class Unknown : Interfaces.IUnknown
    {
        private int _referenceCount;

        protected abstract HResult QueryInterface(in Guid guid, out nint ptr);

        HResult Interfaces.IUnknown.QueryInterface(in System.Guid guid, out nint ptr) => QueryInterface(guid, out ptr);

        int Interfaces.IUnknown.AddRef()
        {
            return Interlocked.Increment(ref _referenceCount);
        }

        int Interfaces.IUnknown.Release()
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
