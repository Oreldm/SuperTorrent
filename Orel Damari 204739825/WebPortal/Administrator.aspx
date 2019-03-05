<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administrator.aspx.cs" Inherits="WebPortal.Administrator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <h1>Manage Users</h1>
        <div>
            <asp:Label ID="lblUser" runat="server" Text="Enter Username to search for update/delete:"></asp:Label>
            <br />
            <asp:TextBox ID="txtUser" runat="server" Width="438px"></asp:TextBox>

            <asp:ImageButton ID="iBtnSearch" runat="server" Height="60px" ImageUrl="~/Images/serach.png" OnClick="ImageButton1_Click" Width="438px" />
            <br />
            <br />
            <br />

            <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
            <br />
            <asp:TextBox ID="txtUsername" runat="server" Width="438px" ></asp:TextBox>
            <br />
            <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
            <br />
            <asp:TextBox ID="txtPassword" runat="server" Width="438px"></asp:TextBox>
            <br />
            <br />
            <br />
            <br />
            <asp:ImageButton ID="iBtnUpdate" runat="server" ImageUrl="~/Images/updateUser.png" ValidationGroup="UpdateUser" OnClick="iBtnUpdate_Click" Height="62px" Style="margin-top: 29px" />
            <asp:ImageButton ID="iBtnDelete" runat="server" Height="62px" ImageUrl="~/Images/deleteUser.png" Width="220px" OnClick="iBtnDelete_Click" />
        </div>

    </form>
</body>
</html>
