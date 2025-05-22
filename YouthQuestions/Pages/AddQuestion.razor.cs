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
using Newtonsoft.Json.Linq;

namespace YouthQuestions.Pages
{
    public partial class AddQuestion
    {
        public string txtQuestion = "";
        public string txtName = "";
        public string errorMessage = "";
        public string EditerrorMessage = "";
        public string txtEditQuestion = "";
        public string editQuestion = "";
        public string nameMessage = "يمكنك اخفاء نفسك ان تركت هذا الحقل فارغا";
        public bool HideMe = false;
        public List<string> yourQuestions = new List<string>();
        public SfListView<Question> list;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await RefreshYourQuestionTable();
                if (firstRender) StateHasChanged();
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
                if (txtQuestion.Trim() == "")
                {
                    errorMessage = "من فضلك ادخل سؤال";
                    return;
                }
                else
                {
                    errorMessage = "";
                    if (txtName.Trim() == "" || HideMe == true)
                        txtName = "اسم مخفي ";
                    data.insertQuestion(txtQuestion, txtName);
                    yourQuestions.Add(txtQuestion);
                    await CommitYourQuestionList();
                    IsVisible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        void btnCancel()
        {
            try
            {
                navegateTohome();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private bool IsVisible { get; set; } = false;
        private string ClickStatus { get; set; }

        private void OkClick()
        {
            try
            {
                this.IsVisible = false;
                    navegateTohome();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        void navegateTohome()
        {
            NavManager.NavigateTo("/", replace: true, forceLoad: true);
        }

        async Task btnEditOK(string question)
        {
            try
            {
                data.updateQuestion(question, txtEditQuestion);
                yourQuestions.Remove(question);
                yourQuestions.Add(txtEditQuestion);
                txtEditQuestion = "";
                await CommitYourQuestionList();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        void btnEditCancel(string question)
        {
            try
            {
                editQuestion = "";
                txtEditQuestion = "";
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        void btnEdit(string question)
        {
            try
            {
                editQuestion = question;
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task RefreshYourQuestionTable()
        {
            //get local json -> decrepted -> convert to yourQuestions

            await getLocalQuestionsArray();
            if (yourQuestions == null)
            {
                //createLocalIfEmpty();
                await createLocal();
                await getLocalQuestionsArray();
            }
            else if (yourQuestions.Count > 1) //[0]=today's date
            {
                //deletePast-local if date-old ();
                await deleteOldDateLoveIDs();
            }
        }

        private async Task getLocalQuestionsArray()
        {
            try
            {
                string QuestionsJsonArray = await localStorage.GetItemAsync<string>("sflistview--6070dfYUQ");
                string decodedValue = Helper.customBase64Decode(QuestionsJsonArray);
                JArray jsonarray = JArray.Parse(decodedValue);
                if (jsonarray.Count > 0)
                {
                    if (yourQuestions != null) yourQuestions.Clear(); //clear date also
                    yourQuestions = jsonarray.ToObject<List<string>>();
                }
            }
            catch (Exception)
            {
                yourQuestions = null;
            }
        }

        async Task CommitYourQuestionList()
        {
            //convert yourQuestions to json -> increpted -> replace in local 
            JArray NewLoveJArray = JArray.FromObject(yourQuestions);
            await replaceLocal(NewLoveJArray);
        }

        private async Task replaceLocal(JArray array)
        {
            string encodedEmptyJarray = Helper.customBase64Encode(array.ToString());
            await localStorage.SetItemAsync("sflistview--6070dfYUQ", encodedEmptyJarray);
        }

        private async Task createLocal()
        {
            // LoveIDs = null 
            string EmptyJarray = $"['{DateTime.Today.ToString()}']";
            string encodedEmptyJarray = Helper.customBase64Encode(EmptyJarray);
            await localStorage.SetItemAsync("sflistview--6070dfYUQ", encodedEmptyJarray);
        }

        private async Task deleteOldDateLoveIDs()
        {
            if (!yourQuestions.Contains(DateTime.Today.ToString()))
            {
                //old -> createLocal(); then getLocalLoveIDsArray();
                await createLocal();
                await RefreshYourQuestionTable();
            }
        }

        private async Task btnEditDelete(string question)
        {
            try
            {
                if (yourQuestions.Contains(question))
                {
                    txtEditQuestion = "";
                    yourQuestions.Remove(question);
                    await CommitYourQuestionList();
                }
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }


    }
}