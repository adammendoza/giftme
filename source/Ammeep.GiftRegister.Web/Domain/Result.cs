using System.Collections.Generic;
using System.Web.Mvc;

namespace Ammeep.GiftRegister.Web.Domain
{
    public class Result
    {
        public Result()
        {
            Errors = new Dictionary<string, string>();
        }
        public bool Successful{ get; set; }
        public IDictionary<string, string> Errors { get; private set; }
    }

    public static class ResultModelHelpers
    {
        public static void AddModelErrors(this Result result, ModelStateDictionary modelStateDictionary)
        {
            foreach (var error in result.Errors)
            {
                modelStateDictionary.AddModelError(error.Key,error.Value);
            }
        }
    }
}