namespace ProfilerLib
{
    public readonly unsafe struct MethodProperties
    {
        private readonly byte* _signature;
        private readonly int _signatureLength;

        public readonly MdTypeDef Class;
        public readonly string Name;
        public readonly int Attributes;
        public readonly uint RVA;
        public readonly int ImplementationFlags;

        public Span<byte> Signature => new(_signature, _signatureLength);

        public MethodProperties(MdTypeDef @class, string name, int attributes, byte* signature, int signatureLength, uint rva, int implementationFlags)
        {
            Class = @class;
            Name = name;
            Attributes = attributes;
            _signature = signature;
            _signatureLength = signatureLength;
            RVA = rva;
            ImplementationFlags = implementationFlags;
        }
    }
}
