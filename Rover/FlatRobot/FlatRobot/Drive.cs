using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet;
using Scarlet.Utilities;
using Scarlet.Communications;
using Scarlet.Controllers;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using OpenTK.Input;
using Scarlet.Components.Motors;
using Scarlet.Components;
namespace FlatRobot
{
    class Drive
    {
        bool FourWheel; //True if 4 Motor controllers used for Drive, False if 2
        IMotor[] Motors = new IMotor[4];
        /// <summary>
        /// Constructs a drive object with two motor controllers.
        /// </summary>
        /// <param name="LeftMC">Left Motor Controller</param>
        /// <param name="RightMC">Right Motor Controller</param>
        public Drive(IMotor LeftMC, IMotor RightMC)
        {
            Motors[0] = LeftMC;
            Motors[1] = RightMC;
            FourWheel = false;
        }
        /// <summary>
        /// Constructs a drive object with four motor controllers.
        /// </summary>
        /// <param name="FrontLeftMC">Front Left Motor Controller</param>
        /// <param name="FrontRightMC">Front Right Motor Controller</param>
        /// <param name="BackLeftMC">Back Left Motor Controller</param>
        /// <param name="BackRightMC">Back Right Motor Controller</param>
        public Drive(IMotor FrontLeftMC, IMotor FrontRightMC, IMotor BackLeftMC, IMotor BackRightMC)
        {
            Motors[0] = FrontLeftMC;
            Motors[1] = FrontRightMC;
            Motors[2] = BackLeftMC;
            Motors[3] = BackRightMC;
            FourWheel = true;
        }
        /// <summary>
        /// Drives in the direction of the given x,y vector components.
        /// </summary>
        /// <param name="x">X component (-1 to 1)</param>
        /// <param name="y">Y component (-1 to 1)</param>
        public void Move(float x, float y)
        {
            //algorithms to convert cartesian coordinate components to drive inputs.
            float LeftSpeed = (float) (y + x);  //(float)((y / Math.Abs(y)) * (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) - (x + Math.Abs(x) / 2)));
            float RightSpeed = (float) (y - x);   //(float)((y / Math.Abs(y)) * (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) - (x - Math.Abs(x) / 2)));
            Motors[0].SetSpeed(LeftSpeed);
            Motors[1].SetSpeed(RightSpeed);
            if (FourWheel)
            {
                Motors[2].SetSpeed(LeftSpeed);
                Motors[3].SetSpeed(RightSpeed);
            }
        }
        /// <summary>
        /// Gives direct input values to a two motor controller drive.
        /// </summary>
        /// <param name="LeftInput">Input for left motor controller.</param>
        /// <param name="RightInput">Input for right motor controller.</param>
        public void DirectInput(float LeftInput, float RightInput)
        {
            Motors[0].SetSpeed(LeftInput);
            Motors[1].SetSpeed(RightInput);
        }
        /// <summary>
        /// Gives direct input values to a four motor controller drive.
        /// </summary>
        /// <param name="FrontLeftInput">Input for front left motor controller.</param>
        /// <param name="FrontRightInput">Input for front right motor controller.</param>
        /// <param name="BackLeftInput">Input for back left motor controller.</param>
        /// <param name="BackRightInput">Input for back right motor controller.</param>
        public void DirectInput(float FrontLeftInput, float FrontRightInput, float BackLeftInput, float BackRightInput)
        {
            Motors[0].SetSpeed(FrontLeftInput);
            Motors[1].SetSpeed(FrontRightInput);
            Motors[2].SetSpeed(BackLeftInput);
            Motors[3].SetSpeed(BackRightInput);
        }
    }
}