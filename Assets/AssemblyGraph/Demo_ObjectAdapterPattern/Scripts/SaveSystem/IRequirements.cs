using System;
using System.Threading.Tasks;

namespace Plugins.Demo.ObjectAdapter.SaveSystem
{
    public interface IRequirements
    {
        Task<string> Load(DateTime timestamp);

        Task Save(DateTime timestamp, string text);
    }
}