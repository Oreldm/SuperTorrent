using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
namespace WebPortal
{
    public partial class Show : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblActiveUsers.Text = showActiveUsers();
            lblTotalUsers.Text = showTotalUsers();
            lblFiles.Text = showListOfFiles();
        }
        protected string showActiveUsers()
        {
            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            string activeUsers = siService.getActiveUsers();
            return activeUsers;
        }

        protected string showTotalUsers()
        {
            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            
            return siService.getTotalUsers(); ;
        }
        protected string showListOfFiles()
        {
            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            string ret = "";
            string[] files = siService.getAllFiles();
            for(int i = 0; i < files.Length; i++)
            {
                ret += files[i] + "\n";
            }


            return ret;
        }



        protected void iBtnSerach_Click(object sender, ImageClickEventArgs e)
        {

            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            string [] peers= siService.getPeers(txtFilename.Text);
            if (peers.Length > 0)
            {
                LabelFile.Text = "exists!";
            }
            else
            {
                LabelFile.Text = "not exists";
            }
        }

        protected void listboxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}