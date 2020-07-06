
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngineers
{
    class SpaceEngineersTemplate : MyGridProgram
    {
        IMyGridTerminalSystem GridTerminalSystem;
        public SpaceEngineersTemplate()

        {
            //Runtime.UpdateFrequency = UpdateFrequency...;
        }

        public void Save() { }

        public void Main(string argument, UpdateType updateSource)
        {

        }
    }
}
