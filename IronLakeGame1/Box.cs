using System.Linq;
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

            _spriteRenderer = GetComponent<SpriteRenderer>();

            base.OnActivate();
        }

        public bool TryMove(double elapsedSeconds, out GameObject gameObject, out Rectangle newBoundingBox)
        {
            var currentPosition = Transform.Position;

            var newPosition = new Vector2(
                currentPosition.X + Movement.LambdaMove(XSpeed, XDirection, elapsedSeconds),
                currentPosition.Y + Movement.LambdaMove(YSpeed, YDirection, elapsedSeconds));

            newBoundingBox = new Rectangle(
                (int) newPosition.X, 
                (int) newPosition.Y, 
                _boxCollider.BoundingBox.Width, 
                _boxCollider.BoundingBox.Height);

            var collidingWith = Scene.CheckCollision(newBoundingBox)
                .Where(go => go != this)
                .ToList();

            if (collidingWith.Any())
            {
                gameObject = collidingWith.First();
                return false;
            }

            gameObject = null;

            if (CheckBounds(newBoundingBox, Scene.Game.GraphicsDevice.Viewport.Width, Scene.Game.GraphicsDevice.Viewport.Height))
                return false;
            
            Transform.Position = newPosition;
            return true;
        }

        public bool CheckBounds(Rectangle boundingBox, int viewportWidth, int viewportHeight)
        {
            return Movement.IsTouchingBounds(boundingBox.X, boundingBox.Width, viewportWidth)
                   || Movement.IsTouchingBounds(boundingBox.Y, boundingBox.Height, viewportHeight);
        }

        public void OnCollision(BoxCollider collidedWith, Rectangle boundingBox)
        {
            if (collidedWith != null)
            {
                var (depthX, depthY) = boundingBox.GetAbsIntersectionDepth(collidedWith.BoundingBox);

                if (depthX == 0 && depthY == 0) //I know this will be dead on 0
                    return;

                if (depthX < depthY)
                    XDirection = Movement.Directions[XDirection].Opposite;
                else
                    YDirection = Movement.Directions[YDirection].Opposite;
            }
            else
            {
                if(Movement.IsTouchingBounds(boundingBox.X, boundingBox.Width, Scene.Game.GraphicsDevice.Viewport.Width))
                    XDirection = Movement.Directions[XDirection].Opposite;

                if (Movement.IsTouchingBounds(boundingBox.Y, boundingBox.Height, Scene.Game.GraphicsDevice.Viewport.Height))
                    YDirection = Movement.Directions[YDirection].Opposite;
            }
        }

        public override void Update(double elapsedSeconds)
        {
            if (TryMove(elapsedSeconds, out var collidedWith, out var newBoundingBox))
                return;

            collidedWith?.OnCollision(collidedWith);
            OnCollision(collidedWith?.GetComponent<BoxCollider>(), newBoundingBox);
            TryMove(elapsedSeconds, out var _, out var _);
        }
    }
}