namespace GravityTest
{
    class Display
    {
        static int _verticalOffset = 0;
        static int _horizontalOffset = 0;

        public static int HorizontalOffset { get { return _horizontalOffset; } set { _horizontalOffset = value; } }
        public static int VerticalOffset { get { return _verticalOffset; } set { _verticalOffset = value; } }
    }
}
