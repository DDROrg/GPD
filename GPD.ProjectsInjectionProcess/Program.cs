using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using GPD.DAL.SqlDB;
using System.Xml.XPath;
using System.Data;

namespace GPD.ProjectsInjectionProcess
{
    class Program
    {
        private string _projects_archive_folder_path = @"C:\Users\ruslan_prybyslavskyy\Desktop\GPD-Solution\projects";
        private string _projects_archive_folder_path_done = @"C:\Users\ruslan_prybyslavskyy\Desktop\GPD-Solution\projects\DONE";

        private string _api_cookiebased_session_identifier = "1bcba8f3ab5e4b0ab87cf5e424de59f6";
        private string _inject_into_api_url = "http://ec2-52-201-172-243.compute-1.amazonaws.com/api/ULSPOT/Project";
        //private string _inject_into_api_url = "http://localhost:52308/api/TEST/Project";

        private string _db_conn = "Data Source=gpd-sql-server.ca6z1pmdeu40.us-east-1.rds.amazonaws.com;Initial Catalog=GPDMaster;User ID=gpdWebUser;Password=1Kalinovka;Persist Security Info=True;";
        //private string _db_conn = "Data Source=gpddbinstance.ca6z1pmdeu40.us-east-1.rds.amazonaws.com;Initial Catalog=GPD2;User ID=gpdUser;Password=1Kalinovka;Persist Security Info=True;";

        static void Main(string[] args)
        {
            Program program = new Program();

            //program.GetListOfProjects();
            program.InjectProjectsBasedOnFiles();
        }

        private void InjectProjectsBasedOnFiles()
        {
            XDocument xDoc = XDocument.Load(_projects_archive_folder_path_done + @"\progress.xml");
            DirectoryInfo d = new DirectoryInfo(_projects_archive_folder_path);
            FileInfo[] Files = d.GetFiles("*.json");

            try
            {
                foreach (FileInfo file in Files)
                {
                    string jsonString = File.ReadAllText(file.FullName, System.Text.Encoding.UTF8);
                    JObject jObj = JObject.Parse(jsonString);

                    // project id
                    string projectId = jObj["id"].ToString();

                    xDoc.Root.Add(new XElement("item",
                       new XAttribute("original-project-id", projectId)
                       ));

                    // user user email address
                   string userEmail = (jObj.SelectToken(@"session.account.username") == null) ?
                        string.Empty : jObj.SelectToken(@"session.account.username").Value<string>();

                    // user user sso id
                    string userSSOId = (jObj.SelectToken(@"session.account.sso-id") == null) ?
                        string.Empty : jObj.SelectToken(@"session.account.sso-id").Value<string>();

                    string firstName = "", lastName = "";

                    // get GPD user id
                    int userId = this.CreateUser(userSSOId, userEmail, firstName, lastName);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this._inject_into_api_url);
                    request.ContentType = "application/json";
                    request.Method = "POST";

                    if (userId > 0)
                        request.Headers.Add("user-identifier", userId.ToString());

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(jsonString);
                    }

                    using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
                    {
                        try
                        {
                            using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                            {
                                JavaScriptSerializer js = new JavaScriptSerializer();
                                var objText = reader.ReadToEnd();
                                var respJObj = JObject.Parse(objText);

                                string newProjectId = respJObj["project-id"].ToString();

                                // add project id
                                xDoc.XPathSelectElement("//*[local-name()='item'][@original-project-id='" + projectId + "']").Value = newProjectId;
                            }
                        }
                        catch (Exception exc)
                        {
                            string tt = "";
                        }

                    }
                }

            }
            catch (Exception e)
            {
                string yy = "";
            }

            xDoc.Save(_projects_archive_folder_path_done + @"\progress.xml");
        }

        private int CreateUser(string userSSOId, string userEmail, string firstName, string lastName)
        {
            #region xml-document
            XDocument userDetails = XDocument.Parse(@"
<UserDetails xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.gpd.com"">
  <firstName></firstName>
  <lastName></lastName>
  <email></email>
  <jobTitle></jobTitle>
  <phone></phone>
  <company>
    <id>0</id>
    <name i:nil=""true"" />
    <website i:nil=""true"" />
    <country i:nil=""true"" />
    <address i:nil=""true"" />
    <address2 i:nil=""true"" />
    <city i:nil=""true"" />
    <state i:nil=""true"" />
    <postalCode i:nil=""true"" />
    <defaultIndustry i:nil=""true"" />
  </company>
  <password>1password</password>
  <additional-data>
    <item type=""enewsletters-communication-flag"">false</item>
    <item type=""email-communication-flag"">false</item>
  </additional-data>
</UserDetails>
");
            #endregion xml-document

            userDetails.XPathSelectElement("//*[local-name()='firstName']").Value = firstName;
            userDetails.XPathSelectElement("//*[local-name()='lastName']").Value = lastName;
            userDetails.XPathSelectElement("//*[local-name()='email']").Value = userEmail;
            userDetails.XPathSelectElement("//*[local-name()='jobTitle']").Value = "SSO: " + userSSOId;

            // add user details
            int errorCode;
            string errorMsg;
            int userId = new ProjectDB(this._db_conn).AddUserDetails(userDetails, "1.1.1.1", out errorCode, out errorMsg);

            if (userId == -1 && errorMsg.Equals("the provided e-mail address is already registered"))
            {
                //TODELETE_GetUsers
                DataSet dataSet = new ProjectDB(this._db_conn).TODELETE_GetUsers(userEmail, null, null, null);
                DataRow[] dataRow = dataSet.Tables[0].Select(string.Format("email = '{0}'", userEmail));
                userId = (int)dataRow[0]["user_id"];
            }

            return userId;
        }

        private void GetListOfProjects()
        {
            List<string> projectList = new List<string>() { "13051",
"13056",
"13057",
"13069",
"13071",
"13072",
"13074",
"13079",
"12687",
"12689",
"12748",
"12751",
"12758",
"12779",
"12773",
"12781",
"12829",
"12816",
"12806",
"12892",
"12942",
"13012",
"13001",
"12999",
"12998",
"13023",
"13029",
"13033",
"13034",
"13085"
            };

            projectList.ForEach(T => {
                this.GetProjectById(T);
            });
        }

        private bool GetProjectById(string projectId)
        {
            string apiUrl = "https://api.sweets.construction.com/v1/projects/{0}";

            try
            {
                apiUrl = string.Format(apiUrl, projectId);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Headers.Add("session-identifier", this._api_cookiebased_session_identifier);

                string
                       userEmail = string.Empty,
                       userSSOId = string.Empty,
                       projectFile = string.Empty;

                var apiResponse = string.Empty;

                using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                    {
                        apiResponse = reader.ReadToEnd();
                    }
                }

                if (apiResponse.Length == 0)
                    return false;

                var jObject = JObject.Parse(apiResponse);

                // user sso-id
                userSSOId = (jObject.SelectToken(@"session.account.sso-id") == null) ?
                    string.Empty : jObject.SelectToken(@"session.account.sso-id").Value<string>();

                // user user email address
                userEmail = (jObject.SelectToken(@"session.account.username") == null) ?
                    string.Empty : jObject.SelectToken(@"session.account.username").Value<string>();

                // project file
                projectFile = string.Format(_projects_archive_folder_path + @"\{0}.json", projectId);

                // This text is added only once to the file.
                if (!File.Exists(projectFile))
                {
                    // Create a file to write to.
                    File.WriteAllText(projectFile, jObject.ToString(), System.Text.Encoding.UTF8);
                }
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }

        protected bool GetProjectsList(int itemIndex)
        {
            string url = string.Format("https://api.sweets.construction.com/v1/projects?page-number={0}&utc-offset=-4&include-project-items=true&include-categories=true", itemIndex);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("session-identifier", this._api_cookiebased_session_identifier);

            JArray array = new JArray();
            string
                        projectId = string.Empty,
                        userEmail = string.Empty,
                        userSSOId = string.Empty,
                        projectFile = string.Empty;

            using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
            {
                try
                {
                    using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var objText = reader.ReadToEnd();

                        if (objText.Length == 0)
                            return false;

                        var respList = JArray.Parse(objText);

                        if (respList.Count == 0)
                            return false;


                        foreach (JObject jObj in respList)
                        {
                            string jsonString = jObj.ToString();

                            if (jObj["id"] == null)
                            {
                                projectId = string.Empty;
                                continue;
                            }

                            // get project id
                            projectId = jObj["id"].ToString();

                            // user sso-id
                            userSSOId = (jObj.SelectToken(@"session.account.sso-id") == null) ?
                                string.Empty : jObj.SelectToken(@"session.account.sso-id").Value<string>();

                            // user user email address
                            userEmail = (jObj.SelectToken(@"session.account.username") == null) ?
                                string.Empty : jObj.SelectToken(@"session.account.username").Value<string>();

                            // project file
                            projectFile = string.Format(_projects_archive_folder_path + @"\{0}.json", projectId);

                            // This text is added only once to the file.
                            if (!File.Exists(projectFile))
                            {
                                // Create a file to write to.
                                File.WriteAllText(projectFile, jObj.ToString(), System.Text.Encoding.UTF8);
                            }

                        }
                    }
                }
                catch (Exception exc)
                {
                    return false;
                }

                return true;
            }

        }
    }
}
