// Must include Otter in your project, so we add this line
using Otter;

using OtterTutorial;
using OtterTutorial.Scenes;

using System;
using System.Collections.Generic;
using System.Text;

namespace OtterTutorial // This can be anything you choose, I opted for my project's name
{
    public class Program
    {
        static void Main(string[] args)
        {
            Global.TUTORIAL = new Game("OtterTutorial", 640, 480);
            Global.TUTORIAL.SetWindow(640, 480);

            Global.PlayerSession = Global.TUTORIAL.AddSession("Player");
            Global.PlayerSession.Controller.Start.AddKey(Key.Return);
            Global.PlayerSession.Controller.Up.AddKey(Key.Up);
            Global.PlayerSession.Controller.Left.AddKey(Key.Left);
            Global.PlayerSession.Controller.Down.AddKey(Key.Down);
            Global.PlayerSession.Controller.Right.AddKey(Key.Right);
            Global.PlayerSession.Controller.X.AddKey(Key.X);

            Global.TUTORIAL.FirstScene = new TitleScene();
            Global.TUTORIAL.Start();
        }
    }
}
