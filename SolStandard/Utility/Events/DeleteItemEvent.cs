﻿using SolStandard.Containers.Components.Global;
using SolStandard.Entity;
using SolStandard.Utility.Assets;

namespace SolStandard.Utility.Events
{
    public class DeleteItemEvent : IEvent
    {
        private readonly IItem itemToDelete;
        public bool Complete { get; private set; }

        public DeleteItemEvent(IItem itemToDelete)
        {
            this.itemToDelete = itemToDelete;
        }

        public void Continue()
        {
            GlobalContext.ActiveUnit.RemoveItemFromInventory(itemToDelete);
            AssetManager.MenuConfirmSFX.Play();
            Complete = true;
        }
    }
}