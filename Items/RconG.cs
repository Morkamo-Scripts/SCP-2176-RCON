using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups.Projectiles;
using UnityEngine;
using System.ComponentModel;
using DarkAPI.Features;
using Exiled.API.Extensions;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using SCP_2176_RCON.Components;
using static DarkAPI.Features.ParticlesSystem;
using evArgs = Exiled.Events.Handlers.Player;
using mapArgs = Exiled.Events.Handlers.Map;

namespace SCP_2176_RCON.Items
{
    namespace SCP_2176_RCON.Items
    {
        [CustomItem(ItemType.SCP2176)]
        public class RconG : Scp2176Rcon
        {
            public override uint Id { get; set; } = 8;
            public override string Name { get; set; } = "SCP-2176-RCON-G";
            public override string Description { get; set; } = "";
            public override float Weight { get; set; } = 1f;
            public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
            public override ItemType Type => ItemType.SCP2176;
            public override Vector3 Scale => Vector3.one;
            public override Color LampColor { get; set; } = new Color32(150, 50, 255, 255);
            public override RconType CurrentType { get; } = RconType.RconG;
            public sealed override float EffectsDuration { get; set; } = 0f;
            public override ushort TracerGlowIntensity { get; set; } = 50;

            [Description("Получаемые эффекты")]
            public Dictionary<EffectType, EffectDetails> Effects { get; set; }

            [Description("Предметы, разрешенные к спавну")]
            public HashSet<ItemType> SpawnableItems { get; set; } = new HashSet<ItemType>()
            {
                ItemType.KeycardJanitor,
                ItemType.KeycardScientist,
                ItemType.KeycardResearchCoordinator,
                ItemType.KeycardZoneManager,
                ItemType.KeycardGuard,
                ItemType.KeycardMTFPrivate,
                ItemType.KeycardContainmentEngineer,
                ItemType.KeycardMTFOperative,
                ItemType.KeycardMTFCaptain,
                ItemType.KeycardFacilityManager,
                ItemType.KeycardChaosInsurgency,
                ItemType.KeycardO5,
                ItemType.Radio,
                ItemType.GunCOM15,
                ItemType.Medkit,
                ItemType.Flashlight,
                ItemType.MicroHID,
                ItemType.SCP500,
                ItemType.SCP207,
                ItemType.Ammo12gauge,
                ItemType.GunE11SR,
                ItemType.GunCrossvec,
                ItemType.Ammo556x45,
                ItemType.GunFSP9,
                ItemType.GunLogicer,
                ItemType.GrenadeHE,
                ItemType.GrenadeFlash,
                ItemType.Ammo44cal,
                ItemType.Ammo762x39,
                ItemType.Ammo9x19,
                ItemType.GunCOM18,
                ItemType.SCP018,
                ItemType.SCP268,
                ItemType.Adrenaline,
                ItemType.Painkillers,
                ItemType.Coin,
                ItemType.ArmorLight,
                ItemType.ArmorCombat,
                ItemType.ArmorHeavy,
                ItemType.GunRevolver,
                ItemType.GunAK,
                ItemType.GunShotgun,
                ItemType.SCP330,
                ItemType.SCP2176,
                ItemType.SCP244a,
                ItemType.SCP244b,
                ItemType.SCP1853,
                ItemType.ParticleDisruptor,
                ItemType.GunCom45,
                ItemType.SCP1576,
                ItemType.Jailbird,
                ItemType.AntiSCP207,
                ItemType.GunFRMG0,
                ItemType.GunA7,
                /*ItemType.Lantern,*/
                ItemType.SCP1344,
                /*ItemType.Snowball,
                ItemType.Coal,
                ItemType.SpecialCoal,
                ItemType.SCP1507Tape,*/
            };
            
            public override void OnExplode2176Rcon(Projectile projectile, Vector3 position, Player player)
            {
                DynamicLight(MakeLight(position, LampColor, LightShadows.Soft, 300, 0), 10000f, 0.1f);
                Map.ExplodeEffect(position, ProjectileType.Flashbang);

                Room room = projectile.Room;
                room.TurnOffLights(0.1f);
                room.Color = LampColor;
                
                Pickup.CreateAndSpawn(SpawnableItems.GetRandomValue(), position);
                
                GameObject gameObject = new GameObject();
                gameObject.transform.position = position;
                ProceduralExplosionParticles(gameObject, LampColor, 150, new Vector3(2, 2, 2), 0.5f, ExParticlesGlowInensity, 2, 10, 2);
            }

            protected override void OnRconDropped(Player player, Pickup pickup, RconType rconType, Scp2176Rcon scp2176Rcon)
            {
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