﻿using System;
using Microsoft.Xna.Framework;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.Unit.Actions.Archer
{
    public class HuntingTrap : LayTrap
    {
        public HuntingTrap(int damage, int maxTriggers) : base(
            skillIcon: SkillIconProvider.GetSkillIcon(SkillIcon.Trap, new Vector2(GameDriver.CellSize)),
            trapSprite: SkillIconProvider.GetSkillIcon(SkillIcon.Trap, new Vector2(GameDriver.CellSize)),
            title: "Hunting Trap",
            damage: damage,
            maxTriggers: maxTriggers,
            description: "Place a tile that will deal [" + damage +
                         "] damage to enemies that start their turn on it and immobilize them." +
                         Environment.NewLine + "Max activations: [" + maxTriggers + "]"
        )
        {
        }
    }
}