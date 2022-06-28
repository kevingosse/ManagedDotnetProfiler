using System;
using System.Collections;
using System.Collections.Generic;

namespace ManagedDotnetProfiler
{
    internal class ManagedThreadList
    {
        private const int MinBufferSize = 50;

        private readonly object _lock = new();

        private readonly List<ManagedThreadInfo> _threads;
        private readonly Dictionary<ThreadId, ManagedThreadInfo> _lookupByClrThreadId;
        private readonly Dictionary<uint, ManagedThreadInfo> _lookupByProfilerThreadInfoId;

        public ManagedThreadList()
        {
            _threads = new(MinBufferSize);
            _lookupByClrThreadId = new(MinBufferSize);
            _lookupByProfilerThreadInfoId = new(MinBufferSize);
        }

        public int Count => _threads.Count;

        public ManagedThreadInfo GetOrCreate(ThreadId clrThreadId)
        {
            lock (_lock)
            {
                var info = FindByClrId(clrThreadId);

                if (info == null)
                {
                    info = new ManagedThreadInfo(clrThreadId);
                    _threads.Add(info);

                    _lookupByClrThreadId[clrThreadId] = info;
                    _lookupByProfilerThreadInfoId[info.ProfilerThreadInfoId] = info;
                }

                return info;
            }
        }

        private ManagedThreadInfo FindByClrId(ThreadId clrThreadId)
        {
            lock (_lock)
            {
                if (_threads.Count == 0)
                {
                    return null;
                }

                return _lookupByClrThreadId.TryGetValue(clrThreadId, out var info) ? info : null;
            }
        }

        public Enumerator GetEnumerator() => new(this);

        public struct Enumerator : IEnumerator<ManagedThreadInfo>
        {
            private int _position;
            private readonly ManagedThreadList _threadList;

            public Enumerator(ManagedThreadList threadList)
            {
                _position = int.MaxValue;
                _threadList = threadList;
                Current = null;
            }

            public bool MoveNext()
            {
                lock (_threadList._lock)
                {
                    var activeThreadCount = _threadList._threads.Count;

                    if (activeThreadCount == 0)
                    {
                        Current = null;
                        return false;
                    }

                    if (_position < activeThreadCount - 1)
                    {
                        _position++;
                    }
                    else
                    {
                        _position = 0;
                    }

                    Current = _threadList._threads[_position];
                    return true;
                }
            }

            public void Reset()
            {
            }

            public ManagedThreadInfo Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }

        public bool UnregisterThread(ThreadId threadId, out ManagedThreadInfo threadInfo)
        {
            lock (_lock)
            {
                foreach (var thread in _threads)
                {
                    if (thread.ClrThreadId == threadId)
                    {
                        threadInfo = thread;

                        _threads.Remove(thread);
                        _lookupByClrThreadId.Remove(threadInfo.ClrThreadId);
                        _lookupByProfilerThreadInfoId.Remove(threadInfo.ProfilerThreadInfoId);

                        return true;
                    }
                }
            }

            threadInfo = null;
            return false;
        }

        public void SetThreadOsInfo(ThreadId managedThreadId, int osThreadId, IntPtr osThreadHandle)
        {
            lock (_lock)
            {
                var info = GetOrCreate(managedThreadId);

                info.SetOsInfo(osThreadId, osThreadHandle);
            }
        }

        public void SetThreadName(ThreadId threadId, string threadName)
        {
            lock (_lock)
            {
                var info = GetOrCreate(threadId);

                info.ThreadName = threadName;
            }
        }
    }
}
