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

    public override string ToString() => Value.ToString("x2");
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

    public override string ToString() => Value.ToString("x2");
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

    public override string ToString() => Value.ToString("x2");
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
public enum COR_PRF_MONITOR : uint
{
    // These flags represent classes of callback events
    COR_PRF_MONITOR_NONE = 0x00000000,

    // MONITOR_FUNCTION_UNLOADS controls the
    // FunctionUnloadStarted callback.
    COR_PRF_MONITOR_FUNCTION_UNLOADS = 0x00000001,

    // MONITOR_CLASS_LOADS controls the ClassLoad*
    // and ClassUnload* callbacks.
    // See the comments on those callbacks for important
    // behavior changes in V2.
    COR_PRF_MONITOR_CLASS_LOADS = 0x00000002,

    // MONITOR_MODULE_LOADS controls the
    // ModuleLoad*, ModuleUnload*, and ModuleAttachedToAssembly
    // callbacks.
    COR_PRF_MONITOR_MODULE_LOADS = 0x00000004,

    // MONITOR_ASSEMBLY_LOADS controls the
    // AssemblyLoad* and AssemblyUnload* callbacks
    COR_PRF_MONITOR_ASSEMBLY_LOADS = 0x00000008,

    // MONITOR_APPDOMAIN_LOADS controls the
    // AppDomainCreation* and AppDomainShutdown* callbacks
    COR_PRF_MONITOR_APPDOMAIN_LOADS = 0x00000010,

    // MONITOR_JIT_COMPILATION controls the
    // JITCompilation*, JITFunctionPitched, and JITInlining
    // callbacks.
    COR_PRF_MONITOR_JIT_COMPILATION = 0x00000020,


    // MONITOR_EXCEPTIONS controls the ExceptionThrown,
    // ExceptionSearch*, ExceptionOSHandler*, ExceptionUnwind*,
    // and ExceptionCatcher* callbacks.
    COR_PRF_MONITOR_EXCEPTIONS = 0x00000040,

    // MONITOR_GC controls the GarbageCollectionStarted/Finished,
    // MovedReferences, SurvivingReferences,
    // ObjectReferences, ObjectsAllocatedByClass,
    // RootReferences*, HandleCreated/Destroyed, and FinalizeableObjectQueued
    // callbacks.
    COR_PRF_MONITOR_GC = 0x00000080,

    // MONITOR_OBJECT_ALLOCATED controls the
    // ObjectAllocated callback.
    COR_PRF_MONITOR_OBJECT_ALLOCATED = 0x00000100,

    // MONITOR_THREADS controls the ThreadCreated,
    // ThreadDestroyed, ThreadAssignedToOSThread,
    // and ThreadNameChanged callbacks.
    COR_PRF_MONITOR_THREADS = 0x00000200,

    // CORECLR DEPRECATION WARNING: Remoting no longer exists in coreclr
    // MONITOR_REMOTING controls the Remoting*
    // callbacks.
    COR_PRF_MONITOR_REMOTING = 0x00000400,

    // MONITOR_CODE_TRANSITIONS controls the
    // UnmanagedToManagedTransition and
    // ManagedToUnmanagedTransition callbacks.
    COR_PRF_MONITOR_CODE_TRANSITIONS = 0x00000800,

    // MONITOR_ENTERLEAVE controls the
    // FunctionEnter*/Leave*/Tailcall* callbacks
    COR_PRF_MONITOR_ENTERLEAVE = 0x00001000,

    // MONITOR_CCW controls the COMClassicVTable*
    // callbacks.
    COR_PRF_MONITOR_CCW = 0x00002000,

    // CORECLR DEPRECATION WARNING: Remoting no longer exists in coreclr
    // MONITOR_REMOTING_COOKIE controls whether
    // a cookie will be passed to the Remoting* callbacks
    COR_PRF_MONITOR_REMOTING_COOKIE = 0x00004000 | COR_PRF_MONITOR_REMOTING,

    // CORECLR DEPRECATION WARNING: Remoting no longer exists in coreclr
    // MONITOR_REMOTING_ASYNC controls whether
    // the Remoting* callbacks will monitor async events
    COR_PRF_MONITOR_REMOTING_ASYNC = 0x00008000 | COR_PRF_MONITOR_REMOTING,

    // MONITOR_SUSPENDS controls the RuntimeSuspend*,
    // RuntimeResume*, RuntimeThreadSuspended, and
    // RuntimeThreadResumed callbacks.
    COR_PRF_MONITOR_SUSPENDS = 0x00010000,

    // MONITOR_CACHE_SEARCHES controls the
    // JITCachedFunctionSearch* callbacks.
    // See the comments on those callbacks for important
    // behavior changes in V2.
    COR_PRF_MONITOR_CACHE_SEARCHES = 0x00020000,

    // NOTE: ReJIT is now supported again.  The profiler must set this flag on
    // startup in order to use RequestReJIT or RequestRevert.  If the profiler specifies
    // this flag, then the profiler must also specify COR_PRF_DISABLE_ALL_NGEN_IMAGES
    COR_PRF_ENABLE_REJIT = 0x00040000,

    // V2 MIGRATION WARNING: DEPRECATED
    // Inproc debugging is no longer supported. ENABLE_INPROC_DEBUGGING
    // has no effect.
    COR_PRF_ENABLE_INPROC_DEBUGGING = 0x00080000,

    // V2 MIGRATION NOTE: DEPRECATED
    // The runtime now always tracks IL-native maps; this flag is thus always
    // considered to be set.
    COR_PRF_ENABLE_JIT_MAPS = 0x00100000,

    // DISABLE_INLINING tells the runtime to disable all inlining
    COR_PRF_DISABLE_INLINING = 0x00200000,

    // DISABLE_OPTIMIZATIONS tells the runtime to disable all code optimizations
    COR_PRF_DISABLE_OPTIMIZATIONS = 0x00400000,

    // ENABLE_OBJECT_ALLOCATED tells the runtime that the profiler may want
    // object allocation notifications.  This must be set during initialization if the profiler
    // ever wants object notifications (using COR_PRF_MONITOR_OBJECT_ALLOCATED)
    COR_PRF_ENABLE_OBJECT_ALLOCATED = 0x00800000,

    // MONITOR_CLR_EXCEPTIONS controls the ExceptionCLRCatcher*
    // callbacks.
    COR_PRF_MONITOR_CLR_EXCEPTIONS = 0x01000000,

    // All callback events are enabled with this flag
    COR_PRF_MONITOR_ALL = 0x0107FFFF,

    // ENABLE_FUNCTION_ARGS enables argument tracing through FunctionEnter2.
    COR_PRF_ENABLE_FUNCTION_ARGS = 0X02000000,

    // ENABLE_FUNCTION_RETVAL enables retval tracing through FunctionLeave2.
    COR_PRF_ENABLE_FUNCTION_RETVAL = 0X04000000,

    // ENABLE_FRAME_INFO enables retrieval of exact ClassIDs for generic functions using
    // GetFunctionInfo2 with a COR_PRF_FRAME_INFO obtained from FunctionEnter2.
    COR_PRF_ENABLE_FRAME_INFO = 0X08000000,

    // ENABLE_STACK_SNAPSHOT enables the used of DoStackSnapshot calls.
    COR_PRF_ENABLE_STACK_SNAPSHOT = 0X10000000,

    // USE_PROFILE_IMAGES causes the native image search to look for profiler-enhanced
    // images.  If no profiler-enhanced image is found for a given assembly the
    // runtime will fallback to JIT for that assembly.
    COR_PRF_USE_PROFILE_IMAGES = 0x20000000,

    // COR_PRF_DISABLE_TRANSPARENCY_CHECKS_UNDER_FULL_TRUST will disable security
    // transparency checks normally done during JIT compilation and class loading for
    // full trust assemblies. This can make some instrumentation easier to perform.
    COR_PRF_DISABLE_TRANSPARENCY_CHECKS_UNDER_FULL_TRUST
                                        = 0x40000000,

    // Prevents all NGEN images (including profiler-enhanced images) from loading.  If
    // this and COR_PRF_USE_PROFILE_IMAGES are both specified,
    // COR_PRF_DISABLE_ALL_NGEN_IMAGES wins.
    COR_PRF_DISABLE_ALL_NGEN_IMAGES = 0x80000000,

    // The mask for valid mask bits
    COR_PRF_ALL = 0x8FFFFFFF,

    // COR_PRF_REQUIRE_PROFILE_IMAGE represents all flags that require profiler-enhanced
    // images.
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
                                          COR_PRF_MONITOR_EXCEPTIONS |
                                          COR_PRF_MONITOR_JIT_COMPILATION |
                                          COR_PRF_ENABLE_REJIT,

    COR_PRF_ALLOWABLE_NOTIFICATION_PROFILER
                                        = COR_PRF_MONITOR_FUNCTION_UNLOADS |
                                              COR_PRF_MONITOR_CLASS_LOADS |
                                              COR_PRF_MONITOR_MODULE_LOADS |
                                              COR_PRF_MONITOR_ASSEMBLY_LOADS |
                                              COR_PRF_MONITOR_APPDOMAIN_LOADS |
                                              COR_PRF_MONITOR_JIT_COMPILATION |
                                              COR_PRF_MONITOR_EXCEPTIONS |
                                              COR_PRF_MONITOR_OBJECT_ALLOCATED |
                                              COR_PRF_MONITOR_THREADS |
                                              COR_PRF_MONITOR_CODE_TRANSITIONS |
                                              COR_PRF_MONITOR_CCW |
                                              COR_PRF_MONITOR_SUSPENDS |
                                              COR_PRF_MONITOR_CACHE_SEARCHES |
                                              COR_PRF_DISABLE_INLINING |
                                              COR_PRF_DISABLE_OPTIMIZATIONS |
                                              COR_PRF_ENABLE_OBJECT_ALLOCATED |
                                              COR_PRF_MONITOR_CLR_EXCEPTIONS |
                                              COR_PRF_ENABLE_STACK_SNAPSHOT |
                                              COR_PRF_USE_PROFILE_IMAGES |
                                              COR_PRF_DISABLE_ALL_NGEN_IMAGES,

    // MONITOR_IMMUTABLE represents all flags that may only be set during initialization.
    // Trying to change any of these flags elsewhere will result in a
    // failed HRESULT.
    COR_PRF_MONITOR_IMMUTABLE = COR_PRF_MONITOR_CODE_TRANSITIONS |
                                          COR_PRF_MONITOR_REMOTING |
                                          COR_PRF_MONITOR_REMOTING_COOKIE |
                                          COR_PRF_MONITOR_REMOTING_ASYNC |
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

/// <summary>
/// Additional flags the profiler can specify via SetEventMask2 when loading
/// </summary>
[Flags]
public enum COR_PRF_HIGH_MONITOR : uint
{
    COR_PRF_HIGH_MONITOR_NONE = 0x00000000,

    // CORECLR DEPRECATION WARNING: This flag is no longer checked by the runtime
    COR_PRF_HIGH_ADD_ASSEMBLY_REFERENCES = 0x00000001,

    COR_PRF_HIGH_IN_MEMORY_SYMBOLS_UPDATED = 0x00000002,

    COR_PRF_HIGH_MONITOR_DYNAMIC_FUNCTION_UNLOADS = 0x00000004,

    COR_PRF_HIGH_DISABLE_TIERED_COMPILATION = 0x00000008,

    COR_PRF_HIGH_BASIC_GC = 0x00000010,

    // Enables the MovedReferences/MovedReferences2 callback for compacting GCs only.
    COR_PRF_HIGH_MONITOR_GC_MOVED_OBJECTS = 0x00000020,

    COR_PRF_HIGH_REQUIRE_PROFILE_IMAGE = 0,

    // Enables the large object allocation monitoring according to the LOH threshold.
    COR_PRF_HIGH_MONITOR_LARGEOBJECT_ALLOCATED = 0x00000040,

    COR_PRF_HIGH_MONITOR_EVENT_PIPE = 0x00000080,

    // Enables the pinned object allocation monitoring.
    COR_PRF_HIGH_MONITOR_PINNEDOBJECT_ALLOCATED = 0x00000100,

    COR_PRF_HIGH_ALLOWABLE_AFTER_ATTACH = COR_PRF_HIGH_IN_MEMORY_SYMBOLS_UPDATED |
                                                      COR_PRF_HIGH_MONITOR_DYNAMIC_FUNCTION_UNLOADS |
                                                      COR_PRF_HIGH_BASIC_GC |
                                                      COR_PRF_HIGH_MONITOR_GC_MOVED_OBJECTS |
                                                      COR_PRF_HIGH_MONITOR_LARGEOBJECT_ALLOCATED |
                                                      COR_PRF_HIGH_MONITOR_EVENT_PIPE,

    COR_PRF_HIGH_ALLOWABLE_NOTIFICATION_PROFILER
                                        = COR_PRF_HIGH_IN_MEMORY_SYMBOLS_UPDATED |
                                              COR_PRF_HIGH_MONITOR_DYNAMIC_FUNCTION_UNLOADS |
                                              COR_PRF_HIGH_DISABLE_TIERED_COMPILATION |
                                              COR_PRF_HIGH_BASIC_GC |
                                              COR_PRF_HIGH_MONITOR_GC_MOVED_OBJECTS |
                                              COR_PRF_HIGH_MONITOR_LARGEOBJECT_ALLOCATED |
                                              COR_PRF_HIGH_MONITOR_EVENT_PIPE,

    // MONITOR_IMMUTABLE represents all flags that may only be set during initialization.
    // Trying to change any of these flags elsewhere will result in a
    // failed HRESULT.
    COR_PRF_HIGH_MONITOR_IMMUTABLE = COR_PRF_HIGH_DISABLE_TIERED_COMPILATION,
}

public enum COR_PRF_FINALIZER_FLAGS
{
    None = 0,
    COR_PRF_FINALIZER_CRITICAL = 1
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

/// <summary>
/// Enum for specifying how much data to pass back with a stack snapshot
/// </summary>
public enum COR_PRF_SNAPSHOT_INFO : uint
{
    COR_PRF_SNAPSHOT_DEFAULT = 0x0,

    // Return a register context for each frame
    COR_PRF_SNAPSHOT_REGISTER_CONTEXT = 0x1,

    // Use a quicker stack walk algorithm based on the EBP frame chain. This is available
    // on x86 only.
    COR_PRF_SNAPSHOT_X86_OPTIMIZED = 0x2,
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

public readonly record struct ClassIdInfo(ModuleId ModuleId, MdTypeDef TypeDef);
public readonly record struct ClassIdInfo2(ModuleId ModuleId, MdTypeDef TypeDef, ClassId ParentClassId);
public readonly record struct TypeDefProps(string TypeName, int TypeDefFlags, MdToken Extends);
public readonly record struct FunctionInfo(ClassId ClassId, ModuleId ModuleId, MdToken Token);
public readonly record struct ModuleInfo(nint BaseLoadAddress, AssemblyId AssemblyId);
public readonly record struct ModuleInfoWithName(string ModuleName, nint BaseLoadAddress, AssemblyId AssemblyId);
public readonly record struct ModuleInfo2(nint BaseLoadAddress, AssemblyId AssemblyId, uint ModuleFlags);
public readonly record struct ModuleInfoWithName2(string ModuleName, nint BaseLoadAddress, AssemblyId AssemblyId, uint ModuleFlags);
public readonly record struct AppDomainInfo(string AppDomainName, ProcessId ProcessId);
public readonly record struct AssemblyInfo(AppDomainId AppDomainId, ModuleId ModuleId);
public readonly record struct AssemblyInfoWithName(string AssemblyName, AppDomainId AppDomainId, ModuleId ModuleId);
public readonly record struct StringLayout(uint BufferLengthOffset, uint StringLengthOffset, uint BufferOffset);
public readonly record struct StringLayout2(uint StringLengthOffset, uint BufferOffset);
public readonly record struct CodeInfo(IntPtr Start, uint Size);
public readonly record struct ArrayClassInfo(CorElementType BaseElemType, ClassId BaseClassId, uint Rank);
public readonly record struct TokenAndMetaData(Guid Riid, IntPtr Import, MdToken Token);
public readonly record struct ILFunctionBody(IntPtr MethodHeader, uint MethodSize);
public readonly record struct FunctionLeave3Info(COR_PRF_FRAME_INFO FrameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE RetvalRange);
public readonly record struct RuntimeInformation(ushort ClrInstanceId, COR_PRF_RUNTIME_TYPE RuntimeType, ushort MajorVersion, ushort MinorVersion, ushort BuildNumber, ushort QFEVersion);
public readonly record struct FunctionFromIP2(FunctionId FunctionId, ReJITId ReJitId);
public readonly record struct EventMask2(COR_PRF_MONITOR EventsLow, COR_PRF_HIGH_MONITOR EventsHigh);
public readonly record struct NgenModuleMethodsInliningThisMethod(IntPtr enumerator, bool incompleteData);

public readonly record struct MethodProps
{
    private readonly IntPtr _signature;
    private readonly int _signatureLength;

    public readonly MdTypeDef Class;
    public readonly string Name;
    public readonly int Attributes;
    public readonly uint RVA;
    public readonly int ImplementationFlags;

    public unsafe Span<byte> Signature => new((void*)_signature, _signatureLength);

    public MethodProps(MdTypeDef @class, string name, int attributes, IntPtr signature, int signatureLength, uint rva, int implementationFlags)
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