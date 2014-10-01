using Otter;

using OtterTutorial;

using System;

namespace OtterTutorial.Util
{
    public class CameraShaker : Entity
    {
        // Variables that will store our camera X,Y coordinates before shaking
        private float priorCameraX = 0f;
        private float priorCameraY = 0f;

        // Variable used to keep track of how long we have been shaking for
        private float shakeTimer = 0f;
        // Number of frames to shake the camera for. Gets set in constructor
        private float shakeFrames = 0f;
        // Bool used to determine if the camera needs shaking or not
        private bool shakeCamera = false;

        // Default constructor
        public CameraShaker()
        {
        }

        public void ShakeCamera(float shakeDur = 20f)
        {
            // If camera isn't already shaking
            if (!shakeCamera)
            {
                // Save our original X,Y values
                priorCameraX = this.Scene.CameraX;
                priorCameraY = this.Scene.CameraY;

                // Set shakeCamera to true, and our shake duration
                shakeCamera = true;
                shakeFrames = shakeDur;
            }
        }

        public override void Update()
        {
            if (shakeCamera)
            {
                // Move the Camera X,Y values a random, but controlled amount
                this.Scene.CameraX = priorCameraX + (10 - 6 * 2 * Rand.Float(0, 1));
                this.Scene.CameraY = priorCameraY + (10 - 6 * 2 * Rand.Float(0, 1));
                // Increase the shake timer by one frame
                // and check if we have been shaking long enough
                shakeTimer++;
                if (shakeTimer >= shakeFrames)
                {
                    shakeCamera = false;
                    shakeTimer = 0;
                    shakeFrames = 0;

                    this.Scene.CameraX = priorCameraX;
                    this.Scene.CameraY = priorCameraY;
                }
            }
        }
    }
}