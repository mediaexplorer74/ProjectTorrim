
// Type: BrawlerSource.Collision.CollisionEventArgs
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

#nullable disable
namespace BrawlerSource.Collision
{
  public class CollisionEventArgs : BrawlerEventArgs
  {
    public GameObject Trigger { get; set; }

    public Collider Collider { get; set; }

    public TouchType Type { get; set; }
  }
}
