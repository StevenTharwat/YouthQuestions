using Syncfusion.Blazor.Lists;
using System.Data;

namespace YouthQuestions.Pages
{
    public partial class Admin
    {
        public SfListView<Question> list;
        public SfListView<Question> EditQuestionlist;
        public string txtPass;
        private List<Question> Data = new List<Question>();
        private List<Question> EditData = new List<Question>();
        string localPass = "";
        public string errorMessage = "";
        public DataTable AnswersTable = new DataTable();

        public Admin()
        {
            try
            {
                refreshData();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        void refreshData()
        {
            Data.Clear();
            EditData.Clear();
            foreach (DataRow item in data.getAllQustionAdmin().Rows)
            {
                string text = item["Question"].ToString();
                //if (item["Name"].ToString().Trim().ToLower() != "null")
                //    text += $" - ({item["Name"]})";
                Data.Add(new Question { Id = item["ID"].ToString(), Text = text, IsChecked = false });
            }

            foreach (DataRow item in data.getAllEditQustionAdmin().Rows)
            {
                string text = item["Question"].ToString();
                //if (item["Name"].ToString().Trim().ToLower() != "null")
                //    text += $" - ({item["Name"]})";
                EditData.Add(new Question { Id = item["ID"].ToString(), Text = text, IsChecked = false });
            }
            AnswersTable = data.getAnswersAdmin();


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    localPass = await localStorage.GetItemAsync<string>("sflistview--6070dfSUQ");
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task btnOk()
        {
            try
            {
                await ApproveQuestions(list);
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task btnEditQuestionOk()
        {
            try
            {
                await ApproveQuestions(EditQuestionlist);
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task ApproveQuestions(SfListView<Question> WatingList)
        {
            var selected = await WatingList.GetCheckedItemsAsync();
            List<Question> selectedData = selected.Data;
            List<string> IDs = new List<string>();
            foreach (var item in selectedData)
            {
                IDs.Add(item.Id);
            }

            data.ApproveQuestionIDs(IDs);
            pageReload();
        }

        async Task btnDelete()
        {
            try
            {
                await deleteQuestions(list);
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task btnEditQuestionDelete()
        {
            try
            {
                await deleteQuestions(EditQuestionlist);
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task deleteQuestions(SfListView<Question> WatingList)
        {
            var selected = await WatingList.GetCheckedItemsAsync();
            List<Question> selectedData = selected.Data;
            List<string> IDs = new List<string>();
            foreach (var item in selectedData)
            {
                IDs.Add(item.Id);
            }

            data.deleteQuestionID(IDs);
            pageReload();
        }

        async Task UserPassEnter()
        {
            try
            {
                if (data.ifPassTheSame(txtPass.Trim())) //if (data.ifPassTheSame(Helper.customBase64Encode(txtPass.Trim())))
                {
                    await PutThePass();
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
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        void pageReload()
        {
            NavManager.NavigateTo(NavManager.Uri, replace: true, forceLoad: true);
        }

        //async Task addLocal()
        //{
        //    await localStorage.SetItemAsync("sflistview--6070dfSUQ", "MjAwMTk=");
        //}
        //string value = "";
        //async Task GetLocal()
        //{
        //    value = await localStorage.GetItemAsync<string>("sflistview--6070dfSUQ");
        //}
        bool ifHasPass()
        {
            //Task.WaitAll(getLocal());
            return data.ifPassTheSame(localPass);
        }

        async Task PutThePass()
        {
            string pass64 = data.getpass64();
            await localStorage.SetItemAsync("sflistview--6070dfSUQ", pass64);
            localPass = await localStorage.GetItemAsync<string>("sflistview--6070dfSUQ");
        }

        void btnApproveAnswer(string ID)
        {
            try
            {
                data.ApproveAnswersIDs(new List<string> { ID });
                refreshData();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        void btnDeleteAnswer(string ID)
        {
            try
            {
                data.deleteAnswersID(new List<string> { ID });
                refreshData();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}