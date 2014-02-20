using Otter;

using OtterTutorial;
using OtterTutorial.Effects;
using OtterTutorial.Scenes;

using System;


namespace OtterTutorial.Entities
{
    public class Player : Entity
    {
        public const int WIDTH = 32;
        public const int HEIGHT = 40;
        public const float DIAGONAL_SPEED = 1.4f;

        public float moveSpeed = 4.0f;

        // Our entity's graphic will be a Spritemap
        public Spritemap<string> sprite;

        public int direction = 0;
        public bool dead = false;


        public Player(float x = 0, float y = 0)
        {
            // When creating a new player, the desired X,Y coordinates are passed in. If excluded, we start at 0,0
            X = x;
            Y = y;
            direction = Global.DIR_DOWN; // CASH: ADDED FOR BULLET TRACKING

            // Create a new spritemap, with the player.png image as our source, 32 pixels wide, and 40 pixels tall
            sprite = new Spritemap<string>(Assets.PLAYER, WIDTH, HEIGHT);

            // We must define each animation for our spritemap.
            // An animation is made up of a group of frames, ranging from 1 frame to many frames.
            // Each 32x40 box is a single frame in our particular sprite map. 
            // The animations start counting from 0, and count from left-to-right
            sprite.Add("standLeft", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("standRight", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("standDown", new int[] { 3, 4 }, new int[] { 10, 10 });
            sprite.Add("standUp", new int[] { 6, 7 }, new int[] { 10, 10 });
            sprite.Add("walkLeft", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("walkRight", new int[] { 0, 1 }, new int[] { 10, 10 });
            sprite.Add("walkDown", new int[] { 3, 4 }, new int[] { 10, 10 });
            sprite.Add("walkUp", new int[] { 6, 7 }, new int[] { 10, 10 });

            // Tell the sprite which animation to play when the scene starts
            sprite.Play("standDown");

            // Lastly, we must set our Entity's graphic, otherwise it will not display
            Graphic = sprite; ;

            SetHitbox(32, 40, (int)Global.Type.PLAYER);
        }


        public override void Update()
        {
            base.Update();

            // Used to determine which directions we are moving in
            bool horizontalMovement = true;
            bool verticalMovement = true;

            float xSpeed = 0;
            float ySpeed = 0;
            float newX;
            float newY;
            GameScene checkScene = (GameScene)Scene;

            // Check horizontal movement
            if (Global.PlayerSession.Controller.Left.Down)
            {
                newX = X - moveSpeed;

                if (!checkScene.grid.GetRect(newX, Y, newX + WIDTH, Y + HEIGHT, false))
                {
                    xSpeed = -moveSpeed;
                }

                sprite.FlippedX = true;
                direction = Global.DIR_LEFT;
            }
            else if (Global.PlayerSession.Controller.Right.Down)
            {
                newX = X + moveSpeed;

                if (!checkScene.grid.GetRect(newX, Y, newX + WIDTH, Y + HEIGHT, false))
                {
                    xSpeed = moveSpeed;
                }

                sprite.FlippedX = false;
                direction = Global.DIR_RIGHT;
            }
            else
            {
                horizontalMovement = false;
            }

            // Check vertical movement
            if (Global.PlayerSession.Controller.Up.Down)
            {
                newY = Y - moveSpeed;

                if (!checkScene.grid.GetRect(X, newY, X + WIDTH, newY + HEIGHT, false))
                {
                    ySpeed = -moveSpeed;
                }

                sprite.FlippedX = false;
                direction = Global.DIR_UP;
            }
            else if (Global.PlayerSession.Controller.Down.Down)
            {
                newY = Y + moveSpeed;

                if (!checkScene.grid.GetRect(X, newY, X + WIDTH, newY + HEIGHT, false))
                {
                    ySpeed = moveSpeed;
                }

                sprite.FlippedX = false;
                direction = Global.DIR_DOWN;
            }
            else
            {
                verticalMovement = false;
            }

            // If we are not moving play our idle animations
            // Currently our spritesheet lacks true idle
            // animations, but this helps get the idea across
            if (!horizontalMovement && !verticalMovement)
            {
                if (sprite.CurrentAnim.Equals("walkLeft"))
                {
                    sprite.Play("standLeft");
                }
                else if (sprite.CurrentAnim.Equals("walkRight"))
                {
                    sprite.Play("standRight");
                }
                else if (sprite.CurrentAnim.Equals("walkDown"))
                {
                    sprite.Play("standDown");
                }
                else if (sprite.CurrentAnim.Equals("walkUp"))
                {
                    sprite.Play("standUp");
                }
            }

            if (Global.PlayerSession.Controller.X.Pressed)
            {
                Global.TUTORIAL.Scene.Add(new Bullet(X, Y, direction));
            }

            // Add particles if the player is moving in any direction
            if (verticalMovement || horizontalMovement)
            {
                // Add walking particles
                float particleXBuffer = 0;
                float particleYBuffer = 0;
                switch (direction)
                {
                    case Global.DIR_UP:
                        {
                            particleXBuffer = Otter.Rand.Float(8, 24);
                            particleYBuffer = Otter.Rand.Float(0, 5);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + particleXBuffer, Y + HEIGHT));
                            break;
                        }
                    case Global.DIR_DOWN:
                        {
                            particleXBuffer = Otter.Rand.Float(8, 24);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + particleXBuffer, Y));
                            break;
                        }
                    case Global.DIR_LEFT:
                        {
                            particleYBuffer = Otter.Rand.Float(-2, 2);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + WIDTH - 3, Y + HEIGHT + particleYBuffer));
                            break;
                        }
                    case Global.DIR_RIGHT:
                        {
                            particleYBuffer = Otter.Rand.Float(-2, 2);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + 3, Y + HEIGHT + particleYBuffer));
                            break;
                        }
                }
            }

            if (verticalMovement && horizontalMovement)
            {
                X += xSpeed / DIAGONAL_SPEED;
                Y += ySpeed / DIAGONAL_SPEED;
            }
            else
            {
                X += xSpeed;
                Y += ySpeed;
            }

            var collb = Collider.Collide(X, Y, (int)Global.Type.BOSS_BULLET);
            if (collb != null)
            {
                BossBullet b = (BossBullet) collb.Entity;
                b.RemoveSelf();

                dead = true;
                Global.TUTORIAL.Scene.Add(new Explosion(X, Y, true));
                RemoveSelf();
            }
        }
    }
}
