
// Payables Transaction
// Handle Repeating payables modal 
// Update payables transaction status

function Initialize(status, sortBy) {
    if (sortBy != 'undefined') {
        //sort
        $("#sort-" + sortBy).addClass('active');
        $("#sort-" + sortBy).siblings('.active').removeClass('active');

    }

    if (status != '0') {
        //sort
        $("#status-" + status).addClass('active');
        $("#status-" + status).siblings('.active').removeClass('active');
    } else {

        $("#status-" + status).addClass('active');
        $("#status-" + status).siblings('.active').removeClass('active');

    }

    Init_Repeating();
}

//update filter on list
function UpdateFilter(statusId, sort) {

    if (sort == undefined) {
        sort = "DueDate";
    }

    if (statusId == undefined) {
        statusId = 0;
    }

    window.location.href = "/Payables/ApTransactions/Index?status=" + statusId + "&sort=" + sort;

}


// ---------- Update Status -------------- //

//update payables status
function UpdateStatus(transId, statusId) {
    $("#overlay").show();
    var result = $.post("/Payables/ApTransactions/UpdateTransStatus", {
        transId: transId,
        statusId: statusId
        }, (response) => {
            //console.log("Update Status : " + response);
            if (response == "True") {
                $("#overlay").hide();

                AddStatusLabel(transId, statusId);
            } else {
                alert("Unable to Update transaction.");
                $("#overlay").hide();
            }
        }
    );

    //console.log(result);
    if (result["ResponseCode"] == 500) {
        alert("Unable to Update transaction.");
        $("#overlay").hide();
    }
}


//update payables status
function UpdateStatusAsync(e, transId, statusId) {
    $("#overlay").show();
    var result = $.post("/Payables/ApTransactions/UpdateTransStatus", {
        transId: transId,
        statusId: statusId
    }, (response) => {
        //console.log("Update Status : " + response);
        if (response == "True") {
            $("#overlay").hide();

            AddStatusLabel(transId, statusId);

        } else {
            alert("Unable to Update transaction.");
            $("#overlay").hide();
        }
    }
    );

    //console.log(result);
    if (result["ResponseCode"] == 500) {
        alert("Unable to Update transaction.");
        $("#overlay").hide();
    }
}

function GetStatusName(statusId) {
    switch (statusId) {
        case 1:
            return "<span class='label label-primary'> Requested </span>";
        case 2:
            return "<span class='label label-info'> Approved </span>";
        case 3:
            return "<span class='label label-warning'> Released </span>";
        case 4:
            return "<span class='label label-success'> Closed </span>";
        case 5:
            return "<span class='label label-success'> Returned </span>";
        case 6:
            return "<span class='label label-info'> Cancelled </span>";
        case 7:
            return "<span class='label label-default'> New </span>";
    }
}


function AddStatusLabel(transId, statusId) {
    //console.log(transId);

    $("#expense-Status-" + transId).children().remove();
    $("#expense-Status-today-" + transId).children().remove();

    $("#expense-Status-" + transId).append(GetStatusName(statusId));
    $("#expense-Status-today-" + transId).append(GetStatusName(statusId));
    return true;
}

//close payables status
function UpdateStatusClose(e, transId) {
    //$("#overlay").show();
    var result = $.post("/Payables/ApTransactions/UpdateTransStatus", {
        transId: transId,
        statusId: 4
    }, (response) => {
        //console.log("Update Status : " + response);
        if (response == "True") {
            $("#overlay").hide();
            //window.location.reload(false);
            $(e).parent().parent().parent().parent().parent().fadeOut(250);
        } else {
            alert("Unable to Update transaction.");
            $("#overlay").hide();
        }
    }
    );

    //console.log(result);
    if (result["ResponseCode"] == 500) {
        alert("Unable to Update transaction.");
        $("#overlay").hide();
    }
}


//close payables status
function UpdateStatusCloseAsync(e, transId) {
    //$("#overlay").show();
    var result = $.post("/Payables/ApTransactions/UpdateTransStatus", {
        transId: transId,
        statusId: 4
    }, (response) => {
        //console.log("Update Status : " + response);
        if (response == "True") {
            $("#overlay").hide();
            //window.location.reload(false);
            $(e).parent().parent().fadeOut(250);
        } else {
            alert("Unable to Update transaction.");
            $("#overlay").hide();
        }
    }
    );

    //console.log(result);
    if (result["ResponseCode"] == 500) {
        alert("Unable to Update transaction.");
        $("#overlay").hide();
    }
}


//---------- Due Expenses ----------------//

$("#RepeatingPayables-Modal").on("hidden.bs.modal", function () {
    //on repeating modal close
    GetDuePayables();
});

//Get Payables Due Date
function GetDuePayables() {
    //remove all rows except header
    $("#DuePayables-table").find("tr:gt(0)").remove();

    $.get("/Payables/ApTransactions/GetDuePayables", null, (response) => {
        
        if (response.length > 0) {
            //console.log(response.length + " Due Payables");
            //console.log(response);

            $("#DuePayables-Modal").modal("show");

            for (var i = 0; i < response.length; i++) {
                var duePayables = '<tr> '
                    + '<td> ' + moment(response[i]["DtInvoice"]).format("MMM DD YYYY") + '</td>'
                    + '<td> ' + response[i]["InvoiceNo"] + '</td>'
                    + '<td> ' + response[i]["Name"] + '</td>'
                    + '<td> ' + response[i]["Description"] + '</td>'
                    + '<td> ' + moment(response[i]["DtDue"]).format("MMM DD YYYY") + '</td>'
                    + '<td> ' + response[i]["Amount"] + '</td>'
                    + '<td style="color:green;"> ' + response[i]["totalPayment"] + '</td>'
                    + '<td> <span class="label label-default">' + response[i]["Status"] + '<span></td>'
                    + '<td> <a href="/Payables/ApTransactions/Details/' + response[i]["Id"] + '" target="_blank">  Details </a> </td>'
                    + ' </tr> ';

                $("#DuePayables-table").append(duePayables);
            }

        } else {
            console.log("No Due Payables");
        }
    });

    //add to session storage
    //will expire on browser closed
    sessionStorage.setItem('ApTrans-DueShowed', true);
}


//---------- Printing Expenses ----------------//
//Get checked selected payables 
//@return int[] - array of checked items in the table
function GetSelectedPayables_ForPrint() {
    var checkedArr = $("#payables-table").find("input[type=checkbox]:checked").map(function () {
        return parseInt(this.value);
    }).get();

    return checkedArr;
}

//print checked payables
function CheckSelected_Print() {
    let ForPrintIds = GetSelectedPayables_ForPrint();

    //console.log("ForPrint");
    //console.log(ForPrintIds);

    if (ForPrintIds.length > 0) {

        var res = $.post('/Payables/ApTransactions/SendPrintRequest', { transIds: ForPrintIds }, (response) => {
            if (response > 0) {
                //console.log(response);
                alert("Generating Print Request form");
                window.location.href = "/Payables/ApTransactions/PrintRequestForm/" + response;
            } else {
                alert("Unable to update payables print status.");
                //console.log(response);
            }
        });

    } else {
        console.log("Please select atleast 1 payable to print.");
    }

}

//print checked payables
function CheckSelected_PrintPO() {
    let ForPrintIds = GetSelectedPayables_ForPrint();

    //console.log("ForPrint");
    //console.log(ForPrintIds);

    if (ForPrintIds.length > 0) {

        var res = $.post('/Payables/ApTransactions/SendPrintRequest', { transIds: ForPrintIds }, (response) => {
            if (response > 0) {
                //console.log(response);
                alert("Generating Print Request form");
                window.location.href = "/Payables/ApTransactions/PrintRequestPOForm/" + response;
            } else {
                alert("Unable to update payables print status.");
                //console.log(response);
            }
        });

    } else {
        console.log("Please select atleast 1 payable to print.");
    }

}

//click on payables 
function CheckAllPayableItems() {
    $("#payables-table").find("input[type=checkbox]").map(function () {
        $('input[type="checkbox"]').prop('checked', true);
        CheckRepeatItems(this);
    });
}


//on click print button per item
function OnPrintClicked(e, transId) {

    $.post("/Payables/ApTransactions/UpdatePrintStatus", { id: transId }, (response) => {
        if (response == "True") {
            $(e).remove();
        } else {
            alert("Unable to update payables print status.")
            //console.log(response);
        }
    });
}


//---------- Release Payment ----------------//
//show release payment modal
function ShowReleaseModal(Id, Desription, Budget, Type) {
    $("#ReleasePayment-Modal").modal('show');

    //Populate Fields
    $("#ReleasePayment-Id").val(Id);
    $("#ReleasePayment-Budget").val(Budget);
    $("#ReleasePayment-Description").val(Desription);
    $('#ReleasePayment-Date').val(moment().format('MM/DD/YYYY hh:mm A'));
    $("#ReleasePayment-Amount").val(0);

    if (Type == 'PO') {
        $("#ReleasePayment-Amount").prop('disabled', true);
    } else {
        $("#ReleasePayment-Amount").prop('disabled', false);
    }
}

//release payment amount
function ReleasePayment(e) {
    var amount = $("#ReleasePayment-Amount").val();
    var id = $("#ReleasePayment-Id").val();
    var date = $("#ReleasePayment-Date").val();

    $(e).prop('disabled', true);

    $.post("/Payables/ApTransactions/ReleasePayment", { id: id, amount: amount, date: date }, (res) => {
        if (res == "OK") {
            UpdateStatus(id, 3); //update to release
            $(e).prop('disabled', false);
            $("#ReleasePayment-Modal").modal('hide');
        }
    });
}


//---------- Return Amount ----------------//
//release payment amount
function ReturnAmount(e) {
    var remarks = $("#ReturnAmount-Remarks").val();
    var amount = $("#ReturnAmount-Amount").val();
    var invoiceDate = $("#ReturnAmount-Date").val();
    var id = $("#ReturnAmount-Id").val();

    var budgetAmount = $("#ReturnAmount-BudgetAmount").val();
    var varianceCheck = CheckAmountVarianceIsPass(budgetAmount, amount);

    $(e).prop('disabled', true);

    if (varianceCheck) {
        POST_ReturnAmount(id, amount, remarks, invoiceDate);
    } else {
        if (confirm("The Returned Amount " + amount 
            + " have reached the 30% variance of Budget Amount "
            + budgetAmount + ". Do you want to return?")) {
            POST_ReturnAmount(id, amount, remarks, invoiceDate);
        }
    }
}

function POST_ReturnAmount(id, amount, remarks, invoiceDate) {

    $.post("/Payables/ApTransactions/ReturnAmount",
        { id: id, amount: amount, remarks: remarks, invoiceDate: invoiceDate },
        (res) => {
            if (res == "OK") {
                UpdateStatus(id, 5); //update to return
                //$(e).prop('disabled', false);
                $("#ReturnAmount-Modal").modal('hide');
                window.location.reload(false);
            }
    });
}

function ShowReturnAmountModal(id, Description, amount, budget, invoiceDate, remarks) {

    $("#ReturnAmount-Id").val(id);
    $("#ReturnAmount-Date").val(invoiceDate);
    $("#ReturnAmount-PreAmount").val(amount);
    $("#ReturnAmount-BudgetAmount").val(budget);
    $("#ReturnAmount-Amount").val(0);
    $("#ReturnAmount-Remarks").val(remarks);
    $("#ReturnAmount-Description").val(Description);
    $("#ReturnAmount-Modal").modal('show');

}

function CheckAmountVarianceIsPass(budgetAmount, returnedAmount) {

    var budgetAmount = budgetAmount ?? 0;
    var amountVariance = budgetAmount * 0.3;

    if (returnedAmount < (budgetAmount - amountVariance) ||
        returnedAmount > (budgetAmount + amountVariance)) {
        return false;
    }

    return true;
}


// ---------- Add Payment -------------- //
//show add payment modal
function ShowPaymentModal(Id, Description, Amount) {
    $("#Payment-Modal").modal('show');

    $("#Payment-Id").val(Id);
    $('#Payment-Date').val(moment().format('MM/DD/YYYY hh:mm A'));
    $("#Payment-Description").val(Description);
    $("#Payment-Remarks").val("");
    $("#Payment-Amount").val(0);
    $("#Payment-TmpAmount").val(Amount);
}

//submit add payment amount
function Payment(e) {
    var amount = $("#Payment-Amount").val();
    var id = $("#Payment-Id").val();
    var date = $("#Payment-Date").val();
    var remarks = $("#Payment-Remarks").val();

    $(e).prop('disabled', true);

    $.post("/Payables/ApTransactions/AddPayment", { id: id, amount: amount, date: date, remarks: remarks }, (res) => {
        if (res == "OK") {
            //reload page
            $("#overlay").hide();
            window.location.reload(false);
            $(e).prop('disabled', false);
        }
    });
}


// ----- Funded Action ---- //
function FundedTrans(e, id) {

    $.post("/Payables/ApTransactions/SetFunded", { id: id }, (res) => {

        $(e).prop('disabled', false);

    })
}