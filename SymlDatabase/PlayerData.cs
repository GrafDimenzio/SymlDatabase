using System.Collections.Generic;
using Syml;

namespace SymlDatabase;

public class PlayerData : IDocumentSection
{
    public string NickName { get; set; }
    
    public Dictionary<string, string> Data = new();
}