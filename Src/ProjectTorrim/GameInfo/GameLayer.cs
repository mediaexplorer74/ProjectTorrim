
// Type: BrawlerSource.GameInfo.GameLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Framework;
using BrawlerSource.Input;
using BrawlerSource.Mechanics;
using BrawlerSource.Mechanics.Bosses;
using BrawlerSource.Mechanics.Enemies;
using Microsoft.Xna.Framework.Input;
//using System.Text.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Storage;

#nullable disable
namespace BrawlerSource.GameInfo
{
  public class GameLayer : Layer
  {
    public Tower ActiveTower;
    public Spawner Spawner;
    private string myProfile;
    public int basesID;

    public GameLayer(Level level, int index, string profile)
      : base(level, index)
    {
      this.myProfile = profile;
      this.DrawStates = GameStates.Pause | GameStates.Live;
    }

    public override void Initilize()
    {
      base.Initilize();
      this.ViewCamera.SetDraggable();
      this.ViewCamera.RelativeScale = 1.5f;
      this.ViewCamera.Position = new Position(650f, 0.0f);
      this.ViewCamera.MinPosition = new Position(-650f, -500f);
      this.ViewCamera.MaxPosition = new Position(650f, 200f);
      InputEvents inputEvents = new InputEvents((Layer) this);
      inputEvents.AddKey((Keys) 27, InputType.Pressed, new KeyFunction(this.PauseGame));
      inputEvents.AddKey((Keys) 13, InputType.Pressed, new KeyFunction(this.ShowTimers));
      this.Load("Level0_Save.json");
      this.GenerateEnemyWaves();
      this.LoadGame();
    }

    public void ShowTimers(object sender, BrawlerEventArgs e) => Timer.Write();

    public void PauseGame(object sender, BrawlerEventArgs e) => this.Level.State = GameStates.Pause;

        public void SaveGame()
        {
            string contents = JsonConvert.SerializeObject(this.GetProperties(), Formatting.Indented);
            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;

            File.WriteAllText(
                System.IO.Path.Combine(appdataFolder.Path, "gamesave_" + this.myProfile + ".json"), 
                contents);
        }

    public GameProperties GetProperties()
    {
      GameProperties properties = new GameProperties()
      {
        Waves = this.Spawner.Waves.Count - 1,
        Score = ((GameLevel) this.Level).Score,
        Towers = new List<TowerProperties>(),
        Bases = new List<BasesProperties>()
      };
      foreach (GameObject gameObject in (Collection<GameObject>) this.GameObjects)
      {
        if (!(gameObject is Tower tower))
        {
          if (gameObject is Bases bases)
            properties.Bases.Add(bases.GetProperties());
        }
        else
          properties.Towers.Add(tower.GetProperties());
      }
      return properties;
    }

    public void LoadGame()
    {
        StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
        string path = System.IO.Path.Combine(appdataFolder.Path, "gamesave_" + this.myProfile + ".json");
        if (!File.Exists(path))
            return;
        this.SetProperties(JsonConvert.DeserializeObject<GameProperties>(File.ReadAllText(path)));
    }

    public void SetProperties(GameProperties properties)
    {
      if (properties.Waves < 0)
        return;
      this.Spawner.SetWave(properties.Waves);
      ((GameLevel) this.Level).Score = properties.Score;
      foreach (Bases bases in this.GameObjects.OfType<Bases>())
      {
        Bases b = bases;
        b.SetProperties(properties.Bases.Find((Predicate<BasesProperties>) (bp => bp.ID == b.ID)));
      }
      properties.Towers.ForEach((Action<TowerProperties>) (tp =>
      {
        Tower instance = (Tower) Activator.CreateInstance(Type.GetType(tp.Type), (object) this, (object) tp.Position);
        instance.SetProperties(tp);
        instance.AddToDraw();
      }));
    }

    public void GenerateEnemyWaves()
    {
      int[] numArray1 = new int[4]{ 3, 8, 21, 55 };
      int[] numArray2 = new int[4]{ 3, 5, 8, 13 };
      Type[] typeArray1 = new Type[7]
      {
        typeof (Yak),
        typeof (Goat),
        typeof (Snake),
        typeof (Bison),
        typeof (Rhino),
        typeof (Lion),
        typeof (Elephant)
      };
      List<Type> collection = new List<Type>();
      Type[] typeArray2 = new Type[7]
      {
        typeof (Bull),
        typeof (Faun),
        typeof (Hydra),
        typeof (Minotaur),
        typeof (Aeternae),
        typeof (Griffin),
        typeof (Cyclops)
      };
      List<List<Wave>> waveListList = new List<List<Wave>>();
      this.Spawner = new Spawner((Layer) this);
      for (int index1 = 0; index1 < typeArray1.Length; ++index1)
      {
        Type type = typeArray1[index1];
        waveListList.Add(new List<Wave>());
        List<Wave> waveList = waveListList[index1];
        for (int index2 = 0; index2 < numArray1.Length; ++index2)
        {
          int num1 = numArray1[index2];
          Queue<Type> typeQueue = new Queue<Type>();
          int num2 = 15;
          for (int index3 = 0; index3 < num1; ++index3)
          {
            typeQueue.Enqueue(type);
            --num2;
            if (num2 == 0)
            {
              num2 = 16;
              if (collection.Count > 0)
              {
                for (int index4 = 0; index4 < 8; ++index4)
                  typeQueue.Enqueue(collection[collection.Count - 1]);
              }
            }
          }
          float num3 = (float) (numArray1.Length / (index2 + 1));
          waveList.Add(new Wave()
          {
            EnemySpawnRate = TimeSpan.FromMilliseconds((double) num3 * 1250.0),
            SupportSpawnRate = TimeSpan.FromMilliseconds((double) num3 * 900.0),
            SupportGroupSize = numArray2[index2],
            ToSupport = new List<Type>((IEnumerable<Type>) collection),
            ToSpawn = typeQueue
          });
        }
        collection.Add(type);
        waveList.Add(new Wave()
        {
          EnemySpawnRate = TimeSpan.FromMilliseconds(15000.0),
          SupportSpawnRate = TimeSpan.FromMilliseconds(900.0),
          SupportGroupSize = 21,
          ToSupport = new List<Type>((IEnumerable<Type>) collection),
          ToSpawn = new Queue<Type>((IEnumerable<Type>) new List<Type>()
          {
            typeArray2[index1],
            type
          })
        });
      }
      this.Spawner.Waves.Enqueue(waveListList[0][0]);
      for (int index5 = 0; index5 < waveListList.Count; ++index5)
      {
        List<Wave> waveList = waveListList[index5];
        bool flag = false;
        int num = waveList.Count / 2;
        for (int index6 = 1; index6 < waveList.Count; ++index6)
        {
          if (!flag && index6 > num && index5 < waveList.Count - 1)
          {
            this.Spawner.Waves.Enqueue(waveListList[index5 + 1][0]);
            flag = true;
          }
          this.Spawner.Waves.Enqueue(waveList[index6]);
        }
      }
    }
  }
}
