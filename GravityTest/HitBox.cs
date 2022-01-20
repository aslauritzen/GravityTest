using Microsoft.Xna.Framework;
using System;

namespace GravityTest
{
    public class HitBox
    {
        private int _rotationAngle;
        private int _height;
        private int _width;
        private Vector2 _centerPosition;
        private Vector2 _topLeftCorner;
        private Vector2 _topRightCorner;
        private Vector2 _bottomLeftCorner;
        private Vector2 _bottomRightCorner;

        public HitBox(Vector2 centerPosition, int rotationAngle, int height, int width)
        {
            _rotationAngle = rotationAngle;
            _height = height;
            _width = width;
            _centerPosition = centerPosition;
            _topLeftCorner = new Vector2(centerPosition.X - width / 2, centerPosition.Y + height / 2);
            _topRightCorner = new Vector2(centerPosition.X + width / 2, centerPosition.Y + height / 2);
            _bottomLeftCorner = new Vector2(centerPosition.X - width / 2, centerPosition.Y - height / 2);
            _bottomRightCorner = new Vector2(centerPosition.X + width / 2, centerPosition.Y - height / 2);
            if (rotationAngle != 0) RotateHitbox();
        }

        public void Reposition(Vector2 centerPosition)
        {
            _centerPosition = new Vector2(centerPosition.X - Display.HorizontalOffset, centerPosition.Y - Display.VerticalOffset);
            _topLeftCorner = new Vector2((centerPosition.X - Width / 2) - Display.HorizontalOffset, (centerPosition.Y + Height / 2) - Display.VerticalOffset);
            _topRightCorner = new Vector2((centerPosition.X + Width / 2) - Display.HorizontalOffset, (centerPosition.Y + Height / 2) - Display.VerticalOffset);
            _bottomLeftCorner = new Vector2((centerPosition.X - Width / 2) - Display.HorizontalOffset, (centerPosition.Y - Height / 2) - Display.VerticalOffset);
            _bottomRightCorner = new Vector2((centerPosition.X + Width / 2) - Display.HorizontalOffset, (centerPosition.Y - Height / 2) - Display.VerticalOffset);
            if (_rotationAngle != 0) RotateHitbox();
        }

        private void RotateHitbox()
        {
            _topLeftCorner = getRotatedCornerPosition(_topLeftCorner);
            _topRightCorner = getRotatedCornerPosition(_topRightCorner);
            _bottomLeftCorner = getRotatedCornerPosition(_bottomLeftCorner);
            _bottomRightCorner = getRotatedCornerPosition(_bottomRightCorner);
        }

        private Vector2 getRotatedCornerPosition(Vector2 corner)
        {
            float cosVal = (float)Math.Cos(MathUtils.toRadians(_rotationAngle));
            float sinVal = (float)Math.Sin(MathUtils.toRadians(_rotationAngle));
            float tempX = corner.X - CenterPosition.X;
            float tempY = corner.Y - CenterPosition.Y;

            float rotatedX = tempX * cosVal - tempY * sinVal;
            float rotatedY = tempX * sinVal + tempY * cosVal;

            return new Vector2(rotatedX + CenterPosition.X, rotatedY + CenterPosition.Y);
        }

        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        public int RotationAngle
        {
            get { return _rotationAngle; }
            set { _rotationAngle = value; }
        }
        public Vector2 CenterPosition
        {
            get { return _centerPosition; }
            set
            {
                Vector2 difference = _centerPosition - value;
                _centerPosition -= difference;
                TopLeftCorner -= difference;
                TopRightCorner -= difference;
                BottomLeftCorner -= difference;
                BottomRightCorner -= difference;
            }
        }
        public Vector2 TopLeftCorner
        {
            get { return _topLeftCorner; }
            set { _topLeftCorner = value; }
        }
        public Vector2 TopRightCorner
        {
            get { return _topRightCorner; }
            set { _topRightCorner = value; }
        }
        public Vector2 BottomLeftCorner
        {
            get { return _bottomLeftCorner; }
            set { _bottomLeftCorner = value; }
        }
        public Vector2 BottomRightCorner
        {
            get { return _bottomRightCorner; }
            set { _bottomRightCorner = value; }
        }
    }
}
