using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRover
{
    class GetCSVData
    {
        private static List<MotorBoardData> MB1 = new List<MotorBoardData>();
        private static List<MotorBoardData> MB2 = new List<MotorBoardData>();
        private static List<MotorBoardData> MB3 = new List<MotorBoardData>();
        private static List<MotorBoardData> MB4 = new List<MotorBoardData>();
        private static List<MotorBoardData> MB5 = new List<MotorBoardData>();
        private static List<MotorBoardData> MB6 = new List<MotorBoardData>();
        private static List<MotorBoardData> MB7 = new List<MotorBoardData>();

        public void Initialize()
        {

            int LineCount = 2;
            using (var reader = new StreamReader("Motorboards.csv"))
            {

                // Ignore first line in CSV for comment about formatting 
                var skipline = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values.Length < 6)
                    {
                        Console.WriteLine("Motor data fields not complete at line " + LineCount);
                    }
                    else
                    {
                        MotorBoardData add = new MotorBoardData();
                        add.Model = int.Parse(values[1]);
                        add.P = UInt16.Parse(values[2]);
                        add.I = UInt16.Parse(values[3]);
                        add.D = UInt16.Parse(values[4]);
                        add.TicksPerRev = UInt16.Parse(values[5]);
                        switch (values[0])
                        {
                            case "MB1":
                                MB1.Add(add);
                                break;
                            case "MB2":
                                MB2.Add(add);
                                break;
                            case "MB3":
                                MB3.Add(add);
                                break;
                            case "MB4":
                                MB4.Add(add);
                                break;
                            case "MB5":
                                MB5.Add(add);
                                break;
                            case "MB6":
                                MB6.Add(add);
                                break;
                            case "MB7":
                                MB7.Add(add);
                                break;
                            default:
                                Console.WriteLine("Motor name not found for line " + LineCount);
                                break;
                        }

                    }
                    LineCount++;
                }
            }
        }

        public MotorBoardData getMB1(int ModelNumber)
        {
            return MB1.Find(x => x.Model == ModelNumber);
        }

        public MotorBoardData getMB2(int ModelNumber)
        {
            return MB2.Find(x => x.Model == ModelNumber);
        }

        public MotorBoardData getMB3(int ModelNumber)
        {
            return MB3.Find(x => x.Model == ModelNumber);
        }

        public MotorBoardData getMB4(int ModelNumber)
        {
            return MB4.Find(x => x.Model == ModelNumber);
        }

        public MotorBoardData getMB5(int ModelNumber)
        {
            return MB5.Find(x => x.Model == ModelNumber);
        }

        public MotorBoardData getMB6(int ModelNumber)
        {
            return MB6.Find(x => x.Model == ModelNumber);
        }

        public MotorBoardData getMB7(int ModelNumber)
        {
            return MB7.Find(x => x.Model == ModelNumber);
        }

    }

}
