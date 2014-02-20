using Otter; 

using OtterTutorial.Entities;
using OtterTutorial.Util;

using System;
using System.Text;


namespace OtterTutorial
{
    public class Global
    {
        public static Game TUTORIAL = null;

        public static Session PlayerSession = null;

        public static Player player = null;

        public const int DIR_UP = 0;
        public const int DIR_DOWN = 1;
        public const int DIR_LEFT = 2;
        public const int DIR_RIGHT = 3;

        public static CameraShaker camShaker;

        public enum Type
        {
            BULLET,
            ENEMY,
            BOSS_BULLET,
            PLAYER
        }

        public const int GAME_WIDTH = 640;
        public const int GAME_HEIGHT = 480;
        public const int GRID_WIDTH = 32;
        public const int GRID_HEIGHT = 32;

        public static bool paused = false;
        public static Music gameMusic = null;

        public static Boss boss = null;
    }
}
