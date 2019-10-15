using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    interface IMenuMultiItem : IMenuItem
    {
        List<MenuSubItem> SubItems { get; set; }
        MenuSubItem SelectedItem { get; set; }

        void SwitchToNextItem();
        void SwitchToPreviousItem();
        string GetFullString();
    }
}
