﻿using Harmony;
using System;
using System.Reflection;
using NitroxClient.MonoBehaviours;

namespace NitroxPatcher.Patches
{
    public class CellManager_ShowEntities_Patch : NitroxPatch
    {
        public static readonly Type TARGET_CLASS = typeof(CellManager);
        public static readonly MethodInfo TARGET_METHOD = TARGET_CLASS.GetMethod("ShowEntities", new Type[] { typeof(Int3.Bounds), typeof(int) });
        
        public static bool Prefix(CellManager __instance, Int3.Bounds blockRange, int level)
        {
            if(!LargeWorldStreamer.main.IsReady())
            {
                return false;
            }

            if (level == 2 || level == 3)
            {
                Int3 minBatch = blockRange.mins / LargeWorldStreamer.main.blocksPerBatch;
                Int3 maxBatch = blockRange.maxs / LargeWorldStreamer.main.blocksPerBatch;
                Int3.Bounds batchBounds = new Int3.Bounds(minBatch, maxBatch);

                Multiplayer.Logic.Chunks.ChunksLoaded(batchBounds);
            }
            
            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            this.PatchPrefix(harmony, TARGET_METHOD);
        }
    }
}