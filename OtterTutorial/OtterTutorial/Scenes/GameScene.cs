using Otter;

using OtterTutorial.Entities;
using OtterTutorial.Util;

using System;
using System.IO;


namespace OtterTutorial.Scenes
{
    public class GameScene : Scene
    {
        public const int WIDTH = Global.GAME_WIDTH * 3;
        public const int HEIGHT = Global.GAME_HEIGHT * 2;

        public Tilemap tilemap = null;
        public GridCollider grid = null;

        // Scene object that will hold the next scene that we transition to
        public Scene nextScene;

        // Use a J,I coordinate system for our map's screens to avoid 
        // confusion with our already existing X,Y coordinate systems
        public int screenJ;
        public int screenI;


        public GameScene(int nextJ = 0, int nextI = 0, Player player = null) : base()
        {
            screenJ = nextJ;
            screenI = nextI;

            // If a Player object isn't passed in, start at the default x,y position of 100,100
            if (player == null)
            {
                Global.player = new Player(100, 100);
            }
            else
            {
                Global.player = player;
            }

            if (Global.boss == null)
            {
                Global.boss = new Boss(900, 600);
            }

            if (Global.camShaker == null)
            {
                Global.camShaker = new CameraShaker();
            }

            // Create and load our Tilemap and GridCollider
            tilemap = new Tilemap(WIDTH, HEIGHT, Global.GRID_WIDTH, Global.GRID_HEIGHT, Assets.TILESET);
            grid = new GridCollider(WIDTH, HEIGHT, Global.GRID_WIDTH, Global.GRID_HEIGHT);

            string mapToLoad = Assets.MAP_WORLD;
            string solidsToLoad = Assets.MAP_SOLID;
            LoadWorld(mapToLoad, solidsToLoad);

            // Since we are constantly switching Scenes we need to do some checking,
            // ensuring that the music doesn't get restarted.
            // We should probably add an isPlaying boolean to the Music class. I will do this soon.
            if (Global.gameMusic == null)
            {
                Global.gameMusic = new Music(Assets.MUSIC_GAME);
                Global.gameMusic.Play();
                Global.gameMusic.Volume = 0.40f;
            }
        }


        // We now add our Entities and Graphics once the Scene has been switched to
        public override void Begin()
        {
            AddGraphic(tilemap);

            // Ensure that the player is not null
            if (Global.player != null)
            {
                Add(Global.player);

                // Never should be paused once transitioning is complete
                Global.paused = false;
            }

            Add(Global.camShaker);

            // This is rather crude, as we re-add the Enemy every time we switch screens
            // A good task beyond these tutorials would be ensuring that non-player
            // Entities retain their state upon switching screens
            Add(new Enemy(500, 400));
            Add(Global.boss);
        }


        public override void Update()
        {
            if (Global.paused)
            {
                return;
            }

            // Check the Player's X,Y position, and determine if we need to move a
            // Scene up, down, left, or right. We also check the current J and I values, 
            // ensuring that we don't move past our actual tileset, into a plain grey screen
            const float HALF_TILE = Global.GRID_WIDTH / 2;
            if (Global.player.X - CameraX < HALF_TILE)
            {
                if (screenJ > 0)
                {
                    if (Global.player.X > 50)
                    {
                        screenJ--;
                        this.Scroll(-1, 0);
                    }
                }
            }

            if (Global.player.Y - CameraY < HALF_TILE)
            {
                if (screenI > 0)
                {
                    if (Global.player.Y > 32)
                    {
                        screenI--;
                        this.Scroll(0, -1);
                    }
                }
            }

            if (Global.player.X - CameraX - Global.GAME_WIDTH > -HALF_TILE)
            {
                if (screenJ < 2)
                {
                    screenJ++;
                    this.Scroll(1, 0);
                }
            }

            if (Global.player.Y - CameraY - Global.GAME_HEIGHT > -HALF_TILE)
            {
                if (screenI < 1)
                {
                    screenI++;
                    this.Scroll(0, 1);
                }
            }
        }


        private void LoadWorld(string map, string solids)
        {
            string newMap = CSVToString(map);
            tilemap.LoadFromCSV(newMap);

            string newSolids = CSVToString(solids);
            grid.LoadFromCSV(newSolids);
        }


        private static string CSVToString(string csvMap)
        {
            string ourMap = "";

            using (var reader = new StreamReader(csvMap))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ourMap += line;
                    ourMap += "\n";
                }
            }

            return ourMap;
        }


        // Scroll method that moves the CameraX, CameraY
        // coordinates by the multiple dx, dy values
        public void Scroll(int dx, int dy)
        {
            // Pause the game when we start scrolling
            Global.paused = true;

            // Set the nextScene and call UpdateLists to 
            // ensure all Entities are cleaned up properly
            nextScene = new GameScene(screenJ, screenI, Global.player);
            nextScene.UpdateLists();

            // Push the player over with the screen via a Tween
            float pushPlayerX = dx * 30;
            float pushPlayerY = dy * 30;
            Glide.Tweener.Tween(Global.player, new
            {
                X = Global.player.X + pushPlayerX,
                Y = Global.player.Y + pushPlayerY
            }, 30f, 0);

            // Finally, push the Camera over by a multiple of the 
            // Game's width and height, and set the call back method
            // to our new ScrollDone method
            Glide.Tweener.Tween(this, new
            {
                CameraX = CameraX + Global.GAME_WIDTH * dx,
                CameraY = CameraY + Global.GAME_HEIGHT * dy
            }, 30f, 0).OnComplete(ScrollDone);
        }


        // Method called once the screen scrolling is all done
        public void ScrollDone()
        {
            // Once the scroll is done remove all added graphics
            // and call UpdateLists to clean everything up and 
            // then switch to the nextScene
            RemoveAll();
            UpdateLists();

            // Set the nextScene's Camera values to the current Scene's 
            // freshly tweened camera values, otherwise we snap back to screen 0,0
            nextScene.CameraX = CameraX;
            nextScene.CameraY = CameraY;
            Global.TUTORIAL.RemoveScene();
            Global.TUTORIAL.AddScene(nextScene);
        }
    }
}
