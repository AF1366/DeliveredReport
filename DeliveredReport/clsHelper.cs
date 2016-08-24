using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace DeliveredReport
{
    class clsHelper
    {
        clsDAL DAL = new clsDAL();

        #region ComboDBEvent==>Add to dicDBName & Load cmbxTbl
        /// <summary>
        /// اضافه کردن نام پایگاه به دیکشنری و 
        /// پرکردن کمبو مربوط به لیست جداول
        /// </summary>
        /// <param name="State">شماره جدول</param>
        /// <param name="dicDBName">دیکشنری نام دیتابیس</param>
        /// <param name="cmbxDB">کمبو لیست نام دیتابیس</param>
        /// <param name="cmbxTbl">کمبو لیست نام جداول</param>
        public void ComboDBEvent(int State, Dictionary<int, string> dicDBName, ComboBox cmbxDB, ComboBox cmbxTbl)
        {
            if (!dicDBName.ContainsKey(State)) { dicDBName.Add(State, cmbxDB.Text); }
            else { dicDBName[State] = cmbxDB.Text; }

            DAL.ComboBoxSource(cmbxTbl, DAL.sqlGetTableNameInDB(dicDBName[State]));
        }
        public void ComboDBEvent(int State, Dictionary<int, string> dicDBName, string cmbxDB, ComboBox cmbxTbl)
        {
            if (!dicDBName.ContainsKey(State)) { dicDBName.Add(State, cmbxDB); }
            else { dicDBName[State] = cmbxDB; }

            DAL.ComboBoxSource(cmbxTbl, DAL.sqlGetTableNameInDB(dicDBName[State]));
        }

        #endregion ComboDBEvent==>Add to dicDBName & Load cmbxTbl

        #region LoadColumn==> In CheckedListBox & ComboBox

        /// <summary>
        /// اضافه کردن نام ستون ها به چک لیست
        /// </summary>
        /// <param name="DBName">نام دیتابیس</param>
        /// <param name="cmbxTbl">کمبو لیست نام جداول</param>
        /// <param name="ChkLstBx">چک لیست باکس</param>
        public void LoadColumn(string DBName, ComboBox cmbxTbl, CheckedListBox ChkLstBx)
        {
            ChkLstBx.Items.Clear();
            DataTable dt = DAL.sqlColumnsNameCLB(DBName, cmbxTbl.Text);
            for (int i = 0; i < dt.Rows.Count; i++)
            { ChkLstBx.Items.Add(dt.Rows[i][0].ToString()); }
        }

        public void LoadColumnNew(string DBName, ComboBox cmbxTbl, CheckedListBox ChkLstBx)
        {
            ChkLstBx.Items.Clear();
            DataTable dt = DAL.sqlColumnsNameCLBNew(DBName, cmbxTbl.Text);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][4].ToString() == "")
                {
                    if (dt.Rows[i][2].ToString() == "NO")
                    {
                        ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + "] not null");
                    }
                    else
                    {
                        ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + "] null");
                    }
                }
                else
                {
                    if (dt.Rows[i][4].ToString() == "-1")
                    {
                        if (dt.Rows[i][2].ToString() == "NO")
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (max)] not null");
                        }
                        else
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (max)] null");
                        }
                    }
                    else
                    {
                        if (dt.Rows[i][2].ToString() == "NO")
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (" + dt.Rows[i][4].ToString() + ")] not null");
                        }
                        else
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (" + dt.Rows[i][4].ToString() + ")] null");
                        }
                    }

                }

            }
        }
        /// <summary>
        /// اضافه کردن نام ستون ها به کمبو 
        /// </summary>
        /// <param name="DBName">نام دیتابیس</param>
        /// <param name="cmbxTbl">کمبو لیست نام جداول</param>
        /// <param name="cmbxTblColmn">کمبو که پرمیشود</param>
        public void LoadColumn(string DBName, ComboBox cmbxTbl, ComboBox cmbxTblColmn)
        {
            DataTable dt = DAL.sqlColumnsName(DBName, cmbxTbl.Text);
            DAL.ComboBoxSource(cmbxTblColmn, dt);
        }
        #endregion LoadColumn==> In CheckedListBox & ComboBox

        #region Select & Unselect
        /// <summary>
        /// انتخاب و حذف انتخاب همه رکوردها
        /// </summary>
        /// <param name="ChkLstBx">چک لیست</param>
        /// <param name="btnSelect">نام دکمه</param>
        public void SelectUnselect(CheckedListBox ChkLstBx, Button btnSelect)
        {
            if (btnSelect.Text == "SelectAll")
            {
                for (int i = 0; i < ChkLstBx.Items.Count; i++)
                { ChkLstBx.SetItemChecked(i, true); }
                btnSelect.Text = "UnselectAll";
            }
            else
            {
                for (int i = 0; i < ChkLstBx.Items.Count; i++)
                { ChkLstBx.SetItemChecked(i, false); }
                btnSelect.Text = "SelectAll";
            }
        }
        #endregion Select & Unselect

        #region View ==> CraeteView & Add column name to list
        /// <summary>
        /// حذف ویو در صورت وجود
        /// ساخت ویو
        /// پرکردن لیست از نام ستون ها
        /// </summary>
        /// <param name="ChkLstBx">چک لیست</param>
        /// <param name="DBName">نام دیتابیس</param>
        /// <param name="cmbxTbl">کمبو نام جداول</param>
        /// <param name="lstColumnName">لیست نام ستون ها</param>
        public void CraeteView(CheckedListBox ChkLstBx, string DBName, ComboBox cmbxTbl, List<string> lstColumnName)
        {
            DAL.sqlCommand("DROP VIEW [" + cmbxTbl.Text + "0]", DBName);

            string strColumnName = string.Empty;

            #region if
            if (ChkLstBx.CheckedItems.Count != 0)
            {
                for (int i = 0; i < ChkLstBx.CheckedItems.Count; i++)
                {
                    if (i == 0) { strColumnName = ChkLstBx.Items[i].ToString(); }
                    if (i > 0) { strColumnName = strColumnName + "," + ChkLstBx.Items[i].ToString(); }
                    lstColumnName.Add(ChkLstBx.Items[i].ToString());
                }
            }
            else { strColumnName = "*"; }
            #endregion if

            string q = "CREATE VIEW [" + cmbxTbl.Text + "0] AS " + "SELECT " + strColumnName + " FROM " + cmbxTbl.Text;
            DAL.sqlCommand(q, DBName, "View");
        }
        #endregion View ==> CraeteView & Add column name to list

        #region Join Views

        ///// <summary>
        ///// انتخاب ستونها مختلف از جداول مختلف و نمایش به صورت یکجا
        ///// </summary>
        ///// <param name="lstAllColumn">لیست نام ستون ها</param>
        ///// <param name="dicDBName">دیکشنری نام دیتابیس</param>
        ///// <param name="lstTblName">لیست نام جداول</param>
        ///// <param name="lstTblColmn">لیست نام ستون های مشترک</param>
        ///// <returns></returns>
        //public string JoinViews(List<string> lstAllColumn, Dictionary<int, string> dicDBName, List<ComboBox> lstTblName, List<ComboBox> lstTblColmn, string JoinType)
        //{
        //    List<string> lstUniqColumn = new List<string>();
        //    for (int i = 0; i < lstAllColumn.Count; i++)
        //    { if (!lstUniqColumn.Contains(lstAllColumn[i])) { lstUniqColumn.Add(lstAllColumn[i]); } }
        //    string AllColumn = string.Join(",", lstUniqColumn);

        //    string a = lstTblColmn[0].Text; string b = lstTblColmn[1].Text; string c = lstTblColmn[2].Text;
        //    string EndQ = " SELECT a." + AllColumn + " FROM [" + lstTblName[0].Text + "0] a " +
        //                  " "+JoinType+" " + dicDBName[2] + ".dbo.[" + lstTblName[1].Text + "0] b ON a.[" + a + "]=b.[" + b + "] " +
        //                  " " + JoinType + " " + dicDBName[3] + ".dbo.[" + lstTblName[2].Text + "0] c ON a.[" + a + "]=c.[" + c + "] ";
        //    return EndQ;
        //}

        ////public string JoinViews(List<string> lstAllColumn, Dictionary<int, string> dicDBName, List<ComboBox> lstTblName, List<ComboBox> lstTblColmn, string JoinType, string ExportName)
        //{
        //    List<string> lstUniqColumn = new List<string>();
        //    for (int i = 0; i < lstAllColumn.Count; i++)
        //    { if (!lstUniqColumn.Contains(lstAllColumn[i])) { lstUniqColumn.Add(lstAllColumn[i]); } }
        //    string AllColumn = string.Join(",", lstUniqColumn);

        //    string a = lstTblColmn[0].Text; string b = lstTblColmn[1].Text; string c = lstTblColmn[2].Text;
        //    string EndQ = " SELECT a." + AllColumn + " INTO [" + ExportName + "] FROM [" + lstTblName[0].Text + "0] a " +
        //                  " "+JoinType+" " + dicDBName[2] + ".dbo.[" + lstTblName[1].Text + "0] b ON a.[" + a + "]=b.[" + b + "] " +
        //                  " " + JoinType + " " + dicDBName[3] + ".dbo.[" + lstTblName[2].Text + "0] c ON a.[" + a + "]=c.[" + c + "] ";
        //    return EndQ;
        //}

        #endregion Join Views

        public string JoinViews(List<string> lstAllColumn, Dictionary<int, string> dicDBName, List<ComboBox> lstTblName, List<ComboBox> lstTblColmn, string JoinType)
        {
            List<string> lstUniqColumn = new List<string>();
            for (int i = 0; i < lstAllColumn.Count; i++)
            { if (!lstUniqColumn.Contains(lstAllColumn[i])) { lstUniqColumn.Add(lstAllColumn[i]); } }
            string AllColumn = string.Join(",", lstUniqColumn);

            string a = lstTblColmn[0].Text; string b = lstTblColmn[1].Text; string c = lstTblColmn[2].Text;
            string EndQ = " SELECT a." + AllColumn + " FROM [" + lstTblName[0].Text + "0] a " +
                          " " + JoinType + " " + dicDBName[2] + ".dbo.[" + lstTblName[1].Text + "0] b ON a.[" + a + "]=b.[" + b + "] " +
                          " " + JoinType + " " + dicDBName[3] + ".dbo.[" + lstTblName[2].Text + "0] c ON a.[" + a + "]=c.[" + c + "] ";
            return EndQ;
        }

        public string JoinViewsExport(List<string> lstAllColumn, Dictionary<int, string> dicDBName, List<ComboBox> lstTblName, List<ComboBox> lstTblColmn, string JoinType, string ExportName)
        {
            List<string> lstUniqColumn = new List<string>();
            for (int i = 0; i < lstAllColumn.Count; i++)
            { if (!lstUniqColumn.Contains(lstAllColumn[i])) { lstUniqColumn.Add(lstAllColumn[i]); } }
            string AllColumn = string.Join(",", lstUniqColumn);

            string a = lstTblColmn[0].Text; string b = lstTblColmn[1].Text; string c = lstTblColmn[2].Text;
            string EndQ = " SELECT a." + AllColumn + " INTO " + ExportName + " FROM [" + lstTblName[0].Text + "0] a " +
                          " " + JoinType + " " + dicDBName[2] + ".dbo.[" + lstTblName[1].Text + "0] b ON a.[" + a + "]=b.[" + b + "] " +
            " " + JoinType + " " + dicDBName[3] + ".dbo.[" + lstTblName[2].Text + "0] c ON a.[" + a + "]=c.[" + c + "] ";
            return EndQ;
        }

        public string JoinViewsTwo(List<string> lstAllColumn, Dictionary<int, string> dicDBName, List<ComboBox> lstTblName, List<ComboBox> lstTblColmn, string JoinType)
        {
            List<string> lstUniqColumn = new List<string>();
            for (int i = 0; i < lstAllColumn.Count; i++)
            { if (!lstUniqColumn.Contains(lstAllColumn[i])) { lstUniqColumn.Add(lstAllColumn[i]); } }
            string AllColumn = string.Join(",", lstUniqColumn);

            string a = lstTblColmn[0].Text; string b = lstTblColmn[1].Text; string c = lstTblColmn[2].Text;
            string EndQ = " SELECT a." + AllColumn + " FROM [" + lstTblName[0].Text + "0] a " +
                          " " + JoinType + " " + dicDBName[2] + ".dbo.[" + lstTblName[1].Text + "0] b ON a.[" + a + "]=b.[" + b + "] ";
            return EndQ;
        }

        public string JoinViewsExportTwo(List<string> lstAllColumn, Dictionary<int, string> dicDBName, List<ComboBox> lstTblName, List<ComboBox> lstTblColmn, string JoinType, string ExportName)
        {
            List<string> lstUniqColumn = new List<string>();
            for (int i = 0; i < lstAllColumn.Count; i++)
            { if (!lstUniqColumn.Contains(lstAllColumn[i])) { lstUniqColumn.Add(lstAllColumn[i]); } }
            string AllColumn = string.Join(",", lstUniqColumn);

            string a = lstTblColmn[0].Text; string b = lstTblColmn[1].Text; string c = lstTblColmn[2].Text;
            string EndQ = " SELECT a." + AllColumn + " INTO " + ExportName + " FROM [" + lstTblName[0].Text + "0] a " +
                          " " + JoinType + " " + dicDBName[2] + ".dbo.[" + lstTblName[1].Text + "0] b ON a.[" + a + "]=b.[" + b + "] ";
            return EndQ;
        }


        public void LoadColumnlist(string DBName, ComboBox cmbxTbl, ListBox LstBx)
        {
            LstBx.Items.Clear();
            DataTable dt = DAL.sqlColumnsNameCLBNew(DBName, cmbxTbl.Text);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][4].ToString() == "")
                {
                    if (dt.Rows[i][2].ToString() == "NO")
                    {
                        LstBx.Items.Add(dt.Rows[i][1].ToString());
                    }
                    else
                    {
                        LstBx.Items.Add(dt.Rows[i][1].ToString());
                    }
                }
                else
                {
                    if (dt.Rows[i][4].ToString() == "-1")
                    {
                        if (dt.Rows[i][2].ToString() == "NO")
                        {
                            LstBx.Items.Add(dt.Rows[i][1].ToString());
                        }
                        else
                        {
                            LstBx.Items.Add(dt.Rows[i][1].ToString());
                        }
                    }
                    else
                    {
                        if (dt.Rows[i][2].ToString() == "NO")
                        {
                            LstBx.Items.Add(dt.Rows[i][1].ToString());
                        }
                        else
                        {
                            LstBx.Items.Add(dt.Rows[i][1].ToString());
                        }
                    }

                }

            }
        }

    }
}
