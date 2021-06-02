using System;
using System.Collections.Generic;
using vJoyInterfaceWrap;

namespace AndroidController
{

    public class VJoyHandler
    {
        public List<uint> deviceIds;
        public bool IsEnabled;
        private vJoy joystick;
        
        private Dictionary<uint, VJoyDevice> vJoyDevices;
        
        public VJoyHandler()
        {
            joystick = new vJoy();
            IsEnabled = joystick.vJoyEnabled();
            deviceIds = new List<uint>();
            if (IsEnabled)
            {
                UInt32 DllVer = 0, DrvVer = 0;
                bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
                if (match)
                    Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
                else
                    Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);

                bool[] availableDeviceIds = new bool[17];
                for (uint id = 1; id <= 16; id++)
                {
                    if (joystick.GetVJDStatus(id) == VjdStat.VJD_STAT_FREE)
                    {
                        availableDeviceIds[id] = true;
                    }
                }

                vJoyDevices = new Dictionary<uint, VJoyDevice>();
                for (uint id = 1; id <= 16; id++) {
                    if (availableDeviceIds[id] && joystick.AcquireVJD(id)) {
                        deviceIds.Add(id);
                        vJoyDevices.Add(id, new VJoyDevice(joystick, id)); 
                    }
                }
            }
            else
            {
                if (!joystick.vJoyEnabled())
                {
                    Console.WriteLine("vJoy driver not enabled");
                }
            }
        }
        public void RunCommand(uint deviceId, string[] parts)
        {
            if (ValidId(deviceId) && vJoyDevices.ContainsKey(deviceId))
            {
                VJoyDevice vJoyDevice = vJoyDevices[deviceId];
                string partType = parts[1];
                string partName = parts[2];
                bool partIsPressed = int.Parse(parts[3]) == 1;
                switch (partType)
                {
                    case "S":
                        double actuatorX = Double.Parse(parts[4]);
                        double actuatorY = Double.Parse(parts[5]);
                        vJoyDevice.SetStick(partName, partIsPressed, actuatorX, actuatorY);
                        break;
                    case "B":
                        vJoyDevice.SetButton(partName, partIsPressed);
                        break;
                    case "D":
                        double padX = Double.Parse(parts[4]);
                        double padY = Double.Parse(parts[5]);
                        vJoyDevice.SetPOV(partName, partIsPressed, padX, padY);
                        break;
                    case "BU":
                        vJoyDevice.SetBumper(partName, partIsPressed);
                        break;
                    case "T":
                        vJoyDevice.SetTrigger(partName, partIsPressed);
                        break;
                }
            }
        }
        public void Update(uint deviceId)
        {
            vJoyDevices[deviceId].Update();
        }
        private bool ValidId(uint deviceId)
        {
            return deviceId > 0 && deviceId <= 16;
        }
        public void Close()
        {
            foreach(uint id in deviceIds)
            {
                joystick.RelinquishVJD(id);
            }
        }
    }
}