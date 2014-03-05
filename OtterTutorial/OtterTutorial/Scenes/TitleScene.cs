using OtterTutorial;

using Otter;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtterTutorial.Scenes
{
    public class TitleScene : Scene
    {
        public Image titleImage = new Image(Assets.TITLE_IMG);
        public Text titleText = new Text("Otter Tutorial", Assets.FONT_PANIC, 84);
        public Text enterText = new Text("Press Enter", Assets.FONT_PANIC, 40);

        public const float TIMER_BLINK = 25f;
        public float blinkTimer = 0;

        public Music titleSong = new Music(Assets.MUSIC_TITLE, true);

        public Image darkScreen = Image.CreateRectangle(Global.GAME_WIDTH, Global.GAME_HEIGHT, new Otter.Color("000000"));


        public TitleScene()
        {
            // Center the title picture 
            titleImage.CenterOrigin();
            titleImage.X = Global.TUTORIAL.HalfWidth;
            titleImage.Y = 1000;
            this.AddGraphic(titleImage);

            Glide.Tweener.Tween(titleImage, new { Y = 245 }, 30f, 0f).Ease(Ease.BackOut);

            titleText.OutlineColor = new Otter.Color("7FA8D2");
            titleText.OutlineThickness = 3;
            titleText.CenterOrigin();
            titleText.X = Global.TUTORIAL.HalfWidth;
            titleText.Y = 55;
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
                Glide.Tweener.Tween(darkScreen, new { Alpha = 1 },  30f, 0).OnComplete(PlayGame);
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
