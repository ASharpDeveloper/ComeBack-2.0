using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComeBack_2._0
{
    public partial class AddLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownRating.Items.Add("1");
            DropDownRating.Items.Add("2");
            DropDownRating.Items.Add("3");
            DropDownRating.Items.Add("4");
            DropDownRating.Items.Add("5");
            DropDownRating.Items.Add("6");
            DropDownRating.Items.Add("7");
            DropDownRating.Items.Add("8");
            DropDownRating.Items.Add("9");
            DropDownRating.Items.Add("10");
           
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Utils Util = new Utils();
            Util.AddLog(Session["username"].ToString(), exercise1.Text.Trim(), exercise2.Text.Trim(), exercise3.Text.Trim(), exercise4.Text.Trim(), exercise5.Text.Trim(), DropDownRating.SelectedItem.ToString(), txtsession.Text.Trim(), txtweight.Text.Trim());
            Response.Redirect("WorkoutPlanner.aspx");
        }
    }
}