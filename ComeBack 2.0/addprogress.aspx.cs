using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComeBack_2._0
{
    public partial class addprogress : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdUpload_Click(object sender, EventArgs e)
        {
            Utils util = new Utils();
            HttpFileCollection files = Request.Files;
            foreach (string fileTagName in files)
            {
                HttpPostedFile file = Request.Files[fileTagName];
                if (file.ContentLength > 0)
                {

                    try
                    {
                        // Due to the limit of the max for a int type, the largest file can be
                        // uploaded is 2147483647, which is very large anyway.
                        int size = file.ContentLength;
                        string name = file.FileName;
                        int position = name.LastIndexOf("\\");
                        name = name.Substring(position + 1);
                        string contentType = file.ContentType;
                        byte[] fileData = new byte[size];
                        file.InputStream.Read(fileData, 0, size);

                        string imageurl = "(DB)" + name;
                        util.UploadFile(fileData, size, imageurl, contentType);
                        lblfilename.Value = imageurl;
                    }
                    catch
                    {

                    }


                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Utils utils = new Utils();
            utils.AddProgress(lblfilename.Value, Session["username"].ToString());
            Response.Redirect("home.aspx");
        }
    }
}