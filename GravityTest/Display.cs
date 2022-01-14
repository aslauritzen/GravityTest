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

        public static void handleScrollScreenY(int jumpSpeed)
        {
            if (jumpSpeed > 0) return;
            Display.VerticalOffset += (int)Math.Abs(jumpSpeed);
        }

        public static bool isTimeToScrollScreenY(Player player)
        {
            if (Display.VerticalOffset <= 0) return false;
            return player.CenterPosition.Y < Display.MaxHeight * 0.2f;
        }

        public static void handleScrollScreenX(Player player, float movementDistance)
        {
            Display.HorizontalOffset += (int)(player.CenterPosition.X < Display.MaxWidth * 0.2f ? movementDistance : -movementDistance);
        }

        public static bool isTimeToScrollScreenX(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (Display.HorizontalOffset >= 0 && keyState.IsKeyDown(Keys.Left)) return false;
            return (player.CenterPosition.X < Display.MaxWidth * 0.2f && keyState.IsKeyDown(Keys.Left)) || (player.CenterPosition.X > Display.MaxWidth * 0.8f && keyState.IsKeyDown(Keys.Right));
        }

        public static int HorizontalOffset { get { return _horizontalOffset; } set { _horizontalOffset = value; } }
        public static int VerticalOffset { get { return _verticalOffset; } set { _verticalOffset = value; } }
        public static int MaxWidth { get { return _maxWidth; } set { _maxWidth = value; } }
        public static int MaxHeight { get { return _maxHeight; } set { _maxHeight = value; } }
    }
}
