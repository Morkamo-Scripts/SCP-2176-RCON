using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups.Projectiles;
using UnityEngine;
using System.ComponentModel;
using System.Linq;
using DarkAPI.Features;
using CustomPlayerEffects;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using MEC;
using SCP_2176_RCON.Components;
using static DarkAPI.Features.ParticlesSystem;
using evArgs = Exiled.Events.Handlers.Player;
using mapArgs = Exiled.Events.Handlers.Map;

namespace SCP_2176_RCON.Items
{
    namespace SCP_2176_RCON.Items
    {
        [CustomItem(ItemType.SCP2176)]
        public class RconD : Scp2176Rcon
        {
            public RconD()
            {
                Effects = new Dictionary<EffectType, EffectDetails>
                {
                    [EffectType.DamageReduction] = new EffectDetails(200, EffectsDuration)
                };
            }

            public override uint Id { get; set; } = 5;
            public override string Name { get; set; } = "SCP-2176-RCON-D";
            public override string Description { get; set; } = "";
            public override float Weight { get; set; } = 1f;
            public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
            public override ItemType Type => ItemType.SCP2176;
            public override Vector3 Scale => Vector3.one;
            public override Color LampColor { get; set; } = new Color32(255, 54, 78, 255);
            public override RconType CurrentType { get; } = RconType.RconD;
            public sealed override float EffectsDuration { get; set; } = 30f;
            public override ushort TracerGlowIntensity { get; set; } = 70;

            [Description("Получаемые эффекты")]
            public Dictionary<EffectType, EffectDetails> Effects { get; set; }

            public override void OnExplode2176Rcon(Projectile projectile, Vector3 position, Player player)
            {
                DynamicLight(MakeLight(position, LampColor, LightShadows.Soft, 300, 0), 10000f, 0.1f);
                Map.ExplodeEffect(position, ProjectileType.Flashbang);

                Room room = projectile.Room;
                room.TurnOffLights(0.1f);
                room.Color = LampColor;
                
                foreach (Player target in Player.List.Where(target => target.CurrentRoom == room))
                {
                    PlayerProperties playerProperties = target.ReferenceHub.GetComponent<PlayerProperties>();
                    
                    if (!Effects.IsEmpty())
                    {
                        // Выдача эффектов
                        foreach (var kvp in Effects)
                        {
                            var effect = kvp.Key;
                            var effectDetails = kvp.Value;

                            target.EnableEffect(effect, effectDetails.Intensity, effectDetails.Duration);
                        }
                    }
                    
                    target.DisableAllEffects(EffectCategory.Negative);

                    // Наложение трассирующего эффекта
                    DynamicTracing(target.GameObject, LampColor, EffectsDuration, 50, 2f, 0.01f);
                    
                    // Обновление параметров "расширенных свойств игрока"
                    playerProperties.Scp2176Rcon.ActiveRcons.Add(CurrentType);
                    playerProperties.Scp2176Rcon.IsRconInfluenced = true;

                    // Откат
                    Timing.CallDelayed(EffectsDuration, () =>
                    {
                        playerProperties.Scp2176Rcon.ActiveRcons.Remove(CurrentType);
                        if (playerProperties.Scp2176Rcon.ActiveRcons.IsEmpty())
                            playerProperties.Scp2176Rcon.IsRconInfluenced = false;
                    });
                }
                
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
        }
    }
}