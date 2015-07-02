using System;
using System.IO;

namespace TraktLogin
{
    public partial class TraktLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string code = Request["code"];
            var file = "EFC1AEF31C5F001BEAE4FB75A236A621.txt";
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), file), code);
        }
    }
}