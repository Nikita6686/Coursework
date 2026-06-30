using System.Linq;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public static class GridHelper
    {
        public static void FitColumns(DataGridView grid)
        {
            if (grid.Columns.Count == 0)
                return;

            UiTheme.StyleGrid(grid);
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            var visibleColumns = grid.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .ToList();

            if (visibleColumns.Count == 0)
                return;

            foreach (var column in visibleColumns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            int totalWidth = visibleColumns.Sum(c => c.Width);
            int available = grid.ClientSize.Width - 4;

            if (available < 80)
                available = grid.Width - 4;

            if (totalWidth > available)
            {
                foreach (var column in visibleColumns)
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                var fillColumn = visibleColumns.Last();
                int fixedWidth = 0;
                foreach (var column in visibleColumns)
                {
                    if (column == fillColumn)
                        continue;

                    int width = System.Math.Max(60, (int)(column.Width * ((double)available / totalWidth)));
                    column.Width = width;
                    fixedWidth += width;
                }

                fillColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                fillColumn.Width = System.Math.Max(80, available - fixedWidth);
            }
            else
            {
                visibleColumns.Last().AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                visibleColumns.Last().MinimumWidth = 80;
            }

            grid.ScrollBars = ScrollBars.Both;
        }
    }
}
