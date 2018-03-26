using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComeBack_2._0
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("login.aspx");
            }
            litHello.Text = "<h4 class=\"header-line\">Welcome, " + Session["username"].ToString() + "!</h4>";

            string html = "";
            Utils util = new Utils();
            List<Utils.productitem> items = util.GetLogHome(Session["username"].ToString());

            foreach (var item in items)
            {
                html += "<div class=\"col-md-3 col-sm-3 col-xs-6\">";
                html += "<div class=\"alert alert-info back-widget-set text-center\">";
                html += "<i class=\"fa fa-history fa-5x\"></i>";
                html += "<h3> " + item.weight.ToString() + " KG &nbsp; <i class=\"\"></i></h3>";
                html += item.date.ToString();
                html += "</div>";
                html += "</div>";
            }
            LitLog.Text = html;
            
        }
    }
}