using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GravityTest
{
    class Player : Entity
    {
        public Player(Vector2 centerPosition, List<HitBox> hitBoxes, Texture2D texture) : base(centerPosition, hitBoxes, texture, rotationAngle: 0) { }

    }
}
