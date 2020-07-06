
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace SpaceEngineers
{
    class Scanner : MyGridProgram
    {
        IMyGridTerminalSystem GridTerminalSystem;
        double sens = 10;
        IMyCameraBlock camera;
        IMyMotorStator hinge;
        IMyMotorStator rotor;
        IMyCockpit control;
        List<IMyTextPanel> screens = new List<IMyTextPanel>();
        ScannedItem lastDetect;
        public Scanner()

        {
            lastDetect = new ScannedItem();
            GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(screens);
            screens = screens.Where(x => x.CustomData.Contains("ScannerScreen")).ToList();
            camera = (IMyCameraBlock)GridTerminalSystem.GetBlockWithName("Scanner Camera");
            camera.EnableRaycast = true;
            hinge = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("Scanner Hinge");
            rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("Scanner Rotor");
            control = (IMyCockpit)GridTerminalSystem.GetBlockWithName("Scanner Control");
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save() { }

        public void Main(string argument, UpdateType updateSource)
        {
            if (argument == "Scan")
            {
                Scan();
                return;
            }
            if (!control.IsUnderControl)
            {
                hinge.TargetVelocityRPM = 0;
                rotor.TargetVelocityRPM = 0;
                return;
            }
            Echo(control.RotationIndicator.X.ToString() + ":" + control.RotationIndicator.Y.ToString());
            hinge.TargetVelocityRPM = -(float)sens * control.RotationIndicator.X / 8;
            rotor.TargetVelocityRPM = (float)sens * control.RotationIndicator.Y / 8;
            WriteData();
        }

        public void WriteData()
        {
            var output = lastDetect.ToString() + "ScanRange: " + camera.AvailableScanRange.ToString();
            foreach (var screen in screens)
            {
                screen.WriteText(output);
            }
        }

        public void Scan()
        {
            lastDetect.NewScan(camera.Raycast(20000), Me);
            WriteData();
        }

        public class ScannedItem
        {
            public ScannedItem()
            {
                Name = "None";
                distance = 0;
                volume = 0;
            }
            public void NewScan(MyDetectedEntityInfo info, IMyProgrammableBlock block)
            {
                Name = info.Name;
                volume = (long)info.BoundingBox.Volume;
                distance = (int)(info.HitPosition == null ? 0 : Vector3D.Distance(block.GetPosition(), info.HitPosition.Value));
            }
            public override string ToString()
            {
                var output = "Name: " + Name + "\nDistance: " + distance.ToString() + "\nVolume: " + volume.ToString() + "\n";
                return output;
            }
            public string Name;
            public int distance;
            public long volume;
        }
    }
}
