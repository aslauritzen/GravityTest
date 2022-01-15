using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GravityTest
{
    class Platform : Entity
    {
        public Platform(Vector2 centerPosition, List<HitBox> hitBoxes, Texture2D texture, int rotationAngle) : base(centerPosition, hitBoxes, texture, rotationAngle) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                new Vector2(CenterPosition.X + Display.HorizontalOffset, CenterPosition.Y + Display.VerticalOffset),
                null,
                Color.White,
                MathUtils.toRadians(RotationAngle),
                new Vector2(Texture.Width / 2, Texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(
                Game1.blackBox,
                new Vector2(HitBoxes[0].TopLeftCorner.X, HitBoxes[0].TopLeftCorner.Y),
                null,
                Color.White,
                0f,
                new Vector2(2, 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(
                Game1.blackBox,
                new Vector2(HitBoxes[0].TopRightCorner.X, HitBoxes[0].TopRightCorner.Y),
                null,
                Color.White,
                0f,
                new Vector2(2, 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(
                Game1.blackBox,
                new Vector2(HitBoxes[0].BottomLeftCorner.X, HitBoxes[0].BottomLeftCorner.Y),
                null,
                Color.White,
                0f,
                new Vector2(2, 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            spriteBatch.Draw(
                Game1.blackBox,
                new Vector2(HitBoxes[0].BottomRightCorner.X, HitBoxes[0].BottomRightCorner.Y),
                null,
                Color.White,
                0f,
                new Vector2(2, 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }
    }
}
