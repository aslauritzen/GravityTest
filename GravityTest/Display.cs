using Microsoft.Xna.Framework.Input;
using System;

namespace GravityTest
{
    class Display
    {
        static int _verticalOffset = 0;
        static int _horizontalOffset = 0;
        static int _maxWidth = 0;
        static int _maxHeight = 0;

        public static void handleScrollScreenY(Player player, int jumpSpeed)
        {
            VerticalOffset += player.CenterPosition.Y < MaxHeight * 0.2f ? -Math.Abs(jumpSpeed) : Math.Abs(jumpSpeed);
        }

        public static bool isTimeToScrollScreenY(Player player, int jumpSpeed)
        {
            if (!player.IsJumping || VerticalOffset <= 0 || player.CenterPosition.Y + VerticalOffset >= LevelData.Height) return false;

            return (player.CenterPosition.Y < MaxHeight * 0.2f && jumpSpeed < 0 && VerticalOffset > 0) ||
                (player.CenterPosition.Y > MaxHeight * 0.8f && jumpSpeed > 0 && MaxHeight + VerticalOffset < LevelData.Height);
        }

        public static void handleScrollScreenX(Player player, float movementDistance)
        {
            HorizontalOffset += (int)(player.CenterPosition.X < MaxWidth * 0.2f ? -movementDistance : movementDistance);
        }

        public static bool isTimeToScrollScreenX(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();
            if ((HorizontalOffset <= 0 && keyState.IsKeyDown(Keys.Left)) ||
                (player.CenterPosition.X + HorizontalOffset >= LevelData.Width && keyState.IsKeyDown(Keys.Right)))
            {
                return false;
            }

            return (player.CenterPosition.X < MaxWidth * 0.2f && keyState.IsKeyDown(Keys.Left)) ||
                (player.CenterPosition.X > MaxWidth * 0.8f && keyState.IsKeyDown(Keys.Right));
        }

        public static int HorizontalOffset { get { return _horizontalOffset; } set { _horizontalOffset = value; } }
        public static int VerticalOffset { get { return _verticalOffset; } set { _verticalOffset = value; } }
        public static int MaxWidth { get { return _maxWidth; } set { _maxWidth = value; } }
        public static int MaxHeight { get { return _maxHeight; } set { _maxHeight = value; } }
    }
}
