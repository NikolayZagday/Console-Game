using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleGame
{
    class Menu : Creature
    {
        public IMenuItem SelectedItem { get; set; }

        public bool IsSelectedItemMultiItem
        {
            get
            {
                if (SelectedItem is MultiItem)
                    return true;

                return false;
            }
        }

        private int SelectedItemId { get; set; }
        public List<IMenuItem> Items { get; set; }
        public char ItemPointer { get; set; } = '>';
        private Stopwatch SwitchSW { get; set; } = new Stopwatch();
        private double SwitchsPerSecond { get; set; } = 5;

        private Menu()
        {

        }

        public Menu(ICollection<IMenuItem> items)
        {
            Items = new List<IMenuItem>();
            Items.AddRange(items);

            if (Items.Count != 0)
            {
                SelectedItemId = 0;
                SelectedItem = Items[SelectedItemId];
            }

            SwitchSW.Start();
        }

        public void NextSubItem()
        {
            if (IsSelectedItemMultiItem)
            {
                ((MultiItem)SelectedItem).SwitchToNextItem();
                RecalculatePoints();
            }
        }

        public void PreviousSubItem()
        {
            if (IsSelectedItemMultiItem)
            {
                ((MultiItem)SelectedItem).SwitchToPreviousItem();
                RecalculatePoints();
            }
        }

        public void ExecuteAction()
        {
             SelectedItem.Action?.Invoke();
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

        public void SwitchToLowerItem()
        {
            if (CanSwitch())
            {
                if (Items.Count <= SelectedItemId + 1)
                {
                    return;
                }
                else
                {
                    SelectedItemId++;
                    SelectedItem = Items[SelectedItemId];
                    RecalculatePoints();
                }
            }
        }

        public void SwitchToUpperItem()
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
                    SelectedItem = Items[SelectedItemId];
                    RecalculatePoints();
                }
            }
        }

        public override void RecalculatePoints()
        {
            SkinCoordinates.Clear();

            int local_x = X;
            int local_y = Y;

            foreach(var mItem in Items)
            {
                if (mItem == SelectedItem)
                {
                    string menuLine = "";

                    if (mItem is IMenuMultiItem)
                        menuLine = ItemPointer + " " + ((IMenuMultiItem)mItem).GetFullString();
                    else
                        menuLine = ItemPointer + " " + mItem.Title;

                    for (int i = 0; i < menuLine.Length; i++)
                    {
                        SkinCoordinates.Add(new Point(local_x, local_y), menuLine[i]);
                        local_x++;
                    }

                    local_y++;
                    local_x = X;
                }
                else
                {
                    string menuLine = "";

                    if (mItem is IMenuMultiItem)
                        menuLine = "  " + ((IMenuMultiItem)mItem).GetFullString();
                    else
                        menuLine = "  " + mItem.Title;

                    for (int i = 0; i < menuLine.Length; i++)
                    {
                        SkinCoordinates.Add(new Point(local_x, local_y), menuLine[i]);
                        local_x++;
                    }

                    local_y++;
                    local_x = X;
                }
            }
        }
    }
}
