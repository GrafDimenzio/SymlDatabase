using System;
using System.Collections.Generic;
using System.IO;
using Neuron.Core;
using Neuron.Core.Modules;
using Neuron.Modules.Configs;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Events;

namespace SymlDatabase;

[Module(
    Author = "Dimenzio",
    Dependencies = new []{ typeof(Synapse) },
    Description = "Adds a Databse to Synapse based of SYML",
    Name = "SymlDatabse",
    Version = "1.0.0"
    )]
public class SymlDatabaseModule : ReloadableModule
{
    public ConfigContainer PlayerContainer { get; private set; }
    public ConfigContainer ServerContainer { get; private set; }

    public Dictionary<string, PlayerData> PlayerData { get; } = new ();
    public ServerData ServerData { get; private set; }

    public override void EnableModule()
    {
        Synapse.Get<NeuronBase>().PrepareRelativeDirectory("Database");
        var service = Synapse.Get<ConfigService>();
        PlayerContainer = service.GetContainer(Path.Combine("Database", "players.syml"));
        ServerContainer = service.GetContainer(Path.Combine("Database", "server.syml"));

        Reload();
    }

    public override void FirstSetUp() { }

    public override void Reload(ReloadEvent _ = null)
    {
        try
        {
            foreach (var section in PlayerContainer.Document.Sections)
            {
                try
                {
                    var data = section.Value.Export<PlayerData>();
                    PlayerData[section.Key] = data;
                }
                catch (Exception ex)
                {
                    Logger.Error("Loading data for " + section.Key + " failed\n" + ex);
                }
            }

            ServerData = ServerContainer.Get<ServerData>();
        }
        catch (Exception ex)
        {
            Logger.Error("Loading Database failed" + ex);
        }
    }
}
