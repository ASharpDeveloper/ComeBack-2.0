using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComeBack_2._0
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                LitLogin.Text = "<a href=\"login.aspx\" class=\"btn btn-info pull-right\">LOG ME IN</a>";
            }
            else if (Session["username"] != null)
            {
                LitLogin.Text = "<a href=\"logout.aspx\" class=\"btn btn-danger pull-right\">LOG ME OUT</a>";
            }
        }
    }
}