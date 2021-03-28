using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;

namespace PlayerAbilities
{
	public class PlayerAbilities : Mod
	{
#if DEBUG
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Add(new AbilityDebugIntefaceLayer());
        }
#endif
    }
}