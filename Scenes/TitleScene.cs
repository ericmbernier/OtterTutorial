using Otter;

using OtterTutorial;

using System;
using System.Collections.Generic;
using System.Text;

namespace OtterTutorial.Scenes
{
    public class TitleScene : Scene
    {
        // Create a new Image object, referencing the Otter image in our Assets folder
        public Image titleImage = new Image(Assets.TITLE_IMG);

        public Text titleText = new Text("Otter Tutorial", Assets.FONT_PANIC, 84);
        public Text enterText = new Text("Press Enter", Assets.FONT_PANIC, 40);

        public const float TIMER_BLINK = 25f;
        public float blinkTimer = 0;

        // Create a new, looping sound object, with our MUSIC_TITLE as its source
        public Music titleSong = new Music(Assets.MUSIC_TITLE, true);

        // We will tween this image in when the Scene switches
        public Image darkScreen = Image.CreateRectangle(Global.GAME_WIDTH, Global.GAME_HEIGHT, new Otter.Color("000000"));

        public TitleScene()
        {
            // Center the title picture 
            titleImage.CenterOrigin();
            titleImage.X = Global.TUTORIAL.HalfWidth;
            titleImage.Y = 1000; // When tweening something in, make sure it is actually off the screen first
            this.AddGraphic(titleImage);

            // Otter utilizes the C# Tweening library called Glide
            // More info can be found here: http://www.reddit.com/r/gamedev/comments/1fabdh/
            // Below, we tween the titleImage to its new Y position, in .30 seconds. Our Easing function utilized is Ease.BackOut
            Glide.GlideManagerImpl.Tweener.Tween(titleImage, new { Y = 250 }, 30f, 0f).Ease(Ease.BackOut);

            // Set the text's outline color to the 
            // hex color #7FA8D2 (Otter2d.com Blue)
            titleText.OutlineColor = new Otter.Color("7FA8D2");
            titleText.OutlineThickness = 3; // Set the outline thickness to 3 pixels
            titleText.CenterOrigin();
            titleText.X = Global.TUTORIAL.HalfWidth;
            titleText.Y = 25;
            this.AddGraphic(titleText);

            enterText.OutlineColor = new Otter.Color("7FA8D2");
            enterText.OutlineThickness = 2;
            enterText.CenterOrigin();
            enterText.X = Global.TUTORIAL.HalfWidth;
            enterText.Y = 450;
            this.AddGraphic(enterText);

            titleSong.Play();

            darkScreen.Alpha = 0;
            this.AddGraphic(darkScreen);
        }

        public override void Update()
        {
            base.Update();

            blinkTimer++;
            if (blinkTimer >= TIMER_BLINK)
            {
                enterText.Visible = !enterText.Visible;
                blinkTimer = 0;
            }

            if (Global.PlayerSession.Controller.Start.Pressed)
            {
                // Before we jumped right to the GameScene. Now we fade the TitleScene out
                if (Global.PlayerSession.Controller.Start.Pressed)
                {
                    Glide.GlideManagerImpl.Tweener.Tween(darkScreen, new { Alpha = 1 }, 30f, 0).OnComplete(PlayGame);
                }
            }
        }

        private void PlayGame()
        {
            titleSong.Stop();
            Global.TUTORIAL.RemoveScene();
            Global.TUTORIAL.AddScene(new GameScene());
        }
    }
}