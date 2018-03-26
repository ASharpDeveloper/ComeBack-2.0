using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComeBack_2._0
{
    public partial class loginaspx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnsignin_Click(object sender, EventArgs e)
        {
            Utils util = new Utils();
            int usernumber = 0;
            string username = "", errormessage = "";
            // string fld_email = "";

            util.CheckLogon(txtusernameLogin.Text.Trim(), ref username, ref usernumber, ref errormessage, Request.UserHostAddress, Request.ServerVariables["LOCAL_ADDR"], txtpasswordLogin.Text.Trim());

            if (errormessage != "")
            {
                lblerrorlogin.Text = errormessage;
                lblerrorlogin.Visible = true;
                return;
            }
            Session["username"] = txtusernameLogin.Text.Trim();


            Response.Redirect("home.aspx");
        }

        protected void btnsignup_Click(object sender, EventArgs e)
        {
            string errormessage = "";
            Utils util = new Utils();
            util.RegisterUser(txtusername.Text.Trim(), txtpassword.Text.Trim(), ref errormessage);

            if (errormessage != "")
            {
                lblerror.Text = errormessage;
                lblerror.Visible = true;
                return;
            }
            Session["username"] = txtusername.Text.Trim();
            Response.Redirect("home.aspx");
        }
    }
}