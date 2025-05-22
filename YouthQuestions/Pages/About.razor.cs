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
using Syncfusion.Blazor.Grids;

namespace YouthQuestions.Pages
{
    public partial class About
    {
        SfToast ToastObj;
        private string ToastPosition = "Right";
        ToastModel toast = new ToastModel{Title = "تم !", Content = "لقد تم اضافة اقتراحك", CssClass = "e-toast-success", Icon = "e-success toast-icons"};
        public string txtSuggestion = "";
        public string txtName = "";
        public string errorMessage = "";
        public bool HideMe = false;
        async Task btnOk()
        {
            try
            {
                if (txtSuggestion.Trim() == "")
                {
                    errorMessage = "من فضلك ادخل اقتراح";
                    return;
                }
                else
                {
                    errorMessage = "";
                    if (txtName.Trim() == "" || HideMe == true)
                        txtName = "NULL";
                    data.insertSuggestion(txtSuggestion, txtName);
                    await suggestionAdded();
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
                clearFildes();
            }
            catch (Exception ex)
            {
                Helper.logError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        async Task suggestionAdded()
        {
            //clear fildes
            clearFildes();
            //notification -> added successfully
            await this.ToastObj.ShowAsync(toast);
        }

        void clearFildes()
        {
            txtSuggestion = "";
            txtName = "";
            errorMessage = "";
        }
    }
}