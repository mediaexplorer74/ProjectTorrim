
// Type: BrawlerSource.Framework.LevelEditor.TileInfo
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

//using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
    public struct TileInfo
    {
        [JsonProperty("Sprite")]  
        public string SpriteName { get; set; }

        [JsonProperty("Class")] 
        public string ClassName { get; set; }

        public bool IsResizeable { get; set; }

        public bool IsRepeating { get; set; }
    }
}
