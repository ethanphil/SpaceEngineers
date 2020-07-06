
using EmptyKeys.UserInterface.Generated.StoreBlockView_Bindings;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders;

namespace SpaceEngineers
{
    class InventoryManager : MyGridProgram
    {
        IMyGridTerminalSystem GridTerminalSystem;
        TimeSpan interval = TimeSpan.FromSeconds(30);
        DateTime lastExecuted = DateTime.Now;
        List<IMyEntity> inventories = new List<IMyEntity>();
        List<IMyTextPanel> screens = new List<IMyTextPanel>();
        readonly List<Item> allItems = new List<Item>{
            new Item("Stone", "MyObjectBuilder_Ingot"),
            new Item("Iron", "MyObjectBuilder_Ingot"),
            new Item("Nickel", "MyObjectBuilder_Ingot"),
            new Item("Cobalt", "MyObjectBuilder_Ingot"),
            new Item("Magnesium", "MyObjectBuilder_Ingot"),
            new Item("Silicon", "MyObjectBuilder_Ingot"),
            new Item("Silver", "MyObjectBuilder_Ingot"),
            new Item("Gold", "MyObjectBuilder_Ingot"),
            new Item("Platinum", "MyObjectBuilder_Ingot"),
            new Item("Uranium", "MyObjectBuilder_Ingot"),

            new Item("Construction", "MyObjectBuilder_Component"),
            new Item("MetalGrid", "MyObjectBuilder_Component"),
            new Item("InteriorPlate", "MyObjectBuilder_Component"),
            new Item("SteelPlate", "MyObjectBuilder_Component"),
            new Item("Girder", "MyObjectBuilder_Component"),
            new Item("SmallTube", "MyObjectBuilder_Component"),
            new Item("LargeTube", "MyObjectBuilder_Component"),
            new Item("Motor", "MyObjectBuilder_Component"),
            new Item("Display", "MyObjectBuilder_Component"),
            new Item("BulletproofGlass", "MyObjectBuilder_Component"),
            new Item("Superconductor", "MyObjectBuilder_Component"),
            new Item("Computer", "MyObjectBuilder_Component"),
            new Item("Reactor", "MyObjectBuilder_Component"),
            new Item("Thrust", "MyObjectBuilder_Component"),
            new Item("GravityGenerator", "MyObjectBuilder_Component"),
            new Item("Medical", "MyObjectBuilder_Component"),
            new Item("RadioCommunication", "MyObjectBuilder_Component"),
            new Item("Detector", "MyObjectBuilder_Component"),
            new Item("Explosives", "MyObjectBuilder_Component"),
            new Item("SolarCell", "MyObjectBuilder_Component"),
            new Item("PowerCell", "MyObjectBuilder_Component")
        };
        
        public class Item
        {
            public Item(string subtype, string type)
            {
                name = subtype;
                itemType = new MyItemType(type, subtype);
                amount = 0;
            }
            public string name;
            public MyItemType itemType;
            public long amount;
        }

        public InventoryManager()

        {
            GridTerminalSystem.GetBlocksOfType<IMyEntity>(inventories);
            GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(screens);
            screens = screens.Where(x => x.CustomData.Contains("InventoryScreen")).ToList();
            inventories = inventories.Where(x => x.HasInventory == true).ToList();
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save() { }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((DateTime.Now - lastExecuted) > interval)
            {
                lastExecuted = DateTime.Now;
            } else { return; }
            foreach (var item in allItems)
            {
                item.amount = 0;
                foreach (var entity in inventories)
                {
                    for (var i = 0; i < entity.InventoryCount; i++)
                    {
                        item.amount += entity.GetInventory(i).GetItemAmount(item.itemType).RawValue/1000000;
                    }
                }
            }
            var ingotOutput = "";
            var componentOutput = "";
            foreach (var item in allItems)
            {
                if (item.itemType.TypeId == "MyObjectBuilder_Ingot")
                {
                    ingotOutput += (item.name + ": " + item.amount.ToString() + "\n");
                }else if (item.itemType.TypeId == "MyObjectBuilder_Component")
                {
                    componentOutput += (item.name + ": " + item.amount.ToString() + "\n");
                }
            }
            foreach (var screen in screens)
            {
                if (screen.CustomData.Contains("Ingots"))
                {
                    screen.WriteText(ingotOutput);
                }else if (screen.CustomData.Contains("Components"))
                {
                    screen.WriteText(componentOutput);
                }
            }
        }
    }
}
