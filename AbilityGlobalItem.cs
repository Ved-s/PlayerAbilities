using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace PlayerAbilities
{
    class AbilityGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine tl = tooltips.FirstOrDefault((t) => t.Name == "Damage" && t.mod == "Terraria");
            if (tl is null) return;

            AbilityPlayer ap = Main.player[Main.myPlayer].GetModPlayer<AbilityPlayer>();
            float value = 1f;
            if (item.melee) value = ap.MeleeDamage;
            else if (item.ranged) value = ap.RangedDamage;
            else if (item.magic) value = ap.MagicDamage;
            else if (item.summon) value = ap.SummonDamage;
            else if (item.thrown) value = ap.ThrowDamage;
            else value = ap.OtherDamage;

            
            tl.text += $" ({value:N2}x)";
        }
    }
}
