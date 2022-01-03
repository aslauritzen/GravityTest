using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GravityTest
{
    class Platform : Entity
    {
        public Platform(Vector2 centerPosition, List<HitBox> hitBoxes, Texture2D texture, int rotationAngle) : base(centerPosition, hitBoxes, texture, rotationAngle) { }
    }
}
