using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        List<Platform> onScreenPlatforms;

        float ballSpeed;
        LevelData levelData;
        string levelJson;
        Dictionary<string, Texture2D> textureMap = new Dictionary<string, Texture2D>();
        public static Texture2D blackBox;


        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _spriteSheet;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Display.MaxWidth = 500;
            Display.MaxHeight = 800;
        }

        protected override void Initialize()
        {
            ballSpeed = 300f;
            platforms = new List<Platform>();
            onScreenPlatforms = new List<Platform>();
            _graphics.PreferredBackBufferWidth = Display.MaxWidth;
            _graphics.PreferredBackBufferHeight = Display.MaxHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            blackBox = Content.Load<Texture2D>("blackBox");

            levelJson = File.ReadAllText("../../../data/level1.json");
            levelData = JsonSerializer.Deserialize<LevelData>(@levelJson);
            textureMap.Add("ball", Content.Load<Texture2D>("ball"));
            _spriteSheet = Content.Load<Texture2D>("newDude");
            Display.VerticalOffset = LevelData.Height - Display.MaxHeight;
            Display.HorizontalOffset = 500;

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
                if (Entity.isOnScreen(platforms[i]))
                {
                    onScreenPlatforms.Add(platforms[i]);
                }
            }

            player = new Player(new Vector2(Display.MaxWidth / 2, Display.MaxHeight), new List<HitBox> { new HitBox(new Vector2(Display.MaxWidth / 2, Display.MaxHeight), 0, 64, 64) }, _spriteSheet);
        }

        protected override void Update(GameTime gameTime)
        {
            //onScreenPlatforms.Clear();
            for (int i = 0; i < platforms.Count; i++)
            {
                //if (Entity.isOnScreen(platforms[i]))
                //{
                List<HitBox> hitBoxes = platforms[i].HitBoxes;
                //onScreenPlatforms.Add(platforms[i]);

                for (int j = 0; j < hitBoxes.Count; j++)
                {
                    hitBoxes[j].Reposition(platforms[i].CenterPosition);
                }
                //}
            }
            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                Exit();

            float movementDistance = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.Move(movementDistance, platforms);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            player.Draw(_spriteBatch);

            for (int i = 0; i < platforms.Count; i++)
            {
                Platform currentPlatform = platforms[i];
                currentPlatform.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
