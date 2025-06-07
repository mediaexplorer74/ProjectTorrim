
// Type: BrawlerSource.Collision.Collider
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision.Intersections;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace BrawlerSource.Collision
{
  public class Collider : GameObject
  {
    public LayerType CollisionLayer;
    private Position myPosition;
    public bool HasChanged;
    public Dictionary<IIntersectionable, Position> IntersectionOffsets;
    private Dictionary<Type, Dictionary<TouchState, CollisionFunc>> Events;
    private bool myIsDisabled;
    private HashSet<Collider> myCollisions;
    private HashSet<Collider> myPreviousCollisions;

    public Position Position
    {
      get => this.myPosition;
      set
      {
        if (!(value != this.myPosition))
          return;
        this.myPosition = value;
        if (this.IntersectionOffsets == null)
          return;
        foreach (IIntersectionable key in this.IntersectionOffsets.Keys)
          key.SetPosition(this.myPosition + this.IntersectionOffsets[key]);
        this.HasChanged = true;
      }
    }

    public bool IsDisabled
    {
      get => this.myIsDisabled;
      set
      {
        this.myIsDisabled = value;
        this.HasChanged = true;
      }
    }

    public Collider(GameObject parent)
      : base(parent)
    {
      this.CollisionLayer = LayerType.None;
      this.Position = new Position();
      this.Events = new Dictionary<Type, Dictionary<TouchState, CollisionFunc>>();
      this.IntersectionOffsets = new Dictionary<IIntersectionable, Position>();
      this.IsDisabled = false;
      this.myCollisions = new HashSet<Collider>();
      this.myPreviousCollisions = new HashSet<Collider>();
    }

    public void AddIntersection(IIntersectionable i)
    {
      Position position = new Position();
      if (i.GetPosition() == (Position) null)
        i.SetPosition(this.myPosition);
      else
        position = this.myPosition - i.GetPosition();
      this.IntersectionOffsets.Add(i, position);
      this.HasChanged = true;
    }

    public void RemoveIntersection(IIntersectionable i)
    {
      this.IntersectionOffsets.Remove(i);
      this.HasChanged = true;
    }

    public void AddEvent(Type target, TouchType touchType, CollisionFunc action)
    {
      Dictionary<TouchState, CollisionFunc> dictionary;
      if (!this.Events.TryGetValue(target, out dictionary))
        this.Events.Add(target, dictionary = new Dictionary<TouchState, CollisionFunc>());
      TouchState key = TouchState.None;
      if (touchType == TouchType.Touching || touchType == TouchType.TouchStart)
        key |= TouchState.IsTouching;
      if (touchType == TouchType.Touching || touchType == TouchType.TouchEnd)
        key |= TouchState.WasTouching;
      dictionary.Add(key, action);
    }

    public Tuple<Position, Position> GetAreaPositions()
    {
      Position position1 = new Position(this.Position.X, this.Position.Y);
      Position position2 = new Position(this.Position.X, this.Position.Y);
      foreach (IIntersectionable key in this.IntersectionOffsets.Keys)
      {
        Tuple<Position, Position> areaPositions = key.GetAreaPositions();
        if ((double) areaPositions.Item1.X < (double) position1.X)
          position1.X = areaPositions.Item1.X;
        if ((double) areaPositions.Item1.Y < (double) position1.Y)
          position1.Y = areaPositions.Item1.Y;
        if ((double) areaPositions.Item2.Y > (double) position2.Y)
          position2.Y = areaPositions.Item2.Y;
        if ((double) areaPositions.Item2.X > (double) position2.X)
          position2.X = areaPositions.Item2.X;
      }
      return new Tuple<Position, Position>(position1, position2);
    }

    public bool IsColliding(Collider collider)
    {
      return !this.IsDisabled && !collider.IsDisabled && this.IntersectionOffsets.Keys.Any<IIntersectionable>((Func<IIntersectionable, bool>) (first => collider.IntersectionOffsets.Keys.Any<IIntersectionable>((Func<IIntersectionable, bool>) (second => first.IsColliding(second)))));
    }

    public void ClearCollisions()
    {
      this.RemoveCollisions();
      this.myPreviousCollisions = this.myCollisions;
      this.myCollisions = new HashSet<Collider>();
    }

    public void RegisterCollision(Collider collider, bool isColliding)
    {
      if (isColliding)
        this.myCollisions.Add(collider);
      else
        this.myCollisions.Remove(collider);
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      this.Layer.CollisionMap.Add(this);
      base.FirstUpdate(gameTime);
    }

    public void Collide(GameTime gameTime)
    {
      this.HasChanged = false;
      foreach (Type key1 in this.Events.Keys)
      {
        Dictionary<TouchState, CollisionFunc> dictionary = this.Events[key1];
        HashSet<Collider> colliders = this.Layer.CollisionMap.GetColliders(this.CollisionLayer, key1);
        if (colliders != null)
        {
          foreach (Collider collider in colliders)
          {
            TouchState key2 = TouchState.None;
            if (this.myCollisions.Contains(collider))
              key2 |= TouchState.IsTouching;
            if (this.myPreviousCollisions.Contains(collider))
              key2 |= TouchState.WasTouching;
            CollisionFunc collisionFunc1;
            if (dictionary.TryGetValue(key2, out collisionFunc1))
            {
              CollisionFunc collisionFunc2 = collisionFunc1;
              CollisionEventArgs e = new CollisionEventArgs();
              e.GameTime = gameTime;
              e.Trigger = collider.Parent;
              e.Type = (TouchType) key2;
              e.Collider = collider;
              collisionFunc2((object) this, e);
            }
          }
        }
      }
      this.myPreviousCollisions = new HashSet<Collider>((IEnumerable<Collider>) this.myCollisions);
    }

    public override void Dispose()
    {
      this.Layer.CollisionMap.Remove(this);
      this.RemoveCollisions();
      base.Dispose();
    }

    public void RemoveCollisions()
    {
      foreach (Collider collision in this.myCollisions)
        collision.RegisterCollision(this, false);
    }
  }
}
