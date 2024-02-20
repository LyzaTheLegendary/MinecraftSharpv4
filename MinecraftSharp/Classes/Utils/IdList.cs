namespace MinecraftSharp.Classes.Utils
{
    public class IdList
    {
        private readonly List<Identifiable> list;
        public IdList(int size = 70)
            => list = new List<Identifiable>(size);
        public void Add(Identifiable item) { lock (list) list.Add(item); }
        public void Remove(Identifiable item) { lock (list) list.Remove(item); }
        public Identifiable? Get(uint id)
        {
            lock (list)
                return list.Find(e => { return id == e.Id; });
        }
    }
}
