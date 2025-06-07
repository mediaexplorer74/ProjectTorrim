
// Type: BrawlerSource.Collections.QuadTree`1
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision.Intersections;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Collections
{
  public class QuadTree<T>
  {
    private int myCapacity;
    private int myDepth;
    private Rectangular myBounds;
    private Rectangular myBoundsTop;
    private Rectangular myBoundsBottom;
    private Rectangular myBoundsLeft;
    private Rectangular myBoundsRight;
    private List<Tuple<IIntersectionable, T>> myData;
    private bool myIsSubdivided;
    private QuadTree<T>[] myQuads;

    public QuadTree(Position position, Position size, int capacity, int depth)
    {
      this.myQuads = new QuadTree<T>[4];
      this.myIsSubdivided = false;
      this.myDepth = depth;
      this.myCapacity = capacity;
      this.myData = new List<Tuple<IIntersectionable, T>>();
      this.SetBounds(position, size);
    }

    private void SetBounds(Position position, Position size)
    {
      this.myBounds = new Rectangular()
      {
        Position = position,
        Dimensions = size
      };
      Position position1 = size * 0.5f;
      Position position2 = position1 * 0.5f;
      this.myBoundsTop = new Rectangular()
      {
        Position = this.myBounds.Centre - new Position(0.0f, position2.Y),
        Dimensions = new Position(size.X, position1.Y)
      };
      this.myBoundsBottom = new Rectangular()
      {
        Position = this.myBounds.Centre + new Position(0.0f, position2.Y),
        Dimensions = new Position(size.X, position1.Y)
      };
      this.myBoundsLeft = new Rectangular()
      {
        Position = this.myBounds.Centre - new Position(position2.X, 0.0f),
        Dimensions = new Position(position1.X, size.Y)
      };
      this.myBoundsRight = new Rectangular()
      {
        Position = this.myBounds.Centre + new Position(position2.X, 0.0f),
        Dimensions = new Position(position1.X, size.Y)
      };
    }

    public void Clear()
    {
      this.myData.Clear();
      if (!this.myIsSubdivided)
        return;
      for (int index = 0; index < this.myQuads.Length; ++index)
      {
        this.myQuads[index].Clear();
        this.myQuads[index] = (QuadTree<T>) null;
      }
      this.myIsSubdivided = false;
    }

        // Fix for CS0246: Replace '__Boxed<T>' with direct usage of 'T' as boxing is unnecessary and '__Boxed<T>' is not defined.

        public bool Insert(IIntersectionable intersection, T data)
        {
            if (!intersection.IsColliding((IIntersectionable)this.myBounds))
                return false;
            if (this.myIsSubdivided)
            {
                int index = this.GetIndex(intersection);
                if (index != -1)
                    return this.myQuads[index].Insert(intersection, data);
            }
            this.InsertData(intersection, data);
            return true;
        }

    private void InsertData(IIntersectionable intersection, T data)
    {
      this.myData.Add(new Tuple<IIntersectionable, T>(intersection, data));
      if (this.myIsSubdivided || this.myData.Count <= this.myCapacity || this.myDepth <= 0)
        return;
      this.Subdivide();
      int index1 = 0;
      while (index1 < this.myData.Count)
      {
        Tuple<IIntersectionable, T> tuple = this.myData[index1];
        int index2 = this.GetIndex(tuple.Item1);
        if (index2 != -1)
        {
          this.myQuads[index2].Insert(tuple.Item1, tuple.Item2);
          this.myData.RemoveAt(index1);
        }
        else
          ++index1;
      }
    }

    private List<Tuple<IIntersectionable, T>> QuerySubQuads(
      IIntersectionable range,
      T data,
      Func<Tuple<IIntersectionable, T>, bool> predicate)
    {
      List<Tuple<IIntersectionable, T>> tupleList = new List<Tuple<IIntersectionable, T>>();
      foreach (QuadTree<T> quad in this.myQuads)
        tupleList.AddRange((IEnumerable<Tuple<IIntersectionable, T>>) quad.Query(range, data, predicate));
      return tupleList;
    }

    private List<Tuple<IIntersectionable, T>> GetQuadData(
      T data,
      Func<Tuple<IIntersectionable, T>, bool> predicate)
    {
      List<Tuple<IIntersectionable, T>> quadData = new List<Tuple<IIntersectionable, T>>();
      foreach (Tuple<IIntersectionable, T> tuple in this.myData)
      {
        if (!tuple.Item2.Equals((object) data) && predicate(tuple))
          quadData.Add(tuple);
      }
      return quadData;
    }

        public List<Tuple<IIntersectionable, T>> Query(
            IIntersectionable range,
            T data,
            Func<Tuple<IIntersectionable, T>, bool> predicate)
        {
            List<Tuple<IIntersectionable, T>> tupleList = new List<Tuple<IIntersectionable, T>>();
            if (!range.IsColliding((IIntersectionable)this.myBounds))
                return tupleList;
            if (this.myIsSubdivided)
                tupleList.AddRange(this.QuerySubQuads(range, data, predicate));
            tupleList.AddRange(this.GetQuadData(data, predicate));
            return tupleList;
        }

        public List<Tuple<IIntersectionable, T>> Quersert(
            IIntersectionable intersection,
            T data,
            Func<Tuple<IIntersectionable, T>, bool> predicate)
        {
            List<Tuple<IIntersectionable, T>> tupleList = new List<Tuple<IIntersectionable, T>>();
            if (!intersection.IsColliding((IIntersectionable)this.myBounds))
                return tupleList;
            int index = -1;
            if (this.myIsSubdivided)
            {
                index = this.GetIndex(intersection);
                if (index != -1)
                    tupleList.AddRange(this.myQuads[index].Quersert(intersection, data, predicate));
                else
                    tupleList.AddRange(this.QuerySubQuads(intersection, data, predicate));
            }
            tupleList.AddRange(this.GetQuadData(data, predicate));
            if (index == -1)
                this.InsertData(intersection, data);
            return tupleList;
        }

    private int GetIndex(IIntersectionable intersection)
    {
      int index = -1;
      bool flag1 = this.myBoundsTop.IsColliding(intersection);
      bool flag2 = this.myBoundsBottom.IsColliding(intersection);
      if (flag1 ^ flag2)
      {
        bool flag3 = this.myBoundsLeft.IsColliding(intersection);
        bool flag4 = this.myBoundsRight.IsColliding(intersection);
        if (flag3 ^ flag4)
          index = flag1 ? (flag3 ? 0 : 1) : (flag3 ? 2 : 3);
      }
      return index;
    }

    private void Subdivide()
    {
      this.myIsSubdivided = true;
      Position size = this.myBounds.Dimensions * 0.5f;
      Position position = size * 0.5f;
      int depth = this.myDepth - 1;
      this.myQuads[0] = new QuadTree<T>(this.myBounds.Centre - position, size, this.myCapacity, depth);
      this.myQuads[1] = new QuadTree<T>(this.myBounds.Centre + new Position(position.X, -position.Y), size, this.myCapacity, depth);
      this.myQuads[2] = new QuadTree<T>(this.myBounds.Centre + new Position(-position.X, position.Y), size, this.myCapacity, depth);
      this.myQuads[3] = new QuadTree<T>(this.myBounds.Centre + position, size, this.myCapacity, depth);
    }
  }
}
