
// Type: BrawlerSource.ContentGameObject
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

#nullable disable
namespace BrawlerSource
{
  public class ContentGameObject : GameObject
  {
    public bool IsLoaded { get; private set; }

    public ContentGameObject()
    {
    }

    public ContentGameObject(Layer layer)
      : base(layer)
    {
    }

    public ContentGameObject(GameObject parent)
      : base(parent)
    {
    }

    public virtual void LoadContent()
    {
      this.SubGameObjects.LoadContent();
      this.IsLoaded = true;
    }

    public virtual void UnloadContent()
    {
      this.SubGameObjects.UnloadContent();
      this.IsLoaded = false;
    }
  }
}
