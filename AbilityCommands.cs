using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace PlayerAbilities
{
    class AbilityMainCommand : ModCommand
    {
        public override string Command => "pl_abils";
        public override CommandType Type => CommandType.Chat;
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0) 
            {
                caller.Reply("Player Abilities mod commands help:\n" +
                    "\"/pl_abils reset\" - reset all abilities to 1x\n" +
                    "\"/pl_abils random\" - set abilities to random (based on player name)\n" +
                    "\"/pl_abils absrandom\" - set abilities to random\n" +
                    "\"/pl_abils values\" - print out current stats");
            }
            else
            {
                if (args[0] == "reset")
                {
                    AbilityPlayer ap = caller.Player.GetModPlayer<AbilityPlayer>();
                    ap.MeleeDamage  = 1f;
                    ap.MagicDamage  = 1f;
                    ap.RangedDamage = 1f;
                    ap.SummonDamage = 1f;
                    ap.ThrowDamage  = 1f;
                    ap.OtherDamage  = 1f;
                }
                else if (args[0] == "random")
                {
                    caller.Player.GetModPlayer<AbilityPlayer>().SetupRandomValues();
                }
                else if (args[0] == "absrandom")
                {
                    caller.Player.GetModPlayer<AbilityPlayer>().SetupRandomValues(true);
                }
                else if (args[0] == "values") 
                {
                    string v = "";
                    AbilityPlayer ap = caller.Player.GetModPlayer<AbilityPlayer>();

                    foreach (FieldInfo fi in typeof(AbilityPlayer).GetFields(BindingFlags.Instance | BindingFlags.Public))
                    {
                        v += $"{fi.Name}: {fi.GetValue(ap)}\n";
                    }
                    caller.Reply(v);
                }
            }
        }
    }
}
