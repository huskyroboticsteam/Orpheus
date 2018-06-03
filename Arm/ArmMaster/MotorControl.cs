using System;
using Scarlet.Filters;
using Scarlet.IO.BeagleBone;
using Scarlet.Components.Motors;
using Scarlet.IO;

namespace ArmMaster
{
    static class MotorControl
    {
        static CytronMD30C MotBaseRotation;
        static TalonMC MotShoulder;
        static TalonMC MotElbow;

        public const int BaseRotation = 0;
        public const int Shoulder = 1;
        public const int Elbow = 2;

        public static void Initialize()
        {
            MotBaseRotation = new CytronMD30C(PWMBBB.PWMDevice2.OutputA, new DigitalOutBBB(Pins.BaseRotationDir), 0.3f, new LowPass<float>());
            MotShoulder = new TalonMC(PWMBBB.PWMDevice1.OutputA, 0.6f, new LowPass<float>());
            MotElbow = new TalonMC(PWMBBB.PWMDevice1.OutputB, 0.3f, new LowPass<float>());
        }

        public static void SetMotorSpeed(int Motor, float Speed)
        {
            switch (Motor)
            {
                case BaseRotation:
                    SetBaseSpeed(Speed);
                    break;
                case Shoulder:
                    SetShoulderSpeed(Speed);
                    break;
                case Elbow:
                    SetElbowSpeed(Speed);
                    break;
            }
        }

        public static void SetAllMotorSpeed(float Speed)
        {
            for (int i = 0; i < 2; i++)
                SetMotorSpeed(i, Speed);
        }

        public static void SetBaseSpeed(float Speed)
        {
            MotBaseRotation.SetSpeed(Speed);
        }

        public static void SetShoulderSpeed(float Speed)
        {
            MotShoulder.SetSpeed(Speed);
        }

        public static void SetElbowSpeed(float Speed)
        {
            MotElbow.SetSpeed(Speed);
        }

        public static void EmergencyStop()
        {
            MotBaseRotation.SetSpeed(0.0f);
            MotShoulder.SetSpeed(0.0f);
            MotElbow.SetSpeed(0.0f);
        }
    }
}
