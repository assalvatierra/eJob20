
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

        //Repeating Payables
        if (sessionStorage.getItem('ApTrans-RepeatingShowed') == null) {
            GetRepeatingPayablesCount();

        }

        //Due Payables
        if ($('#RepeatingPayables-Modal').is(':visible')) {
            //GetDuePayables();
        } else {
            if (!sessionStorage.getItem('ApTrans-DueShowed')) {
               // GetDuePayables();
            }
        }

        if (sessionStorage.getItem('ApTrans-DueShowed') == null) {
           // GetDuePayables();
        }

    }
}

function GetRepeatingPayablesCount() {
    $.get("/Payables/ApTransactions/GetRepeatingPayablesCount", null, (response) => {
        //console.log(response);

        $("#RepeatingPayables-loading").show();

        if (response > 0) {
            //if system have repeating payables
            GetRepeatingPayables();


            //add to session storage
            //will expire on browser closed
            sessionStorage.setItem('ApTrans-RepeatingShowed', true);
        }

    });
}

//get and check repeating payables
//show modal 
function GetRepeatingPayables() {
    $("#RepeatingPayables-Modal").modal("show");

    $.get("/Payables/ApTransactions/GetRepeatingPayables", null, (response) => {

        $("#RepeatingPayables-loading").hide();

        //remove all rows except header
        $("#RepeatingPayables-table").find("tr:gt(0)").remove();

        var repeatingPayables = response;

        for (var i = 0; i < repeatingPayables.length; i++) {
            let interval = parseInt(repeatingPayables[i]['Interval']);
            let payableId = parseInt(repeatingPayables[i]['Id']);

            let payablesRow = '<tr>'
                + '<td> <input type="checkbox" onclick="CheckRepeatItems(this)" name="check-repeat" value="' + payableId + '"/> </td>'
                + '<td><b>' + repeatingPayables[i]['Name'] + '</b></td>'
                + '<td>' + repeatingPayables[i]['Description'] + '</td>'
                + '<td>' + repeatingPayables[i]['Amount'] + '</td>'
                + '<td>' + interval + ' Days </td>'
                + '<td>' + moment(repeatingPayables[i]['DtDue']).format('MMM DD YYYY') + ' </td>'
                + '<td> &#8594; </td>'
                + '<td>' + moment(repeatingPayables[i]['DtDue']).add(interval, 'days').format('MMM DD YYYY') + '</td>'
                + '<td> <a target="_blank" href="/Payables/ApTransactions/Details/' + payableId + '" onclick="ShowPayableDetails(' + payableId + ')"> Details </a> | '
                + '<a href="#" onclick="CancelRepeat(this,' + payableId + ')"> Cancel Repeat</a>'
                + '</td>'
                + '</tr>';

            $("#RepeatingPayables-table").append(payablesRow);
        }
    });
}

//check all selected repeating payables
function CheckAllRepeatItems() {
    $("#RepeatingPayables-table").find("input[type=checkbox]").map(function () {
        $('input[type="checkbox"]').prop('checked', true);
        CheckRepeatItems(this);
    });
}

//get selected repeating items on modal payables
function GetAllSelectedItems() {
    var checkedArr = $("#RepeatingPayables-table").find("input[type=checkbox]:checked").map(function () {
        return this.value;
    }).get();

    return checkedArr;
}

//select repeating 
function CheckRepeatItems(e) {
    if ($(e).prop('checked')) {
        $(e).parent().parent().addClass('active')
    } else {
        $(e).parent().parent().removeClass('active')
    }
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

//update payables status
function UpdateStatus(transId, statusId) {
    $("#overlay").show();
    var result = $.post("/Payables/ApTransactions/UpdateTransStatus", {
        transId: transId,
        statusId: statusId
        }, (response) => {
            console.log("Update Status : " + response);
            if (response == "True") {
                $("#overlay").hide();
                window.location.reload(false);
            } else {
                alert("Unable to Update transaction.");
                $("#overlay").hide();
            }
        }
    );

    console.log(result);
    if (result["ResponseCode"] == 500) {
        alert("Unable to Update transaction.");
        $("#overlay").hide();
    }
}

//highlight repeating payables
function RepeatCheck(e) {
    if ($(e).hasClass('active')) {
        $(e).removeClass('active');
    } else {
        $(e).addClass('active');
    }
}

//copy selected repeating 
function RepeatSelected(e) {

    $(e).text("Copying");
    $(e).prop('disabled', true);

    var selectedIds = GetAllSelectedItems();

    console.log(selectedIds.length);

    if (selectedIds.length > 0) {

        $.post('/Payables/ApTransactions/RepeatSelectedPayables', { payableIds: selectedIds }, (response) => {
            console.log(response);

            if (response == "True") {
                $(e).text("Copying Done");
                $(e).prop('disabled', false);

                alert("Copying Repeating Payables Done.  " + selectedIds.length + " items copied.");

                window.location.reload(true);
            } else {
                alert("Unable to copy selected items.");
            }

        });

    } else {
        alert("Please select at least 1 item.");
        $(e).prop('disabled', false);
        $(e).text("Repeat Selected");
    }
}

//cancel repeated payable on modal popup
function CancelRepeat(e, id) {

    $.post('/Payables/ApTransactions/CancelRepeatingTrans', { transId: id }, (response) => {
        if (response) {
            $(e).parent().parent().remove();
        } else {
            alert("Unable to update payables transaction.")
        }
    });
}



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

    console.log("ForPrint");
    console.log(ForPrintIds);

    if (ForPrintIds.length > 0) {

        var res = $.post('/Payables/ApTransactions/SendPrintRequest', { transIds: ForPrintIds }, (response) => {
            if (response > 0) {
                console.log(response);
                alert("Generating Print Request form");
                window.location.href = "/Payables/ApTransactions/PrintRequestForm/" + response;
            } else {
                alert("Unable to update payables print status.");
                console.log(response);
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
            console.log(response);
        }
    });
}



//show release payment modal
function ShowReleaseModal(Id) {
    $("#ReleasePayment-Modal").modal('show');
    $("#ReleasePayment-Id").val(Id);
    $('#ReleasePayment-Date').val(moment().format('MM/DD/YYYY hh:mm A'));
    $("#ReleasePayment-Amount").val(0);
}

//release payment amount
function ReleasePayment() {
    var amount = $("#ReleasePayment-Amount").val();
    var id = $("#ReleasePayment-Id").val();
    var date = $("#ReleasePayment-Date").val();

    $.post("/Payables/ApTransactions/ReleasePayment", { id: id, amount: amount }, (res) => {
        if (res == "OK") {
            UpdateStatus(id, 3); //update to release
        }
    });
}


//Return Amount

//release payment amount
function ReturnAmount() {
    var amount = $("#ReturnAmount-Amount").val();
    var id = $("#ReturnAmount-Id").val();

    $.post("/Payables/ApTransactions/ReturnAmount", { id: id, amount: amount }, (res) => {
        if (res == "OK") {
            UpdateStatus(id, 5); //update to return
        }
    });
}

function ShowReturnAmountModal(id) {
    $("#ReturnAmount-Id").val(id);
    $("#ReturnAmount-Modal").modal('show');
}