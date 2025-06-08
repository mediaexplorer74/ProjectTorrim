
// Type: BrawlerSource.Collision.CollisionLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collections;
using BrawlerSource.Collision.Intersections;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace BrawlerSource.Collision
{
  public class CollisionLayer : GameObject
  {
    public Dictionary<System.Type, HashSet<Collider>> TypeColliders;
    public HashSet<Collider> Colliders;
    private HashSet<Collider> myCollidersToRemove;
    private IEnumerable<Collider> myCurrentColliders;
    private QuadTree<Collider> myTree;
    public LayerType Type;

    public CollisionLayer(
      Layer layer,
      LayerType type,
      Position position,
      Position size,
      int capacity,
      int depth)
      : base(layer)
    {
      this.Construct(type, position, size, capacity, depth);
    }

    public CollisionLayer(
      GameObject parent,
      LayerType type,
      Position position,
      Position size,
      int capacity,
      int depth)
      : base(parent)
    {
      this.Construct(type, position, size, capacity, depth);
    }

    protected virtual void Construct(
      LayerType type,
      Position position,
      Position size,
      int capacity,
      int depth)
    {
      this.Type = type;
      this.myTree = new QuadTree<Collider>(position, size, capacity, depth);
      this.TypeColliders = new Dictionary<System.Type, HashSet<Collider>>();
      this.Colliders = new HashSet<Collider>();
      this.myCollidersToRemove = new HashSet<Collider>();
    }

    public void Add(Collider collider)
    {
      this.Colliders.Add(collider);
      collider.Parent.GetTypes().ForEach((Action<System.Type>) (t =>
      {
        HashSet<Collider> colliderSet;
        if (!this.TypeColliders.TryGetValue(t, out colliderSet))
          this.TypeColliders.Add(t, colliderSet = new HashSet<Collider>());
        colliderSet.Add(collider);
      }));
      collider.HasChanged = true;
    }

    public void Remove(Collider collider) => this.myCollidersToRemove.Add(collider);

    private void RemoveColliders()
    {
      foreach (Collider collider in this.myCollidersToRemove)
      {
        Collider c = collider;
        this.Colliders.Remove(c);
        c.Parent.GetTypes().ForEach((Action<System.Type>) (t => this.TypeColliders[t].Remove(c)));
      }
      this.myCollidersToRemove.Clear();
    }

    private void RegisterCollision(Collider first, Collider second, bool isColliding)
    {
      first.RegisterCollision(second, isColliding);
      second.RegisterCollision(first, isColliding);
    }

    private void CheckCollisions()
    {
      this.myTree.Clear();
      this.myCurrentColliders = this.Colliders.Except<Collider>((IEnumerable<Collider>) this.myCollidersToRemove);
      foreach (Collider currentCollider in this.myCurrentColliders)
      {
        Collider collider = currentCollider;
        if (!collider.IsDisabled)
        {
          if (collider.HasChanged)
            collider.ClearCollisions();
          HashSet<Collider> colliderSet = new HashSet<Collider>();
          foreach (IIntersectionable key in collider.IntersectionOffsets.Keys)
          {
            IIntersectionable intersection = key;
            foreach (Tuple<IIntersectionable, Collider> tuple in this.myTree.Quersert(intersection, collider, (Func<Tuple<IIntersectionable, Collider>, bool>) (d => (collider.HasChanged || d.Item2.HasChanged) && d.Item1.IsColliding(intersection))))
              colliderSet.Add(tuple.Item2);
          }
          foreach (Collider second in colliderSet)
            this.RegisterCollision(collider, second, true);
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      this.CheckCollisions();
      foreach (Collider currentCollider in this.myCurrentColliders)
        currentCollider.Collide(gameTime);
      this.RemoveColliders();
    }
  }
}
