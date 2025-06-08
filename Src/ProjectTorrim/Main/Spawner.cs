
// Type: BrawlerSource.Spawner
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;
using BrawlerSource.Mechanics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace BrawlerSource
{
  public class Spawner : DrawableGameObject
  {
    public Queue<Wave> Waves;
    private Wave Wave;
    private TimeSpan? myLastWaveEnd;
    private TimeSpan myWaveRate;
    private readonly Queue<BrawlerSource.PathFinding.Node> myPath;
    private Sound myWarningSound;
    private Sprite myWarningBackground;
    private Text myWarningText;
    private EnemySpawner myEnemySpawner;
    private SupportSpawner mySupportSpawner;

    public event EventHandler OnWaveEnd;

    public Spawner(Layer layer)
      : base(layer)
    {
      this.Waves = new Queue<Wave>();
      this.myWaveRate = TimeSpan.FromMilliseconds(3000.0);
      this.myPath = new Queue<BrawlerSource.PathFinding.Node>();
      foreach (GameObject gameObject in (Collection<GameObject>) this.Layer.GameObjects)
      {
        if (gameObject is BrawlerSource.PathFinding.Node node)
          this.myPath.Enqueue(node);
      }
      this.myEnemySpawner = new EnemySpawner((GameObject) this, this.myPath);
      this.mySupportSpawner = new SupportSpawner((GameObject) this, this.myPath);
      Sound sound = new Sound(this.Layer, this.Game.Effects);
      sound.AudioPath = "Sound/Drums";
      sound.Pitch = 0.0f;
      this.myWarningSound = sound;
      Sprite sprite = new Sprite(this.Level.UILayer);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite.Position = new Position(0.0f, 0.0f);
      sprite.Depth = 0.0f;
      sprite.Scale = Vector2.Divide(new Vector2(256f, 80f), 256f);
      sprite.Colour = new Color(Color.White, 0.5f);
      sprite.IsHidden = true;
      this.myWarningBackground = sprite;
      this.myWarningBackground.AddToDraw();
      Text text = new Text(this.Level.UILayer);
      text.String = "Wave Complete";
      text.FontPath = "Font";
      text.Colour = Color.White;
      text.Depth = 1f;
      text.Scale = new Vector2(1f);
      text.Position = new Position(0.0f, 0.0f);
      text.IsHidden = true;
      this.myWarningText = text;
      this.myWarningText.AddToDraw();
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.myWarningBackground.LoadContent();
      this.myWarningText.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.myLastWaveEnd.HasValue && (this.Wave == null || this.myEnemySpawner.EnemiesLeft == 0) && this.myEnemySpawner.SubGameObjects.Count == 0)
      {
        if (this.mySupportSpawner.SubGameObjects.Count == 0)
        {
          this.myLastWaveEnd = new TimeSpan?(this.Layer.TotalTime);
          ((GameLayer) this.Layer).SaveGame();
          if (this.Wave != null)
            this.ShowWarning();
          EventHandler onWaveEnd = this.OnWaveEnd;
          if (onWaveEnd != null)
            onWaveEnd((object) this, new EventArgs());
        }
        else
          this.mySupportSpawner.Enabled = false;
      }
      if (this.myLastWaveEnd.HasValue)
      {
        TimeSpan totalTime = this.Layer.TotalTime;
        TimeSpan? lastWaveEnd = this.myLastWaveEnd;
        TimeSpan? nullable = lastWaveEnd.HasValue ? new TimeSpan?(totalTime - lastWaveEnd.GetValueOrDefault()) : new TimeSpan?();
        TimeSpan waveRate = this.myWaveRate;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > waveRate ? 1 : 0) : 0) != 0)
        {
          this.HideWarning();
          this.MoveNext();
          this.myLastWaveEnd = new TimeSpan?();
        }
      }
      base.Update(gameTime);
    }

    private void MoveNext()
    {
      if (this.Waves.Count > 0)
      {
        this.Wave = this.Waves.Dequeue();
        this.SetWaveSpawners();
      }
      else
      {
        EndLevel endLevel = new EndLevel(this.Game, true, ((GameLayer) this.Layer).GetProperties());
        endLevel.Initialize();
        endLevel.LoadContent();
        this.Game.LevelManager.AddLevel(Levels.End, (Level) endLevel);
        this.Game.LevelManager.SetLevel(Levels.End, true);
      }
    }

    public void SetWave(int waves)
    {
      do
      {
        this.Wave = this.Waves.Dequeue();
      }
      while (this.Waves.Count > waves);
      this.SetWaveSpawners();
    }

    private void SetWaveSpawners()
    {
      this.myEnemySpawner.SpawnRate = this.Wave.EnemySpawnRate;
      this.myEnemySpawner.Pool(new Queue<Type>((IEnumerable<Type>) this.Wave.ToSpawn));
      this.mySupportSpawner.SpawnRate = this.Wave.SupportSpawnRate;
      this.mySupportSpawner.GroupSize = this.Wave.SupportGroupSize;
      this.mySupportSpawner.SupportEnemies = new List<Type>((IEnumerable<Type>) this.Wave.ToSupport);
      this.mySupportSpawner.Enabled = true;
      this.mySupportSpawner.CreateSupportGroup();
    }

    public void ShowWarning() => this.SetWarningIsHidden(false);

    public void HideWarning() => this.SetWarningIsHidden(true);

    private void SetWarningIsHidden(bool isHidden)
    {
      if (!isHidden)
        this.myWarningSound.Play();
      this.myWarningText.IsHidden = isHidden;
      this.myWarningBackground.IsHidden = isHidden;
    }
  }
}
