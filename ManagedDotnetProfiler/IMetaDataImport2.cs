using System;
using ULONG = System.UInt32;
using DWORD = System.Int32;

namespace ManagedDotnetProfiler;

[GenerateNativeStub]
internal unsafe interface IMetaDataImport2 : IMetaDataImport
{
    HResult EnumGenericParams(
        HCORENUM* phEnum,                // [IN|OUT] Pointer to the enum.
        MdToken tk,                    // [IN] TypeDef or MethodDef whose generic parameters are requested
        MdGenericParam* rGenericParams,    // [OUT] Put GenericParams here.
        ULONG cMax,                   // [IN] Max GenericParams to put.
        out ULONG pcGenericParams); // [OUT] Put # put here.

    HResult GetGenericParamProps(        // S_OK or error.
        MdGenericParam gp,                  // [IN] GenericParam
        out ULONG pulParamSeq,          // [OUT] Index of the type parameter
        out DWORD pdwParamFlags,        // [OUT] Flags, for future use (e.g. variance)
        out MdToken ptOwner,              // [OUT] Owner (TypeDef or MethodDef)
        out DWORD reserved,              // [OUT] For future use (e.g. non-type parameters)
        char* wzname,                // [OUT] Put name here
        ULONG cchName,               // [IN] Size of buffer
        out ULONG pchName);        // [OUT] Put size of name (wide chars) here.

    HResult GetMethodSpecProps(
        MdMethodSpec mi,                    // [IN] The method instantiation
        out MdToken tkParent,                  // [OUT] MethodDef or MemberRef
        out IntPtr ppvSigBlob,        // [OUT] point to the blob value of meta data
        out ULONG pcbSigBlob);      // [OUT] actual size of signature blob

    HResult EnumGenericParamConstraints(
        IntPtr phEnum,                // [IN|OUT] Pointer to the enum.
        MdGenericParam tk,                  // [IN] GenericParam whose constraints are requested
        MdGenericParamConstraint* rGenericParamConstraints,    // [OUT] Put GenericParamConstraints here.
        ULONG cMax,                   // [IN] Max GenericParamConstraints to put.
        out ULONG pcGenericParamConstraints); // [OUT] Put # put here.

    HResult GetGenericParamConstraintProps( // S_OK or error.
        MdGenericParamConstraint gpc,       // [IN] GenericParamConstraint
        out MdGenericParam ptGenericParam,     // [OUT] GenericParam that is constrained
        out MdToken ptkConstraintType); // [OUT] TypeDef/Ref/Spec constraint

    HResult GetPEKind(                   // S_OK or error.
        out DWORD pdwPEKind,                   // [OUT] The kind of PE (0 - not a PE)
        out DWORD pdwMAchine);            // [OUT] Machine as defined in NT header

    HResult GetVersionString(            // S_OK or error.
        char* pwzBuf,                 // [OUT] Put version string here.
        DWORD ccBufSize,              // [IN] size of the buffer, in wide chars
        out DWORD pccBufSize);      // [OUT] Size of the version string, wide chars, including terminating nul.

    HResult EnumMethodSpecs(
        IntPtr phEnum,                // [IN|OUT] Pointer to the enum.
        MdToken tk,                    // [IN] MethodDef or MemberRef whose MethodSpecs are requested
        MdMethodSpec* rMethodSpecs,        // [OUT] Put MethodSpecs here.
        ULONG cMax,                   // [IN] Max tokens to put.
        out ULONG pcMethodSpecs);   // [OUT] Put actual count here.
}