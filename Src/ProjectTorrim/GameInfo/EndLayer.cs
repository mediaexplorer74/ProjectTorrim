
// Type: BrawlerSource.GameInfo.EndLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.GameInfo
{
  public class EndLayer : Layer
  {
    private bool myHasWon;
    private GameProperties myProperties;

    public EndLayer(Level level, int index, bool hasWon, GameProperties properties)
      : base(level, index)
    {
      this.DrawStates = GameStates.Pause | GameStates.Live;
      this.myHasWon = hasWon;
      this.myProperties = properties;
    }

    public override void Initilize()
    {
      base.Initilize();
      Sprite sprite = new Sprite((Layer) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite.Position = new Position(0.0f, 0.0f);
      sprite.Depth = 0.0f;
      sprite.Scale = Vector2.Divide(new Vector2(384f, 434f), 256f);
      sprite.Colour = new Color(Color.White, 0.5f);
      sprite.AddToDraw();
      Text text1 = new Text((Layer) this);
      text1.String = this.myHasWon ? "You Win!" : "You Lost.";
      text1.FontPath = "Font";
      text1.Colour = Color.White;
      text1.Depth = 1f;
      text1.Scale = new Vector2(1f);
      text1.Position = new Position(0.0f, -180f);
      text1.AddToDraw();
      Text text2 = new Text((Layer) this);
      text2.String = string.Format("Score: {0}", (object) this.myProperties.Score);
      text2.FontPath = "Font";
      text2.Colour = Color.White;
      text2.Depth = 1f;
      text2.Scale = new Vector2(0.75f);
      text2.Position = new Position(0.0f, (float) sbyte.MinValue);
      text2.AddToDraw();
      int baseHealth = 0;
      this.myProperties.Bases.ForEach((Action<BasesProperties>) (b => baseHealth += b.Health));
      Text text3 = new Text((Layer) this);
      text3.String = this.myHasWon ? string.Format("Health Remaining: {0}", (object) baseHealth) : string.Format("Waves Remaining: {0}", (object) this.myProperties.Waves);
      text3.FontPath = "Font";
      text3.Colour = Color.White;
      text3.Depth = 1f;
      text3.Scale = new Vector2(0.75f);
      text3.Position = new Position(0.0f, -96f);
      text3.AddToDraw();
      this.DisplayTowers(new Position(0.0f, -64f));
      Button button = new Button((Layer) this, Align.Centre, new Position(0.0f, 184f), new Position(128f, 40f), new MouseFunction(this.Exit));
      button.SetText("Back");
      button.SetTextScale(new Vector2(1f));
      button.AddToDraw();
    }

    private void DisplayTowers(Position position)
    {
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      foreach (TowerProperties tower in this.myProperties.Towers)
      {
        if (!dictionary.ContainsKey(tower.Type))
          dictionary.Add(tower.Type, 0);
        dictionary[tower.Type]++;
      }
      int length = "BrawlerSource.Mechanics.Towers.".Length;
      int num = 0;
      foreach (string key in dictionary.Keys)
      {
        Text text = new Text((Layer) this);
        text.String = string.Format("{0}: {1}", (object) key.Substring(length), (object) dictionary[key]);
        text.FontPath = "Font";
        text.Colour = Color.White;
        text.Depth = 0.5f;
        text.Scale = new Vector2(0.75f);
        text.Position = position + new Position(0.0f, (float) (num * 32));
        text.AddToDraw();
        ++num;
      }
    }

    public void Exit(object sender, BrawlerEventArgs e) => this.Exit();

    public void Exit() => this.Game.LevelManager.SetLevel(Levels.MainMenu);
  }
}
