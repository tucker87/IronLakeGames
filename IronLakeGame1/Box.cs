using IronLake;
using Microsoft.Xna.Framework;

namespace IronLakeGame1
{
    public class Box : GameObject
    {
        public Movement.Direction XDirection = Movement.Direction.Right;
        public Movement.Direction YDirection = Movement.Direction.Down;
        public int XSpeed = 2;
        public int YSpeed = 3;
        private BoxCollider _boxCollider;
        private SpriteRenderer _spriteRenderer;

        public override void OnActivate()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.OnCollision = OnCollision;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            base.OnActivate();
        }

        public void Move(double elapsedSeconds)
        {
            var newPosition = new Vector2(
                Transform.Position.X + Movement.LambdaMove(XSpeed, XDirection, elapsedSeconds),
                Transform.Position.Y + Movement.LambdaMove(YSpeed, YDirection, elapsedSeconds));

            Transform.Position = newPosition;
        }

        public void CheckOutOfBounds(int viewportWidth, int viewportHeight)
        {
            if (Movement.IsTouchingBounds(Transform.Position.X, _spriteRenderer.Texture2D.Width, viewportWidth))
                XDirection = Movement.Directions[XDirection].Opposite;

            if (Movement.IsTouchingBounds(Transform.Position.Y, _spriteRenderer.Texture2D.Height, viewportHeight))
                YDirection = Movement.Directions[YDirection].Opposite;
        }

        public void OnCollision(BoxCollider collidedWith)
        {
            var (depthX, depthY) = _boxCollider.BoundingBox.GetAbsIntersectionDepth(collidedWith.BoundingBox);

            if (depthX == 0 && depthY == 0) //I know this will be dead on 0
                return;

            if (depthX < depthY)
                XDirection = Movement.Directions[XDirection].Opposite;
            else
                YDirection = Movement.Directions[YDirection].Opposite;
        }

        public override void Update(double elapsedSeconds, (int Width, int Height) viewport)
        {
            CheckOutOfBounds(viewport.Width, viewport.Height);
            Move(elapsedSeconds);
        }
    }
}