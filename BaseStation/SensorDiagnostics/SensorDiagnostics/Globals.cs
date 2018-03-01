﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Components.Sensors;
namespace FlatRobot
{
    static class Global
    {

        //Physical component references
        public static List<IMotor> motors = new List<IMotor>();
        public static List<IServo> servos = new List<IServo>();
        public static List<ISensor> sensors = new List<ISensor>();
        public static List<ICamera> cameras = new List<ICamera>();
        public static List<ISubsystem> subsystems = new List<ISubsystem>();
    }

}