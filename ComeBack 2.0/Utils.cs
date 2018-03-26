using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Net.Mail;
using System.Net;


public class Utils
{

    bool is_db_open = false;
    public SqlConnection conn;

    public void WriteTableHeader(HttpResponse Response, int defaultwidth = 100, bool header_row = true)     // writes default HTML table header (and header section if applicable)
    {
        Response.Write("<table class=\"table table-striped\" style=\"width: " + defaultwidth.ToString() + "%;\">");

        if (header_row)
        {
            Response.Write("<thead>");
        }
    }


    public void WriteRowHeader(HttpResponse Response)   // writes HTML row header
    {
        Response.Write("<tr>");
    }

    public void WriteDataCell(string celldata, HttpResponse Response, bool header_row = false, int colspan = 1)        // writes HTML data cell (allows header rows and column spans)
    {
        // colwidth currently ignored, browser will determine column width based on data in cells

        string html_code;

        if (header_row)
        {
            html_code = "th";
        }
        else
        {
            html_code = "td";
        }

        if (colspan <= 1)
        {
            Response.Write("<" + html_code + ">");
        }
        else
        {
            Response.Write("<" + html_code + " colspan=\"" + colspan.ToString() + "\">");
        }
        Response.Write(celldata);
        Response.Write("</" + html_code + ">");
    }

    public void WriteRowFooter(HttpResponse Response, bool header_end = false)      // writes html row footer (and /thead and tbody sections if header end)
    {
        Response.Write("</tr>");
        if (header_end)
        {
            Response.Write("</thead>");
            Response.Write("<tbody>");
        }
    }

    public void WriteTableFooter(HttpResponse Response, bool end_body = true)       // writes table footer
    {
        if (end_body)
        {
            Response.Write("</tbody>");
        }
        Response.Write("</table>");
    }


    public void OpenDBConnection(bool force_live_connection = false)        // opens database connection (unless it is already open) - force live connection not usually necessary (handled in web.config.release), but can be useful temporarily changing in debug mode to troubleshoot against live db
    {                                                                       // force live connection parameter ignored if database has already been opened and not yet closed or class object disposed
        if (is_db_open)
            return;

        is_db_open = true;

        conn = new SqlConnection();

        string connection_string_alias;
        if (force_live_connection)
            connection_string_alias = "ComeBackLive";
        else
            connection_string_alias = "ComeBack";

        conn.ConnectionString = ConfigurationManager.ConnectionStrings[connection_string_alias].ConnectionString;
        conn.Open();
    }

    public void CloseDBConnection()
    {
        if (is_db_open)
        {
            conn.Close();
            conn.Dispose();
        }
        is_db_open = false;
    }

    // from internet
    public string GetPostBackControlId(Page page)       // from internet, returns name of control that did the postback (useful e.g. for checkboxes with auto postback set)
    {
        if (!page.IsPostBack)
            return string.Empty;

        Control control = null;
        // first we will check the "__EVENTTARGET" because if post back made by the controls
        // which used "_doPostBack" function also available in Request.Form collection.
        string controlName = page.Request.Params["__EVENTTARGET"];
        if (!String.IsNullOrEmpty(controlName))
        {
            control = page.FindControl(controlName);
        }
        else
        {
            // if __EVENTTARGET is null, the control is a button type and we need to
            // iterate over the form collection to find it

            // ReSharper disable TooWideLocalVariableScope
            string controlId;
            Control foundControl;
            // ReSharper restore TooWideLocalVariableScope

            foreach (string ctl in page.Request.Form)
            {
                // handle ImageButton they having an additional "quasi-property" 
                // in their Id which identifies mouse x and y coordinates
                if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                {
                    controlId = ctl.Substring(0, ctl.Length - 2);
                    foundControl = page.FindControl(controlId);
                }
                else
                {
                    foundControl = page.FindControl(ctl);
                }

                if (!(foundControl is IButtonControl)) continue;

                control = foundControl;
                break;
            }
        }

        return control == null ? String.Empty : control.ID;
    }

    public void RegisterUser(string username, string password, ref string errormessage)
    {
        errormessage = "";

        OpenDBConnection();

        SqlCommand ucommand;

        bool found = false;
        ucommand = new SqlCommand("SELECT fld_username FROM cb_users WHERE fld_username = @username;", conn);
        ucommand.Parameters.Add(new SqlParameter("username", username));

        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                found = true;
            }
        }
        ucommand.Dispose();

        if (found)
        {
            errormessage = "User Name " + username + " has already been registered, please choose another.";
            return;
        }

        ucommand = new SqlCommand("INSERT INTO cb_users (fld_username, fld_password) VALUES (@username, @password)", conn);

        username = username.Trim();
        if (username.Length > 100)
            username = username.Substring(0, 100);

        byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
        String hash = System.Text.Encoding.ASCII.GetString(data);

      
        ucommand.Parameters.Add(new SqlParameter("username", username));
        ucommand.Parameters.Add(new SqlParameter("password", hash));
        

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();



    }

    public void UpdateUserType(string usernumber, string usertype)
    {
        OpenDBConnection();
        SqlCommand ucommand = new SqlCommand("UPDATE dash_register SET fld_usertype = @usertype WHERE fld_autoinc = @usernumber;", conn);
        ucommand.Parameters.Add(new SqlParameter("usertype", usertype));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();
    }

    public void UpdateDDRef(string usernumber, string dd_ref)
    {
        OpenDBConnection();
        SqlCommand ucommand = new SqlCommand("UPDATE dash_register SET fld_ddref = @ddref WHERE fld_autoinc = @usernumber;", conn);
        ucommand.Parameters.Add(new SqlParameter("dd_ref", dd_ref));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();
    }

    public void AddWebsite(string website, string usernumber)
    {

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("INSERT INTO dash_website (website, date, fld_usernumber) VALUES (@website, GetDate(), @usernumber)", conn);



        ucommand.Parameters.Add(new SqlParameter("website", website));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));




        ucommand.ExecuteNonQuery();
        ucommand.Dispose();


    }

    public void AddGroup(string Name, string pin, string owner)
    {

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("INSERT INTO dash_group (fld_groupname, fld_grouppassword, fld_owner) VALUES (@groupname, @grouppassword, @owner)", conn);



        ucommand.Parameters.Add(new SqlParameter("groupname", Name));
        ucommand.Parameters.Add(new SqlParameter("grouppassword", pin));
        ucommand.Parameters.Add(new SqlParameter("owner", owner));



        ucommand.ExecuteNonQuery();
        ucommand.Dispose();


    }

    public List<productitem> GetLogHome(string user)
    {
        List<productitem> Items = new List<productitem>();

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT TOP 4 * from cb_log WHERE fld_user = @user order by fld_date DESC", conn);

        ucommand.Parameters.Add(new SqlParameter("user", user));

        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                productitem item = new productitem();
                item.exercise1 = reader["fld_exercise1"].ToString();
                item.exercise2 = reader["fld_exercise2"].ToString();
                item.exercise3 = reader["fld_exercise3"].ToString();
                item.exercise4 = reader["fld_exercise4"].ToString();
                item.exercise5 = reader["fld_exercise5"].ToString();
                item.date = reader["fld_date"].ToString();
                item.rating = reader["fld_rating"].ToString();
                item.session = reader["fld_session"].ToString();
                item.weight = reader["fld_weight"].ToString();
                Items.Add(item);
            }

        }
        return Items;
    }

    public List<productitem> GetLog(string user)
    {
        List<productitem> Items = new List<productitem>();

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT * from cb_log WHERE fld_user = @user order by fld_date DESC", conn);

        ucommand.Parameters.Add(new SqlParameter("user", user));

        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                productitem item = new productitem();
                item.exercise1 = reader["fld_exercise1"].ToString();
                item.exercise2 = reader["fld_exercise2"].ToString();
                item.exercise3 = reader["fld_exercise3"].ToString();
                item.exercise4 = reader["fld_exercise4"].ToString();
                item.exercise5 = reader["fld_exercise5"].ToString();
                item.date = reader["fld_date"].ToString();
                item.rating = reader["fld_rating"].ToString();
                item.session = reader["fld_session"].ToString();
                item.weight = reader["fld_weight"].ToString();
                Items.Add(item);
            }

        }
        return Items;
    }

    public List<ScheduleItem> GetSchedule(string user)
    {
        List<ScheduleItem> Items = new List<ScheduleItem>();

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT * from cb_schedule WHERE fld_user = @user", conn);
    
        ucommand.Parameters.Add(new SqlParameter("user", user));

        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                ScheduleItem item = new ScheduleItem();
                item.fld_monday = reader["fld_monday"].ToString();
                item.fld_tuesday = reader["fld_tuesday"].ToString();
                item.fld_wednesday = reader["fld_wednesday"].ToString();
                item.fld_thursday = reader["fld_thursday"].ToString();
                item.fld_friday = reader["fld_friday"].ToString();
                item.fld_saturday = reader["fld_saturday"].ToString();
                item.fld_sunday = reader["fld_sunday"].ToString();
                item.fld_CurrentMonth = reader["fld_CurrentMonth"].ToString();
                
                Items.Add(item);
            }

        }
        return Items;
    }

    public void AddLog(string fld_user, string fld_exercise1, string fld_exercise2, string fld_exercise3, string fld_exercise4, string fld_exercise5, string fld_rating, string fld_session, string fld_weight)
    {

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("INSERT INTO cb_log (fld_user, fld_exercise1, fld_exercise2, fld_exercise3, fld_exercise4, fld_exercise5, fld_date, fld_rating, fld_session, fld_weight) VALUES (@user, @exercise1, @exercise2, @exercise3, @exercise4, @exercise5, getdate(), @rating, @session, @weight)", conn);



        ucommand.Parameters.Add(new SqlParameter("user", fld_user));
        ucommand.Parameters.Add(new SqlParameter("exercise1", fld_exercise1));
        ucommand.Parameters.Add(new SqlParameter("exercise2", fld_exercise2));
        ucommand.Parameters.Add(new SqlParameter("exercise3", fld_exercise3));
        ucommand.Parameters.Add(new SqlParameter("exercise4", fld_exercise4));
        ucommand.Parameters.Add(new SqlParameter("exercise5", fld_exercise5));
        ucommand.Parameters.Add(new SqlParameter("rating", fld_rating));
        ucommand.Parameters.Add(new SqlParameter("session", fld_session));
        ucommand.Parameters.Add(new SqlParameter("weight", fld_weight));



        ucommand.ExecuteNonQuery();
        ucommand.Dispose();


    }

    public void AddSchedule(string fld_user, string fld_CurrentMonth, string fld_monday, string fld_tuesday, string fld_wednesday, string fld_thursday, string fld_friday, string fld_saturday, string fld_sunday)
    {

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("INSERT INTO cb_schedule (fld_user, fld_CurrentMonth, fld_monday, fld_tuesday, fld_wednesday, fld_thursday, fld_friday, fld_saturday, fld_sunday) VALUES (@user, @CurrentMonth, @monday, @tuesday, @wednesday, @thursday, @friday, @saturday, @sunday)", conn);



        ucommand.Parameters.Add(new SqlParameter("user", fld_user));
        ucommand.Parameters.Add(new SqlParameter("CurrentMonth", fld_CurrentMonth));
        ucommand.Parameters.Add(new SqlParameter("monday", fld_monday));
        ucommand.Parameters.Add(new SqlParameter("tuesday", fld_tuesday));
        ucommand.Parameters.Add(new SqlParameter("wednesday", fld_wednesday));
        ucommand.Parameters.Add(new SqlParameter("thursday", fld_thursday));
        ucommand.Parameters.Add(new SqlParameter("friday", fld_friday));
        ucommand.Parameters.Add(new SqlParameter("saturday", fld_saturday));
        ucommand.Parameters.Add(new SqlParameter("sunday", fld_sunday));
       



        ucommand.ExecuteNonQuery();
        ucommand.Dispose();


    }

    public void DeleteWebSite(string id, string usernumber, string enable)
    {
        string strsql = "DELETE from dash_website WHERE id = @id AND fld_UserNumber = @usernumber;";

        OpenDBConnection();

        SqlCommand ucommand = new SqlCommand(strsql, conn);

        ucommand.Parameters.Add(new SqlParameter("id", id));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));
        ucommand.Parameters.Add(new SqlParameter("disabled", enable == "D" ? "D" : ""));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();
    }

    public void DeleteImage(string id, string usernumber, string enable)
    {
        string strsql = "DELETE FROM dash_image WHERE fld_autoinc = @id AND fld_UserNumber = @usernumber;";

        OpenDBConnection();

        SqlCommand ucommand = new SqlCommand(strsql, conn);

        ucommand.Parameters.Add(new SqlParameter("id", id));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));
        ucommand.Parameters.Add(new SqlParameter("disabled", enable == "D" ? "D" : ""));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();
    }

    public void SendEmail(string emailaddress, string msg_subject, string msg_text, string cc_address = "")
    {
        MailMessage msg = new MailMessage();

        if (emailaddress != "")
            msg.To.Add(emailaddress);
        if (cc_address != "")
            msg.CC.Add(cc_address);

        MailAddress address = new MailAddress("sip@hensonitsolutions.co.uk");
        msg.From = address;

        msg.Subject = msg_subject;

        msg.Body = msg_text;

        SmtpClient Client = new SmtpClient();
        Client.UseDefaultCredentials = false;
        NetworkCredential credentials = new NetworkCredential("sip@hensonitsolutions.co.uk", "sippassword2017");
        Client.Credentials = credentials;
        Client.Host = "hensonitsolutions.co.uk";

        Client.Send(msg);

    }




    public void ProcessShowPictureFromDB(string fld_filename, HttpResponse Response, string usernumber)
    {
        for (int n = 1; n <= 10; n++)
        {
            try
            {
                OpenDBConnection();

                string strsql = "SELECT TOP 1 fld_autoinc, fld_file, fld_contenttype FROM dash_image WHERE fld_filename = @fld_filename AND fld_usernumber = @usernumber ORDER BY fld_autoinc DESC;";

                SqlCommand command = new SqlCommand(strsql, conn);
                command.Parameters.Add(new SqlParameter("fld_filename", fld_filename));
                command.Parameters.Add(new SqlParameter("usernumber", usernumber));

                string base64String = "";
                string contentType = "";
                byte[] bytes = null;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bytes = (byte[])reader["fld_file"];
                        base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        contentType = reader["fld_contenttype"].ToString();

                    }
                }

                command.Dispose();
                CloseDBConnection();

                if (base64String != "")
                {
                    Response.ContentType = contentType;
                    Response.AddHeader("Content-type", contentType);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }

                break;
            }
            catch
            {
                // do nothing, retry next loop
            }
        }
    }

    


    public void CheckLogon(string username, ref string ret_userfullname, ref int ret_user_number, ref string ret_err_message, string from_ip, string to_link, string password)
    {
        ret_userfullname = "";
        
        ret_err_message = "";

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT * FROM cb_users WHERE fld_username = @username", conn);

        username = username.Trim();
        if (username.Length > 100)
            username = username.Substring(0, 100);

        ucommand.Parameters.Add(new SqlParameter("username", username.ToUpper()));



        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                String hash = System.Text.Encoding.ASCII.GetString(data);


                if ((hash == reader["fld_password"].ToString()) || (password == reader["fld_password"].ToString()))
                {
                    ret_userfullname = reader["fld_username"].ToString();
                    ret_user_number = Convert.ToInt32(reader["fld_usernumber"].ToString());
                    
                }
            }

        }
        ucommand.Dispose();
        CloseDBConnection();

        if (ret_userfullname == "")
        {
            ret_err_message = "Invalid User Name or Password";
        }
    }



    public void UploadFile(byte[] fileData, int size, string name, string contentType, string usernumber)
    {
        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("INSERT INTO dash_image (fld_file, fld_size, fld_filename, fld_contenttype, fld_usernumber"
                                + " )"
                                + " VALUES(@File, @Size, @FileName, @ContentType, @usernumber"
                                + ");", conn);

        ucommand.Parameters.Add(new SqlParameter("File", fileData));
        ucommand.Parameters.Add(new SqlParameter("Size", size));
        ucommand.Parameters.Add(new SqlParameter("FileName", name));
        ucommand.Parameters.Add(new SqlParameter("ContentType", contentType));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();

    }

    public void UploadFiletogroup(byte[] fileData, int size, string name, string contentType, string usernumber, string group)
    {
        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("INSERT INTO dash_image (fld_file, fld_size, fld_filename, fld_contenttype, fld_usernumber, fld_group"
                                + " )"
                                + " VALUES(@File, @Size, @FileName, @ContentType, @usernumber, @group"
                                + ");", conn);

        ucommand.Parameters.Add(new SqlParameter("File", fileData));
        ucommand.Parameters.Add(new SqlParameter("Size", size));
        ucommand.Parameters.Add(new SqlParameter("FileName", name));
        ucommand.Parameters.Add(new SqlParameter("ContentType", contentType));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));
        ucommand.Parameters.Add(new SqlParameter("group", group));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();

    }


    public List<imageitem> GetImage(string usernumber)
    {
        List<imageitem> Items2 = new List<imageitem>();

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT fld_filename FROM dash_image WHERE fld_usernumber = @usernumber AND fld_group IS NULL AND (fld_Status IS NULL OR fld_Status <> 'D') ORDER BY fld_autoinc", conn);

        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));



        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                imageitem Item = new imageitem();

                Item.fld_filename = reader["fld_filename"].ToString();
                Items2.Add(Item);
            }

        }
        ucommand.Dispose();
        CloseDBConnection();

        return Items2;
    }

    public List<imageitem> GetGroupImage(string group)
    {
        List<imageitem> Items2 = new List<imageitem>();

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT fld_filename FROM dash_image WHERE fld_group = @group AND (fld_Status IS NULL OR fld_Status <> 'D') ORDER BY fld_autoinc", conn);

        
        ucommand.Parameters.Add(new SqlParameter("group", group));


        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                imageitem Item = new imageitem();

                Item.fld_filename = reader["fld_filename"].ToString();
                Items2.Add(Item);
            }

        }
        ucommand.Dispose();
        CloseDBConnection();

        return Items2;
    }

    public List<productitem> GetGroups(string owner)
    {
        List<productitem> Items = new List<productitem>();

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT * from dash_group where fld_owner = @owner order by fld_groupname", conn);

        ucommand.Parameters.Add(new SqlParameter("owner", owner));

        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                productitem item = new productitem();
                item.groups = reader["fld_groupname"].ToString();
               
                Items.Add(item);
            }

        }
        return Items;
    }

    public List<WebsiteLink> GetAllLinks(string usernumber)
    {
        List<WebsiteLink> Links = new List<WebsiteLink>();

        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT * from dash_website where fld_UserNumber = @usernumber AND (fld_Status IS NULL OR fld_Status <> 'D') order by date DESC", conn);
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));


        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                WebsiteLink links = new WebsiteLink();
                links.website = reader["website"].ToString();

                Links.Add(links);
            }

        }
        ucommand.Dispose();
        CloseDBConnection();

        return Links;
    }

    public void GetSlideTime(string usernumber, ref string slide_interval)
    {
        OpenDBConnection();

        SqlCommand ucommand;

        ucommand = new SqlCommand("SELECT * FROM dash_register where fld_usernumber = @usernumber", conn);


        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));

       
        slide_interval = "";




        using (SqlDataReader reader = ucommand.ExecuteReader())
        {
            while (reader.Read())
            {
                slide_interval = reader["fld_settings_timer"].ToString();
                


            }

        }
    }

    public void DisplayImagesForEdit(HttpResponse Response, string usernumber)
    {
        string strsql = "SELECT * from dash_image where fld_UserNumber = @usernumber AND fld_group IS NULL ORDER BY fld_autoinc";

        OpenDBConnection();

        SqlCommand command = new SqlCommand(strsql, conn);
        command.Parameters.Add(new SqlParameter("usernumber", usernumber));

        WriteTableHeader(Response);
        WriteRowHeader(Response);
        WriteDataCell("File Name", Response, true);
        WriteDataCell("Image", Response, true);
        WriteDataCell("Status", Response, true);
        WriteDataCell("Delete", Response, true);
        WriteDataCell("Disable?", Response, true);
        WriteDataCell("Enable?", Response, true);
        WriteRowFooter(Response, true);

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                WriteRowFooter(Response);
                WriteDataCell(reader["fld_filename"].ToString(), Response);
                WriteDataCell("<img style=\"max-width: 200px; max-height: 200px;\" src=\"ShowPicture.aspx?filename=" + reader["fld_FileName"].ToString() + "\">", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "Disabled" : "Enabled", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? " & nbsp; " : " <a href =\"DeleteImage.aspx?action=D&typ=I&id=" + reader["fld_autoinc"].ToString() + "\">Delete</a>", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "&nbsp;" : "<a href=\"DisableItem.aspx?action=D&typ=I&id=" + reader["fld_autoinc"].ToString() + "\">Disable</a>", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "<a href=\"DisableItem.aspx?action=E&typ=I&id=" + reader["fld_autoinc"].ToString() + "\">Enable</a>" : "&nbsp;", Response);
                WriteRowFooter(Response);
            }
        }

        WriteTableFooter(Response);

        command.Dispose();
        CloseDBConnection();
    }

    public void DisplayGroupImagesForEdit(HttpResponse Response, string group)
    {
        string strsql = "SELECT * from dash_image where fld_group = @group ORDER BY fld_autoinc";

        OpenDBConnection();

        SqlCommand command = new SqlCommand(strsql, conn);
        command.Parameters.Add(new SqlParameter("group", group));

        WriteTableHeader(Response);
        WriteRowHeader(Response);
        WriteDataCell("File Name", Response, true);
        WriteDataCell("Image", Response, true);
        WriteDataCell("Status", Response, true);
        WriteDataCell("Delete", Response, true);
        WriteDataCell("Disable?", Response, true);
        WriteDataCell("Enable?", Response, true);
        WriteRowFooter(Response, true);

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                WriteRowFooter(Response);
                WriteDataCell(reader["fld_filename"].ToString(), Response);
                WriteDataCell("<img style=\"max-width: 200px; max-height: 200px;\" src=\"ShowPicture.aspx?filename=" + reader["fld_FileName"].ToString() + "\">", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "Disabled" : "Enabled", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? " & nbsp; " : " <a href =\"DeleteImage.aspx?action=D&typ=I&id=" + reader["fld_autoinc"].ToString() + "\">Delete</a>", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "&nbsp;" : "<a href=\"DisableItem.aspx?action=D&typ=I&id=" + reader["fld_autoinc"].ToString() + "\">Disable</a>", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "<a href=\"DisableItem.aspx?action=E&typ=I&id=" + reader["fld_autoinc"].ToString() + "\">Enable</a>" : "&nbsp;", Response);
                WriteRowFooter(Response);
            }
        }

        WriteTableFooter(Response);

        command.Dispose();
        CloseDBConnection();
    }



    public void DisplayWebSitesForEdit(HttpResponse Response, string usernumber)
    {
        string strsql = "SELECT * from dash_website where fld_UserNumber = @usernumber ORDER BY id";

        OpenDBConnection();

        SqlCommand command = new SqlCommand(strsql, conn);
        command.Parameters.Add(new SqlParameter("usernumber", usernumber));

        WriteTableHeader(Response);
        WriteRowHeader(Response);
        WriteDataCell("Web-Site", Response, true);
        WriteDataCell("Status", Response, true);
        WriteDataCell("Delete", Response, true);
        WriteDataCell("Disable?", Response, true);
        WriteDataCell("Enable?", Response, true);
        WriteRowFooter(Response, true);

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                WriteRowFooter(Response);
                WriteDataCell(reader["website"].ToString(), Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "Disabled" : "Enabled", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "&nbsp;" : "<a href=\"DeleteImage.aspx?action=D&typ=W&id=" + reader["id"].ToString() + "\">Delete</a>", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "&nbsp;" : "<a href=\"DisableItem.aspx?action=D&typ=W&id=" + reader["id"].ToString() + "\">Disable</a>", Response);
                WriteDataCell(reader["fld_Status"].ToString() == "D" ? "<a href=\"DisableItem.aspx?action=E&typ=W&id=" + reader["id"].ToString() + "\">Enable</a>" : "&nbsp;", Response);
                WriteRowFooter(Response);
            }
        }

        WriteTableFooter(Response);

        command.Dispose();
        CloseDBConnection();
    }

    public void DisableImage(string id, string usernumber, string enable)
    {
        string strsql = "UPDATE dash_image SET fld_Status = @disabled WHERE fld_autoinc = @id AND fld_UserNumber = @usernumber;";

        OpenDBConnection();

        SqlCommand ucommand = new SqlCommand(strsql, conn);

        ucommand.Parameters.Add(new SqlParameter("id", id));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));
        ucommand.Parameters.Add(new SqlParameter("disabled", enable == "D" ? "D" : ""));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();
    }

    public void DisableWebSite(string id, string usernumber, string enable)
    {
        string strsql = "UPDATE dash_website SET fld_Status = @disabled WHERE id = @id AND fld_UserNumber = @usernumber;";

        OpenDBConnection();

        SqlCommand ucommand = new SqlCommand(strsql, conn);

        ucommand.Parameters.Add(new SqlParameter("id", id));
        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));
        ucommand.Parameters.Add(new SqlParameter("disabled", enable == "D" ? "D" : ""));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();
    }

    public void ChangeSlideInterval(string settings_timer, string usernumber)
    {
        string strsql = "UPDATE dash_register SET fld_settings_timer = @settings_timer WHERE fld_UserNumber = @usernumber;";

        OpenDBConnection();

        SqlCommand ucommand = new SqlCommand(strsql, conn);

        ucommand.Parameters.Add(new SqlParameter("usernumber", usernumber));
        ucommand.Parameters.Add(new SqlParameter("settings_timer", settings_timer));

        ucommand.ExecuteNonQuery();
        ucommand.Dispose();
        CloseDBConnection();
    }

    public class WebsiteLink
    {
        public string website { get; set; }

    }

    public class Settings
    {
        public string slide_interval { get; set; }
    }

    public class basketItem
    {
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public DateTime fld_tobeshipped { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }

    public class GetShipping
    {
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }

    public class CheckoutItem
    {
        public string fld_adress1 { get; set; }
        public string itemname { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }

    public class productitem
    {
        public string fld_item { get; set; }
        public double fld_price { get; set; }
        public string fld_filename { get; set; }
        public string fld_text { get; set; }
        public string fld_settings_time { get; set; }
        public string groups { get; set; }
        public int productID { get; set; }
        public string session { get; set; }
        public string exercise1 { get; set; }
        public string exercise2 { get; set; }
        public string exercise3 { get; set; }
        public string exercise4 { get; set; }
        public string exercise5 { get; set; }
        public string date { get; set; }
        public string rating { get; set; }
        public string weight { get; set; }

    }

    public class ScheduleItem
    {
        public string fld_CurrentMonth { get; set; }
        public string fld_monday { get; set; }
        public string fld_tuesday { get; set; }
        public string fld_wednesday { get; set; }
        public string fld_thursday { get; set; }
        public string fld_friday { get; set; }
        public string fld_saturday { get; set; }
        public string fld_sunday { get; set; }
    }

    

    public class imageitem
    {

        public string fld_filename { get; set; }

    }


    public class referral
    {
        public string fld_codeused { get; set; }

    }

    public class reportItem
    {
        public string fld_fullname { get; set; }
        public string fld_postcode { get; set; }
        public string fld_address1 { get; set; }
        public string fld_address2 { get; set; }
        public string fld_productid { get; set; }
        public int fld_number { get; set; }
        public string fld_sub { get; set; }
        public string fld_weekly { get; set; }
        public string fld_date { get; set; }
        public string fld_tobeshipped { get; set; }
        //public string fld_items { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
    }

    public class reportShipping
    {
        public string fld_fullname { get; set; }
        public string fld_postcode { get; set; }
        public string fld_address1 { get; set; }
        public string fld_address2 { get; set; }
        public string fld_productid { get; set; }
        public int fld_number { get; set; }
        public string fld_sub { get; set; }
        public string fld_weekly { get; set; }
        public string fld_date { get; set; }
        public string fld_tobeshipped { get; set; }
        //public string fld_items { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
    }
    //public class reportItem
    //{
    //    public string fld_fullname { get; set; }
    //    public string fld_postcode { get; set; }
    //    public string fld_address1 { get; set; }
    //    public string fld_address2 { get; set; }
    //    public string fld_productid { get; set; }
    //    public int fld_number { get; set; }
    //    public string fld_sub { get; set; }
    //    public string fld_weekly { get; set; }
    //    public string fld_date { get; set; }
    //    //public string fld_items { get; set; }
    //    public double price { get; set; }
    //    public int quantity { get; set; }

    public class reportUser
    {
        public string fld_email { get; set; }
        public string fld_password { get; set; }
        public string fld_firstname { get; set; }
        public string fld_secondname { get; set; }
        public string fld_postcode { get; set; }
        //public string fld_items { get; set; }
        public string fld_username { get; set; }
        public string usertype { get; set; }
    }
}


