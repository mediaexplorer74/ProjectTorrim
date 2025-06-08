
// Type: BrawlerSource.Fortress
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Framework.LevelEditor;
using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;

#nullable disable
namespace BrawlerSource
{
  [Prefab(typeof (Fortress), "Fire", true, true)]
  public class Fortress : Bases
  {
    public Fortress(Layer layer, Position position, Position dimensions, float depth)
      : base(layer, position, dimensions, depth)
    {
      this.myMaxHealth = 5000;
      this.myHealthbar.Maximum = this.myMaxHealth;
      this.myHealthbar.Value = this.myMaxHealth;
      this.myHealthbar.Dimensions = new Position(128f, 8f);
      this.myHealth = this.myMaxHealth;
      SequenceManager<BaseSpriteSequence> sequenceManager1 = new SequenceManager<BaseSpriteSequence>((GameObject) this);
      sequenceManager1.AddSequence(BaseSpriteSequence.Standing, new Sequence()
      {
        TexturePath = "Base_A",
        Width = 48,
        Height = 48,
        InitialImageIndex = 6
      });
      sequenceManager1.AddSequence(BaseSpriteSequence.Falling, new Sequence()
      {
        TexturePath = "Base_A",
        Width = 48,
        Height = 48,
        InitialImageIndex = 6,
        ImageTotal = 6,
        FrameSpeed = 8,
        SequenceEnd = new EndFunction(((Bases) this).FallFinish)
      });
      sequenceManager1.AddSequence(BaseSpriteSequence.Down, new Sequence()
      {
        TexturePath = "Base_A",
        Width = 48,
        Height = 48,
        InitialImageIndex = 11
      });
      SequenceManager<BaseSpriteSequence> sequenceManager2 = sequenceManager1;
      Sprite sprite1 = new Sprite((GameObject) this);
      sprite1.Position = this.Position + new Position(-48f, 0.0f);
      sprite1.Depth = this.Depth;
      sequenceManager2.SetSprite(sprite1);
      this.myBaseSequences.Add(sequenceManager1);
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Position = (this.Position + new Position(-48f, 0.0f)),
        Dimensions = new Position(48f, 48f)
      });
      SequenceManager<BaseSpriteSequence> sequenceManager3 = new SequenceManager<BaseSpriteSequence>((GameObject) this);
      sequenceManager3.AddSequence(BaseSpriteSequence.Standing, new Sequence()
      {
        TexturePath = "Base_B",
        Width = 48,
        Height = 48,
        InitialImageIndex = 0
      });
      sequenceManager3.AddSequence(BaseSpriteSequence.Falling, new Sequence()
      {
        TexturePath = "Base_B",
        Width = 48,
        Height = 48,
        InitialImageIndex = 0,
        ImageTotal = 6,
        FrameSpeed = 8,
        SequenceEnd = new EndFunction(((Bases) this).FallFinish)
      });
      sequenceManager3.AddSequence(BaseSpriteSequence.Down, new Sequence()
      {
        TexturePath = "Base_B",
        Width = 48,
        Height = 48,
        InitialImageIndex = 5
      });
      SequenceManager<BaseSpriteSequence> sequenceManager4 = sequenceManager3;
      Sprite sprite2 = new Sprite((GameObject) this);
      sprite2.Position = this.Position + new Position(0.0f, 48f);
      sprite2.Depth = this.Depth;
      sequenceManager4.SetSprite(sprite2);
      this.myBaseSequences.Add(sequenceManager3);
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Position = (this.Position + new Position(0.0f, 48f)),
        Dimensions = new Position(48f, 48f)
      });
      SequenceManager<BaseSpriteSequence> sequenceManager5 = new SequenceManager<BaseSpriteSequence>((GameObject) this);
      sequenceManager5.AddSequence(BaseSpriteSequence.Standing, new Sequence()
      {
        TexturePath = "Base_C",
        Width = 48,
        Height = 48,
        InitialImageIndex = 0
      });
      sequenceManager5.AddSequence(BaseSpriteSequence.Falling, new Sequence()
      {
        TexturePath = "Base_C",
        Width = 48,
        Height = 48,
        InitialImageIndex = 0,
        ImageTotal = 6,
        FrameSpeed = 8,
        SequenceEnd = new EndFunction(((Bases) this).FallFinish)
      });
      sequenceManager5.AddSequence(BaseSpriteSequence.Down, new Sequence()
      {
        TexturePath = "Base_C",
        Width = 48,
        Height = 48,
        InitialImageIndex = 5
      });
      SequenceManager<BaseSpriteSequence> sequenceManager6 = sequenceManager5;
      Sprite sprite3 = new Sprite((GameObject) this);
      sprite3.Position = this.Position + new Position(0.0f, -48f);
      sprite3.Depth = this.Depth;
      sequenceManager6.SetSprite(sprite3);
      this.myBaseSequences.Add(sequenceManager5);
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Position = (this.Position + new Position(0.0f, -48f)),
        Dimensions = new Position(48f, 48f)
      });
      this.myLastBaseIndex = this.myBaseSequences.Count - 1;
    }

    protected override void Damage(object sender, CollisionEventArgs e)
    {
      base.Damage(sender, e);
      if (this.myHealth > 0)
        return;
      EndLevel endLevel = new EndLevel(this.Game, false, ((GameLayer) this.Layer).GetProperties());
      endLevel.Initialize();
      endLevel.LoadContent();
      this.Game.LevelManager.AddLevel(Levels.End, (Level) endLevel);
      this.Game.LevelManager.SetLevel(Levels.End, true);
    }
  }
}
