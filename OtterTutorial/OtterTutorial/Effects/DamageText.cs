using Otter;

using OtterTutorial;

using System;


namespace OtterTutorial.Entities
{
    public class DamageText : Entity
    {
        // Random jitter so that each DamageText object 
        // doesn't appear in the exact same spot
        public const int MIN_X_JITTER = 0;
        public const int MAX_X_JITTER = 30;

        public Text text;

        public DamageText(float x, float y, string dmgText, int fontSize = 16)
        {
            text = new Text(dmgText, fontSize);
            text.Color = Color.Red;

            // Move the X value a bit for each new DamageText so they
            // all don't appear in the same spot if the enemy is idle
            X = x + Otter.Rand.Int(MIN_X_JITTER, MAX_X_JITTER);
            Y = y - 20;

            Graphic = text;
        }


        public override void Update()
        {
            base.Update();

            // Slowly subtract from our alpha value
            text.Alpha -= 0.02f;
            Y -= 1.25f;

            // Remove once invisible
            if (text.Alpha <= 0)
            {
                RemoveSelf();
            }
        }
    }
}
