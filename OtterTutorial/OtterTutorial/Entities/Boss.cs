using Otter;

using OtterTutorial;

using System;


namespace OtterTutorial.Entities
{
    public class Boss : Entity
    {
        // Shoot once every 100 frames
        public const int SHOOT_DURATION = 100;
        // Default health points the boss starts with
        public const int DEFAULT_HEALTH = 15;

        public int health = 1;
        public int shootTimer = 0;
        public Spritemap<string> sprite;
        public Sound hurt = new Sound(Assets.SND_ENEMY_HURT);


        public Boss(float x, float y) : base(x, y)
        {
            health = DEFAULT_HEALTH;

            // Set up the Spritemap in the same manner we did for the player
            sprite = new Spritemap<string>(Assets.BOSS_SPRITE, 32, 40);
            sprite.Add("standLeft", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("standRight", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("standDown", new int[] { 3, 4 }, new int[] { 10, 10 });
            sprite.Add("standUp", new int[] { 6, 7 }, new int[] { 10, 10 });
            sprite.Add("walkLeft", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("walkRight", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("walkDown", new int[] { 3, 4 }, new int[] { 10, 10 });
            sprite.Add("walkUp", new int[] { 6, 7 }, new int[] { 10, 10 });
            sprite.Play("standLeft");
            sprite.Scale = 2; // Our sprite is rather small, so scale it up in size

            Graphic = sprite;

            // Since we scaled up 2x we must make the hitbox 2x big as well
            SetHitbox(64, 80, (int)Global.Type.ENEMY);
        }


        public override void Update()
        {
            base.Update();

            shootTimer++;
            if (shootTimer >= SHOOT_DURATION)
            {
                shootTimer = 0;

                Scene.Add(new BossBullet(X + 16, Y + 20, Global.DIR_UP));
                Scene.Add(new BossBullet(X + 16, Y + 20, Global.DIR_DOWN));
                Scene.Add(new BossBullet(X + 16, Y + 20, Global.DIR_LEFT));
                Scene.Add(new BossBullet(X + 16, Y + 20, Global.DIR_RIGHT));
            }

            // Check if a Player fired Bullet hits us
            var collb = Collider.Collide(X, Y, (int)Global.Type.BULLET);
            if (collb != null)
            {
                Bullet b = (Bullet)collb.Entity;
                b.Destroy();

                Global.camShaker.ShakeCamera();

                // 9999 value is meaningless, and just for fun
                DamageText dt = new DamageText(X, Y, "9999");
                Global.TUTORIAL.Scene.Add(dt);

                hurt.Play();

                health--;

                // If dead add a new explosion
                if (health <= 0)
                {
                    Global.TUTORIAL.Scene.Add(new Explosion(X, Y, true));
                    RemoveSelf();
                }
            }
        }
    }
}
