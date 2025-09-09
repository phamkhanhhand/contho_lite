using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KH.ITFamily.FileCenter
{
    public class KHBasePageModel : PageModel
    {
        protected Dictionary<string, string> lstRef = new Dictionary<string, string>(); 
        public string GetRefID(string key)
        {
            var rs = "";
            if (lstRef.ContainsKey(key))
            {
                rs = lstRef[key];

            }
            else
            {
                var autoID = Guid.NewGuid().ToString();
                rs = autoID;
                lstRef.Add(key, autoID);
            }

            return rs;

        }
    }
}
