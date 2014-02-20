using Otter;

using OtterTutorial;

using System;


namespace OtterTutorial.Effects
{
    // BulletParticle extends the Entity class
    public class BulletParticle : Entity
    {
        // Our bullet particle graphics contain multiple frames. Spritemap makes sense here
        public Spritemap<string> sprite;

        // Once the animation hits this frame, remove our self from the Scene
        public int destroyFrame = 1;

        public BulletParticle(float x, float y) : base(x, y)
        {

        }


        public override void Update()
        {
            base.Update();

            // Have our particle drift up a bit as it dissolves
            Y -= (float)(1.5 / Otter.Rand.Float(1, 3));

            // Check if we have finished playing. If so, remove self
            if (sprite.CurrentFrame == destroyFrame)
            {
                RemoveSelf();
            }
        }
    }
}
