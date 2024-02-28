using HarmonyLib;
using Verse;

namespace DeathrestButton
{
    [StaticConstructorOnStartup]
    public class Mod
    {
        static Mod()
        {
            Harmony harmony = new("nikidigi.DeathrestButton");

            harmony.PatchAll();
        }
    }
}
