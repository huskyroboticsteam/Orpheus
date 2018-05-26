using System;
using Scarlet;
using Scarlet.IO.BeagleBone;
using System.Collections.Generic;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.Communications;

namespace MainRover
{
    public class MainRover
    {
        public const string SERVER_IP = "192.168.0.5";

        public static bool Quit;
        public static List<ISensor> Sensors;

        public static void PinConfig()
        {
            BBBPinManager.AddBusCAN(0);
            BBBPinManager.AddMappingUART(Pins.MTK3339_RX);
            BBBPinManager.AddMappingUART(Pins.MTK3339_TX);
            BBBPinManager.AddMappingsI2C(Pins.BNO055_SCL, Pins.BNO055_SDA);
            BBBPinManager.AddMappingGPIO(Pins.SteeringLimitSwitchPin, false, Scarlet.IO.ResistorState.PULL_UP, true);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
        }

        public static void InitBeagleBone()
        {
            PinConfig();
            BeagleBone.Initialize(SystemMode.DEFAULT, true);
            Sensors = new List<ISensor>();
            Sensors.Add(new BNO055(I2CBBB.I2CBus1));
            Sensors.Add(new MTK3339(UARTBBB.UARTBus1));
            //Add encoders
        }

        public static void SetupClient()
        {
            Client.Start(SERVER_IP, 1025, 1026, "RoverBBB");
        }

        public static void ProcessInstructions()
        {
            foreach (Packet p in Client.ReceivedPackets)
            {
                switch (p.Data.ID)
                {

                }
            }
            Client.ReceivedPackets.Clear();
        }

        public static void UpdateSensors()
        {
            foreach (ISensor Sensor in Sensors)
            {
                Sensor.UpdateState();
            }
        }

        public static void Main()
        {
            Quit = false;
            InitBeagleBone();
            SetupClient();
            do
            {
                UpdateSensors();
                ProcessInstructions();
            } while (!Quit);
        }
    }
}
