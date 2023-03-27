using NativeObjects;

namespace ProfilerLib
{
    public struct IMetaDataImport
    {
        private IMetaDataImportInvoker _impl;

        public IMetaDataImport(IntPtr ptr)
        {
            _impl = new(ptr);
        }

        public HResult QueryInterface(in Guid guid, out nint ptr)
        {
            return _impl.QueryInterface(in guid, out ptr);
        }

        public int AddRef()
        {
            return _impl.AddRef();
        }

        public int Release()
        {
            return _impl.Release();
        }

        public void CloseEnum(HCORENUM hEnum)
        {
            _impl.CloseEnum(hEnum);
        }

        public unsafe HResult CountEnum(HCORENUM hEnum, uint* pulCount)
        {
            return _impl.CountEnum(hEnum, pulCount);
        }

        public HResult ResetEnum(HCORENUM hEnum, uint ulPos)
        {
            return _impl.ResetEnum(hEnum, ulPos);
        }

        public unsafe HResult EnumTypeDefs(HCORENUM* phEnum, MdTypeDef* rTypeDefs, uint cMax, uint* pcTypeDefs)
        {
            return _impl.EnumTypeDefs(phEnum, rTypeDefs, cMax, pcTypeDefs);
        }

        public unsafe HResult EnumInterfaceImpls(HCORENUM* phEnum, MdTypeDef td, MdInterfaceImpl* rImpls, uint cMax, uint* pcImpls)
        {
            return _impl.EnumInterfaceImpls(phEnum, td, rImpls, cMax, pcImpls);
        }

        public unsafe HResult EnumTypeRefs(HCORENUM* phEnum, MdTypeRef* rTypeRefs, uint cMax, uint* pcTypeRefs)
        {
            return _impl.EnumTypeRefs(phEnum, rTypeRefs, cMax, pcTypeRefs);
        }

        public unsafe HResult FindTypeDefByName(char* szTypeDef, MdToken tkEnclosingClass, out MdTypeDef ptd)
        {
            return _impl.FindTypeDefByName(szTypeDef, tkEnclosingClass, out ptd);
        }

        public unsafe HResult GetScopeProps(char* szName, uint cchName, out uint pchName, out Guid pmvid)
        {
            return _impl.GetScopeProps(szName, cchName, out pchName, out pmvid);
        }

        public HResult GetModuleFromScope(out MdModule pmd)
        {
            return _impl.GetModuleFromScope(out pmd);
        }

        public unsafe HResult GetTypeDefProps(MdTypeDef td, char* szTypeDef, uint cchTypeDef, out uint pchTypeDef, out int pdwTypeDefFlags, out MdToken ptkExtends)
        {
            return _impl.GetTypeDefProps(td, szTypeDef, cchTypeDef, out pchTypeDef, out pdwTypeDefFlags, out ptkExtends);
        }

        public HResult GetInterfaceImplProps(MdInterfaceImpl iiImpl, out MdTypeDef pClass, out MdToken ptkIface)
        {
            return _impl.GetInterfaceImplProps(iiImpl, out pClass, out ptkIface);
        }

        public unsafe HResult GetTypeRefProps(MdTypeRef tr, out MdToken* ptkResolutionScope, char* szName, uint cchName, out uint pchName)
        {
            return _impl.GetTypeRefProps(tr, out ptkResolutionScope, szName, cchName, out pchName);
        }

        public unsafe HResult ResolveTypeRef(MdTypeRef tr, in Guid riid, void** ppIScope, out MdTypeDef ptd)
        {
            return _impl.ResolveTypeRef(tr, in riid, ppIScope, out ptd);
        }

        public unsafe HResult EnumMembers(HCORENUM* phEnum, MdTypeDef cl, MdToken* rMembers, uint cMax, out uint pcTokens)
        {
            return _impl.EnumMembers(phEnum, cl, rMembers, cMax, out pcTokens);
        }

        public unsafe HResult EnumMembersWithName(HCORENUM* phEnum, MdTypeDef cl, char* szName, MdToken* rMembers, uint cMax, out uint pcTokens)
        {
            return _impl.EnumMembersWithName(phEnum, cl, szName, rMembers, cMax, out pcTokens);
        }

        public unsafe HResult EnumMethods(HCORENUM* phEnum, MdTypeDef cl, MdMethodDef* rMethods, uint cMax, out uint pcTokens)
        {
            return _impl.EnumMethods(phEnum, cl, rMethods, cMax, out pcTokens);
        }

        public unsafe HResult EnumMethodsWithName(HCORENUM* phEnum, MdTypeDef cl, char* szName, MdMethodDef* rMethods, uint cMax, out uint pcTokens)
        {
            return _impl.EnumMethodsWithName(phEnum, cl, szName, rMethods, cMax, out pcTokens);
        }

        public unsafe HResult EnumFields(HCORENUM* phEnum, MdTypeDef cl, MdFieldDef* rFields, uint cMax, out uint pcTokens)
        {
            return _impl.EnumFields(phEnum, cl, rFields, cMax, out pcTokens);
        }

        public unsafe HResult EnumFieldsWithName(HCORENUM* phEnum, MdTypeDef cl, char* szName, MdFieldDef* rFields, uint cMax, out uint pcTokens)
        {
            return _impl.EnumFieldsWithName(phEnum, cl, szName, rFields, cMax, out pcTokens);
        }

        public unsafe HResult EnumParams(HCORENUM* phEnum, MdMethodDef mb, MdParamDef* rParams, uint cMax, out uint pcTokens)
        {
            return _impl.EnumParams(phEnum, mb, rParams, cMax, out pcTokens);
        }

        public unsafe HResult EnumMemberRefs(HCORENUM* phEnum, MdToken tkParent, MdMemberRef* rMemberRefs, uint cMax, out uint pcTokens)
        {
            return _impl.EnumMemberRefs(phEnum, tkParent, rMemberRefs, cMax, out pcTokens);
        }

        public unsafe HResult EnumMethodImpls(HCORENUM* phEnum, MdTypeDef td, MdToken* rMethodBody, MdToken* rMethodDecl, uint cMax, out uint pcTokens)
        {
            return _impl.EnumMethodImpls(phEnum, td, rMethodBody, rMethodDecl, cMax, out pcTokens);
        }

        public unsafe HResult EnumPermissionSets(HCORENUM* phEnum, MdToken tk, int dwActions, MdPermission* rPermission, uint cMax, out uint pcTokens)
        {
            return _impl.EnumPermissionSets(phEnum, tk, dwActions, rPermission, cMax, out pcTokens);
        }

        public unsafe HResult FindMember(MdTypeDef td, char* szName, nint* pvSigBlob, uint cbSigBlob, out MdToken pmb)
        {
            return _impl.FindMember(td, szName, pvSigBlob, cbSigBlob, out pmb);
        }

        public unsafe HResult FindMethod(MdTypeDef td, char* szName, nint* pvSigBlob, uint cbSigBlob, out MdMethodDef pmb)
        {
            return _impl.FindMethod(td, szName, pvSigBlob, cbSigBlob, out pmb);
        }

        public unsafe HResult FindField(MdTypeDef td, char* szName, nint* pvSigBlob, uint cbSigBlob, out MdFieldDef pmb)
        {
            return _impl.FindField(td, szName, pvSigBlob, cbSigBlob, out pmb);
        }

        public unsafe HResult FindMemberRef(MdTypeRef td, char* szName, nint* pvSigBlob, uint cbSigBlob, out MdMemberRef pmr)
        {
            return _impl.FindMemberRef(td, szName, pvSigBlob, cbSigBlob, out pmr);
        }

        public unsafe HResult GetMethodProps(MdMethodDef mb, out MdTypeDef pClass, char* szMethod, uint cchMethod, out uint pchMethod, out int pdwAttr, out nint* ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out int pdwImplFlags)
        {
            return _impl.GetMethodProps(mb, out pClass, szMethod, cchMethod, out pchMethod, out pdwAttr, out ppvSigBlob, out pcbSigBlob, out pulCodeRVA, out pdwImplFlags);
        }

        public unsafe HResult GetMemberRefProps(MdMemberRef mr, out MdToken ptk, char* szMember, uint cchMember, out uint pchMember, out nint* ppvSigBlob, out uint pbSig)
        {
            return _impl.GetMemberRefProps(mr, out ptk, szMember, cchMember, out pchMember, out ppvSigBlob, out pbSig);
        }

        public unsafe HResult EnumProperties(HCORENUM* phEnum, MdTypeDef td, MdProperty* rProperties, uint cMax, out uint pcProperties)
        {
            return _impl.EnumProperties(phEnum, td, rProperties, cMax, out pcProperties);
        }

        public unsafe HResult EnumEvents(HCORENUM* phEnum, MdTypeDef td, MdEvent* rEvents, uint cMax, out uint pcEvents)
        {
            return _impl.EnumEvents(phEnum, td, rEvents, cMax, out pcEvents);
        }

        public unsafe HResult GetEventProps(MdEvent ev, MdTypeDef* pClass, char* szEvent, uint cchEvent, uint* pchEvent, int* pdwEventFlags, MdToken* ptkEventType, out MdMethodDef pmdAddOn, out MdMethodDef pmdRemoveOn, out MdMethodDef pmdFire, out MdMethodDef* rmdOtherMethod, uint cMax, out uint pcOtherMethod)
        {
            return _impl.GetEventProps(ev, pClass, szEvent, cchEvent, pchEvent, pdwEventFlags, ptkEventType, out pmdAddOn, out pmdRemoveOn, out pmdFire, out rmdOtherMethod, cMax, out pcOtherMethod);
        }

        public unsafe HResult EnumMethodSemantics(HCORENUM* phEnum, MdMethodDef mb, out MdToken* rEventProp, uint cMax, out uint pcEventProp)
        {
            return _impl.EnumMethodSemantics(phEnum, mb, out rEventProp, cMax, out pcEventProp);
        }

        public HResult GetMethodSemantics(MdMethodDef mb, MdToken tkEventProp, out int pdwSemanticsFlags)
        {
            return _impl.GetMethodSemantics(mb, tkEventProp, out pdwSemanticsFlags);
        }

        public unsafe HResult GetClassLayout(MdTypeDef td, out int pdwPackSize, COR_FIELD_OFFSET* rFieldOffset, uint cMax, out uint pcFieldOffset, out uint pulClassSize)
        {
            return _impl.GetClassLayout(td, out pdwPackSize, rFieldOffset, cMax, out pcFieldOffset, out pulClassSize);
        }

        public unsafe HResult GetFieldMarshal(MdToken tk, out nint* ppvNativeType, out uint pcbNativeType)
        {
            return _impl.GetFieldMarshal(tk, out ppvNativeType, out pcbNativeType);
        }

        public unsafe HResult GetRVA(MdToken tk, uint* pulCodeRVA, int* pdwImplFlags)
        {
            return _impl.GetRVA(tk, pulCodeRVA, pdwImplFlags);
        }

        public unsafe HResult GetPermissionSetProps(MdPermission pm, out int pdwAction, out void* ppvPermission, out uint pcbPermission)
        {
            return _impl.GetPermissionSetProps(pm, out pdwAction, out ppvPermission, out pcbPermission);
        }

        public unsafe HResult GetSigFromToken(MdSignature mdSig, out nint* ppvSig, out uint pcbSig)
        {
            return _impl.GetSigFromToken(mdSig, out ppvSig, out pcbSig);
        }

        public unsafe HResult GetModuleRefProps(MdModuleRef mur, char* szName, uint cchName, out uint pchName)
        {
            return _impl.GetModuleRefProps(mur, szName, cchName, out pchName);
        }

        public unsafe HResult EnumModuleRefs(HCORENUM* phEnum, MdModuleRef* rModuleRefs, uint cmax, out uint pcModuleRefs)
        {
            return _impl.EnumModuleRefs(phEnum, rModuleRefs, cmax, out pcModuleRefs);
        }

        public unsafe HResult GetTypeSpecFromToken(MdTypeSpec typespec, out nint* ppvSig, out uint pcbSig)
        {
            return _impl.GetTypeSpecFromToken(typespec, out ppvSig, out pcbSig);
        }

        public unsafe HResult GetNameFromToken(MdToken tk, out byte* pszUtf8NamePtr)
        {
            return _impl.GetNameFromToken(tk, out pszUtf8NamePtr);
        }

        public unsafe HResult EnumUnresolvedMethods(HCORENUM* phEnum, MdToken* rMethods, uint cMax, out uint pcTokens)
        {
            return _impl.EnumUnresolvedMethods(phEnum, rMethods, cMax, out pcTokens);
        }

        public unsafe HResult GetUserString(MdString stk, char* szString, uint cchString, out uint pchString)
        {
            return _impl.GetUserString(stk, szString, cchString, out pchString);
        }

        public unsafe HResult GetPinvokeMap(MdToken tk, out int pdwMappingFlags, char* szImportName, uint cchImportName, out uint pchImportName, out MdModuleRef pmrImportDLL)
        {
            return _impl.GetPinvokeMap(tk, out pdwMappingFlags, szImportName, cchImportName, out pchImportName, out pmrImportDLL);
        }

        public unsafe HResult EnumSignatures(HCORENUM* phEnum, MdSignature* rSignatures, uint cmax, out uint pcSignatures)
        {
            return _impl.EnumSignatures(phEnum, rSignatures, cmax, out pcSignatures);
        }

        public unsafe HResult EnumTypeSpecs(HCORENUM* phEnum, MdTypeSpec* rTypeSpecs, uint cmax, out uint pcTypeSpecs)
        {
            return _impl.EnumTypeSpecs(phEnum, rTypeSpecs, cmax, out pcTypeSpecs);
        }

        public unsafe HResult EnumUserStrings(HCORENUM* phEnum, MdString* rStrings, uint cmax, out uint pcStrings)
        {
            return _impl.EnumUserStrings(phEnum, rStrings, cmax, out pcStrings);
        }

        public HResult GetParamForMethodIndex(MdMethodDef md, uint ulParamSeq, out MdParamDef ppd)
        {
            return _impl.GetParamForMethodIndex(md, ulParamSeq, out ppd);
        }

        public unsafe HResult EnumCustomAttributes(HCORENUM* phEnum, MdToken tk, MdToken tkType, MdCustomAttribute* rCustomAttributes, uint cMax, out uint pcCustomAttributes)
        {
            return _impl.EnumCustomAttributes(phEnum, tk, tkType, rCustomAttributes, cMax, out pcCustomAttributes);
        }

        public unsafe HResult GetCustomAttributeProps(MdCustomAttribute cv, out MdToken ptkObj, out MdToken ptkType, out void* ppBlob, out uint pcbSize)
        {
            return _impl.GetCustomAttributeProps(cv, out ptkObj, out ptkType, out ppBlob, out pcbSize);
        }

        public unsafe HResult FindTypeRef(MdToken tkResolutionScope, char* szName, out MdTypeRef ptr)
        {
            return _impl.FindTypeRef(tkResolutionScope, szName, out ptr);
        }

        public unsafe HResult GetMemberProps(MdToken mb, MdTypeDef* pClass, char* szMember, uint cchMember, uint* pchMember, int* pdwAttr, out nint* ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, int* pdwImplFlags, int* pdwCPlusTypeFlag, out byte ppValue, out uint pcchValue)
        {
            return _impl.GetMemberProps(mb, pClass, szMember, cchMember, pchMember, pdwAttr, out ppvSigBlob, out pcbSigBlob, out pulCodeRVA, pdwImplFlags, pdwCPlusTypeFlag, out ppValue, out pcchValue);
        }

        public unsafe HResult GetFieldProps(MdFieldDef mb, MdTypeDef* pClass, char* szField, uint cchField, uint* pchField, int* pdwAttr, out nint* ppvSigBlob, out uint pcbSigBlob, out int pdwCPlusTypeFlag, out byte ppValue, out uint pcchValue)
        {
            return _impl.GetFieldProps(mb, pClass, szField, cchField, pchField, pdwAttr, out ppvSigBlob, out pcbSigBlob, out pdwCPlusTypeFlag, out ppValue, out pcchValue);
        }

        public unsafe HResult GetPropertyProps(MdProperty prop, out MdTypeDef pClass, char* szProperty, uint cchProperty, out uint pchProperty, out int pdwPropFlags, out nint* ppvSig, out uint pbSig, out int pdwCPlusTypeFlag, out byte ppDefaultValue, out uint pcchDefaultValue, out MdMethodDef pmdSetter, out MdMethodDef pmdGetter, out MdMethodDef rmdOtherMethod, uint cMax, out uint pcOtherMethod)
        {
            return _impl.GetPropertyProps(prop, out pClass, szProperty, cchProperty, out pchProperty, out pdwPropFlags, out ppvSig, out pbSig, out pdwCPlusTypeFlag, out ppDefaultValue, out pcchDefaultValue, out pmdSetter, out pmdGetter, out rmdOtherMethod, cMax, out pcOtherMethod);
        }

        public unsafe HResult GetParamProps(MdParamDef tk, out MdMethodDef pmd, out uint pulSequence, char* szName, uint cchName, out uint pchName, out int pdwAttr, out int pdwCPlusTypeFlag, out byte ppValue, out uint pcchValue)
        {
            return _impl.GetParamProps(tk, out pmd, out pulSequence, szName, cchName, out pchName, out pdwAttr, out pdwCPlusTypeFlag, out ppValue, out pcchValue);
        }

        public unsafe HResult GetCustomAttributeByName(MdToken tkObj, char* szName, out void* ppData, out uint pcbData)
        {
            return _impl.GetCustomAttributeByName(tkObj, szName, out ppData, out pcbData);
        }

        public bool IsValidToken(MdToken tk)
        {
            return _impl.IsValidToken(tk);
        }

        public HResult GetNestedClassProps(MdTypeDef tdNestedClass, out MdTypeDef ptdEnclosingClass)
        {
            return _impl.GetNestedClassProps(tdNestedClass, out ptdEnclosingClass);
        }

        public unsafe HResult GetNativeCallConvFromSig(void* pvSig, uint cbSig, out uint pCallConv)
        {
            return _impl.GetNativeCallConvFromSig(pvSig, cbSig, out pCallConv);
        }

        public HResult IsGlobal(MdToken pd, out int pbGlobal)
        {
            return _impl.IsGlobal(pd, out pbGlobal);
        }
    }
}
