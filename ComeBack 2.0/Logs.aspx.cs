using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComeBack_2._0
{
    public partial class Logs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {

                }
                if (Request.Params["ID"] == null)
                {
                    Response.Redirect("home.aspx");
                    return;
                }
                lbllogid.Value = Request.Params["ID"].ToString();

            }
        }
    }
}