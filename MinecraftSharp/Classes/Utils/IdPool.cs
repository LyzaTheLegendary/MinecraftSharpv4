using System.Collections.Concurrent;

namespace MinecraftSharp.Classes.Utils
{
    public class IdPool
    {
        ConcurrentQueue<uint> m_numbers = new();
        uint m_count = 1;
        public IdPool()
        {

        }
        public Id GetId()
        {
            if (m_numbers.TryDequeue(out uint result))
                return new Id(Return, result);

            return new Id(Return, m_count++);
        }
        public void Return(uint id) => m_numbers.Enqueue(id);

    }
    public class Id : IDisposable
    {
        readonly Action<uint> m_onDispose;
        readonly uint m_id;
        public Id(Action<uint> onDispose, uint id)
        {
            m_onDispose = onDispose;
            m_id = id;
        }

        public void Dispose() => m_onDispose.Invoke(m_id);
        public override string ToString() => m_id.ToString();

        public static explicit operator uint(Id identifier) => identifier.m_id;
        public static bool operator ==(uint id, Id identifier) => identifier.m_id == id;
        public static bool operator !=(uint id, Id identifier) => identifier.m_id != id;

    }
    public abstract class Identifiable
    {
        protected Id m_id;
        public Identifiable(Id id) => m_id = id;
        public Id Id => m_id;
    }
}
