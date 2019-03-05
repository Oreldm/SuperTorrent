<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="WebPortal.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <h1>Registration</h1>
        <div>
            <asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label>
            <br />
            <asp:TextBox ID="txtUsername" runat="server" Width="438px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="reqUsername" ControlToValidate="txtUsername" ErrorMessage="Please enter your username" Display="Dynamic" ValidationGroup="Registration" Style="font-weight: 700" />
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
            <br />
            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="438px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="reqPassword" ControlToValidate="txtPassword" ErrorMessage="Please enter your password" Display="Dynamic" ValidationGroup="Registration" Style="font-weight: 700" />
            <br />
            <br />
            <br />
            <br />
            <!--<asp:Button ID="btnRegister" type="submit" runat="server" Text="Register"  ValidationGroup="Registration"/>-->

            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/singup2.png" OnClick="ImageButton1_Click" type="submit" Text="Register" ValidationGroup="Registration" Height="60px" Width="438px" />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Italic="True" Font-Overline="False" Font-Size="X-Large"></asp:Label>
        </div>
    </form>
</body>
</html>
