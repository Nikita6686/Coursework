using System.Windows.Forms;

namespace AttractionParkApp
{
    public static class GridHelper
    {
        public static void FitColumns(DataGridView grid)
        {
            if (grid.Columns.Count == 0)
                return;

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            foreach (DataGridViewColumn column in grid.Columns)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            grid.ScrollBars = ScrollBars.Both;
        }
    }
}
