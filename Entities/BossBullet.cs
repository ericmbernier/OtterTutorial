using Otter;

using OtterTutorial;
using OtterTutorial.Effects;

using System;

namespace OtterTutorial.Entities
{
    public class BossBullet : Entity
    {
        public float bulletSpeed = 10.0f;
        public int direction = 0;
        public float distanceTraveled = 0f;
        public float maxDistance = 350f;
        public Image image;
        public Sound shootSnd = new Sound(Assets.SND_BOSS_SHOOT);

        public BossBullet(float x, float y, int dir)
        {
            X = x;
            Y = y;
            direction = dir;

            image = new Image(Assets.BOSS_BULLET);
            Graphic = image;

            Global.TUTORIAL.Scene.Add(new BossTrail(X, Y));
            SetHitbox(16, 14, (int)Global.Type.BOSS_BULLET);

            shootSnd.Play();
        }

        public override void Update()
        {
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
                Global.TUTORIAL.Scene.Add(new BossTrail(X, Y));
            }

            distanceTraveled += bulletSpeed;
            if (distanceTraveled >= maxDistance)
            {
                RemoveSelf();
            }
        }
    }
}