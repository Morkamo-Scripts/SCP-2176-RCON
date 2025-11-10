using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using static DarkAPI.Features.ParticlesSystem;
using evArgs = Exiled.Events.Handlers.Player;
using mapArgs = Exiled.Events.Handlers.Map;

namespace SCP_2176_RCON.Components
{
    [CustomItem(ItemType.SCP2176)]
    public abstract class Scp2176Rcon : CustomItem
    {
        public abstract Color LampColor { get; set; }
        public enum RconType
        {
            RconA, RconB, RconC,
            RconD, RconE, RconF,
            RconG,
        }
        public abstract RconType CurrentType { get; }
        public abstract float EffectsDuration { get; set; }
        public virtual ushort TracerGlowIntensity { get; set; } = 50;
        
        /// <summary>
        /// Интенсивность частиц в стандартной симуляции
        /// </summary>
        public virtual ushort ParticlesGlowInensity { get; set; } = 30;
        
        /// <summary>
        /// Интенсивность частиц в симуляции взрыва
        /// </summary>
        public virtual ushort ExParticlesGlowInensity { get; set; } = 30;

        /// <summary>
        /// Вызывается при взрыве SCP-2176-RCON
        /// </summary>
        /// <param name="projectile">Компонент "снаряда"</param>
        /// <param name="position">Точка взрыва</param>
        /// <param name="player">Метатель</param>
        public abstract void OnExplode2176Rcon(Projectile projectile, Vector3 position, Player player);

        /// <summary>
        /// Вызывается, когда экземпляр SCP-2176-RCON был выброшен игроком.
        /// </summary>
        /// <param name="player">Игрок, бросивший лампу.</param>
        /// <param name="pickup">Компонент лампы как Pickup.</param>
        /// <param name="rconType">Текущий RCON экзмепляр.</param>
        /// <param name="rconComponent">Компонент 2176-RCON.</param>
        protected virtual void OnRconDropped(Player player, Pickup pickup, RconType rconType, Scp2176Rcon rconComponent) { }

        /// <summary>
        /// Вызывается когда экземпляр SCP-2176-RCON был брошен как Projectile.
        /// </summary>
        /// <param name="player">Игрок, бросивший лампу.</param>
        /// <param name="pickup">Объект подбора</param>
        /// <param name="projectile">Компонент лампы как Projectile.</param>
        /// <param name="rconType">Текущий RCON экзмепляр.</param>
        /// <param name="rconComponent">Компонент 2176-RCON.</param>
        protected virtual void OnRconThrowed(Player player, Pickup pickup, Projectile projectile, RconType rconType, Scp2176Rcon rconComponent) { }

        protected override void SubscribeEvents()
        {
            mapArgs.ExplodingGrenade += OnExplodingGrenade;
            evArgs.DroppedItem += OnDroppedItem;
            evArgs.ThrownProjectile += OnThrownProjectile;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            mapArgs.ExplodingGrenade -= OnExplodingGrenade;
            evArgs.DroppedItem -= OnDroppedItem;
            evArgs.ThrownProjectile -= OnThrownProjectile;
            base.UnsubscribeEvents();
        }

        private void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (Check(ev.Projectile))
            {
                DynamicTracing(ev.Projectile.GameObject, LampColor, 0, TracerGlowIntensity);
                OnRconThrowed(ev.Player, ev.Pickup, ev.Projectile, CurrentType, this);
            }
        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (Check(ev.Projectile))
            {
                ev.IsAllowed = false;
                CustomItem.TryGive(ev.Player, Id);
                OnExplode2176Rcon(ev.Projectile, ev.Position, ev.Player);
            }
        }

        private void OnDroppedItem(DroppedItemEventArgs ev)
        {
            if (Check(ev.Pickup))
                OnRconDropped(ev.Player, ev.Pickup, CurrentType, this);
        }
    }
}