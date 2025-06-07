
// Type: BrawlerSource.Collision.CollisionMap
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Collision
{
  public class CollisionMap : GameObject
  {
    private Dictionary<LayerType, CollisionLayer> myCollisionLayers;
    private Position myPosition;
    private Position mySize;
    private int myCapacity;
    private int myDepth;

    public CollisionMap(Layer layer, Position position, Position size, int capacity, int depth)
      : base(layer)
    {
      this.myCollisionLayers = new Dictionary<LayerType, CollisionLayer>();
      this.myPosition = position;
      this.mySize = size;
      this.myCapacity = capacity;
      this.myDepth = depth;
    }

    public void Add(Collider collider)
    {
      CollisionLayer collisionLayer;
      if (!this.myCollisionLayers.TryGetValue(collider.CollisionLayer, out collisionLayer))
      {
        collisionLayer = new CollisionLayer((GameObject) this, collider.CollisionLayer, this.myPosition, this.mySize, this.myCapacity, this.myDepth);
        this.myCollisionLayers.Add(collider.CollisionLayer, collisionLayer);
      }
      collisionLayer.Add(collider);
    }

    public void Remove(Collider collider)
    {
      CollisionLayer collisionLayer;
      if (!this.myCollisionLayers.TryGetValue(collider.CollisionLayer, out collisionLayer))
        return;
      collisionLayer.Remove(collider);
    }

    public HashSet<Collider> GetColliders(LayerType layer, Type type)
    {
      HashSet<Collider> colliders;
      this.myCollisionLayers[layer].TypeColliders.TryGetValue(type, out colliders);
      return colliders;
    }
  }
}
