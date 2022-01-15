using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GravityTest
{
    class Player : Entity
    {
        bool isJumping;
        int jumpSpeed = 0;
        bool isSpaceDown = false;
        bool isLeftDown = false;
        bool isRightDown = false;

        public Player(Vector2 centerPosition, List<HitBox> hitBoxes, Texture2D texture) : base(centerPosition, hitBoxes, texture, rotationAngle: 0) { }

        public void Move(float movementDistance, List<Platform> platforms)
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector2 initialCenter = CenterPosition;
            Vector2 currentCenter = CenterPosition;

            isSpaceDown = keyState.IsKeyDown(Keys.Space);
            isLeftDown = keyState.IsKeyDown(Keys.Left);
            isRightDown = keyState.IsKeyDown(Keys.Right);

            if (isLeftDown)
            {
                currentCenter.X -= movementDistance;
                CenterPosition = currentCenter;

                if (Display.isTimeToScrollScreenX(this))
                {
                    Display.handleScrollScreenX(this, movementDistance);
                    currentCenter.X += movementDistance;
                    CenterPosition = currentCenter;
                }
            }

            if (isRightDown)
            {
                currentCenter.X += movementDistance;
                CenterPosition = currentCenter;
                if (Display.isTimeToScrollScreenX(this))
                {
                    Display.handleScrollScreenX(this, movementDistance);
                    currentCenter.X -= movementDistance;
                    CenterPosition = currentCenter;
                }
            }

            if (isJumping) handleIsJumping(initialCenter.Y);
            else
            {
                if (isSpaceDown)
                {
                    isJumping = true;
                    jumpSpeed = -20;
                }
            }

            KeepInBoundaries();
            MoveHitbox();
            HandleCollisions(platforms);
        }

        void HandleCollisions(List<Platform> platforms)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                CollisionUtils.PolygonCollisionResult pCR = CollisionUtils.TestForCollision(HitBoxes[0], platforms[i].HitBoxes[0]);
                if (pCR.isIntersecting)
                {
                    Vector2 offsetVector = new Vector2(pCR.resetVector.X * pCR.resetDistance, pCR.resetVector.Y * pCR.resetDistance); ;
                    CenterPosition += offsetVector;
                    HitBox first = HitBoxes[0];
                    first.CenterPosition += offsetVector;
                    HitBoxes[0] = first;
                    if (!isSpaceDown) isJumping = false;
                }
                else
                {
                    if ((isRightDown || isLeftDown) && !isJumping && CenterPosition.Y < Display.MaxHeight - Texture.Height / 2)
                    {
                        isJumping = true;
                        jumpSpeed = 0;
                    }
                }
            }
        }

        void MoveHitbox()
        {
            int width = Texture.Width;
            int height = Texture.Height;

            for (int i = 0; i < HitBoxes.Count; i++)
            {
                HitBox currentHitbox = HitBoxes[i];
                currentHitbox.TopLeftCorner = new Vector2(CenterPosition.X - width / 2, CenterPosition.Y + height / 2);
                currentHitbox.TopRightCorner = new Vector2(CenterPosition.X + width / 2, CenterPosition.Y + height / 2);
                currentHitbox.BottomLeftCorner = new Vector2(CenterPosition.X - width / 2, CenterPosition.Y - height / 2);
                currentHitbox.BottomRightCorner = new Vector2(CenterPosition.X + width / 2, CenterPosition.Y - height / 2);
                currentHitbox.CenterPosition = new Vector2(CenterPosition.X, CenterPosition.Y);
            }
        }

        void KeepInBoundaries()
        {
            Vector2 currentCenterPosition = CenterPosition;

            if (CenterPosition.X > Display.MaxWidth - Texture.Width / 2)
            {
                currentCenterPosition.X = Display.MaxWidth - Texture.Width / 2;
                CenterPosition = currentCenterPosition;
            }
            else if (CenterPosition.X < Texture.Width / 2)
            {
                currentCenterPosition.X = Texture.Width / 2;
                CenterPosition = currentCenterPosition;
            }

            if (CenterPosition.Y > Display.MaxHeight - Texture.Height / 2)
            {
                currentCenterPosition.Y = Display.MaxHeight - Texture.Height / 2;
                CenterPosition = currentCenterPosition;
            }
            else if (CenterPosition.Y < Texture.Height / 2)
            {
                currentCenterPosition.Y = Texture.Height / 2;
                CenterPosition = currentCenterPosition;
            }
        }

        void handleIsJumping(float initialY)
        {
            Vector2 currentCenterPosition = CenterPosition;
            currentCenterPosition.Y += jumpSpeed;
            CenterPosition = currentCenterPosition;

            if (isJumping)
            {
                if (Display.isTimeToScrollScreenY(this))
                {
                    Display.handleScrollScreenY(jumpSpeed);
                    currentCenterPosition.Y = initialY;
                    CenterPosition = currentCenterPosition;
                }
                jumpSpeed += 1;
                if (CenterPosition.Y >= Display.MaxHeight - Texture.Height / 2)
                {
                    currentCenterPosition.Y = Display.MaxHeight - Texture.Height / 2;
                    CenterPosition = currentCenterPosition;
                    isJumping = false;
                    jumpSpeed = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                CenterPosition,
                null,
                Color.White,
                0f,
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
            spriteBatch.Draw(
                Game1.blackBox,
                new Vector2(HitBoxes[0].CenterPosition.X, HitBoxes[0].CenterPosition.Y),
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
