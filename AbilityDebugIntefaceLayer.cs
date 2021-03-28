using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

#if DEBUG

namespace PlayerAbilities
{
    class AbilityDebugIntefaceLayer : GameInterfaceLayer
    {
        public AbilityDebugIntefaceLayer() : base("PlayerAbilityDebug", InterfaceScaleType.None) 
        {

        }

        protected override bool DrawSelf()
        {
            Vector2 pos = new Vector2(0, Main.screenHeight - 20);
            AbilityPlayer ap = Main.player[Main.myPlayer].GetModPlayer<AbilityPlayer>();

            foreach (FieldInfo fi in typeof(AbilityPlayer).GetFields(BindingFlags.Instance | BindingFlags.Public)) 
            {
                Main.spriteBatch.DrawString(Main.fontItemStack, $"{fi.Name}: {fi.GetValue(ap)}" , pos, Color.White);
                pos.Y -= 15;
            }

            return true;
        }
    }
}

#endif