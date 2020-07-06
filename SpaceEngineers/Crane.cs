
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngineers
{
    class Crane : MyGridProgram
    {
        //just testing git
        //testing git again
        IMyGridTerminalSystem GridTerminalSystem;
        List<IMyTerminalBlock> test = new List<IMyTerminalBlock>();
        IMyShipController controller;
        IMyMotorStator rotor;
        List<IMyPistonBase> upPistons = new List<IMyPistonBase>();
        List<IMyPistonBase> forwardPistons = new List<IMyPistonBase>();
        List<IMyPistonBase> downPistons = new List<IMyPistonBase>();
        List<IMyPistonBase> backPistons = new List<IMyPistonBase>();
        IMyMotorStator toolRotor;
        public Crane()
        {
            toolRotor = GridTerminalSystem.GetBlockWithName("Crane Tool Rotor") as IMyMotorStator;
            GridTerminalSystem.SearchBlocksOfName("Crane Up Piston", test);
            controller = (IMyShipController)GridTerminalSystem.GetBlockWithName("Crane Controller");
            rotor = (IMyMotorStator)GridTerminalSystem.GetBlockWithName("Crane Rotor");
            GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(upPistons);
            GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(forwardPistons);
            GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(downPistons);
            GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(backPistons);
            upPistons = upPistons.Where(x => x.CustomName.Contains("Crane Up Piston")).ToList();
            forwardPistons = forwardPistons.Where(x => x.CustomName.Contains("Crane Forward Piston")).ToList();
            downPistons = downPistons.Where(x => x.CustomName.Contains("Crane Downward Piston")).ToList();
            backPistons = backPistons.Where(x => x.CustomName.Contains("Crane Backward Piston")).ToList();
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save() { }

        public void Main(string argument, UpdateType updateSource)
        {
            switch (argument)
            {
                case "Welders":
                    toolRotor.UpperLimitDeg = float.MaxValue;
                    toolRotor.UpperLimitDeg = 0;
                    break;
                case "Connector":
                    toolRotor.UpperLimitDeg = float.MaxValue;
                    toolRotor.UpperLimitDeg = 90;
                    break;
                case "Grinders":
                    toolRotor.UpperLimitDeg = float.MaxValue;
                    toolRotor.UpperLimitDeg = 180;
                    break;
                case "LandingGear":
                    toolRotor.UpperLimitDeg = float.MaxValue;
                    toolRotor.UpperLimitDeg = 270;
                    break;
                default:
                    break;

            }
            var movement = controller.MoveIndicator;
            rotor.TargetVelocityRPM = movement.X * 2 / (1 + forwardPistons[0].CurrentPosition/forwardPistons[0].MaxLimit * 9);
            foreach (var piston in upPistons)
            {
                piston.Velocity = movement.Y / (upPistons.Count() + downPistons.Count)*2;
            }
            foreach (var piston in forwardPistons)
            {
                piston.Velocity = -movement.Z / (forwardPistons.Count() + backPistons.Count())*2;
            }
            foreach (var piston in downPistons)
            {
                piston.Velocity = -movement.Y / (upPistons.Count() + downPistons.Count)*2;
            }
            foreach (var piston in backPistons)
            {
                piston.Velocity = movement.Z / (forwardPistons.Count() + backPistons.Count())*2;
            }
        }
    }
}
