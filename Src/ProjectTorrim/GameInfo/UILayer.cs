
// Type: BrawlerSource.GameInfo.UILayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.Mechanics.Towers;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.GameInfo
{
  public class UILayer(Level level, int index, GameLayer fl) : Layer(level, index)
  {
    private Text ScoreText;
    public Buyer ActiveBuyer;
    public TowerManager ActiveTowerManager;

    public override void Initilize()
    {
      base.Initilize();
      this.ActiveTowerManager = new TowerManager((Layer) this);
      this.ActiveTowerManager.AddToDraw();
      Text text = new Text((Layer) this);
      text.FontPath = "Font";
      text.Colour = Color.White;
      text.Depth = 1f;
      text.Scale = new Vector2(1f);
      text.Position = new Position(320f, (float) byte.MaxValue);
      this.ScoreText = text;
      this.ScoreText.AddToDraw();
      Panel panel = new Panel((Layer) this, new Vector2(100f, 8f), Align.Bottom, new Position());
      panel.mySprite.Colour = new Color(Color.Black, 0.5f);
      panel.AddToDraw();
      panel.OnRealign += new EventHandler(this.BottomPanel_OnRealign);
      List<Type> typeList = new List<Type>();
      typeList.Add(typeof (Soldier));
      typeList.Add(typeof (Ballista));
      typeList.Add(typeof (Freezer));
      typeList.Add(typeof (Officer));
      typeList.Add(typeof (Exploder));
      typeList.Add(typeof (Homer));
      typeList.Add(typeof (Chainer));
      for (int index = 0; index < typeList.Count; ++index)
        new Buyer((Layer) this, new Position((float) (96 * index), -4f), typeList[index]).AddToDraw();
      Button button = new Button((Layer) this, Align.Top | Align.Right, new Position(), new Position(32f, 32f), new MouseFunction(this.PauseGame));
      button.SetColour(new Color(Color.White, 0.5f));
      button.SetSprite("Cog", new Vector2?(new Vector2(32f, 32f)));
      button.AddToDraw();
      new InputEvents((Layer) this).AddKey((Keys) 80, InputType.Pressed, new KeyFunction(this.PauseGame));
    }

    private void BottomPanel_OnRealign(object sender, EventArgs e)
    {
      this.ScoreText.Position.Y = ((AlignEventArgs) e).Position.Y;
    }

    public void PauseGame(object sender, BrawlerEventArgs e) => this.Level.State = GameStates.Pause;

    public override void Update(GameTime gameTime)
    {
      this.ScoreText.String = string.Format("Score: {0}d", (object) ((GameLevel) this.Level).Score);
      base.Update(gameTime);
    }
  }
}
