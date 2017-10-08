using Scarlet.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.Components.Sensors
{
    public class MPU6050 : ISensor
    {
        private byte Address;
        private II2CBus Bus;
        private MPU6050Configuration Configuration;
        private MPUData LastReading;
        private static double[] AccelSensitivity = new double[] { 2.0, 4.0, 8.0, 16.0 };
        private static double[] GyroSensitivity = new double[] { 250.0, 500.0, 1000.0, 2000.0 };

        public MPU6050(II2CBus Bus, byte DeviceID)
        {
            Address = DeviceID;
            this.Bus = Bus;
        }

        public void Initialize() { }

        public void Configure(MPU6050Configuration Config)
        {
            Bus.WriteRegister(Address, 0x19, new byte[] { Config.SampleRateDivider });

            // Get Current Configs
            byte ConfigRegister = Bus.ReadRegister(Address, 0x1A, 1)[0];
            byte GyroConfig = Bus.ReadRegister(Address, 0x1B, 1)[0];
            byte AccelConfig = Bus.ReadRegister(Address, 0x1C, 1)[0];

            // Clean the configs
            GyroConfig &= 0b11100111;
            AccelConfig &= 0b11100111;
            ConfigRegister &= 0b11111100;

            // Set Config
            GyroConfig |= (byte)(Config.GyroSensitivity << 3);
            AccelConfig |= (byte)(Config.AccelSensitivity << 3);
            ConfigRegister |= Config.DLPFSetup;

            // Write the config
            Bus.WriteRegister(Address, 0x1A, new byte[] { ConfigRegister });
            Bus.WriteRegister(Address, 0x1B, new byte[] { GyroConfig });
            Bus.WriteRegister(Address, 0x1C, new byte[] { AccelConfig });

        }

        // Uses a default configuration
        public void Configure()
        {
            MPU6050Configuration Configuration = new MPU6050Configuration
            {
                AccelSensitivity = 0,
                GyroSensitivity = 0,
                SampleRateDivider = 0,
                DLPFSetup = 0
            };
            Configure(Configuration);
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            throw new NotImplementedException();
        }

        public bool Test()
        {
            // TODO: Implement Self Test Features
            throw new NotImplementedException();
        }

        public void UpdateState()
        {
            LastReading = new MPUData();
            byte[] AX = Bus.ReadRegister(Address, 0x3B, 2);
            byte[] AY = Bus.ReadRegister(Address, 0x3D, 2);
            byte[] AZ = Bus.ReadRegister(Address, 0x3F, 2);
            byte[] GX = Bus.ReadRegister(Address, 0x43, 2);
            byte[] GY = Bus.ReadRegister(Address, 0x45, 2);
            byte[] GZ = Bus.ReadRegister(Address, 0x47, 2);
            LastReading.AccelX = ((AX[0] << 8) | AX[1]) / AccelSensitivity[Configuration.AccelSensitivity];
            LastReading.AccelY = ((AY[0] << 8) | AY[1]) / AccelSensitivity[Configuration.AccelSensitivity];
            LastReading.AccelZ = ((AZ[0] << 8) | AZ[1]) / AccelSensitivity[Configuration.AccelSensitivity];
            LastReading.GyroX = ((GX[0] << 8) | GX[1]) / GyroSensitivity[Configuration.GyroSensitivity];
            LastReading.GyroY = ((GY[0] << 8) | GY[1]) / GyroSensitivity[Configuration.GyroSensitivity];
            LastReading.GyroZ = ((GZ[0] << 8) | GZ[1]) / GyroSensitivity[Configuration.GyroSensitivity];
        }

        public MPUData GetData() { return LastReading; }

        public double GetInternalTemp()
        {
            byte[] Temp = Bus.ReadRegister(Address, 0x41, 2);
            return 0.0;
        }

        public struct MPUData
        {
            public double AccelX, AccelY, AccelZ;
            public double GyroX, GyroY, GyroZ;
        }

        public struct MPU6050Configuration
        {
            private byte P_DLPFSetup;
            public byte DLPFSetup
            {
                get { return P_DLPFSetup; }
                set
                {
                    if (value > 6) { P_DLPFSetup = 6; }
                    else { P_DLPFSetup = value; }
                }
            }

            private byte P_GyroSensitivity;
            public byte GyroSensitivity
            {
                get { return P_GyroSensitivity; }
                set
                {
                    if (value > 3) { P_GyroSensitivity = 3; }
                    else { P_GyroSensitivity = value; }
                }
            }

            private byte P_AccelSensitivity;
            public byte AccelSensitivity
            {
                get { return P_AccelSensitivity; }
                set
                {
                    if (value > 3) { P_AccelSensitivity = 3; }
                    else { P_AccelSensitivity = value; }
                }
            }
            
            // Higher the number, the lower the sample rate
            public byte SampleRateDivider;
        }

    }

}
