using System;
using System.Diagnostics;
using System.Drawing;

namespace HuskyRobotics.Arm
{
    //Base 6.8 in            -4pi to 4pi (yaw) Stow: 0
    //First Joint 28.0 in    -76 degrees to 100 degrees (Pitch) Stow: -76
    //2nd Joint 28.0 in      11.59 degrees to 170 degrees (Pitch) Stow: 11.59
    //Hand 12.75 in          -90 degrees to 90 degrees (Pitch) Stow: 0
    //                       -2pi to 2pi (Roll) Stow: 0


    /*
     * Basic arm construction:
     * Arm a = new Arm((0, 0, -1.5f, 1.5f, 0, 0, 3), (0.78f, 3), (0, 0, -0.5f, 0.5f, 0, 0, 3), (0, 3));
     * Creates an arm with 4 parts.
     * First part: Cannot roll and cannot yaw. Goes between about -Pi / 2 and Pi / 2 in pitch. Length 3.
     * Second part: Cannot roll and cannot yaw. Fixed at about Pi / 4 to the horizontal. Length 3.
     * Third part: Cannot roll and cannot yaw. Goes between about -Pi / 6 and Pi / 6 in pitch. Length 3.
     * Fourth part: Cannot roll and cannot yaw. Fixed parallel to the horizontal. Length 3. 
     * 
     * To move the arm somewhere. call MoveTo(X, Y, Z).
     * To see what arm segment i should be turned to, check CurrentRolls[i], CurrentPitches[i],
     * and CurrentYaws[i].
     */

    public struct ArmPart
    {
        /*
         * I'm breaking conventions here a bit:
         * The front of the rover is along the X Axis
         * The Y Axis is vertical, parallel to gravity
         * The Z Axis is the cross product of the X and Y Axis
         * 
         * Here is how I'm defining the angles to be:
         * Roll - Rotation about the X Axis
         * Pitch - Rotation about the Z Axis
         * Yaw - Rotation about the Y Axis
         * This is contrary to norm. Usually Yaw is about the Z Axis
         * and the Z Axis is parallel to gravity. Not here!
         * 
         * Construction:
         * - Either provide MinRoll, MaxRoll, MinPitch, MaxPitch,
         *   MinYaw, MaxYaw, and Length
         * - Or provide Fixed Pitch angle and Length, in which case 
         *   the arm will ensure that that ArmPart will always be at
         *   that angle with respect to the ground. 
         */

        public float MinRoll { get; private set; }
        public float MaxRoll { get; private set; }
        public float MinPitch { get; private set; }
        public float MaxPitch { get; private set; }
        public float MinYaw { get; private set; }
        public float MaxYaw { get; private set; }
        public float Length { get; private set; }

        public bool Fixed;
        public float FixedAngle;

        public ArmPart(float MinRoll, float MaxRoll,
                       float MinPitch, float MaxPitch,
                       float MinYaw, float MaxYaw,
                       float L)
        {
            this.MinRoll = MinRoll;
            this.MaxRoll = MaxRoll;
            this.MinPitch = MinPitch;
            this.MaxPitch = MaxPitch;
            this.MinYaw = MinYaw;
            this.MaxYaw = MaxYaw;
            Length = L;
            Fixed = false;
            FixedAngle = 0.0f;
        }

        // FixedAngle makes Yaw constant
        public ArmPart(float FixedAngle, float Length, float MinPitch = 0.0f, float MaxPitch = 2.0f * (float)Math.PI)
        {
            this.MinRoll = 0.0f;
            this.MaxRoll = 0.0f;
            this.MinPitch = MinPitch;
            this.MaxPitch = MaxPitch;
            this.MinYaw = 0.0f;
            this.MaxYaw = 0.0f;
            this.FixedAngle = FixedAngle;
            this.Fixed = true;
            this.Length = Length;
        }

        public static implicit operator ArmPart((int, int) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2);
        }

        public static implicit operator ArmPart((float, float) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2);
        }

        public static implicit operator ArmPart((double, double) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2);
        }

        public static implicit operator ArmPart((int, int, int, int) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2, (float)t.Item3, (float)t.Item4);
        }

        public static implicit operator ArmPart((float, float, float, float) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2, (float)t.Item3, (float)t.Item4);
        }

        public static implicit operator ArmPart((double, double, double, double) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2, (float)t.Item3, (float)t.Item4);
        }

        public static implicit operator ArmPart((int, int, int, int, int, int, int) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2, (float)t.Item3, (float)t.Item4, (float)t.Item5,
                             (float)t.Item6, (float)t.Item7);
        }

        public static implicit operator ArmPart((double, double, double, double, double, double, double) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2, (float)t.Item3, (float)t.Item4, (float)t.Item5,
                  (float)t.Item6, (float)t.Item7);
        }

        public static implicit operator ArmPart((float, float, float, float, float, float, float) t)
        {
            return new ArmPart((float)t.Item1, (float)t.Item2, (float)t.Item3, (float)t.Item4, (float)t.Item5,
                  (float)t.Item6, (float)t.Item7);
        }
    }

    public class Armature
    {
        public float[] CurrentRolls { get; private set; }
        public float[] CurrentPitches { get; private set; }
        public float[] CurrentYaws { get; private set; }

        public ArmPart[] Params { get; private set; }

        private float[] TempRolls;
        private float[] TempPitches;
        private float[] TempYaws;

        private float[] RollSums;
        private float[] PitchSums;
        private float[] YawSums;

        //Takes an array of ArmParts (which can be conveniently constructed
        //using tuples) and initializes the Arm. Following this constructor,
        //CurrentPitches, CurrentRolls, and CurrentYaws will be initialized
        //to random values close to 0. Randomization is needed because of 
        //everything is 0, gradients will be 0 and nothing will be updated.
        //This is a common technique in machine learning, particularly 
        //neural networks.
        public Armature(params ArmPart[] Parameters)
        {
            this.Params = Parameters;
            this.CurrentRolls = new float[Parameters.Length];
            this.CurrentPitches = new float[Parameters.Length];
            this.CurrentYaws = new float[Parameters.Length];

            Random r = new Random();
            for (int i = 0; i < Params.Length; i++)
            {
                CurrentRolls[i] = (float)r.NextDouble() * 0.00001f;
                CurrentPitches[i] = (float)r.NextDouble() * 0.00001f;
                CurrentYaws[i] = (float)r.NextDouble() * 0.00001f;
            }

            this.RollSums = new float[Parameters.Length];
            this.PitchSums = new float[Parameters.Length];
            this.YawSums = new float[Parameters.Length];

            this.TempRolls = new float[Parameters.Length];
            this.TempPitches = new float[Parameters.Length];
            this.TempYaws = new float[Parameters.Length];
        }


        //Calculates the current position of the endpoint of the arm. Fills in
        //the angle sum arrays which will be used for calculating gradient. 
        private (float X, float Y, float Z) CalculatePosition()
        {
            float X = 0.0f;
            float Y = 0.0f;
            float Z = 0.0f;
            float PitchSum = 0.0f;
            float RollSum = 0.0f;
            float YawSum = 0.0f;
            for (int i = 0; i < CurrentPitches.Length; i++)
            {
                RollSum += CurrentRolls[i];
                PitchSum += CurrentPitches[i];
                YawSum += CurrentYaws[i];

                RollSums[i] = RollSum;
                PitchSums[i] = PitchSum;
                YawSums[i] = YawSum;

                X += (float)(Params[i].Length * Math.Cos(PitchSum) * Math.Cos(YawSum));
                Y += (float)(Params[i].Length * Math.Cos(RollSum) * Math.Sin(PitchSum));
                Z += (float)(Params[i].Length * Math.Sin(RollSum) * Math.Sin(YawSum));
            }
            return (X, Y, Z);
        }

        //Takes the target position, the current position, and the arm part
        //whose gradient should be calculated. 
        private (float, float, float) CalcGrad(float TX, float TY, float TZ, float X, float Y, float Z, int i)
        {
            float RollY = 0.0f;
            float RollZ = 0.0f;
            float PitchX = 0.0f;
            float PitchY = 0.0f;
            float YawX = 0.0f;
            float YawZ = 0.0f;

            for (int a = i; a < Params.Length; a++)
            {
                RollZ += (float)(Params[a].Length * Math.Cos(RollSums[a]) * Math.Sin(YawSums[a]));
                RollY += (float)(Params[a].Length * Math.Sin(RollSums[a]) * Math.Sin(PitchSums[a]));

                PitchX += (float)(Params[a].Length * Math.Sin(PitchSums[a]) * Math.Cos(YawSums[a]));
                PitchY += (float)(Params[a].Length * Math.Cos(PitchSums[a]) * Math.Cos(RollSums[a]));

                YawX += (float)(Params[a].Length * Math.Sin(YawSums[a]) * Math.Cos(PitchSums[a]));
                YawZ += (float)(Params[a].Length * Math.Cos(YawSums[a]) * Math.Sin(RollSums[a]));
            }
            return (2 * RollZ * (TZ - Z) - 2 * RollY * (TY - Y),
                    2 * PitchX * (TX - X) - 2 * PitchY * (TY - Y),
                    2 * YawX * (TX - X) - 2 * YawZ * (TZ - Z));
        }

        //Takes an angle in radians and ensures it is between
        //0 and 2 * PI
        private static float EnsureStandardForm(float Angle)
        {
            return Angle % (float)(2 * Math.PI);
        }

        //Moves the arm to the target X, Y, and Z. The angles of each
        //arm part will be stored in CurrentRoll, CurrentPitch, and 
        //CurrentYaw. 
        public void MoveTo(float X, float Y, float Z)
        {
            const float LearningRate = 0.000005f;
            const int Iterations = 400;

            for (int i = 0; i < Iterations; i++)
            {
                for (int j = 0; j < Params.Length; j++)
                {
                    if (Params[j].Fixed)
                    {
                        var (CurX, CurY, CurZ) = CalculatePosition();
                        float PrevSum = j > 0 ? PitchSums[j - 1] : 0;
                        CurrentPitches[j] = EnsureStandardForm((Params[j].FixedAngle - PrevSum));
                        CurrentPitches[j] = Math.Max(Math.Min(CurrentPitches[j], Params[j].MaxPitch), Params[j].MinPitch);
                    }
                    else
                    {
                        var (CurX, CurY, CurZ) = CalculatePosition();
                        //float Cost = (CurX - X) * (CurX - X) + (CurY - Y) * (CurY - Y) + (CurZ - Z) * (CurZ - Z);
                        var (GradRoll, GradPitch, GradYaw) = CalcGrad(X, Y, Z, CurX, CurY, CurZ, j);


                        GradRoll *= LearningRate;
                        GradPitch *= LearningRate;
                        GradYaw *= LearningRate;

                        TempRolls[j] = CurrentRolls[j] - GradRoll;
                        TempPitches[j] = CurrentPitches[j] - GradPitch;
                        TempYaws[j] = CurrentYaws[j] - GradYaw;

                        float[] tmp = CurrentRolls;
                        CurrentRolls = TempRolls;
                        TempRolls = tmp;

                        tmp = CurrentPitches;
                        CurrentPitches = TempPitches;
                        TempPitches = tmp;

                        tmp = CurrentYaws;
                        CurrentYaws = TempYaws;
                        TempYaws = tmp;
                    }
                    CurrentRolls[j] = Math.Max(Math.Min(CurrentRolls[j], Params[j].MaxRoll), Params[j].MinRoll);
                    CurrentPitches[j] = Math.Max(Math.Min(CurrentPitches[j], Params[j].MaxPitch), Params[j].MinPitch);
                    CurrentYaws[j] = Math.Max(Math.Min(CurrentYaws[j], Params[j].MaxYaw), Params[j].MinYaw);
                }
            }

            //for (int j = 0; j < Params.Length; j++)
            //    Console.WriteLine($"{CurrentRolls[j]}, {CurrentPitches[j]}, {CurrentYaws[j]}");
            //Console.WriteLine();
        }
    }
}
