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

namespace YouthQuestions.Pages
{
    public partial class AddAnswer
    {
        [Parameter]
        public string value { get; set; }
        public string result { get; set; }
        public string txtAnswer { get; set; }
        public string errorMessage { get; set; }
        public int authorized { get; set; } = 0;
        private bool IsVisible { get; set; } = false;

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
                result = data.getQuestionFromCustom64Value(value);
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            // -1 -> not found or admin != 1


        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                string localPass = await localStorage.GetItemAsync<string>("sflistview--6070dfSUQ");
                //if (ifHasPass(localPass))
                //{
                //    authorized = 1;
                //}
                //else
                //{
                //    authorized = -1;
                //}

                if (firstRender) StateHasChanged();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        bool ifHasPass(string localPass)
        {

            //var task = OnAfterRenderAsync(true);
            //Task.WaitAll(getLocal());
            return data.ifPassTheSame(localPass);
        }

        void btnOk()
        {
            try
            {
                if (txtAnswer.Trim() == "")
                {
                    errorMessage = "من فضلك ادخل سؤال";
                    return;
                }
                else
                {
                    //get ID from value
                    string id = Helper.customBase64Decode(value);
                    //add answer to question ID
                    data.addAnswer(id, txtAnswer, authorized);
                    IsVisible = true;
                    StateHasChanged();
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
    }
}