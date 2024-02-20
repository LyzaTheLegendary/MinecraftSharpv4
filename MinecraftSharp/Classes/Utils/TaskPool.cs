using System.Collections.Concurrent;

namespace MinecraftSharp.Classes.Utils
{
    public class TaskPool
    {
        BlockingCollection<Action> m_actions = new BlockingCollection<Action>();
        public TaskPool(int size)
        {
            for (int i = 0; i < size; i++)
                Task.Run(MainLoop);
        }
        public void MainLoop()
        {
            foreach (Action action in m_actions.GetConsumingEnumerable())
            {
                action.Invoke();
            }
        }
        public void PendAction(Action action) => m_actions.Add(action);
    }
}
