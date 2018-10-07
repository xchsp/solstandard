﻿using SolStandard.Entity;
using SolStandard.Utility.Monogame;

namespace SolStandard.Utility.Events
{
    public class ToggleOpenEvent : IEvent
    {
        private readonly IOpenable openable;
        public bool Complete { get; private set; }

        public ToggleOpenEvent(IOpenable openable)
        {
            this.openable = openable;
            Complete = false;
        }

        public void Continue()
        {
            if (!openable.IsOpen)
            {
                openable.Open();
            }
            else
            {
                openable.Close();
            }

            Complete = true;
        }
    }
}