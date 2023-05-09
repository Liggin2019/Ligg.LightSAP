using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Ligg.Uwa.Application.Shared;

namespace Ligg.Uwa.Mvc.ViewComponents
{
    public class CustPageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CustPageModel custPageModel)
        {
            return View(custPageModel.View, custPageModel);
        }
    }

    public class CustPageModel
    {
        public string View { get; set; }

        public string Option { get; set; }
        public string ObjectId { get; set; }

    }


}
