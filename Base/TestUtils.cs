using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using System.IO;
using System.Text;

namespace FM.Base
{
    public static class TestUtils
    {
        private static int width;
        private static int height;
        private static Vector2 zoom;
        private static Matrix view;
        private static Matrix projection;
        public static void Reload(this SpriteBatch spriteBatch, SamplerState _samplerState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = _samplerState;
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }

        public static Vector2 GetRotation(List<Vector2> oldPos, int index)
        {
            if (oldPos.Count == 1)
                return oldPos[0];

            if (index == 0)
            {
                return Vector2.Normalize(oldPos[1] - oldPos[0]).RotatedBy(MathHelper.Pi / 2);
            }

            return (index == oldPos.Count - 1
                ? Vector2.Normalize(oldPos[index] - oldPos[index - 1])
                : Vector2.Normalize(oldPos[index + 1] - oldPos[index - 1])).RotatedBy(MathHelper.Pi / 2);
        }
        public static Texture2D GetExtraTexture(string tex, bool altMethod = false)
        {
            if (altMethod)
                return GetTextureAlt("Trails/" + tex);
            return GetTexture("Trails/" + tex);
        }
        public static Texture2D GetTexture(string path)
        {
            return ModContent.Request<Texture2D>("FM/Assets/Textures/" + path).Value;
        }
        public static Texture2D GetTextureAlt(string path)
        {
            return FM.Instance.Assets.Request<Texture2D>(path).Value;
        }
        public static class TRay
        {
            public static Vector2 Cast(Vector2 start, Vector2 direction, float length)
            {
                direction = direction.SafeNormalize(Vector2.UnitY);
                Vector2 output = start;
                for (int i = 0; i < length; i++)
                {
                    if (Collision.CanHitLine(output, 0, 0, output + direction, 0, 0))
                    {
                        output += direction;
                    }
                    else
                    {
                        break;
                    }
                }
                return output;
            }
            public static float CastLength(Vector2 start, Vector2 direction, float length)
            {
                Vector2 end = Cast(start, direction, length);
                return (end - start).Length();
            }
        }
        public static SpriteSortMode previousSortMode;
        public static BlendState previousBlendState;
        public static SamplerState previousSamplerState;
        public static DepthStencilState previousDepthStencilState;
        public static RasterizerState previousRasterizerState;
        public static Effect previousEffect;
        public static Matrix previousMatrix;

        public static void SaveCurrent(this SpriteBatch spriteBatch)
        {
            previousSortMode = SpriteSortMode.Deferred;
            previousBlendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousSamplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousDepthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousRasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousEffect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousMatrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        }

        public static void ApplySaved(this SpriteBatch spriteBatch)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = previousSortMode;
            BlendState blendState = previousBlendState;
            SamplerState samplerState = previousSamplerState;
            DepthStencilState depthStencilState = previousDepthStencilState;
            RasterizerState rasterizerState = previousRasterizerState;
            Effect effect = previousEffect;
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, previousMatrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, SpriteSortMode sortMode = SpriteSortMode.Deferred)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, BlendState blendState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, Effect effect = null)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
    }
}
