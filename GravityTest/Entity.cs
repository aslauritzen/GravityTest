using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GravityTest
{
    public abstract class Entity
    {
        public enum Directions
        {
            Left = -1,
            Right = 1,
            Up = -1,
            Down = 1
        }
        private List<HitBox> _hitBoxes;
        private Texture2D _texture;
        private Vector2 _centerPosition;
        private int _rotationAngle;



        public Entity(Vector2 centerPosition, List<HitBox> hitBoxes, Texture2D texture, int rotationAngle)
        {
            _centerPosition = centerPosition;
            _hitBoxes = hitBoxes;
            _texture = texture;
            _rotationAngle = rotationAngle;
        }

        public int GetEntityCollisionDepth(Entity entity)
        {

            if (CenterPosition.X < entity.CenterPosition.X + entity.Texture.Width &&
                CenterPosition.X + Texture.Width > entity.CenterPosition.X &&
                CenterPosition.Y < entity.CenterPosition.Y + Texture.Height &&
                Texture.Height + CenterPosition.Y > entity.CenterPosition.Y)
            {
                Texture = Texture;
            }

            return 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                new Vector2(CenterPosition.X - Display.HorizontalOffset, CenterPosition.Y - Display.VerticalOffset),
                null,
                Color.White,
                MathUtils.toRadians(RotationAngle),
                new Vector2(Texture.Width / 2, Texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }

        public void Move(Directions direction, float movementDistance)
        {
            Vector2 newCenter = CenterPosition;
            newCenter.X -= movementDistance;
            //handleXAxisCollision(initialX);
            //if (isTimeToScrollScreenX())
            //{
            //    handleScrollScreenX(movementDistance);
            //    playerPosition.X += movementDistance;
            //}
        }

        public static bool isOnScreen(Entity entity)
        {
            int entityWidth = entity.Texture.Width;
            int entityHeight = entity.Texture.Height;
            float entityCenterX = entity.CenterPosition.X;
            float entityCenterY = entity.CenterPosition.Y;

            return (
                entityCenterX - entityWidth < Display.MaxWidth + Display.HorizontalOffset &&
                entityCenterX - entityWidth > Display.HorizontalOffset) ||
                (
                entityCenterY - entityHeight < Display.MaxHeight + Display.VerticalOffset &&
                entityCenterY - entityHeight > Display.VerticalOffset) ||
                (
                entityCenterX + entityWidth > Display.HorizontalOffset &&
                entityCenterX + entityWidth < Display.MaxWidth + Display.HorizontalOffset) ||
                (
                entityCenterY + entityHeight > Display.VerticalOffset &&
                entityCenterY + entityHeight < Display.MaxHeight + Display.VerticalOffset);
        }

        public Vector2 CenterPosition
        {
            get { return _centerPosition; }
            set { _centerPosition = value; }
        }
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        public List<HitBox> HitBoxes
        {
            get { return _hitBoxes; }
            set { _hitBoxes = value; }
        }
        public int RotationAngle
        {
            get { return _rotationAngle; }
            set { _rotationAngle = value; }
        }
    }
}
