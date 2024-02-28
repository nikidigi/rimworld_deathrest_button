using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace DeathrestButton
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
    class Patch_Pawn_GetGizmos
    {
        static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> gizmos, Pawn __instance)
        {
            foreach (Gizmo gizmo in gizmos)
            {
                yield return gizmo;
            }

            if (!__instance.CanDeathrest() || __instance.Deathresting)
            {
                yield break;
            }

            yield return new Command_Action
            {
                defaultLabel = "StartDeathrest".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/DeathrestButton"),
                action = delegate
                {
                    if (__instance.ownership.AssignedDeathrestCasket != null)
                    {
                        Job job = JobMaker.MakeJob(JobDefOf.Deathrest, __instance.ownership.AssignedDeathrestCasket);
                        job.forceSleep = true;
                        __instance.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        return;

                    }

                    if (__instance.ownership.OwnedBed != null)
                    {
                        Job job = JobMaker.MakeJob(JobDefOf.Deathrest, __instance.ownership.OwnedBed);
                        job.forceSleep = true;
                        __instance.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        return;
                    }

                    Messages.Message("No assigned casket or bed", __instance, MessageTypeDefOf.RejectInput, historical: false);
                }
            };
        }
    }
}
