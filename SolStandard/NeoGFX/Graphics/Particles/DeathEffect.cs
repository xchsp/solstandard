using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using SolStandard.Map;
using SolStandard.NeoUtility.Monogame.Assets;

namespace SolStandard.NeoGFX.Graphics.Particles
{
    public class DeathEffect : AbstractParticleEffect
    {
        public override ParticleEffect Effect { get; }
        public override Layer Layer => Layer.OverlayEffect;
        public override float Height => GameDriver.CellSizeFloat / 2;

        public DeathEffect()
        {
            Texture2D texture = AssetManager.SkullParticle.MonoGameTexture;
            Effect = GenerateEffect(new TextureRegion2D(texture));
        }

        private ParticleEffect GenerateEffect(TextureRegion2D textureRegion)
        {
            return new ParticleEffect(name: "Death", autoTrigger: false)
            {
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 500, TimeSpan.FromMilliseconds(800),
                        Profile.BoxFill(GameDriver.CellSizeFloat / 2, Height))
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 30f),
                            Quantity = 100,
                            Rotation = new Range<float>(-1f, 1f),
                            Mass = 0.4f,
                            Scale = new Range<float>(0.2f, 0.5f)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ScaleInterpolator
                                    {
                                        StartValue = new Vector2(0.1f),
                                        EndValue = new Vector2(0.7f)
                                    },
                                    new OpacityInterpolator
                                    {
                                        StartValue = 1f,
                                        EndValue = 0f
                                    },
                                    new ColorInterpolator
                                    {
                                        StartValue = Color.Crimson.ToHsl(),
                                        EndValue = Color.White.ToHsl()
                                    }
                                }
                            },
                            new RotationModifier {RotationRate = -2.1f},
                            new RectangleContainerModifier {Width = 800, Height = 480},
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 100f}
                        }
                    }
                }
            };
        }
    }
}