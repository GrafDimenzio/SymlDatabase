using System.Collections.Generic;
using Syml;

namespace SymlDatabase;

[DocumentSection("Server")]
public class ServerData : IDocumentSection
{
    public Dictionary<string, string> Data { get; set; } = new();
}