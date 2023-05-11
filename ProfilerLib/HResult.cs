namespace ProfilerLib
{
    public struct HResult
    {
        public const int S_OK = 0;
        public const int S_FALSE = 1;
        public const int E_FAIL = unchecked((int)0x80004005);
        public const int E_INVALIDARG = unchecked((int)0x80070057);
        public const int E_NOTIMPL = unchecked((int)0x80004001);
        public const int E_NOINTERFACE = unchecked((int)0x80004002);
        public const int CORPROF_E_UNSUPPORTED_CALL_SEQUENCE = unchecked((int)0x80131363);

        public bool IsOK => Value == S_OK;

        public int Value { get; set; }

        public HResult(int hr) => Value = hr;

        public static implicit operator HResult(int hr) => new HResult(hr);

        /// <summary>
        /// Helper to convert to int for comparisons.
        /// </summary>
        public static implicit operator int(HResult hr) => hr.Value;

        /// <summary>
        /// This makes "if (hr)" equivalent to SUCCEEDED(hr).
        /// </summary>
        public static implicit operator bool(HResult hr) => hr.Value >= 0;

        public override string ToString()
        {
            return Value switch
            {
                S_OK => "S_OK",
                S_FALSE => "S_FALSE",
                E_FAIL => "E_FAIL",
                E_INVALIDARG => "E_INVALIDARG",
                E_NOTIMPL => "E_NOTIMPL",
                E_NOINTERFACE => "E_NOINTERFACE",
                CORPROF_E_UNSUPPORTED_CALL_SEQUENCE => "CORPROF_E_UNSUPPORTED_CALL_SEQUENCE",
                _ => $"{Value:x8}",
            };
        }
    }
}
