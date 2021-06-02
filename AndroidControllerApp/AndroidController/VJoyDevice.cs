using System;
using vJoyInterfaceWrap;


namespace AndroidController
{
    public class VJoyDevice
    {
        public uint id; 
        private vJoy joystick;
        private vJoy.JoystickState iReport;

        private long minVal;
        private long maxVal;

        #region iReportValues
        private int AxisX;
        private int AxisY;
        private int AxisZ;
        private int AxisXRot;
        private int AxisYRot;
        private uint Buttons;
        private uint bHats;
        #endregion
        public VJoyDevice(vJoy joystick, uint id)
        {
            this.joystick = joystick;
            this.id = id;
            this.iReport = new vJoy.JoystickState();
            iReport.bDevice = (byte)id;

            joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_X, ref minVal);
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxVal);
            Reset();
        }
        public void Reset()
        {
            /*joystick.ResetVJD(id);*/ //only works with robust write access
            AxisX = ScaleVal(0.0);
            AxisY = ScaleVal(0.0);
            AxisZ = ScaleVal(0.0);
            AxisXRot = ScaleVal(0.0);
            AxisYRot = ScaleVal(0.0);
            Buttons = 0x0;

            bHats = 0xFFFFFFFF;
            Update();
        }

        public void SetStick(string name, bool isPressed, double actuatorX, double actuatorY)
        {
            switch (name)
            {
                case "L":
                    AxisX = ScaleVal(actuatorX);
                    AxisY = ScaleVal(actuatorY);
                    break;
                case "R":
                    AxisXRot = ScaleVal(actuatorX);
                    AxisYRot = ScaleVal(actuatorY);
                    break;
            }
        }
        private int ScaleVal(double val)
        {
            val = Math.Min(Math.Max(val * 1.2, -1.0), 1.0);
            return (int)((val + 1) / 2 * maxVal + minVal);
        }
        public void SetButton(string name, bool isPressed)
        {
            switch (name)
            {
                case "A":
                    SetBit(isPressed, 1);
                    break;
                case "B":
                    SetBit(isPressed, 2);
                    break;
                case "X":
                    SetBit(isPressed, 3);
                    break;
                case "Y":
                    SetBit(isPressed, 4);
                    break;
                case "BA":
                    SetBit(isPressed, 7);
                    break;
                case "S":
                    SetBit(isPressed, 8);
                    break;
            }
        }
        public void SetBit( bool val, int position)
        {
            uint mask = Buttons;
            Buttons = val ? (mask |= (uint)(1 << position)) : (mask & (uint)~(1 << position));
        }
        public void SetPOV(string name, bool isPressed, double actuatorX, double actuatorY)
        {
            switch (name)
            {
                case "D":
                    if (isPressed)
                    {
                        bHats = GetHatVal(actuatorX, actuatorY);
                    }
                    else
                    {
                        bHats = 0xFFFFFFFF;
                    }
                    break;
            }
            
        }
        public uint GetHatVal(double actuatorX, double actuatorY)
        {
            double angle = (Math.Atan2(actuatorY, actuatorX) * 180 / Math.PI)+90;
            angle = angle < 0 ? angle + 360 : angle;
            return (uint)( angle * 100);
        }
        public void SetBumper(string name, bool isPressed)
        {
            switch (name)
            {
                case "L":
                    SetBit(isPressed, 5);
                    break;
                case "R":
                    SetBit(isPressed, 6);
                    break;
            }
        }
        public void SetTrigger(string name, bool isPressed)
        {
            switch (name)
            {
                case "L":
                    AxisZ = ScaleVal(isPressed ? -1 : 0);
                    break;
                case "R":
                    AxisZ = ScaleVal(isPressed ? 1 : 0);
                    break;
            }
        }
        public void Update()
        {
            /*iReport.bDevice = (byte)id;*/
            iReport.AxisX = AxisX;
            iReport.AxisY = AxisY;
            iReport.AxisZ = AxisZ;
            iReport.AxisXRot = AxisXRot;
            iReport.AxisYRot = AxisYRot;
            iReport.Buttons = Buttons;
            iReport.bHats = bHats;
            bool res = joystick.UpdateVJD(id, ref iReport);
        }
    }
}