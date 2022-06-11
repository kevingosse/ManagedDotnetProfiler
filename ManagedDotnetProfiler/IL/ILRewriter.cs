using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler.IL
{
    internal class ILRewriter
    {
        private int m_tkLocalVarSig;
        private int m_maxStack;
        private CorILMethodFlags m_flags;
        private int m_CodeSize;
        private int m_nEH;
        private bool m_fGenerateTinyHeader;

        public int TkLocalVarSig
        {
            get => m_tkLocalVarSig;
            set
            {
                m_tkLocalVarSig = value;
                m_fGenerateTinyHeader = false;
            }
        }

        public int EHCount => m_nEH;
        
        public void InitializeTiny()
        {
            m_tkLocalVarSig = 0;
            m_maxStack = 8;
            m_flags = CorILMethodFlags.CorILMethod_TinyFormat;
            m_CodeSize = 0;
            m_nEH = 0;
            m_fGenerateTinyHeader = true;
        }
    }
}
