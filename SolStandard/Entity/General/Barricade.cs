using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Utility;

namespace SolStandard.Entity.General
{
    public class Barricade : BreakableObstacle, IItem
    {
        public string ItemPool { get; private set; }
        
        public Barricade(string name, string type, IRenderable sprite, Vector2 mapCoordinates, int hp, string itemPool) :
            base(name, type, sprite, mapCoordinates, new Dictionary<string, string>(), hp, false, hp < 1, 0)
        {
            ItemPool = itemPool;
        }

        public IRenderable Icon
        {
            get { return Sprite; }
        }


        public UnitAction UseAction()
        {
            return new DeployBarricadeAction(this);
        }

        public UnitAction DropAction()
        {
            return new DeployBarricadeAction(this);
        }

        public IItem Duplicate()
        {
            return new Barricade(Name, Type, Sprite, MapCoordinates, HP, ItemPool);
        }
    }
}