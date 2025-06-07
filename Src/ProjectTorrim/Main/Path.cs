
// Type: BrawlerSource.Path
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Framework.LevelEditor;
using BrawlerSource.Graphics;

#nullable disable
namespace BrawlerSource
{
  [Prefab(typeof (Path), "Path", true, true)]
  public class Path : DrawableGameObject
  {
    private Sprite mySprite;
    private Collider myCollider;

    public Path(Layer layer, Position position, Position dimensions, float depth)
      : base(layer)
    {
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = nameof (Path),
        Width = (int) dimensions.X,
        Height = (int) dimensions.Y
      };
      sprite.Position = position;
      sprite.Depth = depth;
      this.mySprite = sprite;
      this.myCollider = new Collider((GameObject) this);
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Position = position,
        Dimensions = dimensions
      });
    }
  }
}
