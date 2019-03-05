using System;

using System.Web.UI;

using TorrentLibrary;

namespace WebPortal
{
    public partial class Registration : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

            DalService.getInstance().register(txtUsername.Text, txtPassword.Text);
        }
    }
}