namespace ProfilerLib;

public readonly struct ModuleId
{
    public readonly nint Value;
}

public readonly struct ObjectId
{
    public readonly nint Value;
}

public readonly struct GCHandleId
{
    public readonly nint Value;
}

public readonly struct AppDomainId
{
    public readonly nint Value;
}

public readonly struct AssemblyId
{
    public readonly nint Value;
}

public readonly struct ClassId
{
    public readonly nint Value;
}

public readonly struct FunctionId
{
    public readonly nint Value;
}

public readonly struct ReJITId
{
    public readonly nint Value;
}

public readonly struct ThreadId
{
    public readonly nuint Value;
}

public readonly struct ProcessId
{
    public readonly nint Value;
}

public readonly struct ContextId
{
    public readonly nint Value;
}

public readonly struct MdToken
{
    public readonly int Value;
}

public readonly struct MdModule
{
    public readonly int Value;
}

public readonly struct MdTypeDef
{
    public readonly int Value;
}
public readonly struct MdMethodDef
{
    public MdMethodDef(MdToken token)
    {
        Value = token.Value;
    }

    public readonly int Value;
}

public readonly struct MdFieldDef
{
    public readonly int Value;
}

public readonly struct MdInterfaceImpl
{
    public readonly int Value;
}

public readonly struct MdTypeRef
{
    public readonly int Value;
}

public readonly struct MdParamDef
{
    public readonly int Value;
}

public readonly struct MdMemberRef
{
    public readonly int Value;
}

public readonly struct MdPermission
{
    public readonly int Value;
}

public readonly struct MdProperty
{
    public readonly int Value;
}

public readonly struct MdEvent
{
    public readonly int Value;
}

public readonly struct MdSignature
{
    public readonly int Value;
}

public readonly struct MdModuleRef
{
    public readonly int Value;
}

public readonly struct MdTypeSpec
{
    public readonly int Value;
}

public readonly struct MdString
{
    public readonly int Value;
}

public readonly struct MdCustomAttribute
{
    public readonly int Value;
}

public readonly struct CorElementType
{
    public readonly uint Value;
}

public readonly struct HCORENUM
{
    public readonly nint Value;
}

public readonly struct COR_FIELD_OFFSET
{
    public readonly MdFieldDef RidOfField;
    public readonly uint UlOffset;
}

public readonly struct CorIlMap
{
    public readonly uint OldOffset;
    public readonly uint NewOffset;
    public readonly bool fAccurate;
}

public readonly struct CorDebugIlToNativeMap
{
    public readonly uint IlOffset;
    public readonly uint NativeStartOffset;
    public readonly uint NativeEndOffset;
}

public readonly struct COR_PRF_FRAME_INFO
{
    public readonly nint Value;
}

public readonly struct COR_PRF_CODE_INFO
{
    public readonly nint StartAddress;
    public readonly nint Size;
}

public readonly struct COR_PRF_GC_GENERATION_RANGE
{
    public readonly COR_PRF_GC_GENERATION generation;
    public readonly ObjectId RangeStart;
    public readonly nint RangeLength;
    public readonly nint RangeLengthReserved;
}

public readonly struct COR_PRF_EX_CLAUSE_INFO
{
    public readonly COR_PRF_CLAUSE_TYPE ClauseType;
    public readonly nint ProgramCounter;
    public readonly nint FramePointer;
    public readonly nint ShadowStackPointer;
}

public readonly struct COR_PRF_ELT_INFO
{
    public readonly nint Value;
}

public readonly struct COR_PRF_FUNCTION_ARGUMENT_INFO
{
    public readonly uint NumRanges;                // number of chunks of arguments
    public readonly uint TotalArgumentSize;    // total size of arguments
    public readonly COR_PRF_FUNCTION_ARGUMENT_RANGE range1;
    public readonly COR_PRF_FUNCTION_ARGUMENT_RANGE range2;
}

public readonly struct COR_PRF_FUNCTION_ARGUMENT_RANGE
{
    public readonly nint StartAddress;          // start address of the range
    public readonly uint Length;                         // contiguous length of the range
}


[Flags]
public enum CorPrfMonitor : uint
{
    COR_PRF_MONITOR_NONE = 0x00000000,
    COR_PRF_MONITOR_FUNCTION_UNLOADS = 0x00000001,
    COR_PRF_MONITOR_CLASS_LOADS = 0x00000002,
    COR_PRF_MONITOR_MODULE_LOADS = 0x00000004,
    COR_PRF_MONITOR_ASSEMBLY_LOADS = 0x00000008,
    COR_PRF_MONITOR_APPDOMAIN_LOADS = 0x00000010,
    COR_PRF_MONITOR_JIT_COMPILATION = 0x00000020,
    COR_PRF_MONITOR_EXCEPTIONS = 0x00000040,
    COR_PRF_MONITOR_GC = 0x00000080,
    COR_PRF_MONITOR_OBJECT_ALLOCATED = 0x00000100,
    COR_PRF_MONITOR_THREADS = 0x00000200,
    COR_PRF_MONITOR_REMOTING = 0x00000400,
    COR_PRF_MONITOR_CODE_TRANSITIONS = 0x00000800,
    COR_PRF_MONITOR_ENTERLEAVE = 0x00001000,
    COR_PRF_MONITOR_CCW = 0x00002000,
    COR_PRF_MONITOR_REMOTING_COOKIE = 0x00004000 |
                                          COR_PRF_MONITOR_REMOTING,
    COR_PRF_MONITOR_REMOTING_ASYNC = 0x00008000 |
                                          COR_PRF_MONITOR_REMOTING,
    COR_PRF_MONITOR_SUSPENDS = 0x00010000,
    COR_PRF_MONITOR_CACHE_SEARCHES = 0x00020000,
    COR_PRF_ENABLE_REJIT = 0x00040000,
    COR_PRF_ENABLE_INPROC_DEBUGGING = 0x00080000,
    COR_PRF_ENABLE_JIT_MAPS = 0x00100000,
    COR_PRF_DISABLE_INLINING = 0x00200000,
    COR_PRF_DISABLE_OPTIMIZATIONS = 0x00400000,
    COR_PRF_ENABLE_OBJECT_ALLOCATED = 0x00800000,
    COR_PRF_MONITOR_CLR_EXCEPTIONS = 0x01000000,
    COR_PRF_MONITOR_ALL = 0x0107FFFF,
    COR_PRF_ENABLE_FUNCTION_ARGS = 0X02000000,
    COR_PRF_ENABLE_FUNCTION_RETVAL = 0X04000000,
    COR_PRF_ENABLE_FRAME_INFO = 0X08000000,
    COR_PRF_ENABLE_STACK_SNAPSHOT = 0X10000000,
    COR_PRF_USE_PROFILE_IMAGES = 0x20000000,
    COR_PRF_DISABLE_TRANSPARENCY_CHECKS_UNDER_FULL_TRUST
                                        = 0x40000000,
    COR_PRF_DISABLE_ALL_NGEN_IMAGES = 0x80000000,
    COR_PRF_ALL = 0x8FFFFFFF,
    COR_PRF_REQUIRE_PROFILE_IMAGE = COR_PRF_USE_PROFILE_IMAGES |
                                          COR_PRF_MONITOR_CODE_TRANSITIONS |
                                          COR_PRF_MONITOR_ENTERLEAVE,
    COR_PRF_ALLOWABLE_AFTER_ATTACH = COR_PRF_MONITOR_THREADS |
                                          COR_PRF_MONITOR_MODULE_LOADS |
                                          COR_PRF_MONITOR_ASSEMBLY_LOADS |
                                          COR_PRF_MONITOR_APPDOMAIN_LOADS |
                                          COR_PRF_ENABLE_STACK_SNAPSHOT |
                                          COR_PRF_MONITOR_GC |
                                          COR_PRF_MONITOR_SUSPENDS |
                                          COR_PRF_MONITOR_CLASS_LOADS |
                                          COR_PRF_MONITOR_JIT_COMPILATION,
    COR_PRF_MONITOR_IMMUTABLE = COR_PRF_MONITOR_CODE_TRANSITIONS |
                                          COR_PRF_MONITOR_REMOTING |
                                          COR_PRF_MONITOR_REMOTING_COOKIE |
                                          COR_PRF_MONITOR_REMOTING_ASYNC |
                                          COR_PRF_ENABLE_REJIT |
                                          COR_PRF_ENABLE_INPROC_DEBUGGING |
                                          COR_PRF_ENABLE_JIT_MAPS |
                                          COR_PRF_DISABLE_OPTIMIZATIONS |
                                          COR_PRF_DISABLE_INLINING |
                                          COR_PRF_ENABLE_OBJECT_ALLOCATED |
                                          COR_PRF_ENABLE_FUNCTION_ARGS |
                                          COR_PRF_ENABLE_FUNCTION_RETVAL |
                                          COR_PRF_ENABLE_FRAME_INFO |
                                          COR_PRF_USE_PROFILE_IMAGES |
                     COR_PRF_DISABLE_TRANSPARENCY_CHECKS_UNDER_FULL_TRUST |
                                          COR_PRF_DISABLE_ALL_NGEN_IMAGES
}

public enum COR_PRF_JIT_CACHE
{
    COR_PRF_CACHED_FUNCTION_FOUND,
    COR_PRF_CACHED_FUNCTION_NOT_FOUND
}

public enum COR_PRF_TRANSITION_REASON
{
    COR_PRF_TRANSITION_CALL,
    COR_PRF_TRANSITION_RETURN
}

public enum COR_PRF_SUSPEND_REASON
{
    COR_PRF_SUSPEND_OTHER = 0,
    COR_PRF_SUSPEND_FOR_GC = 1,
    COR_PRF_SUSPEND_FOR_APPDOMAIN_SHUTDOWN = 2,
    COR_PRF_SUSPEND_FOR_CODE_PITCHING = 3,
    COR_PRF_SUSPEND_FOR_SHUTDOWN = 4,
    COR_PRF_SUSPEND_FOR_INPROC_DEBUGGER = 6,
    COR_PRF_SUSPEND_FOR_GC_PREP = 7,
    COR_PRF_SUSPEND_FOR_REJIT = 8,
    COR_PRF_SUSPEND_FOR_PROFILER = 9,
}

[Flags]
public enum CorOpenFlags : uint
{
    ofRead = 0x00000000,
    ofWrite = 0x00000001,
    ofReadWriteMask = 0x00000001,
    ofCopyMemory = 0x00000002,
    ofCacheImage = 0x00000004,
    ofManifestMetadata = 0x00000008,
    ofReadOnly = 0x00000010,
    ofTakeOwnership = 0x00000020,
    ofNoTypeLib = 0x00000080,
    ofNoTransform = 0x00001000,
    ofReserved1 = 0x00000100,
    ofReserved2 = 0x00000200,
    ofReserved = 0xffffff40
}

public static class KnownGuids
{
    public static Guid IMetaDataImport = Guid.Parse("7DAC8207-D3AE-4c75-9B67-92801A497D44");
    public static Guid IMetaDataImport2 = Guid.Parse("FCE5EFA0-8BBA-4f8e-A036-8F2022B08466");
    public static Guid ClassFactoryGuid = Guid.Parse("00000001-0000-0000-C000-000000000046");
}

[Flags]
public enum CorILMethodFlags
{
    CorILMethod_InitLocals = 0x0010,           // call default constructor on all local vars
    CorILMethod_MoreSects = 0x0008,           // there is another attribute after this one

    CorILMethod_CompressedIL = 0x0040,           // Not used.

    // Indicates the format for the COR_ILMETHOD header
    CorILMethod_FormatShift = 3,
    CorILMethod_FormatMask = ((1 << CorILMethod_FormatShift) - 1),
    CorILMethod_TinyFormat = 0x0002,         // use this code if the code size is even
    CorILMethod_SmallFormat = 0x0000,
    CorILMethod_FatFormat = 0x0003,
    CorILMethod_TinyFormat1 = 0x0006,         // use this code if the code size is odd
}

public enum COR_PRF_STATIC_TYPE
{
    COR_PRF_FIELD_NOT_A_STATIC = 0,
    COR_PRF_FIELD_APP_DOMAIN_STATIC = 0x1,
    COR_PRF_FIELD_THREAD_STATIC = 0x2,
    COR_PRF_FIELD_CONTEXT_STATIC = 0x4,
    COR_PRF_FIELD_RVA_STATIC = 0x8
}

public enum COR_PRF_GC_GENERATION
{
    COR_PRF_GC_GEN_0 = 0,
    COR_PRF_GC_GEN_1 = 1,
    COR_PRF_GC_GEN_2 = 2,
    COR_PRF_GC_LARGE_OBJECT_HEAP = 3,
    COR_PRF_GC_PINNED_OBJECT_HEAP = 4
}

public enum COR_PRF_CLAUSE_TYPE
{
    COR_PRF_CLAUSE_NONE = 0,
    COR_PRF_CLAUSE_FILTER = 1,
    COR_PRF_CLAUSE_CATCH = 2,
    COR_PRF_CLAUSE_FINALLY = 3,
}

public enum COR_PRF_RUNTIME_TYPE
{
    COR_PRF_DESKTOP_CLR = 0x1,
    COR_PRF_CORE_CLR = 0x2,
}

/// <summary>
/// COR_PRF_GC_REASON describes the reason for a given GC.
/// </summary>
public enum COR_PRF_GC_REASON
{
    COR_PRF_GC_INDUCED = 1,     // Induced by GC.Collect
    COR_PRF_GC_OTHER = 0        // Anything else
}

/// <summary>
/// COR_PRF_GC_ROOT_KIND describes the kind of GC root exposed by
/// the RootReferences2 callback.
/// </summary>
public enum COR_PRF_GC_ROOT_KIND
{
    COR_PRF_GC_ROOT_STACK = 1,        // Variables on the stack
    COR_PRF_GC_ROOT_FINALIZER = 2,    // Entry in the finalizer queue
    COR_PRF_GC_ROOT_HANDLE = 3,        // GC Handle
    COR_PRF_GC_ROOT_OTHER = 0        //Misc. roots
}

/// <summary>
/// COR_PRF_GC_ROOT_FLAGS describes properties of a GC root
/// exposed by the RootReferences callback.
/// </summary>
public enum COR_PRF_GC_ROOT_FLAGS
{
    COR_PRF_GC_ROOT_PINNING = 0x1,    // Prevents GC from moving the object
    COR_PRF_GC_ROOT_WEAKREF = 0x2,    // Does not prevent collection
    COR_PRF_GC_ROOT_INTERIOR = 0x4,   // Refers to a field of the object rather than the object itself
    COR_PRF_GC_ROOT_REFCOUNTED = 0x8, // Whether it prevents collection depends on a refcount - if not,
                                      // COR_PRF_GC_ROOT_WEAKREF will be set also
}

public struct COR_DEBUG_IL_TO_NATIVE_MAP
{
    public uint ilOffset;
    public uint nativeStartOffset;
    public uint nativeEndOffset;
}

public unsafe struct COR_PRF_EVENTPIPE_PROVIDER_CONFIG
{
    public char* providerName;
    public ulong keywords;
    public uint loggingLevel;
    // filterData expects a semicolon delimited string that defines key value pairs
    // such as "key1=value1;key2=value2;". Quotes can be used to escape the '=' and ';'
    // characters. These key value pairs will be passed in the enable callback to event
    // providers
    public char* filterData;
}

public readonly struct EVENTPIPE_SESSION
{
    public readonly ulong Value;
}

public readonly struct EVENTPIPE_PROVIDER
{
    public readonly nint Value;
}

public readonly struct EVENTPIPE_EVENT
{
    public readonly nint Value;
}

public unsafe struct COR_PRF_EVENTPIPE_PARAM_DESC
{
    public uint type;
    // Used if type == ArrayType
    public uint elementType;
    public char* name;
}

public struct COR_PRF_EVENT_DATA
{
    public ulong ptr;
    public uint size;
    public uint reserved;
}

public enum COR_PRF_HANDLE_TYPE
{
    COR_PRF_HANDLE_TYPE_WEAK = 0x1,
    COR_PRF_HANDLE_TYPE_STRONG = 0x2,
    COR_PRF_HANDLE_TYPE_PINNED = 0x3,
}

public readonly struct ObjectHandleId
{
    public readonly nint Value;
}