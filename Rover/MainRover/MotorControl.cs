﻿using System;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
using Scarlet.Filters;
using Scarlet.IO.BeagleBone;

namespace MainRover
{
    public static class MotorControl
    {
        //Front: 42 in = 1.067 m
        //Back:  38 in = 0.965 m
        public const int FR = 0; //Front right
        public const int FL = 1; //Front left
        public const int BR = 2; //Back right
        public const int BL = 3; //Back left

        private static VESC[] DriveMotor;
        private static TalonMC SteerMotor;

        private const float RobotLength = 1.0f;
        private const float FrontWidth = 1.067f;
        private const float BackWidth = 0.965f;

        public static void Initialize()
        {
            DriveMotor = new VESC[4];
            for (uint i = 0; i < DriveMotor.Length; i++)
                DriveMotor[i] = new VESC(CANBBB.CANBus0, 1.0f, 60, 518, i + 1, new LowPass<int>());

            SteerMotor = new TalonMC(PWMBBB.PWMDevice0.OutputA, 0.2f, new LowPass<float>());
        }

        public static void SetAllSpeed(float Speed)
        {
            Speed = Math.Min(Speed, 1.0f);
            Speed = Math.Max(Speed, -1.0f);
            SetAllRPM((int)(Speed * 60));
        }

        public static void SetAllRPM(int RPM)
        {
            float Theta = GetRackAndPinionAngle();
            if (Math.Abs(Theta) < 1e-6)
            {
                foreach (VESC M in DriveMotor)
                    M.SetRPM(RPM);
            }
            else
            {
                float R = (float)(RobotLength / (2 * Math.Sin(Theta)));

                float Ratio = R / (R + FrontWidth);
                int OuterRPM = (int)(RPM);
                int InnerRPM = (int)(Ratio * RPM);

                int Outer = Theta < 0.0f ? FR : FL;
                int Inner = Theta < 0.0f ? FL : FR;
                DriveMotor[Outer].SetRPM(OuterRPM);
                DriveMotor[Inner].SetRPM(InnerRPM);

                Ratio = R / (R + BackWidth);
                OuterRPM = (int)(RPM);
                InnerRPM = (int)(Ratio * RPM);

                Outer = Theta < 0.0f ? BR : BL;
                Inner = Theta < 0.0f ? BL : BR;
                DriveMotor[Outer].SetRPM(OuterRPM);
                DriveMotor[Inner].SetRPM(InnerRPM);
            }
        }

        public static void SetRPM(int Motor, int RPM)
        {
            if (Motor == BL)
                RPM = -RPM;
            if (Motor < DriveMotor.Length)
                DriveMotor[Motor].SetRPM(RPM);
        }

        public static void SetSteerSpeed(float Speed)
        {
            SteerMotor.SetSpeed(Speed);
        }

        public static void SetRackAndPinionPosition(float Position)
        {
            //TODO: Use PID for this once we get encoder feedback
        }

        private static float GetRackAndPinionAngle()
        {
            return 0.0f;
        }
    }
}
