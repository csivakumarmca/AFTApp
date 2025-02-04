<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="BillingAPI.aspx.cs" Inherits="Billing.BillingAPI" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var launch = false;
        function launchModal() {
            $find("mpe").show();
        }
    </script>
    <style type="text/css">
        .ddl {
            border: 2px solid #7d6754;
            border-radius: 5px;
            padding: 3px;
            -webkit-appearance: none;
            background-image: url('Images/Arrowhead-Down-01.png');
            background-position: 88px;
            background-repeat: no-repeat;
            text-indent: 0.01px; /*In Firefox*/
            text-overflow: ''; /*In Firefox*/
        }

        .myGridClass {
            width: 100%;
            /*this will be the color of the odd row*/
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }

            /*data elements*/
            .myGridClass td {
                padding: 10px;
                border: solid 1px #c1c1c1;
                color: #717171;
            }

            /*header elements*/
            .myGridClass th {
                padding: 10px 10px;
                color: #fff;
                background: #424242;
                border-left: solid 1px #525252;
                font-size: 0.9em;
                align-content: center;
                text-align: center;
            }

            /*his will be the color of even row*/
            .myGridClass .myAltRowClass {
                background: #fcfcfc repeat-x top;
            }

            /*and finally, we style the pager on the bottom*/
            .myGridClass .myPagerClass {
                background: #424242;
            }

                .myGridClass .myPagerClass table {
                    margin: 5px 0;
                }

                .myGridClass .myPagerClass td {
                    border-width: 0;
                    padding: 0 6px;
                    border-left: solid 1px #666;
                    font-weight: bold;
                    color: #fff;
                    line-height: 12px;
                }

                .myGridClass .myPagerClass a {
                    color: #666;
                    text-decoration: none;
                }

                    .myGridClass .myPagerClass a:hover {
                        color: #000;
                        text-decoration: none;
                    }

        .theader {
            border-width: 1px;
            border-spacing:;
            border-style: dotted;
            border-color: blue;
            border-collapse: separate;
            background-color: white;
        }

            .theader th {
                padding: 10px;
                border-width: 1px;
                padding: 1px;
                border-style: none;
                border-color: gray;
                background-color: white;
            }

            .theader td {
                border-width: 1px;
                padding: 1px;
                border-style: none;
                border-color: gray;
                background-color: white;
            }

        th, td {
            padding: 15px;
            text-align: left;
        }

        .style2 {
        }

        .auto-style1 {
            width: 467px;
        }

        .style2 {
            padding: 10px;
        }
    </style>


</head>
<body>
    <form id="form1" runat="server">

        <div>
            <div id="dvbanner">
                <center>
              <img id="img1" src="images/alfuttaim logo.JPG" alt="Genesys" /></center>
                <br />
            </div>
            <div id="dvOrgId" runat="server" style="border: dotted 1px">
                <br />
                Organization ID :
            <asp:DropDownList ID="ddlOrgTrustorsList" runat="server" Width="220px" Height="40px" BackColor="#F6F1DB" OnSelectedIndexChanged="ddlOrgTrustorsList_SelectedIndexChanged"
                AutoPostBack="true" ForeColor="#7d6754" Font-Names="Andalus" CssClass="ddl">
            </asp:DropDownList>
                <asp:CheckBox ID="chkbillableUsage" Checked="true" runat="server" />
                <asp:Button ID="btnSearch" runat="server" Width="300px" Height="40px" Text="Search" OnClick="btnSearch_Click" />
                <br />
                <br />
                <br />
            </div>
            <div style="border: dotted 1px;" runat="server" visible="false" id="bodycontant">
                <div id="dvHeaderDetails" runat="server">
                    <table style="padding-right: 20px; border: dotted:1px; margin-left: 20px; font-size: 20px">


                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" CssClass="style2" GroupingText="BillingPeriod" Height="112px">
                                    <br />
                                    StartDate : 
            <asp:Label ID="lblbillingPeriodStartDate" runat="server" ForeColor="DarkOrange" Font-Bold="true"></asp:Label>


                                    <br />
                                    EndDate :
            <asp:Label ID="lblbillingPeriodEndDate" runat="server" ForeColor="DarkOrange" Font-Bold="true"></asp:Label>


                                    <br />


                                    <br />


                                </asp:Panel>
                            </td>
                            <td style="width: 20px"></td>
                            <td class="auto-style1">
                                <asp:Panel ID="Panel2" runat="server" CssClass="style2" Width="450px" GroupingText="ContractPeriod" Height="113px">
                                    <br />
                                    EffectiveDate : 
            <asp:Label ID="lblcontractEffectiveDate" runat="server" ForeColor="DarkOrange" Font-Bold="true"></asp:Label>


                                    <br />
                                    EndDate :
            <asp:Label ID="lblcontractEndDate" runat="server" ForeColor="DarkOrange" Font-Bold="true"></asp:Label>


                                    <br />


                                    <br />


                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="Panel3" runat="server" CssClass="style2" Width="450px" GroupingText="Otherinfo" Height="113px">
                                    <br />
                                    MinimumMonthlyAmount :
            <asp:Label ID="lblminimumMonthlyAmount" runat="server" ForeColor="DarkOrange" Font-Bold="true"></asp:Label>


                                    <br />
                                    SubscriptionType :
            <asp:Label ID="lblsubscriptionType" runat="server" ForeColor="DarkOrange" Font-Bold="true"></asp:Label>


                                    <br />
                                    Currency :
            <asp:Label ID="lblCurrency" runat="server" ForeColor="DarkOrange" Font-Bold="true"></asp:Label>


                                    <br />


                                </asp:Panel>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td style="width: 20px"></td>
                            <td class="auto-style1">
                        </tr>
                    </table>
                </div>
                <div id="dvusage" runat="server" visible="false" style="border: dotted 1px;">
                    <div style="padding: 5px;">
                        <asp:GridView ID="gvLimittedUsage" runat="server" AutoGenerateColumns="False" CssClass="myGridClass" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                            DataKeyNames="LicenseName" OnRowDataBound="gvLimittedUsage_RowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged">
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <Columns>
                                <asp:BoundField DataField="RowNo" HeaderText="RowNo" />
                                <asp:TemplateField HeaderText="Pay">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnLicenseName" runat="server" Text='<%# Bind("LicenseName") %>' CssClass="btn btn-info"
                                            OnClick="Display"></asp:LinkButton>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <%-- <asp:BoundField DataField="LicenseName" HeaderText="LicenseName" />--%>
                                <asp:BoundField DataField="grouping" HeaderText="grouping" />
                                <asp:BoundField DataField="prepayQuantity" HeaderText="PrepayQuantity" />
                                <asp:BoundField DataField="usageQuantity" HeaderText="UsageQuantity" />
                                <asp:BoundField DataField="prepayPrice" HeaderText="prepayPrice" />
                                <asp:BoundField DataField="overagePrice" HeaderText="overagePrice" />
                                <%--                            <asp:BoundField DataField="unitOfMeasureType" HeaderText="unitOfMeasureType" />
                            <asp:BoundField DataField="isCancellable" HeaderText="isCancellable" />
                            <asp:BoundField DataField="bundleQuantity" HeaderText="bundleQuantity" />
                            <asp:BoundField DataField="isThirdParty" HeaderText="isThirdParty" />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div>
                    <div style="text-align: center; padding: 10px;">
                        <asp:GridView GridLines="None"
                            ID="grdTranspose" OnRowCreated="grdTranspose_RowCreated" runat="server" CellPadding="4" ForeColor="#333333">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" BorderWidth="1" BorderStyle="Dotted" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </div>

                </div>
            </div>


        </div>
    </form>
</body>
</html>
