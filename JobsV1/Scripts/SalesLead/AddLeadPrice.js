
/**
 *  Update SalesLead Price
 * 
 * @param {any} leadId
 */


function ShowSalesLeadPrice(leadId) {
    $("#AddLeadPrice-LeadId").val(leadId);

    $("#AddLeadPriceModal").modal("show");

    //get sales item lead Price
    GetSalesLeadPrice(leadId);
}

function Submit_AddSalesLeadPrice() {
    var leadId = $("#AddLeadPrice-LeadId").val();
    var input_Price = $("#AddLeadPrice-Price").val();

    console.log("leadID:" + leadId);
    console.log("Price:" + input_Price);

    $.post("/SalesLeads/UpdateLeadPrice", { id: leadId, price: input_Price })
        .done(() => {
            window.location.reload(false);
        })
        .fail(() => {
            console.log("Unable to update Price");
        });
}

function GetSalesLeadPrice(leadId) {
    //GetLeadWeight

    $.get("/SalesLeads/GetLeadPrice", { id: leadId })
        .done((res) => {
            $("#AddLeadPrice-Price").val(res);
        })
        .fail(() => {
            console.log("Unable to update Price");
        });
}
