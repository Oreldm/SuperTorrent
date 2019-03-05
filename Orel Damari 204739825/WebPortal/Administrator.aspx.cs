using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using TorrentLibrary;

namespace WebPortal
{
    public partial class Administrator : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void Search()
        {
            string password = DalService.getInstance().getUsersPassword(txtUser.Text);
            if (password.Length > 0)
            {
                txtUsername.Text = txtUser.Text;
                txtPassword.Text = password;
            }

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Search();
        }



        protected void iBtnUpdate_Click(object sender, ImageClickEventArgs e)
        {

             DalService.getInstance().delete(Request.Form["txtUsername"]);
             DalService.getInstance().register(txtUsername.Text, txtPassword.Text);
        }

        protected void iBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            DalService.getInstance().delete(Request.Form["txtUser"]);
        }

        protected void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}