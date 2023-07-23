using Neuron.Core.Events;
using Neuron.Core.Meta;
using Synapse3.SynapseModule.Events;

namespace SymlDatabase;

[Automatic]
public class EventHandler : Listener
{
    private readonly SymlDatabaseModule _pluginClass;

    public EventHandler(SymlDatabaseModule pluginClass)
    {
        _pluginClass = pluginClass;
    }
    
    public void StoreToFile()
    {
        _pluginClass.PlayerContainer.Store();
        _pluginClass.ServerContainer.Store();
    }

    [EventHandler]
    public void RoundRestart(RoundRestartEvent _) => StoreToFile();

    [EventHandler]
    public void StopServer(StopServerEvent _) => StoreToFile();
}