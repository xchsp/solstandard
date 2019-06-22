﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.Containers.Contexts;
using SolStandard.Containers.Contexts.Combat;
using SolStandard.Entity.Unit;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Animation;
using SolStandard.HUD.Window.Content;
using SolStandard.HUD.Window.Content.Combat;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Containers.View
{
    public class BattleView : IUserInterface
    {
        private static readonly Vector2 WindowEdgeBuffer = new Vector2(WindowSpacing);

        private const int WindowSlideSpeed = 5;
        private const int WindowSlideDistance = 200;

        private const int WindowSpacing = 5;
        private static readonly Vector2 HpBarSize = new Vector2(350, 80);

        public AnimatedWindow AttackerPortraitWindow { get; private set; }
        private AnimatedWindow AttackerDetailWindow { get; set; }
        private AnimatedWindow AttackerHpWindow { get; set; }
        private AnimatedWindow AttackerAtkWindow { get; set; }
        public AnimatedWindow AttackerBonusWindow { get; private set; }
        private AnimatedWindow AttackerRangeWindow { get; set; }
        private AnimatedWindow AttackerDiceWindow { get; set; }
        private AnimatedWindow AttackerSpriteWindow { get; set; }

        public AnimatedWindow DefenderPortraitWindow { get; private set; }
        private AnimatedWindow DefenderDetailWindow { get; set; }
        private AnimatedWindow DefenderHpWindow { get; set; }
        private AnimatedWindow DefenderAtkWindow { get; set; }
        private AnimatedWindow DefenderBonusWindow { get; set; }
        private AnimatedWindow DefenderRangeWindow { get; set; }
        private AnimatedWindow DefenderDiceWindow { get; set; }
        private AnimatedWindow DefenderSpriteWindow { get; set; }

        private Window HelpTextWindow { get; set; }
        private Window UserPromptWindow { get; set; }

        private bool visible;

        public BattleView()
        {
            visible = true;
        }

        private static IWindowAnimation RightSideWindowAnimation =>
            new WindowSlide(WindowSlide.SlideDirection.Left, WindowSlideDistance, WindowSlideSpeed);

        private static IWindowAnimation LeftSideWindowAnimation =>
            new WindowSlide(WindowSlide.SlideDirection.Right, WindowSlideDistance, WindowSlideSpeed);

        private static IWindowAnimation BottomWindowAnimation =>
            new WindowSlide(WindowSlide.SlideDirection.Up, WindowSlideDistance, WindowSlideSpeed);

        #region View Management

        public void HidePromptWindow()
        {
            UserPromptWindow.Visible = false;
        }

        #endregion View Management

        #region Generation

        public void GenerateHelpTextWindow(WindowContentGrid helpTextContent)
        {
            Color helpTextWindowColor = new Color(20, 20, 20, 200);
            HelpTextWindow = new Window(helpTextContent, helpTextWindowColor);
        }

        public void GenerateUserPromptWindow(WindowContentGrid promptTextContent, Vector2 sizeOverride)
        {
            Color promptWindowColor = new Color(40, 30, 40, 200);
            UserPromptWindow = new Window(promptTextContent, promptWindowColor,
                sizeOverride);
        }

        #region Attacker Windows

        public void GenerateAttackerSpriteWindow(GameUnit attacker, Color spriteColor, UnitAnimationState state)
        {
            AttackerSpriteWindow =
                new AnimatedWindow(BattlerWindow(attacker, spriteColor, state), LeftSideWindowAnimation);
        }

        public void GenerateAttackerDamageWindow(Color attackerWindowColor, CombatDamage attackerDamage)
        {
            AttackerDiceWindow =
                new AnimatedWindow(new Window(attackerDamage, attackerWindowColor), LeftSideWindowAnimation);
        }

        public void GenerateAttackerInRangeWindow(Color attackerWindowColor, bool inRange)
        {
            AttackerRangeWindow =
                new AnimatedWindow(RangeWindow(attackerWindowColor, inRange), LeftSideWindowAnimation);
        }

        public void GenerateAttackerBonusWindow(TerrainBonus terrainBonus, Color attackerWindowColor)
        {
            AttackerBonusWindow =
                new AnimatedWindow(BonusWindow(terrainBonus, attackerWindowColor), LeftSideWindowAnimation);
        }

        public void GenerateAttackerAtkWindow(Color windowColor, UnitStatistics attackerStats, Stats combatStat)
        {
            AttackerAtkWindow = new AnimatedWindow(CombatStatWindow(windowColor, attackerStats, combatStat),
                LeftSideWindowAnimation);
        }

        public void GenerateAttackerHpWindow(Color windowColor, GameUnit attacker)
        {
            AttackerHpWindow = new AnimatedWindow(GenerateHpWindow(attacker, windowColor), LeftSideWindowAnimation);
        }

        public void GenerateAttackerDetailWindow(Color attackerWindowColor,
            IRenderable attackerDetail)
        {
            AttackerDetailWindow =
                new AnimatedWindow(new Window(attackerDetail, attackerWindowColor), BottomWindowAnimation);
        }

        public void GenerateAttackerPortraitWindow(Color attackerWindowColor, IRenderable attackerPortrait)
        {
            AttackerPortraitWindow =
                new AnimatedWindow(new Window(attackerPortrait, attackerWindowColor), BottomWindowAnimation);
        }

        #endregion Attacker Windows

        #region Defender Windows

        public void GenerateDefenderSpriteWindow(GameUnit defender, Color spriteColor, UnitAnimationState state)
        {
            DefenderSpriteWindow =
                new AnimatedWindow(BattlerWindow(defender, spriteColor, state), RightSideWindowAnimation);
        }


        public void GenerateDefenderDamageWindow(Color defenderWindowColor, CombatDamage defenderDamage)
        {
            DefenderDiceWindow =
                new AnimatedWindow(new Window(defenderDamage, defenderWindowColor), RightSideWindowAnimation);
        }

        public void GenerateDefenderRangeWindow(Color defenderWindowColor, bool inRange)
        {
            DefenderRangeWindow =
                new AnimatedWindow(RangeWindow(defenderWindowColor, inRange), RightSideWindowAnimation);
        }


        public void GenerateDefenderBonusWindow(TerrainBonus terrainBonus, Color defenderWindowColor)
        {
            DefenderBonusWindow =
                new AnimatedWindow(BonusWindow(terrainBonus, defenderWindowColor), RightSideWindowAnimation);
        }


        public void GenerateDefenderRetWindow(Color windowColor, UnitStatistics defenderStats, Stats combatStat)
        {
            DefenderAtkWindow = new AnimatedWindow(CombatStatWindow(windowColor, defenderStats, combatStat),
                RightSideWindowAnimation);
        }


        public void GenerateDefenderHpWindow(Color windowColor, GameUnit defender)
        {
            DefenderHpWindow = new AnimatedWindow(GenerateHpWindow(defender, windowColor), RightSideWindowAnimation);
        }

        public void GenerateDefenderDetailWindow(Color defenderWindowColor,
            IRenderable defenderDetail)
        {
            DefenderDetailWindow =
                new AnimatedWindow(new Window(defenderDetail, defenderWindowColor), BottomWindowAnimation);
        }

        public void GenerateDefenderPortraitWindow(Color defenderWindowColor, IRenderable defenderPortrait)
        {
            DefenderPortraitWindow =
                new AnimatedWindow(new Window(defenderPortrait, defenderWindowColor), BottomWindowAnimation);
        }

        #endregion Defender Windows

        #region Window Generators

        private static Window GenerateHpWindow(GameUnit unit, Color windowColor)
        {
            return new Window(unit.GetCombatHealthBar(HpBarSize), windowColor);
        }

        private static Window RangeWindow(Color windowColor, bool inRange)
        {
            IRenderable[,] defenderRangeContent =
            {
                {
                    UnitStatistics.GetSpriteAtlas(Stats.AtkRange),
                    new RenderText(AssetManager.WindowFont, "In Range: "),
                    new RenderText(AssetManager.WindowFont, inRange.ToString(),
                        (inRange) ? GameContext.PositiveColor : GameContext.NegativeColor)
                }
            };

            WindowContentGrid defenderRangeContentGrid = new WindowContentGrid(defenderRangeContent, 1);
            return new Window(defenderRangeContentGrid, windowColor);
        }

        private static Window BonusWindow(TerrainBonus terrainBonus, Color attackerWindowColor)
        {
            IRenderable[,] attackerBonusContent =
            {
                {
                    ((terrainBonus.AtkBonus + terrainBonus.RetBonus + terrainBonus.BlockBonus +
                      terrainBonus.LuckBonus) > 0)
                        ? UnitStatistics.GetSpriteAtlas(Stats.Positive)
                        : UnitStatistics.GetSpriteAtlas(Stats.Negative),

                    new RenderText(AssetManager.WindowFont, "Bonus")
                }
            };
            WindowContentGrid attackerBonusContentGrid = new WindowContentGrid(attackerBonusContent, 0);
            return new Window(attackerBonusContentGrid, attackerWindowColor);
        }

        private static Window BattlerWindow(GameUnit attacker, Color spriteColor, UnitAnimationState state)
        {
            const int spriteSize = 200;

            return new Window(
                new WindowContentGrid(
                    new[,]
                    {
                        {
                            attacker.GetMapSprite(new Vector2(spriteSize), spriteColor, state)
                        }
                    },
                    1
                ),
                Color.Transparent
            );
        }

        private static Window CombatStatWindow(Color windowColor, UnitStatistics stats, Stats combatStat)
        {
            int statValue;
            int baseStatValue;

            switch (combatStat)
            {
                case Stats.Atk:
                    statValue = stats.Atk;
                    baseStatValue = stats.BaseAtk;
                    break;
                case Stats.Retribution:
                    statValue = stats.Ret;
                    baseStatValue = stats.BaseRet;
                    break;
                default:
                    statValue = -1;
                    baseStatValue = -1;
                    break;
            }

            WindowContentGrid atkContentGrid = new WindowContentGrid(
                new IRenderable[,]
                {
                    {
                        UnitStatistics.GetSpriteAtlas(combatStat, new Vector2(GameDriver.CellSize)),
                        new RenderText(AssetManager.WindowFont, UnitStatistics.Abbreviation[combatStat] + ": "),
                        new RenderText(
                            AssetManager.WindowFont,
                            statValue.ToString(),
                            UnitStatistics.DetermineStatColor(statValue, baseStatValue)
                        )
                    }
                },
                1
            );
            return new Window(atkContentGrid, windowColor);
        }

        #endregion Window Generators

        #endregion Generation

        #region Window Positions

        private Vector2 HelpTextWindowPosition()
        {
            //Top-left
            return WindowEdgeBuffer;
        }

        private Vector2 UserPromptWindowPosition()
        {
            return new Vector2(
                GameDriver.ScreenSize.X / 2 - (float) UserPromptWindow.Width / 2,
                GameDriver.ScreenSize.Y - UserPromptWindow.Height - WindowSpacing
            );
        }

        private static float RightAlignWindow(IRenderable placingWindow, IRenderable referenceWindow,
            float referenceWindowXPosition)
        {
            return referenceWindowXPosition + referenceWindow.Width - placingWindow.Width;
        }

        private static float BottomAlignWindow(IRenderable placingWindow, IRenderable referenceWindow,
            float referenceWindowYPosition)
        {
            return referenceWindowYPosition + referenceWindow.Height - placingWindow.Height;
        }

        #region Attacker

        private Vector2 AttackerDetailWindowPosition()
        {
            //Anchored right of portrait window
            Vector2 attackerPortraitWindowPosition = AttackerPortraitWindowPosition();

            return new Vector2(
                attackerPortraitWindowPosition.X + AttackerPortraitWindow.Width,
                BottomAlignWindow(AttackerDetailWindow, AttackerPortraitWindow, attackerPortraitWindowPosition.Y)
            );
        }

        private Vector2 AttackerPortraitWindowPosition()
        {
            //Bottom-left of screen
            return new Vector2(
                WindowSpacing,
                GameDriver.ScreenSize.Y - AttackerPortraitWindow.Height - WindowSpacing
            );
        }

        private Vector2 AttackerHpWindowPosition()
        {
            //Anchored above Sprite window
            Vector2 attackerSpriteWindowPosition = AttackerSpriteWindowPosition();

            return new Vector2(
                RightAlignWindow(AttackerHpWindow, AttackerSpriteWindow, attackerSpriteWindowPosition.X),
                attackerSpriteWindowPosition.Y - AttackerHpWindow.Height - WindowSpacing
            );
        }

        private Vector2 AttackerAtkWindowPosition()
        {
            //Anchored below Range window
            Vector2 attackerRangeWindowPosition = AttackerRangeWindowPosition();

            return new Vector2(
                RightAlignWindow(AttackerAtkWindow, AttackerRangeWindow, attackerRangeWindowPosition.X),
                attackerRangeWindowPosition.Y + AttackerRangeWindow.Height + WindowSpacing
            );
        }

        private Vector2 AttackerBonusWindowPosition()
        {
            //Anchored below Atk window
            Vector2 attackerAtkWindowPosition = AttackerAtkWindowPosition();

            return new Vector2(
                RightAlignWindow(AttackerBonusWindow, AttackerAtkWindow, attackerAtkWindowPosition.X),
                attackerAtkWindowPosition.Y + AttackerAtkWindow.Height + WindowSpacing
            );
        }

        private Vector2 AttackerRangeWindowPosition()
        {
            Vector2 attackerDiceWindowPosition = AttackerDiceWindowPosition();
            //Anchored beneath Dice Window
            return new Vector2(
                RightAlignWindow(AttackerRangeWindow, AttackerDiceWindow, attackerDiceWindowPosition.X),
                attackerDiceWindowPosition.Y + AttackerDiceWindow.Height + WindowSpacing
            );
        }

        private Vector2 AttackerDiceWindowPosition()
        {
            Vector2 attackerSpriteWindowPosition = AttackerSpriteWindowPosition();
            //Anchored beneath Sprite Window
            return new Vector2(
                RightAlignWindow(AttackerDiceWindow, AttackerSpriteWindow, attackerSpriteWindowPosition.X),
                attackerSpriteWindowPosition.Y + AttackerSpriteWindow.Height + WindowSpacing
                - ((float) AttackerSpriteWindow.Height / 3)
            );
        }

        private Vector2 AttackerSpriteWindowPosition()
        {
            //Center of screen
            return GameDriver.ScreenSize / 2 -
                   new Vector2(AttackerSpriteWindow.Width + WindowSpacing, AttackerSpriteWindow.Height);
        }

        #endregion Attacker

        #region Defender

        private Vector2 DefenderDetailWindowPosition()
        {
            //Anchored left of portrait window
            (float x, float y) = DefenderPortraitWindowPosition();

            return new Vector2(
                x - DefenderDetailWindow.Width,
                BottomAlignWindow(DefenderDetailWindow, DefenderPortraitWindow, y)
            );
        }

        private Vector2 DefenderPortraitWindowPosition()
        {
            //Bottom-right of screen
            return new Vector2(
                GameDriver.ScreenSize.X - DefenderPortraitWindow.Width - WindowSpacing,
                GameDriver.ScreenSize.Y - DefenderPortraitWindow.Height - WindowSpacing
            );
        }

        private Vector2 DefenderHpWindowPosition()
        {
            //Anchored above Sprite window
            (float x, float y) = DefenderSpriteWindowPosition();

            return new Vector2(
                x,
                y - DefenderHpWindow.Height - WindowSpacing
            );
        }

        private Vector2 DefenderAtkWindowPosition()
        {
            //Anchored below Range window
            (float x, float y) = DefenderRangeWindowPosition();

            return new Vector2(
                x,
                y + DefenderRangeWindow.Height + WindowSpacing
            );
        }

        private Vector2 DefenderBonusWindowPosition()
        {
            //Anchored below Atk window
            (float x, float y) = DefenderAtkWindowPosition();

            return new Vector2(
                x,
                y + DefenderAtkWindow.Height + WindowSpacing
            );
        }

        private Vector2 DefenderDiceWindowPosition()
        {
            //Anchored beneath Sprite Window
            (float x, float y) = DefenderSpriteWindowPosition();

            return new Vector2(
                x,
                y + DefenderSpriteWindow.Height + WindowSpacing
                - ((float) DefenderSpriteWindow.Height / 3)
            );
        }

        private Vector2 DefenderRangeWindowPosition()
        {
            //Anchored beneath Dice Window
            (float x, float y) = DefenderDiceWindowPosition();

            return new Vector2(
                x,
                y + DefenderDiceWindow.Height + WindowSpacing
            );
        }

        private Vector2 DefenderSpriteWindowPosition()
        {
            return GameDriver.ScreenSize / 2 - new Vector2(-WindowSpacing, DefenderSpriteWindow.Height);
        }

        #endregion Defender

        #endregion Window Positions

        public void ToggleVisible()
        {
            visible = !visible;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!visible) return;

            if (HelpTextWindow != null)
            {
                HelpTextWindow.Draw(spriteBatch, HelpTextWindowPosition());

                if (AttackerPortraitWindow != null)
                {
                    AttackerPortraitWindow.Draw(spriteBatch, AttackerPortraitWindowPosition());
                    AttackerDetailWindow.Draw(spriteBatch, AttackerDetailWindowPosition());
                    AttackerHpWindow.Draw(spriteBatch, AttackerHpWindowPosition());
                    AttackerAtkWindow.Draw(spriteBatch, AttackerAtkWindowPosition());
                    AttackerBonusWindow.Draw(spriteBatch, AttackerBonusWindowPosition());
                    AttackerRangeWindow.Draw(spriteBatch, AttackerRangeWindowPosition());
                    AttackerDiceWindow.Draw(spriteBatch, AttackerDiceWindowPosition());
                    AttackerSpriteWindow.Draw(spriteBatch,
                        AttackerSpriteWindowPosition());
                }

                if (DefenderPortraitWindow != null)
                {
                    DefenderPortraitWindow.Draw(spriteBatch, DefenderPortraitWindowPosition());
                    DefenderDetailWindow.Draw(spriteBatch, DefenderDetailWindowPosition());
                    DefenderHpWindow.Draw(spriteBatch, DefenderHpWindowPosition());
                    DefenderAtkWindow.Draw(spriteBatch, DefenderAtkWindowPosition());
                    DefenderBonusWindow.Draw(spriteBatch, DefenderBonusWindowPosition());
                    DefenderRangeWindow.Draw(spriteBatch, DefenderRangeWindowPosition());
                    DefenderDiceWindow.Draw(spriteBatch, DefenderDiceWindowPosition());
                    DefenderSpriteWindow.Draw(spriteBatch, DefenderSpriteWindowPosition());
                }
            }

            UserPromptWindow?.Draw(spriteBatch, UserPromptWindowPosition());
        }
    }
}