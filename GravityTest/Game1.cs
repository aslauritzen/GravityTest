using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GravityTest
{
    public class Game1 : Game
    {
        int boulderCount = 8;
        Texture2D ballTexture;
        Vector2 playerPosition;
        List<Vector2> boulderPositions;

        float ballSpeed;
        readonly int maxWidth;
        readonly int maxHeight;
        bool isJumping;
        float jumpSpeed = 0;
        int verticalOffset = 0;
        int horizontalOffset = 0;
        bool isGameOver = false;
        SpriteFont spriteFont;


        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            maxWidth = 500;
            maxHeight = 800;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerPosition = new Vector2(maxWidth / 2, maxHeight);
            ballSpeed = 500f;
            isJumping = false;
            jumpSpeed = 0;
            boulderPositions = new List<Vector2> { };
            Random random = new Random();

            for (int i = 0; i < boulderCount; i++)
            {
                boulderPositions.Add(new Vector2((float)random.NextDouble() * maxWidth, (float)random.NextDouble() * maxHeight * 0.9f));
            }
            _graphics.PreferredBackBufferWidth = maxWidth;
            _graphics.PreferredBackBufferHeight = maxHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
            spriteFont = Content.Load<SpriteFont>("font");
        }

        float getCollisionDepth()
        {
            for (int i = 0; i < boulderPositions.Count; i++)
            {
                Vector2 thingPosition = boulderPositions[i];
                float sidea = Math.Abs(thingPosition.X + horizontalOffset - playerPosition.X);
                float sideb = Math.Abs(thingPosition.Y + verticalOffset - playerPosition.Y);
                sidea *= sidea;
                sideb *= sideb;
                float distance = (float)Math.Sqrt(sidea + sideb);
                if (distance < ballTexture.Width) return ballTexture.Width - distance;
            }

            return 0;
        }

        void keepBallInBoundaries()
        {
            if (playerPosition.X > maxWidth - ballTexture.Width / 2)
            {
                playerPosition.X = maxWidth - ballTexture.Width / 2;
            }
            else if (playerPosition.X < ballTexture.Width / 2)
            {
                playerPosition.X = ballTexture.Width / 2;
            }

            if (playerPosition.Y > maxHeight - ballTexture.Height / 2)
            {
                playerPosition.Y = maxHeight - ballTexture.Height / 2;
            }
            else if (playerPosition.Y < ballTexture.Height / 2)
            {
                playerPosition.Y = ballTexture.Height / 2;
            }
        }

        void handleXAxisCollision(float initialX)
        {
            if (getCollisionDepth() > 0)
            {
                playerPosition.X = initialX;
            }
            else if (!isJumping && playerPosition.Y < maxHeight - ballTexture.Height / 2)
            {
                isJumping = true;
                jumpSpeed = 0;
            }
        }

        void handleIsJumping(float initialY)
        {
            playerPosition.Y += jumpSpeed;
            float collisionDepth = getCollisionDepth();
            if (collisionDepth > 0)
            {
                if (jumpSpeed <= 0)
                {
                    playerPosition.Y = initialY;
                    jumpSpeed = 0;
                }
                else
                {
                    playerPosition.Y -= collisionDepth;
                    isJumping = false;
                }
            }

            if (isJumping)
            {
                if (isTimeToScrollScreenY())
                {
                    handleScrollScreenY();
                    playerPosition.Y = initialY;
                }
                jumpSpeed += 1;
                if (playerPosition.Y >= maxHeight - ballTexture.Height / 2)
                {
                    playerPosition.Y = maxHeight - ballTexture.Height / 2;
                    isJumping = false;
                    jumpSpeed = 0;
                }
            }
        }

        void handleScrollScreenY()
        {
            if (jumpSpeed > 0) return;
            verticalOffset += (int)Math.Abs(jumpSpeed);
            for (int i = 0; i < boulderPositions.Count; i++)
            {
                Vector2 boulderPosition = boulderPositions[i];
                if (boulderPosition.Y + verticalOffset - ballTexture.Height / 2 > maxHeight)
                {
                    boulderPositions[i] = new Vector2(boulderPosition.X, -verticalOffset);
                }

            }
        }

        bool isTimeToScrollScreenY()
        {
            return playerPosition.Y < maxHeight * 0.2f;
        }

        void handleScrollScreenX(float movementDistance)
        {
            horizontalOffset += (int)(playerPosition.X < maxWidth * 0.2f ? movementDistance : -movementDistance);
        }

        bool isTimeToScrollScreenX()
        {
            return playerPosition.X < maxWidth * 0.2f || playerPosition.X > maxWidth * 0.8f;
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                Exit();

            if (isGameOver) return;

            float movementDistance = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float initialX = playerPosition.X;
            float initialY = playerPosition.Y;

            if (keyState.IsKeyDown(Keys.Left))
            {
                playerPosition.X -= movementDistance;
                handleXAxisCollision(initialX);
                if (isTimeToScrollScreenX())
                {
                    handleScrollScreenX(movementDistance);
                    playerPosition.X += movementDistance;
                }
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                playerPosition.X += movementDistance;
                handleXAxisCollision(initialX);
                if (isTimeToScrollScreenX())
                {
                    handleScrollScreenX(movementDistance);
                    playerPosition.X -= movementDistance;
                }
            }

            if (isJumping) handleIsJumping(initialY);
            else
            {
                if (keyState.IsKeyDown(Keys.Space))
                {
                    isJumping = true;
                    jumpSpeed = -20;
                }
            }

            keepBallInBoundaries();

            if (verticalOffset > 0 && playerPosition.Y >= maxHeight - ballTexture.Height / 2)
            {
                isGameOver = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                ballTexture,
                playerPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            for (int i = 0; i < boulderPositions.Count; i++)
            {
                Vector2 boulderPosition = boulderPositions[i];
                _spriteBatch.Draw(
                    ballTexture,
                    new Vector2(boulderPosition.X + horizontalOffset, boulderPosition.Y + verticalOffset),
                    null,
                    Color.White,
                    0f,
                    new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                );
            }
            if (isGameOver)
                _spriteBatch.DrawString(spriteFont, "Game Over", new Vector2(maxWidth / 2, maxHeight / 2), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
