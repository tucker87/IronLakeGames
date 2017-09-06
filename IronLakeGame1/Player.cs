using System.Linq;
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
        
        public override void OnActivate()
        {
            _boxCollider = GetComponent<BoxCollider>();
            //_boxCollider.OnCollision = OnCollision;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            base.OnActivate();
        }
        
        public override void Update(double elapsedSeconds)
        {
            Direction = Movement.GetInputDirections();
            TryMove(elapsedSeconds);

            base.Update(elapsedSeconds);
        }

        private void TryMove(double elapsedSeconds)
        {
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

            var newBoundingBox = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                _boxCollider.BoundingBox.Width,
                _boxCollider.BoundingBox.Height);

            var collidingWith = Scene.CheckCollision(newBoundingBox)
                .Where(go => go != this)
                .ToList();

            if(!collidingWith.Any())
                Transform.Position = newPosition;
            else
                OnCollision(collidingWith.First());
        }

        public override void OnCollision(GameObject collidedWith)
        {
            if(Health > 0)
                Health--;
        }
    }
}
