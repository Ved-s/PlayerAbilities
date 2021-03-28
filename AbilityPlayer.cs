using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace PlayerAbilities
{
    class AbilityPlayer : ModPlayer
    {
        public float MeleeDamage = 1f;
        public float RangedDamage = 1f;
        public float MagicDamage = 1f;
        public float SummonDamage = 1f;
        public float ThrowDamage = 1f;
        public float OtherDamage = 1f;

        public const float MinStartDamageMult = 0.5f;
        public const float MaxStartDamageMult = 1f;

        public const float MinTalentDamageMult = 1.2f;
        public const float MaxnTalentDamageMult = 2f;

        public const float MinDamageMult = 0.5f;
        public const float MaxDamageMult = 5f;

        public const float ValueMultiplier = 0.00001f;


        public AbilityPlayer()
        {
            SetupRandomValues();
        }

        internal void SetupRandomValues(bool noPlayerName = false)
        {
            Random random = (player is null || noPlayerName) ? new Random() : new Random(player.name.GetHashCode());

            MeleeDamage = random.NextFloat(MinStartDamageMult, MaxStartDamageMult);
            RangedDamage = random.NextFloat(MinStartDamageMult, MaxStartDamageMult);
            MagicDamage = random.NextFloat(MinStartDamageMult, MaxStartDamageMult);
            SummonDamage = random.NextFloat(MinStartDamageMult, MaxStartDamageMult);
            ThrowDamage = random.NextFloat(MinStartDamageMult, MaxStartDamageMult);
            OtherDamage = random.NextFloat(MinStartDamageMult, MaxStartDamageMult);

            switch (random.Next(0, 5))
            {
                case 0:
                    MeleeDamage = random.NextFloat(MinTalentDamageMult, MaxnTalentDamageMult);
                    break;
                case 1:
                    RangedDamage = random.NextFloat(MinTalentDamageMult, MaxnTalentDamageMult);
                    break;
                case 2:
                    MagicDamage = random.NextFloat(MinTalentDamageMult, MaxnTalentDamageMult);
                    break;
                case 3:
                    SummonDamage = random.NextFloat(MinTalentDamageMult, MaxnTalentDamageMult);
                    break;
                case 4:
                    ThrowDamage = random.NextFloat(MinTalentDamageMult, MaxnTalentDamageMult);
                    break;
                case 5:
                    OtherDamage = random.NextFloat(MinTalentDamageMult, MaxnTalentDamageMult);
                    break;

            }
        }

        public override TagCompound Save()
        {
            TagCompound tc = new TagCompound();
            tc["dmgMelee"] = MeleeDamage;
            tc["dmgRanged"] = RangedDamage;
            tc["dmgMagic"] = MagicDamage;
            tc["dmgSummon"] = SummonDamage;
            tc["dmgThrow"] = ThrowDamage;
            return tc;
        }
        public override void Load(TagCompound tag)
        {
            MeleeDamage = (float)tag["dmgMelee"];
            RangedDamage = (float)tag["dmgRanged"];
            MagicDamage = (float)tag["dmgMagic"];
            SummonDamage = (float)tag["dmgSummon"];
            ThrowDamage = (float)tag["dmgThrow"];
        }

        public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
        {
            if (item.melee) mult = MeleeDamage;
            else if (item.ranged) mult = RangedDamage;
            else if (item.magic) mult = MagicDamage;
            else if (item.summon) mult = SummonDamage;
            else if (item.thrown) mult = ThrowDamage;
            else mult = OtherDamage;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            float mod = 1f;
            if (target.type == NPCID.TargetDummy) mod = 0.1f;
            if (item.melee)       IncreaseMeleeAbility (GetIncreaseValue(DamageType.Melee)  * mod);
            else if (item.ranged) IncreaseRangedAbility(GetIncreaseValue(DamageType.Ranged) * mod);
            else if (item.magic)  IncreaseMagicAbility (GetIncreaseValue(DamageType.Magic)  * mod);
            else if (item.summon) IncreaseSummonAbility(GetIncreaseValue(DamageType.Summon) * mod);
            else if (item.thrown) IncreaseThrowAbility (GetIncreaseValue(DamageType.Throw)  * mod);
            else                  IncreaseOtherAbility (GetIncreaseValue(DamageType.Other)  * mod);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.melee) IncreaseMeleeAbility(GetIncreaseValue(DamageType.Melee));
            else if (proj.ranged) IncreaseRangedAbility(GetIncreaseValue(DamageType.Ranged));
            else if (proj.magic) IncreaseMagicAbility(GetIncreaseValue(DamageType.Magic));
            else if (proj.thrown) IncreaseThrowAbility(GetIncreaseValue(DamageType.Throw));
            else IncreaseOtherAbility(GetIncreaseValue(DamageType.Other));
        }

        public float GetIncreaseValue(DamageType dt) 
        {
            switch (dt)
            {
                case DamageType.Melee:
                    return MeleeDamage < MaxDamageMult ? (MaxDamageMult - MeleeDamage) * ValueMultiplier : 0;
                case DamageType.Ranged:
                    return RangedDamage < MaxDamageMult ? (MaxDamageMult - RangedDamage) * ValueMultiplier : 0;
                case DamageType.Magic:
                    return MagicDamage < MaxDamageMult ? (MaxDamageMult - MagicDamage) * ValueMultiplier : 0;
                case DamageType.Summon:
                    return SummonDamage < MaxDamageMult ? (MaxDamageMult - SummonDamage) * ValueMultiplier : 0;
                case DamageType.Throw:
                    return ThrowDamage < MaxDamageMult ? (MaxDamageMult - ThrowDamage) * ValueMultiplier : 0;
                case DamageType.Other:
                    return OtherDamage < MaxDamageMult ? (MaxDamageMult - OtherDamage) * ValueMultiplier : 0;
            }
            return 0f;
        }

        public void IncreaseMeleeAbility(float v)
        {
            if (v == 0) return;
            float rem = MaxDamageMult - MeleeDamage;
            float percent = v / rem;

            MeleeDamage += v;
            RangedDamage -= (RangedDamage - MinDamageMult) * percent;
            MagicDamage -= (MagicDamage - MinDamageMult) * percent;
            SummonDamage -= (SummonDamage - MinDamageMult) * percent;
            ThrowDamage -= (ThrowDamage - MinDamageMult) * percent;
            OtherDamage -= (OtherDamage - MinDamageMult) * percent;
        }
        public void IncreaseRangedAbility(float v)
        {
            if (v == 0) return;
            float rem = MaxDamageMult - RangedDamage;
            float percent = v / rem;

            MeleeDamage -= (MeleeDamage - MinDamageMult) * percent;
            RangedDamage += v;
            MagicDamage -= (MagicDamage - MinDamageMult) * percent;
            SummonDamage -= (SummonDamage - MinDamageMult) * percent;
            ThrowDamage -= (ThrowDamage - MinDamageMult) * percent;
            OtherDamage -= (OtherDamage - MinDamageMult) * percent;
        }
        public void IncreaseMagicAbility(float v)
        {
            if (v == 0) return;
            float rem = MaxDamageMult - MagicDamage;
            float percent = v / rem;

            MeleeDamage -= (MeleeDamage - MinDamageMult) * percent;
            RangedDamage -= (RangedDamage - MinDamageMult) * percent;
            MagicDamage += v;
            SummonDamage -= (SummonDamage - MinDamageMult) * percent;
            ThrowDamage -= (ThrowDamage - MinDamageMult) * percent;
            OtherDamage -= (OtherDamage - MinDamageMult) * percent;
        }
        public void IncreaseSummonAbility(float v)
        {
            if (v == 0) return;
            float rem = MaxDamageMult - SummonDamage; ;
            float percent = v / rem;

            MeleeDamage -= (MeleeDamage - MinDamageMult) * percent;
            RangedDamage -= (RangedDamage - MinDamageMult) * percent;
            MagicDamage -= (MagicDamage - MinDamageMult) * percent;
            SummonDamage += v;
            ThrowDamage -= (ThrowDamage - MinDamageMult) * percent;
            OtherDamage -= (OtherDamage - MinDamageMult) * percent;
        }
        public void IncreaseThrowAbility(float v)
        {
            if (v == 0) return;
            float rem = MaxDamageMult - ThrowDamage;
            float percent = v / rem;

            MeleeDamage -= (MeleeDamage - MinDamageMult) * percent;
            RangedDamage -= (RangedDamage - MinDamageMult) * percent;
            MagicDamage -= (MagicDamage - MinDamageMult) * percent;
            SummonDamage -= (SummonDamage - MinDamageMult) * percent;
            ThrowDamage += v;
            OtherDamage -= (OtherDamage - MinDamageMult) * percent;
        }
        public void IncreaseOtherAbility(float v)
        {
            if (v == 0) return;
            float rem = MaxDamageMult - OtherDamage;
            float percent = v / rem;

            MeleeDamage -= (MeleeDamage - MinDamageMult) * percent;
            RangedDamage -= (RangedDamage - MinDamageMult) * percent;
            MagicDamage -= (MagicDamage - MinDamageMult) * percent;
            SummonDamage -= (SummonDamage - MinDamageMult) * percent;
            ThrowDamage -= (ThrowDamage - MinDamageMult) * percent;
            OtherDamage += v;
        }

        public enum DamageType { Melee, Ranged, Magic, Summon, Throw, Other }

    }



}

