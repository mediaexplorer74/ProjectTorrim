
// Type: BrawlerSource.Menu.LevelMenu
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System;

#nullable disable
namespace BrawlerSource.Menu
{
  public class LevelMenu(BrawlerGame game) : Level(game)
  {
    public MenuLayer MainMenu;
    public ProfileLayer ProfilePicker;
    public InstructionsLayer Instructions;
    public SplashLayer SplashScreen;
    public SettingsLayer Settings;

    public override void Initialize()
    {
      this.Layers.Add((Layer) new BackgroundLayer((Level) this, 0, false));
      this.SplashScreen = new SplashLayer((Level) this, 1, TimeSpan.FromSeconds(5.0));
      this.Layers.Add((Layer) this.SplashScreen);
      this.MainMenu = new MenuLayer((Level) this, 2);
      this.MainMenu.IsEnabled = false;
      this.Layers.Add((Layer) this.MainMenu);
      this.ProfilePicker = new ProfileLayer((Level) this, 3);
      this.ProfilePicker.IsEnabled = false;
      this.Layers.Add((Layer) this.ProfilePicker);
      this.Instructions = new InstructionsLayer((Level) this, 4);
      this.Instructions.IsEnabled = false;
      this.Layers.Add((Layer) this.Instructions);
      this.Settings = new SettingsLayer((Level) this, 5);
      this.Settings.IsEnabled = false;
      this.Layers.Add((Layer) this.Settings);
      base.Initialize();
    }
  }
}
