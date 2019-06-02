using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.Unit;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Monogame;

namespace SolStandard.HUD.Menu.Options.DraftMenu
{
    public class DraftUnitOption : MenuOption
    {
        private readonly Role role;
        private readonly Team team;
        private readonly bool enabled;
        private const int PortraitSize = 128;

        public DraftUnitOption(Role role, Team team, bool enabled)
            : base(
                DraftUnitLabelContent(role, team, enabled),
                TeamUtility.DetermineTeamColor(team),
                HorizontalAlignment.Centered
            )
        {
            this.role = role;
            this.team = team;
            this.enabled = enabled;
        }

        private static IRenderable DraftUnitLabelContent(Role role, Team team, bool enabled)
        {
            ITexture2D unitPortraitTexture = UnitGenerator.GetUnitPortrait(role, team);

            SpriteAtlas unitPortraitSprite = new SpriteAtlas(
                unitPortraitTexture,
                new Vector2(unitPortraitTexture.Width, unitPortraitTexture.Height),
                new Vector2(PortraitSize),
                0,
                enabled ? Color.White : GameUnit.DeadPortraitColor
            );


            IRenderable[,] unitInfoContent =
            {
                {
                    unitPortraitSprite
                },
                {
                    new RenderText(AssetManager.WindowFont, role.ToString().ToUpper())
                },
                {
                    new RenderText(AssetManager.MapFont, enabled ? "Available" : "Limit Reached")
                }
            };

            WindowContentGrid unitInfoGrid = new WindowContentGrid(unitInfoContent, 1, HorizontalAlignment.Centered);

            return unitInfoGrid;
        }

        public override void Execute()
        {
            if (enabled)
            {
                GameContext.DraftContext.AddUnitToList(role, team);
            }
            else
            {
                AssetManager.WarningSFX.Play();
            }
        }

        public override IRenderable Clone()
        {
            return new DraftUnitOption(role, team, enabled);
        }
    }
}