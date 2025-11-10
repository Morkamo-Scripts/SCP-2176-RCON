using System;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using SCP_2176_RCON.Items.SCP_2176_RCON.Items;

namespace SCP_2176_RCON
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;

        public override string Author => "Morkamo";
        public override string Name => "SCP-2176-RCON";
        public override string Prefix => Name;
        public override Version Version => new Version(1, 0, 0);

        public RconA RconA;
        public RconB RconB;
        public RconC RconC;
        public RconD RconD;
        public RconE RconE;
        public RconF RconF;
        public RconG RconG;

        public override void OnEnabled()
        {
            Instance = this;
            RegisterItems();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnRegisterItems();
            Instance = null;
            base.OnDisabled();
        }

        private void RegisterItems()
        {
            RconA = new RconA();
            RconB = new RconB();
            RconC = new RconC();
            RconD = new RconD();
            RconE = new RconE();
            RconF = new RconF();
            RconG = new RconG();

            Config.Scp2176RconA.Register();
            Config.Scp2176RconB.Register();
            Config.Scp2176RconC.Register();
            Config.Scp2176RconD.Register();
            Config.Scp2176RconE.Register();
            Config.Scp2176RconF.Register();
            Config.Scp2176RconG.Register();
        }

        private void UnRegisterItems()
        {
            Config.Scp2176RconA.Unregister();
            Config.Scp2176RconB.Unregister();
            Config.Scp2176RconC.Unregister();
            Config.Scp2176RconD.Unregister();
            Config.Scp2176RconE.Unregister();
            Config.Scp2176RconF.Unregister();
            Config.Scp2176RconG.Unregister();

            RconA = null;
            RconB = null;
            RconC = null;
            RconD = null;
            RconE = null;
            RconF = null;
        }
    }
}