using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class ScrollBoxControl : GameEntity
    {
        public List<IRenderable> Items { get; set; }

        public int MaxRows { get; set; } = 30;
        public int MaxColumns { get; set; } = 120;

        public int CurrentRow { get; set; }

        public int RowsPerSecond { get; set; } = 10;
        private Stopwatch ScrollSW { get; set; } = new Stopwatch();

        public int ScrollStep { get; set; } = 1;
        public int RowCount
        {
            get
            {
                int rows = 0;
                Items.ForEach(x => rows += x.Height);
                return rows;
            }
        }

        public ScrollBoxControl()
        {
            ScrollSW.Start();
        }

        public override void RecalculatePoints()
        {
            SkinCoordinates.Clear();

            foreach (var thing in Items)
            {
                var en = thing.SkinCoordinates.GetEnumerator();

                while (en.MoveNext())
                {
                    var keyPoint = en.Entry.Key;
                    var charValue = en.Entry.Value;

                    if (!SkinCoordinates.ContainsKey(keyPoint))
                        SkinCoordinates.Add(keyPoint, charValue);
                }
            }
        }

        private bool CanScroll()
        {
            if (ScrollSW.Elapsed.TotalSeconds < 1d / RowsPerSecond)
                return false;
            else
                return true;
        }

        public void ScrollDown()
        {
            if (CanScroll())
            {
                CurrentRow += ScrollStep;
            }
        }

        public void ScrollUp()
        {
            if (CanScroll())
            {
                CurrentRow -= ScrollStep;
            }
        }

        //SkinCoordinates.Clear();

        //    int local_x = X;
        //int local_y = Y;

        //    if (Skin.Length == 1)
        //    {
        //        SkinCoordinates.Add(new Point(local_x, local_y), Skin[0]);
        //        return;
        //    }

        //    for (int i = 0; i<Skin.Length - 1; i++)
        //    {
        //        if (Skin[i] == '\n')
        //        {
        //            local_y++;
        //            local_x = X;
        //        }
        //        else
        //        {
        //            local_x++;
        //            SkinCoordinates.Add(new Point(local_x, local_y), Skin[i]);
        //        }
        //    }
    }
}
