namespace ProfilerLib;

[NativeObject]
public unsafe interface ICorProfilerInfo12 : ICorProfilerInfo11
{
    public static readonly Guid Guid = new("27b24ccd-1cb1-47c5-96ee-98190dc30959");

    HResult EventPipeStartSession(
        uint cProviderConfigs,
        COR_PRF_EVENTPIPE_PROVIDER_CONFIG* pProviderConfigs,
        bool requestRundown,
        out EVENTPIPE_SESSION pSession);

    HResult EventPipeAddProviderToSession(
        EVENTPIPE_SESSION session,
        COR_PRF_EVENTPIPE_PROVIDER_CONFIG providerConfig);

    HResult EventPipeStopSession(
        EVENTPIPE_SESSION session);

    HResult EventPipeCreateProvider(
                char* providerName,
                out EVENTPIPE_PROVIDER pProvider);

    HResult EventPipeGetProviderInfo(
                EVENTPIPE_PROVIDER provider,
                uint cchName,
                out uint pcchName,
                char* providerName);

    HResult EventPipeDefineEvent(
                EVENTPIPE_PROVIDER provider,
                char* eventName,
                uint eventID,
                ulong keywords,
                uint eventVersion,
                uint level,
                byte opcode,
                bool needStack,
                uint cParamDescs,
                COR_PRF_EVENTPIPE_PARAM_DESC* pParamDescs,
                out EVENTPIPE_EVENT pEvent);

    HResult EventPipeWriteEvent(
                EVENTPIPE_EVENT    @event,
                uint cData,
                COR_PRF_EVENT_DATA* data,
                in Guid pActivityId,
                in Guid pRelatedActivityId);
    }