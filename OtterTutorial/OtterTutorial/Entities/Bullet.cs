using Otter;

using OtterTutorial;
using OtterTutorial.Effects;

using System;

namespace OtterTutorial.Entities
{
    public class Bullet : Entity
    {
        // Default bullet speed
        public float bulletSpeed = 10.0f;

        // Direction the bullet is going to travel in
        public int direction = 0;

        // Distance the bullet has traveled
        public float distanceTraveled = 0f;

        // Max distance a bullet can travel
        public float maxDistance = 350f;

        // The image object that is our bullet's graphic
        public virtual Image image { get; set; }

        public Sound shootSnd = new Sound(Assets.SND_BULLET_SHOOT);


        public Bullet(float x, float y, int dir)
        {
            // Set the Bullet's X,Y coordinates, and its direction
            X = x;
            Y = y;
            direction = dir;

            // Set the graphic to our bullet image
            image = new Image(Assets.BULLET);
            Graphic = image;

            shootSnd.Play();

            Global.TUTORIAL.Scene.Add(new BulletTrail(X, Y));

            // Add this line to the Bullet.cs class
            // Set the Bullet hitbox to 16x14
            SetHitbox(16, 14, (int) Global.Type.BULLET);
        }


        public override void Update()
        {
            base.Update();

            // Move in the correct direction that the bullet was fired in
            switch (direction)
            {
                case Global.DIR_UP:
                {
                    Y -= bulletSpeed;
                    break;
                }
                case Global.DIR_DOWN:
                {
                    Y += bulletSpeed;
                    break;
                }
                case Global.DIR_LEFT:
                {
                    X -= bulletSpeed;
                    break;
                }
                case Global.DIR_RIGHT:
                {
                    X += bulletSpeed;
                    break;
                }
            }

            if (distanceTraveled % 60 == 0)
            {
                Global.TUTORIAL.Scene.Add(new BulletTrail(X, Y));
            }

            // If we have traveled the max distance or more, then
            // the bullet will remove itself from the current scene
            distanceTraveled += bulletSpeed;
            if (distanceTraveled >= maxDistance)
            {
                Global.TUTORIAL.Scene.Add(new BulletExplosion(X, Y));
                RemoveSelf();
            }
        }


        public void Destroy()
        {
            RemoveSelf();
        }
    }
}
