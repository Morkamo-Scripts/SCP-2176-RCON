using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups.Projectiles;
using UnityEngine;
using System.ComponentModel;
using DarkAPI.Extensions;
using DarkAPI.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using MEC;
using SCP_2176_RCON.Components;
using static DarkAPI.Features.ParticlesSystem;
using evArgs = Exiled.Events.Handlers.Player;
using Object = UnityEngine.Object;

namespace SCP_2176_RCON.Items
{
    namespace SCP_2176_RCON.Items
    {
        [CustomItem(ItemType.SCP2176)]
        public class RconF : Scp2176Rcon
        {
            public RconF()
            {
                Effects = new Dictionary<EffectType, EffectDetails>
                {
                    [EffectType.MovementBoost] = new EffectDetails(20, EffectsDuration),
                    [EffectType.Ghostly] = new EffectDetails(1, EffectsDuration)
                };
            }

            public override uint Id { get; set; } = 7;
            public override string Name { get; set; } = "SCP-2176-RCON-F";
            public override string Description { get; set; } = "";
            public override float Weight { get; set; } = 1f;
            public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
            public override ItemType Type => ItemType.SCP2176;
            public override Vector3 Scale => Vector3.one;
            public override Color LampColor { get; set; } = new Color32(255, 255, 255, 255);
            public override RconType CurrentType { get; } = RconType.RconF;
            public sealed override float EffectsDuration { get; set; } = 0;
            public override ushort TracerGlowIntensity { get; set; } = 70;

            protected override void SubscribeEvents()
            {
                evArgs.ChangedItem += OnChangedItem;
                evArgs.TriggeringTesla += OnTeslaTriggering;
                base.SubscribeEvents();
            }

            protected override void UnsubscribeEvents()
            {
                evArgs.ChangedItem -= OnChangedItem;
                evArgs.TriggeringTesla -= OnTeslaTriggering;
                base.UnsubscribeEvents();
            }

            [Description("Получаемые эффекты")]
            public Dictionary<EffectType, EffectDetails> Effects { get; set; }

            public override void OnExplode2176Rcon(Projectile projectile, Vector3 position, Player player)
            {
                DynamicLight(MakeLight(position, LampColor, LightShadows.Hard, 100, 0), 1000000f, 10f);
                Map.ExplodeEffect(position, ProjectileType.Flashbang);

                Room room = projectile.Room;
                room.TurnOffLights(0.1f);
                room.Color = LampColor;
                
                // Вызов симуляции взрыва
                GameObject gameObject = new GameObject();
                gameObject.transform.position = position;
                ProceduralExplosionParticles(gameObject, LampColor, 150, new Vector3(2, 2, 2), 0.5f, ExParticlesGlowInensity, 2, 10, 2);
            }

            protected override void OnRconDropped(Player player, Pickup pickup, RconType rconType, Scp2176Rcon scp2176Rcon)
            {
                // Подсветка и вызов симуляции частиц
                HighlightObject(pickup.GameObject, LampColor, LightShadows.None, 1f, 0.5f);
                ProceduralParticles(pickup.GameObject, LampColor, 0, 0.01f, Vector3.one, 0.05f, ParticlesGlowInensity);
            }

            protected override void OnRconThrowed(Player player, Pickup pickup, Projectile projectile, RconType rconType, Scp2176Rcon rconComponent)
            {
                // Вызов подсветки симуляции частиц
                HighlightObject(projectile.GameObject, LampColor, LightShadows.None, 30f, 100f);
                ProceduralParticlesWithLightTracing(projectile.GameObject, LampColor, 0, 0.01f, Vector3.one, 0.5f, ParticlesGlowInensity);
            }

            private void OnChangedItem(ChangedItemEventArgs ev)
            {
                if (Check(ev.Item))
                {
                    PlayerProperties playerProperties = ev.Player.ReferenceHub.GetComponent<PlayerProperties>();
                    playerProperties.Scp2176Rcon.ActiveRcons.Add(CurrentType);
                    playerProperties.Scp2176Rcon.IsRconInfluenced = true;
                    
                    // Наложение трассирующего эффекта
                    GameObject tracingParent = new GameObject();
                    tracingParent.transform.SetParent(ev.Player.GameObject.transform);
                    tracingParent.transform.position = ev.Player.Transform.position;
                    
                    DynamicTracing(tracingParent, LampColor, EffectsDuration, 50, 2f, 0.01f);
                    Timing.RunCoroutine(PlayerEffectsController(ev.Player, playerProperties, ev.Item, tracingParent));
                }
            }

            private void OnTeslaTriggering(TriggeringTeslaEventArgs ev)
            {
                if (ev.Player.PlayerProperties().Scp2176Rcon.ActiveRcons.Contains(CurrentType))
                    ev.IsAllowed = false;
            }

            private IEnumerator<float> PlayerEffectsController(Player player, PlayerProperties playerProperties, Item item, GameObject tracingParent)
            {
                if (!Effects.IsEmpty())
                {
                    // Выдача эффектов
                    foreach (var kvp in Effects)
                    {
                        var effect = kvp.Key;
                        var effectDetails = kvp.Value;

                        player.EnableEffect(effect, effectDetails.Intensity, effectDetails.Duration);
                    }
                }
                
                while (player.CurrentItem == item)
                {
                    yield return Timing.WaitForSeconds(0.3f);
                }
                
                foreach (var effect in Effects)
                {
                    player.DisableEffect(effect.Key);
                }
                
                Object.DestroyImmediate(tracingParent);
                
                playerProperties.Scp2176Rcon.ActiveRcons.Remove(CurrentType);
                if (playerProperties.Scp2176Rcon.ActiveRcons.IsEmpty())
                    playerProperties.Scp2176Rcon.IsRconInfluenced = false;
            }
        }
    }
}