using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Terrain;
using SolStandard.Utility;

namespace SolStandard.Entity.General.Item
{
    public class RecallCharm : TerrainEntity, IItem, IActionTile
    {
        private readonly int[] deployRange;
        private readonly int usesRemaining;
        public string ItemPool { get; private set; }
        public string RecallId { get; private set; }
        public int[] InteractRange { get; private set; }
        private bool recallDeployed;

        public RecallCharm(string name, string type, IRenderable sprite, Vector2 mapCoordinates, string recallId,
            int[] pickupRange, string itemPool, int[] deployRange, int usesRemaining)
            : base(name, type, sprite, mapCoordinates, new Dictionary<string, string>())
        {
            this.deployRange = deployRange;
            this.usesRemaining = usesRemaining;
            RecallId = recallId;
            InteractRange = pickupRange;
            ItemPool = itemPool;
            recallDeployed = false;
        }

        public UnitAction UseAction()
        {
            if (recallDeployed)
            {
                return new ReturnToRecallPointAction(this);
            }

            return new DeployRecallPointAction(this, deployRange);
        }

        public UnitAction DropAction()
        {
            return new DropGiveItemAction(this);
        }

        public IItem Duplicate()
        {
            return new RecallCharm(Name, Type, Sprite, MapCoordinates, RecallId, InteractRange, ItemPool, deployRange,
                usesRemaining);
        }

        public List<UnitAction> TileActions()
        {
            return new List<UnitAction>
            {
                new PickUpItemAction(this, MapCoordinates)
            };
        }

        public void DeployRecall()
        {
            //FIXME HACK: Re-add this item to the unit's inventory to update the UseAction
            GameContext.ActiveUnit.RemoveItemFromInventory(this);
            recallDeployed = true;
            GameContext.ActiveUnit.AddItemToInventory(this);
        }

        public bool IsBroken
        {
            get { return usesRemaining < 1; }
        }

        public IRenderable Icon
        {
            get { return Sprite; }
        }
    }
}