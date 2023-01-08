using Bannerlord.ButterLib.HotKeys;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.InputSystem;

namespace RealmsForgotten.HotKeys.Battle
{
    public class SelectPotionKey : HotKeyBase
    {
        protected override string DisplayName { get; }
        protected override string Description { get; }
        protected override InputKey DefaultKey { get; }
        protected override string Category { get; }

        public SelectPotionKey() : base(nameof(SelectPotionKey))
        {
            DisplayName = "Select Potion";
            Description = "Cycles through available potion types";
            DefaultKey = TaleWorlds.InputSystem.InputKey.R;
            Category = "Magic";
        }
    }

    public class OpenSpellBook : HotKeyBase
    {
        protected override string DisplayName { get; }
        protected override string Description { get; }
        protected override InputKey DefaultKey { get; }
        protected override string Category { get; }

        public OpenSpellBook() : base(nameof(OpenSpellBook))
        {
            DisplayName = "Open Spell Book";
            Description = "Opens your spell book";
            DefaultKey = TaleWorlds.InputSystem.InputKey.LeftAlt| InputKey.B;
            Category = "Magic";
        }
    }
}
