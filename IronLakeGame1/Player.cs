using IronLake;
using Microsoft.Xna.Framework;

namespace IronLakeGame1
{
    public class Player : GameObject
    {
        public int Health = 10;
        public Movement.Direction Direction = Movement.Direction.None;
        public int XSpeed = 2;
        public int YSpeed = 3;
        private BoxCollider _boxCollider;
        private SpriteRenderer _spriteRenderer;
        private bool _colliding;

        public override void OnActivate()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.OnCollision = OnCollision;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            base.OnActivate();
        }
        
        public override void Update(double elapsedSeconds, (int Width, int Height) viewport)
        {
            Direction = Movement.GetInputDirections();

            Move(elapsedSeconds);

            base.Update(elapsedSeconds, viewport);
        }

        private void Move(double elapsedSeconds)
        {
            if (_colliding)
            {
                _colliding = false;
                return;
            }

            var x = Transform.Position.X;
            var xDirection = Direction.HasFlag(Movement.Direction.Left)
                ? Movement.Direction.Left
                : Direction.HasFlag(Movement.Direction.Right)
                    ? Movement.Direction.Right
                    : Movement.Direction.None;

            var y = Transform.Position.Y;
            var yDirection = Direction.HasFlag(Movement.Direction.Up)
                ? Movement.Direction.Up
                : Direction.HasFlag(Movement.Direction.Down)
                    ? Movement.Direction.Down
                    : Movement.Direction.None;

            if (xDirection != Movement.Direction.None)
                x += Movement.LambdaMove(XSpeed, xDirection, elapsedSeconds);

            if (yDirection != Movement.Direction.None)
                y += Movement.LambdaMove(XSpeed, yDirection, elapsedSeconds);
            
            var newPosition = new Vector2(x, y);

            Transform.Position = newPosition;
        }

        public void OnCollision(BoxCollider collidedWith)
        {
            _colliding = true;
        }
    }
}
