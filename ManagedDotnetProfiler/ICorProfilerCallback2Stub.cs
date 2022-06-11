//using System;
//using System.Runtime.InteropServices;

//namespace ManagedDotnetProfiler
//{
//    internal unsafe class ICorProfilerCallback2Stub
//    {
//        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
//        private delegate HResult QueryInterface(IntPtr self, Guid* guid, IntPtr* ptr);

//        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
//        private delegate int AddRef(IntPtr self);

//        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
//        private delegate int Release(IntPtr self);

//        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
//        private delegate HResult Initialize(IntPtr self, IntPtr pICorProfilerInfoUnk);

//        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
//        private delegate HResult Shutdown(IntPtr self);

//        private ICorProfilerCallback2Stub(ICorProfilerCallback2 implementation)
//        {
//            var obj = Marshal.AllocHGlobal(IntPtr.Size);

//            var ptr = (IntPtr*)obj;

//            const int DelegateCount = 5;

//            int vtablePartSize = DelegateCount * IntPtr.Size;
//            IntPtr* vtable = (IntPtr*)Marshal.AllocHGlobal(vtablePartSize);
//            *(void**)obj = vtable;

//            *vtable++ = Marshal.GetFunctionPointerForDelegate(new QueryInterface((_, a1, a2) => implementation.QueryInterface(a1, a2)));
//            *vtable++ = Marshal.GetFunctionPointerForDelegate(new AddRef((_) => implementation.AddRef()));
//            *vtable++ = Marshal.GetFunctionPointerForDelegate(new Release((_) => implementation.Release()));
//            *vtable++ = Marshal.GetFunctionPointerForDelegate(new Initialize((_, a1) => implementation.Initialize(a1)));
//            *vtable++ = Marshal.GetFunctionPointerForDelegate(new Shutdown((_) => implementation.Shutdown()));

//            Object = obj;
//        }

//        public IntPtr Object { get; private set; }

//        public static ICorProfilerCallback2Stub Wrap(ICorProfilerCallback2 implementation) => new(implementation);

//        public static implicit operator IntPtr(ICorProfilerCallback2Stub stub) => stub.Object;

//        public void Dispose()
//        {
//            var target = (IntPtr*)Object;
//            Marshal.FreeHGlobal(*(target));
//            Marshal.FreeHGlobal(Object);
//            Object = IntPtr.Zero;
//        }
//    }
//}
