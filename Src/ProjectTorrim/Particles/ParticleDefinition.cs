
// Type: BrawlerSource.Particles.ParticleDefinition
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Particles
{
  public struct ParticleDefinition
  {
    public Position VelocityMin;
    public Position VelocityMax;
    public float RotationMin;
    public float? RotationMax;
    public float RotationVelocityMin;
    public float? RotationVelocityMax;
    public Vector2 ScaleMin;
    public Vector2? ScaleMax;
    public Vector2 ScaleVelocityMin;
    public Vector2? ScaleVelocityMax;
    public TimeSpan LifespanMin;
    public TimeSpan? LifespanMax;
    public Color Colour;
    public Color ColourChange;
  }
}
