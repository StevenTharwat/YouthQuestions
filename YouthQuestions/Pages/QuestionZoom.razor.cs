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
    public partial class QuestionZoom
    {
        [Parameter]
        public string value { get; set; }
        public string result { get; set; }
        public DataTable Answers = new DataTable();
        public bool IsVisible { get; set; } = false;
        public bool IsManager { get; set; } = false;
        public string deletedID { get; set; }
        public string exption = string.Empty;
        List<string> LoveIDs = new List<string>();

        protected override void OnParametersSet()
        {
            try
            {
                if (value == null)
                {
                    NavManager.NavigateTo("/", replace: true, forceLoad: true);
                    return;
                }
                else if (value.Trim() == "")
                {
                    NavManager.NavigateTo("/", replace: true, forceLoad: true);
                    return;
                }
                refreshData();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

            // -1 -> not found or admin != 1
        }

        void refreshData()
        {
            Answers = data.getAllPublicAnswers(value);
            result = data.getQuestionFromCustom64Value(value);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                string localPass = await localStorage.GetItemAsync<string>("sflistview--6070dfSUQ");
                IsManager = data.ifPassTheSame(localPass);
                if (firstRender)
                {
                    await RefreshLocalLoveArray();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        async Task loveClick(string id)
        {
            try
            {
                if (await IFIDExistInLocalLove(id))
                {
                    //decrease LoveCount in db
                    data.increaseAnswerLoveById(id, -1);
                    //remove from aray
                    LoveIDs.Remove(id);
                    await StoreListInLocal();
                    refreshData();
                }
                else
                {
                    //increase LoveCount in db
                    data.increaseAnswerLoveById(id, 1);
                    //add to aray
                    LoveIDs.Add(id);
                    await StoreListInLocal();
                    refreshData();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

        }

        private async Task StoreListInLocal()
        {
            JArray NewLoveJArray = JArray.FromObject(LoveIDs);
            await replaceLocal(NewLoveJArray);
        }

        void deleteClick(string id)
        {
            try
            {
                IsVisible = true;
                deletedID = id;
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        void DialogOkClick(string id)
        {
            try
            {
                data.deleteAnswerID(id);
                DialogCloseClick();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        void DialogCloseClick()
        {
            try
            {
                IsVisible = false;
                deletedID = "";
                refreshData();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task RefreshLocalLoveArray()
        {
            //get and refresh array with custom 64 decode
            await getLocalLoveIDsArray();
            if (LoveIDs == null)
            {
                //createLocalIfEmpty();
                await createLocal();
                await getLocalLoveIDsArray();
            }
            else if (LoveIDs.Count > 1) //[0]=today's date
            {
                //deletePast-local if date-old ();
                await deleteOldDateLoveIDs();
            }
        }

        private async Task deleteOldDateLoveIDs()
        {
            if (!LoveIDs.Contains(DateTime.Today.ToString()))
            {
                //old -> createLocal(); then getLocalLoveIDsArray();
                await createLocal();
                await getLocalLoveIDsArray();
            }
        }

        private async Task createLocal()
        {
            // LoveIDs = null 
            string EmptyJarray = $"['{DateTime.Today.ToString()}']";
            string encodedEmptyJarray = Helper.customBase64Encode(EmptyJarray);
            await localStorage.SetItemAsync("sflistview--6070dfALUQ", encodedEmptyJarray);
        }

        private async Task replaceLocal(JArray array)
        {
            string encodedEmptyJarray = Helper.customBase64Encode(array.ToString());
            await localStorage.SetItemAsync("sflistview--6070dfALUQ", encodedEmptyJarray);
        }

        private async Task getLocalLoveIDsArray()
        {
            try
            {
                string loveIDsJsonArray = await localStorage.GetItemAsync<string>("sflistview--6070dfALUQ");
                string decodedValue = Helper.customBase64Decode(loveIDsJsonArray);
                JArray jsonarray = JArray.Parse(decodedValue);
                if (jsonarray.Count > 0)
                {
                    if (LoveIDs != null) LoveIDs.Clear(); //clear date also
                    LoveIDs = jsonarray.ToObject<List<string>>();
                }
            }
            catch (Exception)
            {
                LoveIDs = null;
            }
        }

        async Task<bool> IFIDExistInLocalLove(string ID)
        {
            await RefreshLocalLoveArray();

            //return check if ID found in the RefreshLocalLoveArray's array
            bool ifExist = LoveIDs.Contains(ID);

            return ifExist;
        }

        string changeCSS(string id)
        {
            try
            {
                string customLovebefor = @"color:gray;
            background-color: #FFF;
            border-color: gray;";
                string customLoveAfter = @"color: #FFF;
            background-color: #DA0027;
            border-color: gray;";

                if (LoveIDs.Contains(id)) return customLoveAfter;
                else return customLovebefor;
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return "";
            }
        }
    }
}