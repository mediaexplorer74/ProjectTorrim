
// Type: BrawlerSource.GameObjectCollection
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace BrawlerSource
{
  public class GameObjectCollection : Collection<GameObject>
  {
    private IEnumerable<DrawableGameObject> DrawableItems
    {
      get => this.Items.OfType<DrawableGameObject>();
    }

    private IEnumerable<ContentGameObject> ContentItems => this.Items.OfType<ContentGameObject>();

    public void LoadContent()
    {
      foreach (ContentGameObject contentItem in this.ContentItems)
        contentItem.LoadContent();
    }

    public void UnloadContent()
    {
      foreach (ContentGameObject contentItem in this.ContentItems)
        contentItem.UnloadContent();
    }

    public void Update(GameTime gameTime)
    {
      foreach (GameObject gameObject in this.Items.ToArray<GameObject>())
      {
        if (gameObject.IsNew)
          gameObject.FirstUpdate(gameTime);
        gameObject.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (DrawableGameObject drawableItem in this.DrawableItems)
        drawableItem.Draw(spriteBatch);
    }

    public void RemoveAll()
    {
      foreach (GameObject gameObject in this.Items.ToArray<GameObject>())
      {
        this.Remove(gameObject);
        gameObject.Dispose();
      }
    }
  }
}
