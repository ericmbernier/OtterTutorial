using Otter;

using OtterTutorial;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OtterTutorial.Entities
{
    public class Enemy : Entity
    {
        // Default speed an enemy will move in
        public const float DEFAULT_SPEED = 3.4f;

        // Default health points an enemy starts with
        public const int DEFAULT_HEALTH = 4;

        public const float MOVE_DISTANCE = 300;

        public int health = 1;
        public float speed = 1;

        // left = true, right = false
        public bool direction = true; 
        
        // Used to keep track of the enemy distance moved
        public float distMoved = 0f;

        public Spritemap<string> sprite;

        public Sound hurt = new Sound(Assets.SND_ENEMY_HURT);


        public Enemy(float x, float y) : base(x, y)
        {
            health = DEFAULT_HEALTH;
            speed = DEFAULT_SPEED;

            // Set up the Spritemap in the same manner we did for the player
            sprite = new Spritemap<string>(Assets.ENEMY_SPRITE, 32, 40);
            sprite.Add("standLeft", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("standRight", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("standDown", new int[] { 3, 4 }, new int[] { 10, 10 });
            sprite.Add("standUp", new int[] { 6, 7 }, new int[] { 10, 10 });
            sprite.Add("walkLeft", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("walkRight", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("walkDown", new int[] { 3, 4 }, new int[] { 10, 10 });
            sprite.Add("walkUp", new int[] { 6, 7 }, new int[] { 10, 10 });
            sprite.Play("standLeft");

            Graphic = sprite;

            // Set our hitbox to be 32 x 40. This goes in our Enemy class
            SetHitbox(32, 40, (int)Global.Type.ENEMY);
        }


        public override void Update()
        {
            base.Update();

            var collb = Collider.Collide(X, Y, (int)Global.Type.BULLET);
            if (collb != null)
            {
                Bullet b = (Bullet) collb.Entity;
                b.Destroy();

                Global.camShaker.ShakeCamera();
                DamageText dt = new DamageText(X, Y, "1234");
                Global.TUTORIAL.Scene.Add(dt);

                hurt.Play();

                health--;
                if (health <= 0)
                {
                    Global.TUTORIAL.Scene.Add(new Explosion(X, Y));
                    RemoveSelf();
                }
            }

            sprite.FlippedX = direction;
            // if moving left
            if (direction)
            {
                X -= speed;
            }
            else
            {
                X += speed;
            }

            distMoved += speed;
            if (distMoved >= MOVE_DISTANCE)
            {
                direction = !direction;
                distMoved = 0f;
            }
        }
    }
}
