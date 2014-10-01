using Otter;

using OtterTutorial;
using OtterTutorial.Entities;

using System;

namespace OtterTutorial.Scenes
{
    public class EndScene : Scene
    {
        public Image endImage = new Image(Assets.END_IMG);
        public Text endText = new Text("You win! You rock! \\:D/", Assets.FONT_PANIC, 64);
        public Text enterText = new Text("Press Enter to go back to title screen!", Assets.FONT_PANIC, 24);

        public const float TIMER_BLINK = 25f;
        public float blinkTimer = 0;

        public Music endMusic = new Music(Assets.MUSIC_END, true);

        public EndScene()
        {
            // If the Player is dead then display Game Over
            // Otherwise, display that they won
            if (Global.player.dead)
            {
                endImage = new Image(Assets.END_GAME_OVER);
                endText = new Text("Oh no! You died! :(", Assets.FONT_PANIC, 60);
                endText.OutlineColor = new Otter.Color("FF0000");
            }
            else
            {
                // Our victory image is a bit big, so scale it down
                endImage.Scale = 0.75f;
                endText.OutlineColor = new Otter.Color("7FA8D2");
            }

            endImage.CenterOrigin();
            endImage.X = Global.TUTORIAL.HalfWidth;
            endImage.Y = 1000;
            this.AddGraphic(endImage);

            Glide.GlideManagerImpl.Tweener.Tween(endImage, new { Y = 245 }, 30f, 0f).Ease(Ease.BackOut);

            endText.OutlineThickness = 3;
            endText.CenterOrigin();
            endText.X = Global.TUTORIAL.HalfWidth;
            endText.Y = 55;
            this.AddGraphic(endText);

            enterText.OutlineColor = new Otter.Color("7FA8D2");
            enterText.OutlineThickness = 2;
            enterText.CenterOrigin();
            enterText.X = Global.TUTORIAL.HalfWidth;
            enterText.Y = 450;
            this.AddGraphic(enterText);

            endMusic.Play();

            // Clean up some Global variable
            Global.player = null;
            Global.boss = new Boss(900, 600);
            Global.camShaker = null;
            Global.gameMusic = null;
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
                endMusic.Stop();

                // Go back to the TitleScene if the player hits Enter
                Global.TUTORIAL.RemoveScene();
                Global.TUTORIAL.AddScene(new TitleScene());
            }
        }
    }
}
