using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GravityTest
{
    public class Game1 : Game
    {
        Player player;
        List<Platform> platforms;

        float ballSpeed;
        readonly int maxWidth;
        readonly int maxHeight;
        bool isJumping;
        float jumpSpeed = 0;
        bool isGameOver = false;
        SpriteFont spriteFont;
        LevelData levelData;
        string levelJson;
        Dictionary<string, Texture2D> textureMap = new Dictionary<string, Texture2D>();


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
            ballSpeed = 500f;
            isJumping = false;
            jumpSpeed = 0;
            platforms = new List<Platform>();
            _graphics.PreferredBackBufferWidth = maxWidth;
            _graphics.PreferredBackBufferHeight = maxHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            spriteFont = Content.Load<SpriteFont>("font");
            levelJson = File.ReadAllText("../../../data/level1.json");
            levelData = JsonSerializer.Deserialize<LevelData>(@levelJson);
            textureMap.Add("ball", Content.Load<Texture2D>("ball"));

            for (int i = 0; i < levelData.platforms.Length; i++)
            {
                EntityData currentPlatform = levelData.platforms[i];
                if (!textureMap.ContainsKey(currentPlatform.texture))
                {
                    textureMap.Add(currentPlatform.texture, Content.Load<Texture2D>(currentPlatform.texture));
                }
                platforms.Add(
                    new Platform(
                        new Vector2(currentPlatform.centerPosition[0], currentPlatform.centerPosition[1]),
                        new List<HitBox>(currentPlatform.hitBoxes.Select(
                            hitBox => new HitBox(
                                new Vector2(hitBox.centerPosition[0], hitBox.centerPosition[1]),
                                hitBox.rotationAngle,
                                hitBox.height,
                                hitBox.width)
                            )
                        ),
                        textureMap[currentPlatform.texture],
                        currentPlatform.rotationAngle
                    )
                );
            }

            player = new Player(new Vector2(maxWidth / 2, maxHeight), new List<HitBox> { new HitBox(new Vector2(maxWidth / 2, maxHeight), 0, 64, 64) }, textureMap["ball"]);
        }

        float getCollisionDepth()
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                if (player.CenterPosition.X < platforms[i].CenterPosition.X + platforms[i].Texture.Width &&
                player.CenterPosition.X + player.Texture.Width > platforms[i].CenterPosition.X &&
                player.CenterPosition.Y < platforms[i].CenterPosition.Y + player.Texture.Height &&
                player.Texture.Height + player.CenterPosition.Y > platforms[i].CenterPosition.Y)
                {
                    return 1;
                }
            }

            return 0;
        }

        void keepBallInBoundaries()
        {
            Vector2 currentCenterPosition = player.CenterPosition;

            if (player.CenterPosition.X > maxWidth - player.Texture.Width / 2)
            {
                currentCenterPosition.X = maxWidth - player.Texture.Width / 2;
                player.CenterPosition = currentCenterPosition;
            }
            else if (player.CenterPosition.X < player.Texture.Width / 2)
            {
                currentCenterPosition.X = player.Texture.Width / 2;
                player.CenterPosition = currentCenterPosition;
            }

            if (player.CenterPosition.Y > maxHeight - player.Texture.Height / 2)
            {
                currentCenterPosition.Y = maxHeight - player.Texture.Height / 2;
                player.CenterPosition = currentCenterPosition;
            }
            else if (player.CenterPosition.Y < player.Texture.Height / 2)
            {
                currentCenterPosition.Y = player.Texture.Height / 2;
                player.CenterPosition = currentCenterPosition;
            }
        }

        void handleXAxisCollision(float initialX)
        {
            Vector2 currentCenterPosition = player.CenterPosition;
            if (getCollisionDepth() > 0)
            {
                currentCenterPosition.X = initialX;
                player.CenterPosition = currentCenterPosition;
            }
            else if (!isJumping && player.CenterPosition.Y < maxHeight - player.Texture.Height / 2)
            {
                isJumping = true;
                jumpSpeed = 0;
            }
        }

        void handleIsJumping(float initialY)
        {
            Vector2 currentCenterPosition = player.CenterPosition;
            currentCenterPosition.Y += jumpSpeed;
            player.CenterPosition = currentCenterPosition;
            float collisionDepth = getCollisionDepth();
            if (collisionDepth > 0)
            {
                if (jumpSpeed <= 0)
                {
                    currentCenterPosition.Y = initialY;
                    player.CenterPosition = currentCenterPosition;
                    jumpSpeed = 0;
                }
                else
                {
                    currentCenterPosition.Y -= collisionDepth;
                    player.CenterPosition = currentCenterPosition;
                    isJumping = false;
                }
            }

            if (isJumping)
            {
                if (isTimeToScrollScreenY())
                {
                    handleScrollScreenY();
                    currentCenterPosition.Y = initialY;
                    player.CenterPosition = currentCenterPosition;
                }
                jumpSpeed += 1;
                if (player.CenterPosition.Y >= maxHeight - player.Texture.Height / 2)
                {
                    currentCenterPosition.Y = maxHeight - player.Texture.Height / 2;
                    player.CenterPosition = currentCenterPosition;
                    isJumping = false;
                    jumpSpeed = 0;
                }
            }
        }

        void handleScrollScreenY()
        {
            if (jumpSpeed > 0) return;
            Display.VerticalOffset += (int)Math.Abs(jumpSpeed);
        }

        bool isTimeToScrollScreenY()
        {
            return player.CenterPosition.Y < maxHeight * 0.2f;
        }

        void handleScrollScreenX(float movementDistance)
        {
            Display.HorizontalOffset += (int)(player.CenterPosition.X < maxWidth * 0.2f ? movementDistance : -movementDistance);
        }

        bool isTimeToScrollScreenX()
        {
            return player.CenterPosition.X < maxWidth * 0.2f || player.CenterPosition.X > maxWidth * 0.8f;
        }

        float toRadians(float value)
        {
            return (float)(value * (Math.PI)) / 180;
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                Exit();

            if (isGameOver) return;

            float movementDistance = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float initialX = player.CenterPosition.X;
            float initialY = player.CenterPosition.Y;
            Vector2 currentCenter = player.CenterPosition;

            if (keyState.IsKeyDown(Keys.Left))
            {
                currentCenter.X -= movementDistance;
                player.CenterPosition = currentCenter;
                handleXAxisCollision(initialX);
                if (isTimeToScrollScreenX())
                {
                    handleScrollScreenX(movementDistance);
                    currentCenter.X += movementDistance;
                    player.CenterPosition = currentCenter;
                }
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                currentCenter.X += movementDistance;
                player.CenterPosition = currentCenter;
                handleXAxisCollision(initialX);
                if (isTimeToScrollScreenX())
                {
                    handleScrollScreenX(movementDistance);
                    currentCenter.X -= movementDistance;
                    player.CenterPosition = currentCenter;
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

            if (Display.VerticalOffset > 0 && player.CenterPosition.Y >= maxHeight - player.Texture.Height / 2)
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
                player.Texture,
                player.CenterPosition,
                null,
                Color.White,
                0f,
                new Vector2(player.Texture.Width / 2, player.Texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            for (int i = 0; i < platforms.Count; i++)
            {
                Platform currentPlatform = platforms[i];
                _spriteBatch.Draw(
                    currentPlatform.Texture,
                    new Vector2(currentPlatform.CenterPosition.X + Display.HorizontalOffset, currentPlatform.CenterPosition.Y + Display.VerticalOffset),
                    null,
                    Color.White,
                    toRadians(currentPlatform.RotationAngle),
                    new Vector2(currentPlatform.Texture.Width / 2, currentPlatform.Texture.Height / 2),
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
