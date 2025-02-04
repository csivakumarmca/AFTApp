using System;
using System.Diagnostics;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Model;
using PureCloudPlatform.Client.V2.Extensions;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;

using System.Threading;
using System.Web.UI.WebControls;
using Billing.Model;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json;
using System.Web.UI;

namespace Billing
{
    public partial class index : System.Web.UI.Page
    {
        ValidationManager validationManager = new ValidationManager();
        SessionManager sessionManager = new SessionManager();
        TrusteeBillingOverview result = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {







                //dvbillingdetails.Visible = false;
                bodycontant.Visible = false;
                try
                {

                    if (sessionManager.GetUserProfile() != null)
                    {
                        Session.Clear();
                        Session.Abandon();
                    }

                    if (sessionManager.CheckValidSession())
                    {
                        Server.Transfer("Main.aspx");
                    }
                    else
                    {
                        if (Request["code"] != null && Request["code"].ToString() != string.Empty)
                        {
                            string token = GetTokenFromCode(Request["code"]);
                            if (!string.IsNullOrEmpty(token))
                            {
                                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token;
                            }

                            PureCloudRegionHosts region = PureCloudRegionHosts.eu_central_1;
                            PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath(region);
                            LoadTrustorOrgIdValues();
                        }
                        else
                        {
                            try
                            {

                                Response.Redirect(ConfigurationManager.AppSettings["CodeUrl"] + "?" + "client_id=" + ConfigurationManager.AppSettings["ClientId"] + "&" + "response_type=code&redirect_uri=" + ConfigurationManager.AppSettings["RedirectUrl"]);

                            }
                            catch (ThreadAbortException) { }

                        }
                    }

                }
                catch (ThreadAbortException) { }
                catch (Exception ex)
                {
                    //log.Error("Error while logging in..", ex);
                    //lblStatus.Text = "Error occured. Please contact your administrator.";
                }
            }
        }
        protected void BindRowData(String json)
        {
            json = AddSquareBrackets(json);

            System.Data.DataTable dt = ConvertJsonToDataTable(json);
            //grdTranspose.DataSource = Transpose(dt);
            grdTranspose.DataSource = dt;
            grdTranspose.DataBind();
        }


        protected System.Data.DataTable Transpose(System.Data.DataTable dt)
        {




            //DataTable dt = (Datatable)ViewState["GridData"];
            System.Data.DataTable newdt = new System.Data.DataTable();
            //Create New Columns( count of columns will be equal to number of rows including header  
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                //Column Name string is not specified because column name will come in rows at start  
                newdt.Columns.Add("");
            }
            //Now Iterate through old Datatable and fill new Datatable   
            //Create a new row in New Datatable   
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                DataRow newdr = newdt.NewRow();
                //Assign first column Value in NewDatatable with column name of old table  
                newdr[0] = dt.Columns[k].ColumnName.ToString();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    newdr[j + 1] = dt.Rows[j][k].ToString();
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }


        protected void grdTranspose_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].CssClass = "header";
            }
        }

        private static string AddSquareBrackets(string json)
        {
            return $"[{json}]";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Your Saving code.
        }

        private string GetTokenFromCode(string code)
        {

            HttpWebRequest client = WebRequest.CreateHttp(ConfigurationManager.AppSettings["TokenUrl"].ToString());
            client.Method = "POST";
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),

                new KeyValuePair<string, string>("redirect_uri", ConfigurationManager.AppSettings["RedirectUrl"])
            });

            var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(ConfigurationManager.AppSettings["ClientId"] + ":" + ConfigurationManager.AppSettings["Secret"]));
            client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + basicAuth);
            byte[] byteArray = Encoding.GetEncoding("ISO-8859-1").GetBytes("grant_type=authorization_code&code=" + HttpUtility.UrlEncode(code) + "&" + "redirect_uri=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["RedirectUrl"]));
            client.ContentType = "application/x-www-form-urlencoded";
            client.ContentLength = byteArray.Length;
            Stream dataStream = client.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = client.GetResponse();

            var token = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd())["access_token"].ToString();
            return token;

        }

        private void LoadTrustorOrgIdValues()
        {
            PureCloudRegionHosts region = PureCloudRegionHosts.eu_central_1; // Genesys Cloud region
            PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath(region);

            // Configure OAuth2 access token for authorization: PureCloud OAuth
            // The following example is using the Client Credentials Grant
            //var accessTokenInfo = Configuration.Default.ApiClient.PostToken("your_client_credential_grant_id", "your_client_credential_grant_secret");

            var apiInstance = new OrganizationAuthorizationApi();
            var pageSize = 56;  // int? | Page size (optional)  (default to 25)
            var pageNumber = 1;  // int? | Page number (optional)  (default to 1)

            try
            {
                // The list of organizations that have authorized/trusted your organization.
                TrustorEntityListing result = apiInstance.GetOrgauthorizationTrustors(pageSize, pageNumber);

                ddlOrgTrustorsList.Items.Insert(0, new ListItem("--Select--", "0"));
                for (int i = 0; i < result.Entities.Count; i++)
                {
                    PureCloudPlatform.Client.V2.Model.Organization organization = result.Entities[i].Organization;
                    ddlOrgTrustorsList.Items.Add(new ListItem(organization.Name.ToString(), organization.Id.ToString()));
                }

                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling OrganizationAuthorization.GetOrgauthorizationTrustors: " + e.Message);
            }
        }
        private void GetBillingTrusteebillingoverviewTrustorOrgId()
        {
            var apiInstance = new BillingApi();
            var trustorOrgId = ddlOrgTrustorsList.SelectedValue;  // string | The organization ID of the trustor (customer) organization.
            var billingPeriodIndex = 0;  // int? | 0 for active period (overview data may change until period closes). 1 for prior completed billing period. 2 for two billing cycles prior, and so on. (optional)  (default to 0)

            try
            {
                // Get the billing overview for an organization that is managed by a partner.
                result = apiInstance.GetBillingTrusteebillingoverviewTrustorOrgId(trustorOrgId, billingPeriodIndex);
                Session["result"] = result.ToJson();

                lblbillingPeriodStartDate.Text = result.BillingPeriodStartDate.Value.ToString();
                lblbillingPeriodEndDate.Text = result.BillingPeriodEndDate.Value.ToString();
                lblcontractEffectiveDate.Text = result.ContractEffectiveDate.Value.ToString();
                lblcontractEndDate.Text = result.ContractEndDate.Value.ToString();
                lblminimumMonthlyAmount.Text = result.MinimumMonthlyAmount.ToString();
                lblsubscriptionType.Text = result.SubscriptionType.ToString();
                lblCurrency.Text = result.Currency.ToString();




                StringBuilder sbLimittedUsage = new StringBuilder();
                StringBuilder sbOverUsage = new StringBuilder();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.AddRange(new DataColumn[11] { new DataColumn("RowNo", typeof(int)), new DataColumn("LicenseName", typeof(string)), new DataColumn("grouping", typeof(string)),
                            new DataColumn("PrepayQuantity", typeof(string)), new DataColumn("UsageQuantity", typeof(string)),
                           new DataColumn("prepayPrice", typeof(string)),  new DataColumn("OveragePrice",typeof(string))
                 ,new DataColumn("unitOfMeasureType", typeof(string)),  new DataColumn("isCancellable",typeof(string))
                 ,                  new DataColumn("bundleQuantity", typeof(string)),  new DataColumn("isThirdParty",typeof(string))

                });

                int rowCount = 1;
                for (int i = 0; i < result.Usages.Count; i++)
                {
                    if (chkbillableUsage.Checked == false)
                    {
                        //dt.Rows.Add(rowCount, result.Usages[i].Name.ToString(), result.Usages[i].PrepayQuantity == null ? "" : result.Usages[i].PrepayQuantity, result.Usages[i].UsageQuantity == null ? "" : result.Usages[i].UsageQuantity, result.Usages[i].PrepayPrice == null ? "" : result.Usages[i].PrepayPrice, result.Usages[i].OveragePrice == null ? "" : result.Usages[i].OveragePrice.ToString(), result.Usages[i].UnitOfMeasureType, result.Usages[i].IsCancellable, result.Usages[i].BundleQuantity == null ? "" : result.Usages[i].BundleQuantity, result.Usages[i].IsThirdParty);
                        dt.Rows.Add(rowCount, result.Usages[i].Name.ToString(), result.Usages[i].Grouping == null ? "" : result.Usages[i].Grouping.ToString(), result.Usages[i].PrepayQuantity == null ? "" : result.Usages[i].PrepayQuantity, result.Usages[i].UsageQuantity == null ? "" : result.Usages[i].UsageQuantity, result.Usages[i].PrepayPrice == null ? "" : result.Usages[i].PrepayPrice, result.Usages[i].OveragePrice == null ? "" : result.Usages[i].OveragePrice.ToString(), result.Usages[i].UnitOfMeasureType, result.Usages[i].IsCancellable, result.Usages[i].BundleQuantity == null ? "" : result.Usages[i].BundleQuantity, result.Usages[i].IsThirdParty);

                        rowCount++;
                    }
                    else
                    {
                        string[] GroupingNames = ConfigurationManager.AppSettings["GroupingNames"].Split(',');

                        foreach (string currentGroupingNames in GroupingNames)
                        {
                            if (currentGroupingNames == result.Usages[i].Grouping)
                            {
                                
                                dt.Rows.Add(rowCount, result.Usages[i].Name.ToString(), result.Usages[i].Grouping == null ? "" : result.Usages[i].Grouping.ToString(), result.Usages[i].PrepayQuantity == null ? "" : result.Usages[i].PrepayQuantity, result.Usages[i].UsageQuantity == null ? "" : result.Usages[i].UsageQuantity, result.Usages[i].PrepayPrice == null ? "" : result.Usages[i].PrepayPrice, result.Usages[i].OveragePrice == null ? "" : result.Usages[i].OveragePrice.ToString(), result.Usages[i].UnitOfMeasureType, result.Usages[i].IsCancellable, result.Usages[i].BundleQuantity == null ? "" : result.Usages[i].BundleQuantity, result.Usages[i].IsThirdParty);
                                //dt.Rows.Add(rowCount, result.Usages[i].Name.ToString(), result.Usages[i].PrepayQuantity == null ? "" : result.Usages[i].PrepayQuantity, result.Usages[i].UsageQuantity == null ? "" : result.Usages[i].UsageQuantity, result.Usages[i].PrepayPrice == null ? "" : result.Usages[i].PrepayPrice, result.Usages[i].OveragePrice == null ? "" : result.Usages[i].OveragePrice.ToString(), result.Usages[i].UnitOfMeasureType, result.Usages[i].IsCancellable, result.Usages[i].BundleQuantity == null ? "" : result.Usages[i].BundleQuantity, result.Usages[i].IsThirdParty);
                                rowCount++;
                                break;
                            }
                        }

                    }

                    //  dvLimittedUsage.InnerText = sbLimittedUsage.ToString();
                    //  dvOverUsage.InnerText = sbOverUsage.ToString();
                }
                gvLimittedUsage.DataSource = dt;
                gvLimittedUsage.DataBind();

                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling Billing.GetBillingTrusteebillingoverviewTrustorOrgId: " + e.Message);
            }
        }

        protected void Display(object sender, EventArgs e)
        {
            int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
            GridViewRow row = gvLimittedUsage.Rows[rowIndex];

            JObject obj = JObject.Parse(Session["result"].ToString());

            JObject jsonObj = JObject.Parse(Session["result"].ToString());
            JArray myArray = (JArray)jsonObj["usages"];

            string lnkBtnLicenseName = ((System.Web.UI.WebControls.LinkButton)row.Cells[1].Controls[1]).Text;
            string currentRowDetails = "";
            foreach (JObject item in myArray)
            {
                //  LinkButton lnkButton = (((System.Web.UI.WebControls.LinkButton)row.Cells[1].Controls).Items[1])).Text;


                if (item["name"].ToString() == lnkBtnLicenseName)
                {
                    currentRowDetails = item.ToString();
                    break;
                }
            }

            BindRowData(currentRowDetails);

        }




        private static System.Data.DataTable ConvertJsonToDataTable(string jsonData)
        {
            try
            {
                return JsonConvert.DeserializeObject<System.Data.DataTable>(jsonData);
            }
            catch
            {
                return null;
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //dvbillingdetails.Visible = true;
            bodycontant.Visible = true;
            dvusage.Visible = true;
            GetBillingTrusteebillingoverviewTrustorOrgId();
        }
        protected void ddlOrgTrustorsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // dvbillingdetails.Visible = false;
            bodycontant.Visible = false;
            dvusage.Visible = false;
        }
        protected void gvLimittedUsage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int prepayQuantity = int.Parse(e.Row.Cells[3].Text);
                int usageQuantity = int.Parse(e.Row.Cells[4].Text);


                foreach (TableCell cell in e.Row.Cells)
                {
                    if (Convert.ToInt32(prepayQuantity) >= Convert.ToInt32(usageQuantity))
                        cell.ForeColor = Color.DarkGreen;
                    else
                        cell.ForeColor = Color.Red;

                }
                //  e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvLimittedUsage, "Select$" + e.Row.RowIndex);
                //   e.Row.Attributes["style"] = "cursor:pointer";
            }
            //if (ex.CommandName == "Select")
            //{
            //    //Determine the RowIndex of the Row whose LinkButton was clicked.
            //    int rowIndex = Convert.ToInt32(ex.CommandArgument);

            //    //Reference the GridView Row.
            //    GridViewRow row = GridView1.Rows[rowIndex];

            //    //Fetch value of Name.
            //   // string name = (row.FindControl("prepayQuantity") as TextBox).Text;

            //    //Fetch value of Country
            //    string country = row.Cells[1].Text;

            //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name: " + country + "\\nCountry: " + country + "');", true);
            //}

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string commandName = btn.CommandName;
            string commandArgument = btn.CommandArgument;
        }


        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = gvLimittedUsage.SelectedRow.RowIndex;
            string name = gvLimittedUsage.SelectedRow.Cells[1].Text;
            // string country = GridView1.SelectedRow.Cells[1].Text;
            // string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
            // ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + name + "');", true);
        }


    }
}