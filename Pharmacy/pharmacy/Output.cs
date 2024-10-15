using System;
using System.Windows.Forms;

namespace pharmacy
{
    class Output
    {
        static SaveFileDialog save = new SaveFileDialog();
        public static bool SaveToExcel(DataGridView dgv, bool showId = false)
        {

            save.Title = "Save as Excel File";
            save.FileName = "";
            save.Filter = "Excel Files(2003)|*.xls|Excel Files(2007)|*.xlsx";
            if (save.ShowDialog() == DialogResult.Cancel)
                return false;
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
            ExcelApp.Columns.AutoFit();

            for (int i = 1; i < dgv.Columns.Count + 1; i++)
            {
                if (dgv.Columns[i - 1].Visible || showId)
                    ExcelApp.Cells[1, i] = dgv.Columns[i - 1].HeaderText;
            }

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    if (dgv.Columns[j].Visible || showId)
                        ExcelApp.Cells[i + 2, j + 1] = dgv.Rows[i].Cells[j].Value.ToString();
                }
            }
            ExcelApp.ActiveWorkbook.SaveAs(save.FileName);
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Visible = true;
            //ExcelApp.Quit();
            return true;
        }
    }
}
