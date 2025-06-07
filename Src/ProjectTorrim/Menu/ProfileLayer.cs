
// Type: BrawlerSource.Menu.ProfileLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace BrawlerSource.Menu
{
  public class ProfileLayer : Layer
  {
    private Text myOverrideTitle;
    private Sprite myOverrideBackground;
    private Button myOverrideConfirm;
    private Button myOverrideCancel;
    private string myGameSave = System.IO.Path.Combine(/*Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)*/"", "gamesave_{0}.json");
    private Dictionary<string, Button> myProfileButtons;
    private bool myIsNewMode;

    public bool IsNewMode
    {
      get => this.myIsNewMode;
      set
      {
        this.myIsNewMode = value;
        foreach (string key in this.myProfileButtons.Keys)
          this.myProfileButtons[key].IsDisabled = !this.myIsNewMode && !File.Exists(string.Format(this.myGameSave, (object) key));
      }
    }

    public ProfileLayer(Level level, int index)
      : base(level, index)
    {
      this.DrawStates = GameStates.Pause | GameStates.Live;
    }

    public override void Initilize()
    {
      base.Initilize();
      new InputEvents((Layer) this).AddKey((Keys) 27, InputType.Pressed, new KeyFunction(this.Back));
      Sprite sprite1 = new Sprite((Layer) this);
      sprite1.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite1.Position = new Position(0.0f, 0.0f);
      sprite1.Depth = 0.0f;
      sprite1.Scale = Vector2.Divide(new Vector2(384f, 288f), 256f);
      sprite1.Colour = new Color(Color.White, 0.5f);
      sprite1.AddToDraw();
      Text text1 = new Text((Layer) this);
      text1.String = "Pick Profile";
      text1.FontPath = "Font";
      text1.Colour = new Color(247, 209, 0);
      text1.Depth = 1f;
      text1.Scale = new Vector2(1f);
      text1.Position = new Position(0.0f, -100f);
      text1.AddToDraw();
      this.myProfileButtons = new Dictionary<string, Button>();
      this.AddProfileButton("A", new Position(0.0f, -50f));
      this.AddProfileButton("B", new Position(0.0f, 0.0f));
      this.AddProfileButton("C", new Position(0.0f, 50f));
      Sprite sprite2 = new Sprite((Layer) this);
      sprite2.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite2.Position = new Position(0.0f, 0.0f);
      sprite2.Depth = 0.0f;
      sprite2.Scale = Vector2.Divide(new Vector2(288f, 144f), 256f);
      sprite2.Colour = new Color(Color.White, 0.5f);
      sprite2.IsHidden = true;
      this.myOverrideBackground = sprite2;
      this.myOverrideBackground.AddToDraw();
      Text text2 = new Text((Layer) this);
      text2.String = "Pick Profile";
      text2.FontPath = "Font";
      text2.Colour = Color.White;
      text2.Depth = 1f;
      text2.Scale = new Vector2(0.5f);
      text2.Position = new Position(0.0f, -100f);
      text2.IsHidden = true;
      this.myOverrideTitle = text2;
      this.myOverrideTitle.AddToDraw();
      Button button1 = new Button((Layer) this, Align.Centre, new Position(256f, 0.0f), new Position(128f, 40f), new MouseFunction(this.LoadPhase2));
      button1.IsDisabled = true;
      button1.IsHidden = true;
      this.myOverrideConfirm = button1;
      this.myOverrideConfirm.SetText("Yes");
      this.myOverrideConfirm.SetTextScale(new Vector2(1f));
      this.myOverrideConfirm.AddToDraw();
      Button button2 = new Button((Layer) this, Align.Centre, new Position(384f, 0.0f), new Position(128f, 40f), new MouseFunction(this.CancelOverride));
      button2.IsDisabled = true;
      button2.IsHidden = true;
      this.myOverrideCancel = button2;
      this.myOverrideCancel.SetText("Cancel");
      this.myOverrideCancel.SetTextScale(new Vector2(1f));
      this.myOverrideCancel.AddToDraw();
      Button button3 = new Button((Layer) this, Align.Centre, new Position(0.0f, 100f), new Position(256f, 40f), new MouseFunction(this.Back));
      button3.SetText("Back");
      button3.SetTextScale(new Vector2(1f));
      button3.AddToDraw();
    }

    public void Back(object sender, BrawlerEventArgs e) => this.Back();

    public void Back()
    {
      ((LevelMenu) this.Level).MainMenu.IsEnabled = true;
      this.IsEnabled = false;
      this.CancelOverride();
    }

    public void AddProfileButton(string profile, Position position)
    {
      Button button = new Button((Layer) this, Align.Centre, position, new Position(256f, 40f), new MouseFunction(this.LoadPhase1));
      button.Args.Add((object) profile);
      button.Args.Add((object) position);
      button.IsDisabled = !this.myIsNewMode && !File.Exists(string.Format(this.myGameSave, (object) profile));
      button.SetText(string.Format("Profile {0}", (object) profile));
      button.SetTextScale(new Vector2(1f));
      button.AddToDraw();
      this.myProfileButtons.Add(profile, button);
    }

    public void CancelOverride(object sender, BrawlerEventArgs e) => this.CancelOverride();

    public void CancelOverride()
    {
      this.myOverrideConfirm.IsDisabled = true;
      this.myOverrideConfirm.IsHidden = true;
      this.myOverrideCancel.IsDisabled = true;
      this.myOverrideCancel.IsHidden = true;
      this.myOverrideBackground.IsHidden = true;
      this.myOverrideTitle.IsHidden = true;
    }

    public void LoadPhase1(object sender, BrawlerEventArgs e)
    {
      if (this.myIsNewMode && File.Exists(string.Format(this.myGameSave, e.Args[0])))
      {
        this.myOverrideConfirm.Position = (Position) e.Args[1] + new Position(224f, 0.0f);
        this.myOverrideCancel.Position = this.myOverrideConfirm.Position + new Position(128f, 0.0f);
        this.myOverrideBackground.Position = this.myOverrideConfirm.Position + new Position(64f, -32f);
        this.myOverrideTitle.Position = this.myOverrideConfirm.Position + new Position(64f, -64f);
        this.myOverrideTitle.String = string.Format("Profile {0} already exists.\nDo you want to override\nthis save?", e.Args[0]);
        this.myOverrideConfirm.IsHidden = false;
        this.myOverrideConfirm.IsDisabled = false;
        this.myOverrideCancel.IsHidden = false;
        this.myOverrideCancel.IsDisabled = false;
        this.myOverrideBackground.IsHidden = false;
        this.myOverrideTitle.IsHidden = false;
        this.myOverrideConfirm.Args = new List<object>();
        this.myOverrideConfirm.Args.AddRange((IEnumerable<object>) e.Args);
        this.myOverrideConfirm.Args.Add((object) true);
      }
      else
        this.LoadPhase2(sender, e);
    }

    public void LoadPhase2(object sender, BrawlerEventArgs e)
    {
      if (e.Args.Count == 3)
        File.Delete(string.Format(this.myGameSave, e.Args[0]));
      GameLevel gameLevel = new GameLevel(this.Game, (string) e.Args[0]);
      gameLevel.Initialize();
      gameLevel.LoadContent();
      this.Game.LevelManager.RemoveLevel(Levels.MainGame);
      this.Game.LevelManager.AddLevel(Levels.MainGame, (Level) gameLevel);
      this.Game.LevelManager.SetLevel(Levels.MainGame);
      this.Back();
    }
  }
}
