namespace SCP_2176_RCON.Components
{
    public struct EffectDetails
    {
        public byte Intensity { get; set; }
        public float Duration { get; set; }
        
        public EffectDetails(byte intensity, float duration)
        {
            Intensity = intensity;
            Duration = duration;
        }
    }
}