using System.Collections.Generic;
using System.Linq;
using Neuron.Core.Meta;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Database;
using Synapse3.SynapseModule.Player;

namespace SymlDatabase;

[Automatic]
[Database(
    Name = "SymlDatabase",
    Id = 1,
    Priority = 0
    )]
public class SymlDataBase : Database
{
    private SymlDatabaseModule _pluginClass;

    public SymlDataBase(SymlDatabaseModule pluginClass)
    {
        _pluginClass = pluginClass;
    }

    public void StoreToFile()
    {
        _pluginClass.PlayerContainer.Store();
        _pluginClass.ServerContainer.Store();
    }

    public override string GetData(string key, out bool isHandled)
    {
        if (!_pluginClass.ServerData.Data.ContainsKey(key)) return base.GetData(key, out isHandled);
        
        isHandled = true;
        return _pluginClass.ServerData.Data[key];
    }

    public override void SetData(string key, string value, out bool isHandled)
    {
        _pluginClass.ServerData.Data[key] = value;
        _pluginClass.ServerContainer.Document.Sections.ElementAt(0).Value.Import(_pluginClass.ServerData);
        isHandled = true;
    }

    public override string GetPlayerData(SynapsePlayer player, string key, out bool isHandled)
    {
        var id = player.UserId.Replace("@", "AT");

        if (!_pluginClass.PlayerData.ContainsKey(id) || !_pluginClass.PlayerData[id].Data.ContainsKey(key))
            return base.GetPlayerData(player, key, out isHandled);
        ;
        isHandled = true;
        return _pluginClass.PlayerData[id].Data[key];
    }

    public override void SetPlayerData(SynapsePlayer player, string key, string value, out bool isHandled)
    {
        var id = player.UserId.Replace("@", "AT");
        
        if (!_pluginClass.PlayerData.ContainsKey(id))
        {
            var newData = new PlayerData()
            {
                NickName = player.NickName.Replace("[","").Replace("]",""),
                Data = new Dictionary<string, string>() { { key, value } }
            };
            _pluginClass.PlayerContainer.Document.Set(id, newData);
            _pluginClass.PlayerData[id] = newData;
            isHandled = true;
            return;
        }

        var data = _pluginClass.PlayerData[id];
        data.Data[key] = value;
        _pluginClass.PlayerData[id] = data;
        _pluginClass.PlayerContainer.Document.Sections[id].Import(data);
        isHandled = true;
    }

    public override Dictionary<string, string> GetLeaderBoard(string key, out bool isHandled, bool orderFromHighest = true, ushort size = 0)
    {
        var data = _pluginClass.PlayerData.ToList();
        data.RemoveAll(x => !x.Value.Data.ContainsKey(key));
        var result = new Dictionary<string, string>();
        
        if (orderFromHighest)
        {
            result = data
                .OrderByDescending(x => double.Parse(x.Value.Data[key]))
                .ToDictionary(x => x.Key.Replace("AT", "@"),
                    x => x.Value.Data[key]);
            
            if (size > 0)
                result = result.Take(size).ToDictionary(x => x.Key, x => x.Value);
        }
        else
        {
            result = data
                .OrderBy(x => double.Parse(x.Value.Data[key]))
                .ToDictionary(x => x.Key.Replace("AT", "@"),
                    x => x.Value.Data[key]);
            
            if (size > 0)
                result = result.Take(size).ToDictionary(x => x.Key, x => x.Value);
        }

        isHandled = true;
        return result;
    }
    
    public override 
}