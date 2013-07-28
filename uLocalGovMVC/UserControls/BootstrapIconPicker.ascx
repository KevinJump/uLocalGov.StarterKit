<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BootstrapIconPicker.ascx.cs" Inherits="monosnow.umbraco.uCssClassNameDropdown.usercontrols.BootstrapIconPicker" %>
<div id="bootstrapIconPickerHolder" runat="server">
<asp:ListView ID="lvIcons" runat="server" ItemPlaceholderID="itemPlaceHolder" GroupItemCount="4" GroupPlaceholderID="groupPlaceHolder">
<LayoutTemplate>
<div class="uCssClassNamePickerHolder" style="height:250px; width:650px;overflow:scroll">
<asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
</div>
</LayoutTemplate>
<GroupTemplate>
<div style="clear:both"></div>
<div>
<asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
</div>
</GroupTemplate>
<ItemTemplate>
<a style="text-decoration:none; font-size:1.4em;color:#000000" href="#" onclick="setDropdownValue('<%#Container.DataItem %>');"><div class="iconHolder iconHolder-<%#Container.DataItem %>" style="width:150px;height:50px;float:left;"><i class="icon-<%#Container.DataItem %>"></i> <%#Container.DataItem %></div></a>
</ItemTemplate>
</asp:ListView>
<asp:DropDownList ID="ddlIcons" runat="server"></asp:DropDownList>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        // first load, set highlight on selected icon
        setIconHighlight('<%=this.value.ToString() %>');
        $('select#<%=ddlIcons.ClientID %>').change(function(e) {
        setIconHighlight(this.value);
    
        });
       
    });

    function setIconHighlight(selectedValue) {
        //first remove existing highlights
        $('#<%=bootstrapIconPickerHolder.ClientID %> div.iconHolder').css("background-color", "").css("border", "0px").css("font-weight", "normal");
        // add highlight to selected icon
        if (typeof selectedValue != 'undefined' && selectedValue != '') {
      
        $('#<%=bootstrapIconPickerHolder.ClientID %> div.iconHolder-' + selectedValue).css("background-color", "#FFFFCC").css("border", "3px dashed orange").css("font-weight", "bold");
//scroll to highlighted
        var container = $('#<%=bootstrapIconPickerHolder.ClientID %> div.uCssClassNamePickerHolder');
        var scrollTo = $('#<%=bootstrapIconPickerHolder.ClientID %> div.iconHolder-' + selectedValue);

        container.scrollTop(
            scrollTo.offset().top - container.offset().top-100 + container.scrollTop()
        );
      


    }
    }
 

    function setDropdownValue(selectedValue) {
    $('select#<%=ddlIcons.ClientID %>').val(selectedValue);
       // document.getElementById("<%=ddlIcons.ClientID %>").value = selectedValue;  
        setIconHighlight(selectedValue);     
    }


</script>