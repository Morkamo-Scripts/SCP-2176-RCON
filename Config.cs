using System.ComponentModel;
using Exiled.API.Interfaces;
using SCP_2176_RCON.Items.SCP_2176_RCON.Items;

namespace SCP_2176_RCON
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        
        [Description("Вариант - А")]
        public RconA Scp2176RconA { get; set; } = new RconA();
        
        [Description("Вариант - B")]
        public RconB Scp2176RconB { get; set; } = new RconB();
        
        [Description("Вариант - C")]
        public RconC Scp2176RconC { get; set; } = new RconC();
        
        [Description("Вариант - D")]
        public RconD Scp2176RconD { get; set; } = new RconD();
        
        [Description("Вариант - E")]
        public RconE Scp2176RconE { get; set; } = new RconE();
        
        [Description("Вариант - F")]
        public RconF Scp2176RconF { get; set; } = new RconF();
        
        [Description("Вариант - G")]
        public RconG Scp2176RconG { get; set; } = new RconG();
    }
}