﻿using Microsoft.Xna.Framework;
using SolStandard.Containers;
using SolStandard.Containers.Contexts;
using SolStandard.Map.Elements;
using SolStandard.Map.Elements.Cursor;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.Unit.Skills
{
    public class Wait : UnitSkill
    {
        public Wait() : base(
            icon: SkillIconProvider.GetSkillIcon(SkillIcon.Wait, new Vector2(32)),
            name: "Wait",
            description: "Take no action and end your turn.",
            tileSprite: MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Action),
            range: new[] {0}
        )
        {
        }

        public override void ExecuteAction(MapSlice targetSlice, MapContext mapContext, BattleContext battleContext)
        {
            if (targetSlice.DynamicEntity != null)
            {
                MapContainer.ClearDynamicAndPreviewGrids();
                SkipCombatPhase(mapContext);
                AssetManager.MapUnitSelectSFX.Play();
            }
            else
            {
                AssetManager.WarningSFX.Play();
            }
        }
    }
}