
// Type: BrawlerSource.Framework.LevelEditor.LevelEditor
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
//using System.Text.Json;
using System.Text.RegularExpressions;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class LevelEditor : Level
  {
    public TileSelector ActiveSelector;

    public LevelEditor(BrawlerGame game)
      : base(game)
    {
      throw new UnauthorizedAccessException();
    }

        public void GeneratePrefabFile()
        {
            List<TileInfo> tileInfoList = new List<TileInfo>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(PrefabAttribute), true).Length != 0)
                    {
                        PrefabAttribute customAttribute = (PrefabAttribute)Attribute.GetCustomAttribute((MemberInfo)type, typeof(PrefabAttribute));
                        tileInfoList.Add(customAttribute.Info);
                    }
                }
            }
            List<string> stringList = new List<string>();
            stringList.Add(this.Game.Content.RootDirectory + "\\Sprites");
            for (int index = 0; index < stringList.Count; ++index)
            {
                foreach (string directory in Directory.GetDirectories(stringList[index]))
                    stringList.Add(directory);
            }
            foreach (string path in stringList)
            {
                foreach (string file in Directory.GetFiles(path, "*.xnb"))
                {
                    if (new Regex("Microsoft\\.Xna\\.Framework\\.Content\\.[\\d\\w]*([^\\s]*)").Matches(File.ReadAllText(file))[0].Value.Substring("Microsoft.Xna.Framework.Content.".Length, 15) == "Texture2DReader")
                        tileInfoList.Add(new TileInfo()
                        {
                            SpriteName = file.Replace(this.Game.Content.RootDirectory + "\\", "").Replace(".xnb", ""),
                            IsRepeating = true,
                            IsResizeable = true
                        });
                }
            }
            string str;
            using (StringWriter stringWriter = new StringWriter())
            {
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });
                serializer.Serialize(stringWriter, tileInfoList);
                str = stringWriter.ToString();
            }
            using (StreamWriter streamWriter = new StreamWriter(this.Game.Content.RootDirectory + "\\Prefabs.json"))
            {
                streamWriter.WriteLine(str);
            }
        }

    public override void Initialize()
    {
      this.GeneratePrefabFile();
      this.GameLayer = (Layer) new GridLayer((Level) this, 0);
      this.Layers.Add(this.GameLayer);
      this.Layers.Add((Layer) new BrawlerSource.Framework.LevelEditor.UILayer((Level) this, 1)
      {
        gl = (GridLayer) this.GameLayer
      });
      base.Initialize();
    }
  }
}
