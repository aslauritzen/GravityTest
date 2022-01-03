using Microsoft.Xna.Framework;

namespace GravityTest
{
    public class HitBox
    {
        private Vector2 _centerPosition;
        private int _rotationAngle;
        private int _height;
        private int _width;

        public HitBox(Vector2 centerPosition, int rotationAngle, int height, int width)
        {
            _centerPosition = centerPosition;
            _rotationAngle = rotationAngle;
            _height = height;
            _width = width;
        }

        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        public Vector2 CenterPosition
        {
            get { return _centerPosition; }
            set { _centerPosition = value; }
        }
        public int RotationAngle
        {
            get { return _rotationAngle; }
            set { _rotationAngle = value; }
        }
    }
}
