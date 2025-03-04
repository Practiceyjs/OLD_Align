using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;

namespace T_Align
{
    class Temperature
    {
        static public string MainBorad_Temper_Name = "";
        static public string HardDisk_Name = "";

        static Computer Computer = new Computer();
        Temperature_Ad Ad = new Temperature_Ad();
        public Temperature()
        {
            HardDisk_Name = ClassINI.ReadProject("Temperature", "HDD Name");
            MainBorad_Temper_Name = ClassINI.ReadProject("Temperature", "MB Name");
            Computer.HDDEnabled = true;
            Computer.Open();
        }

        public Temp_Infomation Get_Temperature()
        {
            Temp_Infomation Temp = new Temp_Infomation();

            foreach (var hardwareItem in Computer.Hardware)
            {
                IHardware Hardware = hardwareItem;
                Hardware.Update();
                switch (hardwareItem.HardwareType)
                {
                    case HardwareType.HDD:
                        foreach (IHardware subHardware in Hardware.SubHardware)
                            subHardware.Update();
                        foreach (var sensor in Hardware.Sensors)
                        {
                            string name = Hardware.Name;
                            float value = sensor.Value.HasValue ? sensor.Value.Value : -1;

                            switch (sensor.SensorType)
                            {
                                case SensorType.Temperature:
                                    switch (hardwareItem.HardwareType)
                                    {
                                        case HardwareType.HDD:
                                            if (HardDisk_Name == name)
                                                Temp.HDD_Temp = value;
                                            break;
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            Temp.CPU_Temp = Ad.get_cpu_temp();
            Temp.MainBoard_Temp = Ad.get_sys_temp();
            return Temp;
        }

        public struct Temp_Infomation
        {
            public int CPU_Temp;
            public int MainBoard_Temp;
            public float HDD_Temp;
        }

        static public List<string> HardDisk_List()
        {
            List<string> Return_List = new List<string>();
            foreach (var hardwareItem in Computer.Hardware)
            {
                IHardware Hardware = hardwareItem;
                Hardware.Update();
                switch (hardwareItem.HardwareType)
                {
                    case HardwareType.HDD:
                        foreach (IHardware subHardware in Hardware.SubHardware)
                            subHardware.Update();
                        foreach (var sensor in Hardware.Sensors)
                        {
                            string name = Hardware.Name;

                            switch (sensor.SensorType)
                            {
                                case SensorType.Temperature:
                                    Return_List.Add(name);
                                    break;
                            }
                        }
                        break;
                }
            }
            return Return_List;
        }
    }
}
