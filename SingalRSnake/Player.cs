using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TuckerGameToolbox;

namespace SingalRSnake
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

        public Dictionary<Movement.Direction, Enum[]> InputMap = new Dictionary<Movement.Direction, Enum[]>
        {
            {Movement.Direction.Up, new Enum[] {Buttons.DPadUp, Keys.W, Keys.Up}},
            {Movement.Direction.Right, new Enum[] {Buttons.DPadRight, Keys.D, Keys.Right}},
            {Movement.Direction.Down, new Enum[] {Buttons.DPadDown, Keys.S, Keys.Down}},
            {Movement.Direction.Left, new Enum[] {Buttons.DPadLeft, Keys.A, Keys.Left}}
        };
        
        public override void Update(double elapsedSeconds, (int Width, int Height) viewport)
        {
            Direction = Movement.Direction.None;

            foreach (var direction in EnumExtensions.GetValues<Movement.Direction>().Where(d => d > Movement.Direction.None))
                if (GetInput(InputMap[direction]))
                    Direction = Direction | direction;

            Move(elapsedSeconds);

            base.Update(elapsedSeconds, viewport);
        }

        private static bool GetInput(IEnumerable<Enum> inputs)
        {
            return inputs.Any(i =>
            {
                switch (i)
                {
                    case Buttons b:
                        return GamePad.GetState(PlayerIndex.One).IsButtonDown(b);
                    case Keys k:
                        return Keyboard.GetState().IsKeyDown(k);
                    default:
                        return false;
                }
            });
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
