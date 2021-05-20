
/**
 *  Update SalesLead File Link
 * 
 * @param {int} leadId
 */

function ShowSalesLeadLink(leadId) {
    $("#AddLeadFile-LeadId").val(leadId);

    $("#AddLeadFileModal").modal("show");

    //get sales item lead file link
    GetSalesLeadLink(leadId);
}


function GetSalesLeadLink(leadId) {
    //Get latest Lead link from server
    $.get("/SalesLeads/GetLeadLastestLink", { id: leadId })
        .done((result) => {
            $("#AddLeadFile-Link").val(result);
        })
        .fail(() => {
            console.log("Error: Unable to get link.");
            alert("Unable to get sales lead link");
        });
}




function Submit_AddLeadFileLink() {
    var leadId = $("#AddLeadFile-LeadId").val();
    var input_link = $("#AddLeadFile-Link").val();

    console.log("leadID:" + leadId);
    console.log("input_link:" + input_link);

    $.post("/SalesLeads/AddLeadLink", { id: leadId, link: input_link })
        .done((res) => {
            if (res == "OK") {
                console.log("Lead Link added");
                window.location.reload(false);
            }
        })
        .fail(() => { 
            console.log("Unable to add lead link");
            alert("Unable to add sales lead link");
        });
}


function RedirectLeadFileLink(leadId) {
    //Get latest Lead link from server
    $.get("/SalesLeads/GetLeadLastestLink", { id: leadId })
        .done((result) => {
            console.log(result);
            window.open(result);
            //window.location.href = result;
        })
        .fail(() => {
            console.log("Error: Unable to get link.");
            alert("Unable to get sales lead link");
        });
}