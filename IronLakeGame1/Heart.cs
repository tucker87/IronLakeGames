using System.Collections.Generic;
using System.Linq;
using IronLake;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronLakeGame1
{
    public class HeartContainer : UiElement
    {
        private Player _player;

        public HeartContainer(Texture2D heartTexture)
        {
            HeartTexture = heartTexture;
        }

        public Texture2D HeartTexture { get; set; }

        public override void OnActivate()
        {
            _player = Scene.GameObjects.OfType<Player>().First();
        }

        public override void Update(double elapsedSeconds)
        {
            if (_player.Health == Children.Count)
                return;

            var x = 0;
            const int margin = 20;

            Children.Clear();
            Children.AddRange(Enumerable.Range(1, _player.Health)
                .Select(i => new Heart()
                    .SetPosition(new Vector2(x += margin, Transform.Position.Y))
                    .AddComponent(new SpriteRenderer(HeartTexture))));
        }
    }

    internal class Heart : UiElement
    {
        public Rectangle Rectangle { get; set; }
    }
}
