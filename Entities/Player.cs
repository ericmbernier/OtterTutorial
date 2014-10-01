using Otter;

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
            // Create a new spritemap, with the player.png image as our source, 32 pixels wide, and 40 pixels tall
            sprite = new Spritemap<string>(Assets.PLAYER, 32, 40);

            sprite.Add("standLeft", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("standRight", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("standDown", new int[] { 3, 4 }, new float[] { 10f, 10f });
            sprite.Add("standUp", new int[] { 6, 7 }, new float[] { 10f, 10f });
            sprite.Add("walkLeft", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("walkRight", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("walkDown", new int[] { 3, 4 }, new float[] { 10f, 10f });
            sprite.Add("walkUp", new int[] { 6, 7 }, new float[] { 10f, 10f });

            // Tell the spritemap which animation to play when the scene starts
            sprite.Play("standDown");

            // Lastly, we must set our Entity's graphic, otherwise it will not display
            Graphic = sprite;

            SetHitbox(WIDTH, HEIGHT, (int)Global.Type.PLAYER);
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
                // Check if we are colliding with a solid rectangle or not.
                // Ensure the GridCollider snaps our values to a grid, by passing
                // in a false boolean for the usingGrid parameter
                if (!checkScene.grid.GetRect(newX, Y, newX + WIDTH, Y + HEIGHT, false))
                {
                    xSpeed = -moveSpeed;
                }

                direction = Global.DIR_LEFT;
                sprite.FlippedX = true;
            }
            else if (Global.PlayerSession.Controller.Right.Down)
            {
                newX = X + moveSpeed;
                if (!checkScene.grid.GetRect(newX, Y, newX + WIDTH, Y + HEIGHT, false))
                {
                    xSpeed = moveSpeed;
                }

                direction = Global.DIR_RIGHT;
                sprite.FlippedX = false;
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

                direction = Global.DIR_UP;
                sprite.FlippedX = false;
            }
            else if (Global.PlayerSession.Controller.Down.Down)
            {
                newY = Y + moveSpeed;
                if (!checkScene.grid.GetRect(X, newY, X + WIDTH, newY + HEIGHT, false))
                {
                    ySpeed = moveSpeed;
                }

                direction = Global.DIR_DOWN;
                sprite.FlippedX = false;
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
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + particleXBuffer, Y + 40));
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
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + 32 - 3, Y + 40 + particleYBuffer));
                            break;
                        }
                    case Global.DIR_RIGHT:
                        {
                            particleYBuffer = Otter.Rand.Float(-2, 2);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + 3, Y + 40 + particleYBuffer));
                            break;
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
            }

            var collb = Collider.Collide(X, Y, (int)Global.Type.BOSS_BULLET);
            if (collb != null)
            {
                BossBullet b = (BossBullet)collb.Entity;
                b.RemoveSelf();

                dead = true;
                Global.TUTORIAL.Scene.Add(new Explosion(X, Y, true));
                RemoveSelf();
            }
        }
    }
}