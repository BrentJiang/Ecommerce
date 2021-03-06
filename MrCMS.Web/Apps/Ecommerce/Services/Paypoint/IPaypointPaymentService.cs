using System.Collections.Generic;
using System.Web.Mvc;
using MrCMS.Web.Apps.Ecommerce.Models;

namespace MrCMS.Web.Apps.Ecommerce.Services.Paypoint
{
    public interface IPaypointPaymentService
    {
        ProcessDetailsResponse ProcessDetails(PaypointPaymentDetailsModel model, string threeDSecureUrl);
        IEnumerable<SelectListItem> GetCardTypes();
        IEnumerable<SelectListItem> Months();
        IEnumerable<SelectListItem> StartMonths();
        IEnumerable<SelectListItem> StartYears();
        IEnumerable<SelectListItem> ExpiryMonths();
        IEnumerable<SelectListItem> ExpiryYears();
        ProcessDetailsResponse Handle3DSecureResponse(FormCollection formCollection);
        void SetModel(PaypointPaymentDetailsModel model);
        PaypointPaymentDetailsModel GetModel();
    }
}