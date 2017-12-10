using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace UseArm
{
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

        public float MinRoll;
        public float MaxRoll;
        public float MinPitch;
        public float MaxPitch;
        public float MinYaw;
        public float MaxYaw;
        public float Length;

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

        public ArmPart(float FixedAngle, float Length)
        {
            this.MinRoll = 0.0f;
            this.MaxRoll = 0.0f;
            this.MinPitch = 0.0f;
            this.MaxPitch = (float)Math.PI * 2.0f;
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

    public class Arm
    {
        ArmPart[] Params;
        public float[] CurrentPitches { get; private set; }
        public float[] CurrentRolls { get; private set; }
        public float[] CurrentYaws { get; private set; }

        private float[] TempPitches;
        private float[] TempRolls;
        private float[] TempYaws;

        private float[] PitchSums;
        private float[] RollSums;
        private float[] YawSums;

        private (float, float) TransformPoint(float X, float Y)
        {
            const float SCALE = 10.0f;
            const float CX = 100.0f;
            const float CY = 100.0f;
            return (X * SCALE + CX, 300 - (Y * SCALE + CY));
        }

        //Draw is 2D and uses only Pitch
        public void Draw(Graphics g)
        {
            float PrevX = 0;
            float PrevY = 0;
            float X = 0;
            float Y = 0;
            float AngleSum = 0.0f;
            for (int i = 0; i < CurrentPitches.Length; i++)
            {
                AngleSum += CurrentPitches[i];
                X += (float)(Params[i].Length * Math.Cos(AngleSum));
                Y += (float)(Params[i].Length * Math.Sin(AngleSum));
                var Trans1 = TransformPoint(PrevX, PrevY);
                g.FillEllipse(Brushes.DarkRed, Trans1.Item1 - 3, Trans1.Item2 - 3, 6, 6);
                var Trans2 = TransformPoint(X, Y);
                g.DrawLine(Pens.Red, Trans1.Item1, Trans1.Item2, Trans2.Item1, Trans2.Item2);
                PrevX = X;
                PrevY = Y;
            }
            var Trans = TransformPoint(PrevX, PrevY);
            g.FillEllipse(Brushes.DarkRed, Trans.Item1 - 3, Trans.Item2 - 3, 6, 6);
        }

        private (float X, float Y, float Z) ForwardKinematics()
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

        private static float EnsureStandardForm(float Angle)
        {
            while (Angle < 0)
                Angle += (float)(2 * Math.PI);
            return Angle % (float)(2 * Math.PI);
        }

        public void MoveTo(float X, float Y, float Z)
        {
            const float LearningRate = 0.0001f;
            const int Iterations = 40;
            for (int j = 0; j < Params.Length; j++)
            {
                if (Params[j].Fixed)
                {
                    var (CurX, CurY, CurZ) = ForwardKinematics();
                    float PrevSum = j > 0 ? PitchSums[j - 1] : 0;
                    CurrentPitches[j] = EnsureStandardForm((Params[j].FixedAngle - PrevSum));
                }
                else
                {
                    for (int i = 0; i < Iterations; i++)
                    {
                        var (CurX, CurY, CurZ) = ForwardKinematics();
                        //float Cost = (CurX - X) * (CurX - X) + (CurY - Y) * (CurY - Y) + (CurZ - Z) * (CurZ - Z);
                        var (GradRoll, GradPitch, GradYaw) = CalcGrad(X, Y, Z, CurX, CurY, CurZ, j);

                        GradRoll *= LearningRate;
                        GradPitch *= LearningRate;
                        GradYaw *= LearningRate;

                        TempRolls[j] = CurrentRolls[j] - GradRoll;
                        TempPitches[j] = CurrentPitches[j] - GradPitch;
                        TempYaws[j] = CurrentYaws[j] - GradYaw;

                        /*
                        Console.WriteLine("GradRoll:  " + GradRoll);
                        Console.WriteLine("GradPitch: " + GradPitch);
                        Console.WriteLine("GradYaw:   " + GradYaw);
                        */

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
                }
                CurrentRolls[j] = Math.Max(Math.Min(CurrentRolls[j], Params[j].MaxRoll), Params[j].MinRoll);
                CurrentPitches[j] = Math.Max(Math.Min(CurrentPitches[j], Params[j].MaxPitch), Params[j].MinPitch);
                CurrentYaws[j] = Math.Max(Math.Min(CurrentYaws[j], Params[j].MaxYaw), Params[j].MinYaw);

                for (int k = 0; k < CurrentRolls.Length; k++)
                    Console.WriteLine($"({CurrentRolls[k]}, {CurrentPitches[k]}, {CurrentYaws[k]})");
                Console.WriteLine();
            }
        }

        public Arm(params ArmPart[] Parameters)
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
    }
}
