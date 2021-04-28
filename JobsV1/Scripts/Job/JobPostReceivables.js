

/**
 *  Job Post Receivables
 *  Copy data from Jobs to Receivables
 *  
 */

function ShowPostReceivablesModal(jobId) {

    $.get("/JobOrder/GetJobRcvDetails", { id: jobId })
        .done((result) => {
            console.log(result);
            InitPostReceivables(result);
        })
        .fail((err) => { });
}

function InitPostReceivables(jobDetails) {
    $("#PostRcv-JobId").val(jobDetails["Id"]);
    $("#PostRcv-Description").val(jobDetails["Description"]);
    $("#PostRcv-Date").val(moment().format("MM/DD/YYYY"));
    $("#PostRcv-DtStart").val(moment(jobDetails["SvcStart"]).format("MM/DD/YYYY"));
    $("#PostRcv-DtEnd").val(moment(jobDetails["SvcEnd"]).format("MM/DD/YYYY"));
    $("#PostRcv-DtDue").val(moment().add(15, 'days').format("MM/DD/YYYY"));
    $("#PostRcv-Amount").val(jobDetails["Amount"]);

    $("#PostRcv-Company").val(jobDetails["Company"]);
    $("#PostRcv-Customer").val(jobDetails["Customer"]);
    $("#PostRcv-Mobile").val(jobDetails["Contact"]);
    $("#PostRcv-Email").val(jobDetails["Email"]);
}

function SubmitPostReceivables() {

    var data = {
        InvoiceId: $("#PostRcv-JobId").val(),
        Description: $("#PostRcv-Description").val(),
        DtInvoice: $("#PostRcv-Date").val(),
        DtService: $("#PostRcv-DtStart").val(),
        DtServiceTo: $("#PostRcv-DtEnd").val(),
        DtDue: $("#PostRcv-DtDue").val(),
        Amount: $("#PostRcv-Amount").val(),

        //account 
        Name: $("#PostRcv-Customer").val(),
        Company: $("#PostRcv-Company").val(),
        Mobile: $("#PostRcv-Mobile").val(),
        Email: $("#PostRcv-Email").val()
    };

    $.post("/Receivables/ArTransactions/PostJobReceivables", data)
        .done((result) => {
            console.log(result);
            $("#PostJobRecievables").modal("hide");
            alert("Receivables Posted");
        });
}