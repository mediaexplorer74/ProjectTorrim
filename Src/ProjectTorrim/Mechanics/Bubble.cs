
// Type: BrawlerSource.Mechanics.Bubble
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Framework.LevelEditor;
using BrawlerSource.Graphics;
using Microsoft.Xna.Framework;

#nullable disable
namespace BrawlerSource.Mechanics
{
  [Prefab(typeof (Bubble), "Sprites\\Bubble", false, false)]
  public class Bubble : DrawableGameObject
  {
    private Sprite mySprite;
    private SequenceManager<Phases> mySequences;

    public Bubble(Layer layer, Position position, Position dimensions, float depth)
      : base(layer)
    {
      this.Position = position;
      this.mySequences = new SequenceManager<Phases>((GameObject) this);
      this.mySequences.AddSequence(Phases.Waiting, new Sequence()
      {
        TexturePath = "Bubble_Animated",
        Width = 32,
        Height = 32,
        Looping = true,
        ImageTotal = 1,
        InitialImageIndex = 15,
        FrameSpeed = 1,
        SequenceEnd = new EndFunction(this.Grow)
      });
      this.mySequences.AddSequence(Phases.Growing, new Sequence()
      {
        TexturePath = "Bubble_Animated",
        Width = 32,
        Height = 32,
        Looping = true,
        ImageTotal = 16,
        FrameSpeed = 8,
        SequenceEnd = new EndFunction(this.Loop)
      });
      this.mySequences.AddSequence(Phases.Looping, new Sequence()
      {
        TexturePath = "Bubble_Animated",
        Width = 32,
        Height = 32,
        Looping = true,
        ImageTotal = 4,
        InitialImageIndex = 4,
        FrameSpeed = 8,
        SequenceEnd = new EndFunction(this.Pop)
      });
      this.mySequences.AddSequence(Phases.Popping, new Sequence()
      {
        TexturePath = "Bubble_Animated",
        Width = 32,
        Height = 32,
        Looping = true,
        ImageTotal = 7,
        InitialImageIndex = 8,
        FrameSpeed = 16,
        SequenceEnd = new EndFunction(this.Wait)
      });
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Position = this.Position;
      this.mySprite = sprite;
      this.mySequences.SetSprite(this.mySprite);
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      this.Wait();
      base.FirstUpdate(gameTime);
    }

    public void Grow() => this.mySequences.PlaySequence(Phases.Growing, 1);

    public void Loop() => this.mySequences.PlaySequence(Phases.Looping, this.Random.Next(1, 15));

    public void Pop() => this.mySequences.PlaySequence(Phases.Popping, 1);

    public void Wait() => this.mySequences.PlaySequence(Phases.Waiting, this.Random.Next(5, 15));
  }
}
