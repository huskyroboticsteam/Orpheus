using System;
using System.Threading;
using Scarlet.IO;

namespace Scarlet.Components.Sensors
{
    public class BNO055 : ISensor
    {
        public const byte BNO055_ADDRESS_A = 0x28;
        public const byte BNO055_ADDRESS_B = 0x29;
        public const byte BNO055_ID = 0xA0;
        public const byte NUM_BNO055_OFFSET_REGISTERS = 22;

        /// <summary>
        /// Enum of all possible registers on the BNO055. 
        /// </summary>
        private enum Register : byte
        {
            /* Page id register definition */
            BNO055_PAGE_ID_ADDR = 0X07,

            /* PAGE0 REGISTER DEFINITION START*/
            BNO055_CHIP_ID_ADDR = 0x00,
            BNO055_ACCEL_REV_ID_ADDR = 0x01,
            BNO055_MAG_REV_ID_ADDR = 0x02,
            BNO055_GYRO_REV_ID_ADDR = 0x03,
            BNO055_SW_REV_ID_LSB_ADDR = 0x04,
            BNO055_SW_REV_ID_MSB_ADDR = 0x05,
            BNO055_BL_REV_ID_ADDR = 0X06,

            /* Accel data register */
            BNO055_ACCEL_DATA_X_LSB_ADDR = 0X08,
            BNO055_ACCEL_DATA_X_MSB_ADDR = 0X09,
            BNO055_ACCEL_DATA_Y_LSB_ADDR = 0X0A,
            BNO055_ACCEL_DATA_Y_MSB_ADDR = 0X0B,
            BNO055_ACCEL_DATA_Z_LSB_ADDR = 0X0C,
            BNO055_ACCEL_DATA_Z_MSB_ADDR = 0X0D,

            /* Mag data register */
            BNO055_MAG_DATA_X_LSB_ADDR = 0X0E,
            BNO055_MAG_DATA_X_MSB_ADDR = 0X0F,
            BNO055_MAG_DATA_Y_LSB_ADDR = 0X10,
            BNO055_MAG_DATA_Y_MSB_ADDR = 0X11,
            BNO055_MAG_DATA_Z_LSB_ADDR = 0X12,
            BNO055_MAG_DATA_Z_MSB_ADDR = 0X13,

            /* Gyro data registers */
            BNO055_GYRO_DATA_X_LSB_ADDR = 0X14,
            BNO055_GYRO_DATA_X_MSB_ADDR = 0X15,
            BNO055_GYRO_DATA_Y_LSB_ADDR = 0X16,
            BNO055_GYRO_DATA_Y_MSB_ADDR = 0X17,
            BNO055_GYRO_DATA_Z_LSB_ADDR = 0X18,
            BNO055_GYRO_DATA_Z_MSB_ADDR = 0X19,

            /* Euler data registers */
            BNO055_EULER_H_LSB_ADDR = 0X1A,
            BNO055_EULER_H_MSB_ADDR = 0X1B,
            BNO055_EULER_R_LSB_ADDR = 0X1C,
            BNO055_EULER_R_MSB_ADDR = 0X1D,
            BNO055_EULER_P_LSB_ADDR = 0X1E,
            BNO055_EULER_P_MSB_ADDR = 0X1F,

            /* Quaternion data registers */
            BNO055_QUATERNION_DATA_W_LSB_ADDR = 0X20,
            BNO055_QUATERNION_DATA_W_MSB_ADDR = 0X21,
            BNO055_QUATERNION_DATA_X_LSB_ADDR = 0X22,
            BNO055_QUATERNION_DATA_X_MSB_ADDR = 0X23,
            BNO055_QUATERNION_DATA_Y_LSB_ADDR = 0X24,
            BNO055_QUATERNION_DATA_Y_MSB_ADDR = 0X25,
            BNO055_QUATERNION_DATA_Z_LSB_ADDR = 0X26,
            BNO055_QUATERNION_DATA_Z_MSB_ADDR = 0X27,

            /* Linear acceleration data registers */
            BNO055_LINEAR_ACCEL_DATA_X_LSB_ADDR = 0X28,
            BNO055_LINEAR_ACCEL_DATA_X_MSB_ADDR = 0X29,
            BNO055_LINEAR_ACCEL_DATA_Y_LSB_ADDR = 0X2A,
            BNO055_LINEAR_ACCEL_DATA_Y_MSB_ADDR = 0X2B,
            BNO055_LINEAR_ACCEL_DATA_Z_LSB_ADDR = 0X2C,
            BNO055_LINEAR_ACCEL_DATA_Z_MSB_ADDR = 0X2D,

            /* Gravity data registers */
            BNO055_GRAVITY_DATA_X_LSB_ADDR = 0X2E,
            BNO055_GRAVITY_DATA_X_MSB_ADDR = 0X2F,
            BNO055_GRAVITY_DATA_Y_LSB_ADDR = 0X30,
            BNO055_GRAVITY_DATA_Y_MSB_ADDR = 0X31,
            BNO055_GRAVITY_DATA_Z_LSB_ADDR = 0X32,
            BNO055_GRAVITY_DATA_Z_MSB_ADDR = 0X33,

            /* Temperature data register */
            BNO055_TEMP_ADDR = 0X34,

            /* Status registers */
            BNO055_CALIB_STAT_ADDR = 0X35,
            BNO055_SELFTEST_RESULT_ADDR = 0X36,
            BNO055_INTR_STAT_ADDR = 0X37,

            BNO055_SYS_CLK_STAT_ADDR = 0X38,
            BNO055_SYS_STAT_ADDR = 0X39,
            BNO055_SYS_ERR_ADDR = 0X3A,

            /* Unit selection register */
            BNO055_UNIT_SEL_ADDR = 0X3B,
            BNO055_DATA_SELECT_ADDR = 0X3C,

            /* Mode registers */
            BNO055_OPR_MODE_ADDR = 0X3D,
            BNO055_PWR_MODE_ADDR = 0X3E,

            BNO055_SYS_TRIGGER_ADDR = 0X3F,
            BNO055_TEMP_SOURCE_ADDR = 0X40,

            /* Axis remap registers */
            BNO055_AXIS_MAP_CONFIG_ADDR = 0X41,
            BNO055_AXIS_MAP_SIGN_ADDR = 0X42,

            /* SIC registers */
            BNO055_SIC_MATRIX_0_LSB_ADDR = 0X43,
            BNO055_SIC_MATRIX_0_MSB_ADDR = 0X44,
            BNO055_SIC_MATRIX_1_LSB_ADDR = 0X45,
            BNO055_SIC_MATRIX_1_MSB_ADDR = 0X46,
            BNO055_SIC_MATRIX_2_LSB_ADDR = 0X47,
            BNO055_SIC_MATRIX_2_MSB_ADDR = 0X48,
            BNO055_SIC_MATRIX_3_LSB_ADDR = 0X49,
            BNO055_SIC_MATRIX_3_MSB_ADDR = 0X4A,
            BNO055_SIC_MATRIX_4_LSB_ADDR = 0X4B,
            BNO055_SIC_MATRIX_4_MSB_ADDR = 0X4C,
            BNO055_SIC_MATRIX_5_LSB_ADDR = 0X4D,
            BNO055_SIC_MATRIX_5_MSB_ADDR = 0X4E,
            BNO055_SIC_MATRIX_6_LSB_ADDR = 0X4F,
            BNO055_SIC_MATRIX_6_MSB_ADDR = 0X50,
            BNO055_SIC_MATRIX_7_LSB_ADDR = 0X51,
            BNO055_SIC_MATRIX_7_MSB_ADDR = 0X52,
            BNO055_SIC_MATRIX_8_LSB_ADDR = 0X53,
            BNO055_SIC_MATRIX_8_MSB_ADDR = 0X54,

            /* Accelerometer Offset registers */
            ACCEL_OFFSET_X_LSB_ADDR = 0X55,
            ACCEL_OFFSET_X_MSB_ADDR = 0X56,
            ACCEL_OFFSET_Y_LSB_ADDR = 0X57,
            ACCEL_OFFSET_Y_MSB_ADDR = 0X58,
            ACCEL_OFFSET_Z_LSB_ADDR = 0X59,
            ACCEL_OFFSET_Z_MSB_ADDR = 0X5A,

            /* Magnetometer Offset registers */
            MAG_OFFSET_X_LSB_ADDR = 0X5B,
            MAG_OFFSET_X_MSB_ADDR = 0X5C,
            MAG_OFFSET_Y_LSB_ADDR = 0X5D,
            MAG_OFFSET_Y_MSB_ADDR = 0X5E,
            MAG_OFFSET_Z_LSB_ADDR = 0X5F,
            MAG_OFFSET_Z_MSB_ADDR = 0X60,

            /* Gyroscope Offset register s*/
            GYRO_OFFSET_X_LSB_ADDR = 0X61,
            GYRO_OFFSET_X_MSB_ADDR = 0X62,
            GYRO_OFFSET_Y_LSB_ADDR = 0X63,
            GYRO_OFFSET_Y_MSB_ADDR = 0X64,
            GYRO_OFFSET_Z_LSB_ADDR = 0X65,
            GYRO_OFFSET_Z_MSB_ADDR = 0X66,

            /* Radius registers */
            ACCEL_RADIUS_LSB_ADDR = 0X67,
            ACCEL_RADIUS_MSB_ADDR = 0X68,
            MAG_RADIUS_LSB_ADDR = 0X69,
            MAG_RADIUS_MSB_ADDR = 0X6A
        }

        /// <summary>
        /// The power mode of the BNO055. 
        /// </summary>
        private enum PowerMode
        {
            POWER_MODE_NORMAL = 0X00,
            POWER_MODE_LOWPOWER = 0X01,
            POWER_MODE_SUSPEND = 0X02
        }

        /// <summary>
        /// The operation mode of the BNO055. 
        /// </summary>
        public enum OperationMode
        {
            /* Operation mode settings*/
            OPERATION_MODE_CONFIG = 0X00,
            OPERATION_MODE_ACCONLY = 0X01,
            OPERATION_MODE_MAGONLY = 0X02,
            OPERATION_MODE_GYRONLY = 0X03,
            OPERATION_MODE_ACCMAG = 0X04,
            OPERATION_MODE_ACCGYRO = 0X05,
            OPERATION_MODE_MAGGYRO = 0X06,
            OPERATION_MODE_AMG = 0X07,
            OPERATION_MODE_IMUPLUS = 0X08,
            OPERATION_MODE_COMPASS = 0X09,
            OPERATION_MODE_M4G = 0X0A,
            OPERATION_MODE_NDOF_FMC_OFF = 0X0B,
            OPERATION_MODE_NDOF = 0X0C
        }

        /// <summary>
        /// The possible types of vectors. 
        /// </summary>
        public enum VectorType
        {
            VECTOR_ACCELEROMETER = Register.BNO055_ACCEL_DATA_X_LSB_ADDR,
            VECTOR_MAGNETOMETER = Register.BNO055_MAG_DATA_X_LSB_ADDR,
            VECTOR_GYROSCOPE = Register.BNO055_GYRO_DATA_X_LSB_ADDR,
            VECTOR_EULER = Register.BNO055_EULER_H_LSB_ADDR,
            VECTOR_LINEARACCEL = Register.BNO055_LINEAR_ACCEL_DATA_X_LSB_ADDR,
            VECTOR_GRAVITY = Register.BNO055_GRAVITY_DATA_X_LSB_ADDR
        }

        private int ID;
        private byte Address;
        private OperationMode Mode;
        private II2CBus I2C;

        private float X, Y, Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Scarlet.Components.Sensors.BNO055"/> class, which will communicate via
        /// the given I2C bus. 
        /// </summary>
        /// <param name="I2C"> The I2C bus to communicate over. </param>
        /// <param name="ID"> ID of BNO055. -1 by default. You probably shouldn't change this. </param>
        /// <param name="Address"> The I2C address of the BNO055. Defaults to 0x28. You probably shouldn't change this. </param>
        public BNO055(II2CBus I2C, int ID = -1, byte Address = BNO055_ADDRESS_A)
        {
            this.ID = ID;
            this.Address = Address;
            this.I2C = I2C;
            Begin();
        }

        /// <summary>
        /// Begins the operation of the BNO055 with the target operation mode. NDOF is the default operation mode.
        /// This method is called from the constructor. Returns whether or not it succeeded. 
        /// </summary>
        /// <returns> Whether the BNO055 successfully initialized. </returns>
        /// <param name="mode"> Operation mode at startup. NDOF by default. </param>
        public bool Begin(OperationMode mode = OperationMode.OPERATION_MODE_NDOF)
        {
            try { Write8((byte)Register.BNO055_PAGE_ID_ADDR, 0); }
            catch { /* Do nothing */ }

            SetMode(OperationMode.OPERATION_MODE_CONFIG);
            Write8((byte)Register.BNO055_PAGE_ID_ADDR, 0);

            byte id = Read8((byte)Register.BNO055_CHIP_ID_ADDR);
            if (id != BNO055_ID) { return false; }

            Write8((byte)Register.BNO055_SYS_TRIGGER_ADDR, 0x20);
            Thread.Sleep(750); // Minimum of 650 ms wait time for power reset
            Write8((byte)Register.BNO055_PWR_MODE_ADDR, (byte)PowerMode.POWER_MODE_NORMAL & 0xFF);
            Write8((byte)Register.BNO055_SYS_TRIGGER_ADDR, 0x00);
            SetMode(mode);
            return true;
        }

        /// <summary> Gets the vector corresponding to the orientation of the magnetometer according to the type specified. </summary>
        /// <returns> The vector. </returns>
        /// <param name="VectorType"> Type of vector to be read. </param>
        public Tuple<float, float, float> GetVector(VectorType VectorType)
        {
            short X = 0;
            short Y = 0;
            short Z = 0;

            byte[] buffer = I2C.ReadRegister(Address, (byte)VectorType, 6);
            X = (short)((buffer[1] << 8) | buffer[0]);
            Y = (short)((buffer[3] << 8) | buffer[2]);
            Z = (short)((buffer[5] << 8) | buffer[4]);
            switch (VectorType)
            {
                case VectorType.VECTOR_MAGNETOMETER:
                case VectorType.VECTOR_EULER:
                case VectorType.VECTOR_GYROSCOPE:
                    return new Tuple<float, float, float>(X / 16.0f, Y / 16.0f, Z / 16.0f);
                default:
                    return new Tuple<float, float, float>(X / 100.0f, Y / 100.0f, Z / 100.0f);
            }
        }

        /// <summary> Sets the operation mode of the BNO055. Only use if you know what you're doing! </summary>
        /// <param name="Mode"> The target mode that the BNO-55 will be set to. </param>
        public void SetMode(OperationMode Mode)
        {
            this.Mode = Mode;
            Write8((byte)Register.BNO055_OPR_MODE_ADDR, (byte)(((byte)Mode) & 0xFF));
            Thread.Sleep(30);
        }

        /// <summary> Reads a vector from the BNO055 and ensures that none of them are 0. </summary>
        /// <returns> Whether or not the BNO055 worked. </returns>
        public bool Test()
        {
            Begin();
            var xyz = this.GetVector(VectorType.VECTOR_MAGNETOMETER);
            return (xyz.Item1 != 0.0f || xyz.Item2 != 0.0f || xyz.Item3 != 0.0f);
        }

        /// <summary> Reads the position according to the magnetometer and updates the class variables. </summary>
        public void UpdateState()
        {
            var x = GetVector(VectorType.VECTOR_MAGNETOMETER);
            this.X = x.Item1;
            this.Y = x.Item2;
            this.Z = x.Item3;
        }

        /// <summary> Does nothing. </summary>
        /// <param name="sender"> Sender. </param>
        /// <param name="e"> The EventArgs object </param>
        public void EventTriggered(object sender, EventArgs e) { }

        /// <summary> Reads a byte from the specified register. </summary>
        /// <returns> The byte that was read. </returns>
        /// <param name="Register"> The register to read from. </param>
        private byte Read8(byte Register) => I2C.ReadRegister(Address, Register, 1)[0];

        /// <summary> Writes the specified byte and to the Register. </summary>        
        /// <param name="Register"> Register to write to. </param>
        /// <param name="Data"> Byte to write.</param>
        private void Write8(byte Register, byte Data) => I2C.WriteRegister(Address, Register, new byte[] { Data });
    }
}