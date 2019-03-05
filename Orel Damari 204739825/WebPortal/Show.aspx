<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="WebPortal.Show" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <h1>Show</h1>
    <div>
        <asp:Label ID="Label1" runat="server" Text="Active Users:" style="font-weight: 700"></asp:Label>
        <asp:Label ID="lblActiveUsers" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Total Users:" style="font-weight: 700"></asp:Label>
        <asp:Label ID="lblTotalUsers" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label3" runat="server" Text="Active files in server:" style="font-weight: 700"></asp:Label>
    </div>
        <br />
        <asp:Label ID="lblFiles" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label4" runat="server" Text="File name to serach:" style="font-weight: 700"></asp:Label>
        <br />
        <asp:TextBox ID="txtFilename" runat="server" Width="438px"></asp:TextBox>
        <br />
        <asp:ImageButton ID="iBtnSerach" runat="server" Height="60px" ImageUrl="~/Images/serach.png" OnClick="iBtnSerach_Click" Width="438px" />
        <br />
        <br />
        <asp:Label ID="LabelFile" runat="server" Text="Label"></asp:Label>
        <br />
    </form>
</body>
</html>
