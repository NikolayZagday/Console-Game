using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class MultiItem : MenuItem, IMenuMultiItem
    {
        private int SelectedItemId { get; set; }
        public MenuSubItem SelectedItem { get; set; }
        public List<MenuSubItem> SubItems { get; set; }
        private Stopwatch SwitchSW { get; set; } = new Stopwatch();
        private double SwitchsPerSecond { get; set; } = 5;

        private MultiItem()
        {

        }

        public MultiItem(string title, List<MenuSubItem> subItems)
        {
            Title = title;
            SubItems = subItems;

            SelectedItemId = 0;
            SelectedItem = SubItems[SelectedItemId];

            SwitchSW.Start();
        }

        public void SwitchToNextItem()
        {
            if (CanSwitch())
            {
                if (SubItems.Count <= SelectedItemId + 1)
                {
                    return;
                }
                else
                {
                    SelectedItemId++;
                    SelectedItem = SubItems[SelectedItemId];
                }
            }
        }

        public void SwitchToPreviousItem()
        {
            if (CanSwitch())
            {
                if (SelectedItemId - 1 < 0)
                {
                    return;
                }
                else
                {
                    SelectedItemId--;
                    SelectedItem = SubItems[SelectedItemId];
                }
            }
        }

        public string GetFullString()
        {
            return Title + " : " + SelectedItem.Title; 
        }

        private bool CanSwitch()
        {
            bool IsSwitchRateOK = SwitchSW.Elapsed.TotalSeconds > (1 / SwitchsPerSecond);
            if (IsSwitchRateOK)
            {
                SwitchSW = Stopwatch.StartNew();
                SwitchSW.Start();
            }

            return IsSwitchRateOK;
        }
    }
}
