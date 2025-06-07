
// Type: BrawlerSource.GameObject
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

#nullable disable
namespace BrawlerSource
{
  public class GameObject : IGameComponent, IUpdateable, IComparable<GameObject>
  {
    private List<Type> myTypes;
    private bool myIsDisposed;
    private bool myToDispose;
    public bool IsNew;

    public List<Type> GetTypes()
    {
      if (this.myTypes == null)
      {
        Type type = this.GetType();
        this.myTypes = ((IEnumerable<Type>) Assembly.GetAssembly(type).GetTypes()).Where<Type>((Func<Type, bool>) (assemblyType =>
        {
          if (!assemblyType.IsClass || assemblyType.IsAbstract)
            return false;
          return type.IsSubclassOf(assemblyType) || type == assemblyType;
        })).ToList<Type>();
      }
      return this.myTypes;
    }

    public Random Random { get; private set; }

    public BrawlerGame Game { get; }

    public Level Level { get; }

    public Layer Layer { get; protected set; }

    public GameObject Parent { get; }

    public GameObjectCollection SubGameObjects { get; }

    public GameObject()
    {
    }

    public GameObject(Layer layer)
      : this(layer, (GameObject) null)
    {
    }

    public GameObject(GameObject parent)
      : this((Layer) null, parent)
    {
    }

    private GameObject(Layer layer, GameObject parent)
    {
      this.Layer = layer ?? parent.Layer;
      this.Parent = parent;
      (this.Parent == null ? (Collection<GameObject>) this.Layer.GameObjects : (Collection<GameObject>) this.Parent.SubGameObjects).Add(this);
      this.Level = this.Layer.Level;
      this.Game = this.Level.Game;
      this.SubGameObjects = new GameObjectCollection();
      this.myIsDisposed = false;
      this.myToDispose = false;
      this.IsNew = true;
      this.Random = new Random(this.Layer.Random.Next());
      this.Enabled = true;
    }

    public bool Enabled { get; set; }

    public int UpdateOrder { get; set; }

    public event EventHandler<EventArgs> EnabledChanged;

    public event EventHandler<EventArgs> UpdateOrderChanged;

    public int CompareTo(GameObject other) => 0;

    public virtual void Dispose()
    {
      if (this.myIsDisposed)
        return;
      (this.Parent == null ? (Collection<GameObject>) this.Layer.GameObjects : (Collection<GameObject>) this.Parent.SubGameObjects).Remove(this);
      this.SubGameObjects.RemoveAll();
      this.Layer.GameObjects.Remove(this);
      this.myIsDisposed = true;
    }

    public virtual void QueueDispose() => this.myToDispose = true;

    public virtual void Initialize()
    {
    }

    public virtual void FirstUpdate(GameTime gameTime) => this.IsNew = false;

    public virtual void Update(GameTime gameTime)
    {
      if (this.myToDispose && !this.myIsDisposed)
        this.Dispose();
      this.SubGameObjects.Update(gameTime);
    }
  }
}
