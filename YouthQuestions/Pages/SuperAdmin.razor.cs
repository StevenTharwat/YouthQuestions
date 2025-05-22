using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using YouthQuestions;
using YouthQuestions.Shared;
using System.Data;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Cards;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Lists;
using Syncfusion.Blazor.Popups;
using System.Dynamic;
using Syncfusion.Blazor.Grids;

namespace YouthQuestions.Pages
{
    public partial class SuperAdmin
    {
        SfGrid<ExpandoObject> Grid { get; set; }
        public bool RenderGrid = true;
        public string txtPass;
        public string errorMessage = "";
        public string txtSql;
        public string sqlErrorMessage = "";
        public bool ifHasPass = false;
        public List<ExpandoObject> Data = new List<ExpandoObject>();

        void UserPassEnter()
        {
            try
            {
                if (txtPass.Trim() == "123789456")
                {
                    ifHasPass = true;
                    Data = GenerateListFromTable(data.getAllQustionSuperAdmin());
                    StateHasChanged();
                }
                else
                {
                    errorMessage = "خطأ";
                    return;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        async Task sqlRunTable()
        {
            try
            {
                if (txtSql.Trim() != "")
                {
                    //Grid.DataSource = Data;
                    //Grid.RefreshHeaderAsync().Wait();
                    //Grid.Refresh();
                    //StateHasChanged();

                    RenderGrid = false;
                    await Task.Yield();
                    Data = GenerateListFromTable(sqlite.sqlReturnTable(txtSql));
                    sqlErrorMessage = "تم";
                    RenderGrid = true;
                }
                else
                {
                    sqlErrorMessage = "من فضلك اكتب الامر";
                    return;
                }
            }
            catch (Exception ex)
            {
                sqlErrorMessage = ex.Message;
                return;
            }

        }

        void sqlRunCommand()
        {
            try
            {
                if (txtSql.Trim() != "")
                {
                    sqlite.sqlStatment(txtSql);
                    sqlErrorMessage = "تم";
                }
                else
                {
                    sqlErrorMessage = "من فضلك اكتب الامر";
                    return;
                }
            }
            catch (Exception ex)
            {
                sqlErrorMessage = ex.Message;
                return;
            }
            
        }

        public List<ExpandoObject> GenerateListFromTable(DataTable input)
        {
            var list = new List<ExpandoObject>();
            foreach (DataRow row in input.Rows)
            {
                System.Dynamic.ExpandoObject e = new System.Dynamic.ExpandoObject();
                foreach (DataColumn col in input.Columns)
                    e.TryAdd(col.ColumnName, row.ItemArray[col.Ordinal]);
                list.Add(e);
            }
            return list;
        }
    }
}