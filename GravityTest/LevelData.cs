namespace GravityTest
{
    public class LevelData
    {
        public EntityData[] platforms { get; set; }
    }

    public class EntityData
    {
        public string texture { get; set; }
        public int[] centerPosition { get; set; }
        public int rotationAngle { get; set; }
        public HitboxData[] hitBoxes { get; set; }
    }

    public class HitboxData
    {
        public int[] centerPosition { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int rotationAngle { get; set; }
    }
}