
function UpdateStatus(e, transId, statusId) {
    $("#overlay").show();
    var result = $.post("/Receivables/ArMgt/UpdateTransStatus", {
        transId: transId,
        statusId: statusId
    }, (response) => {
        console.log("Update Status : " + response);
        if (response == "True") {
            $("#overlay").hide();
            //window.location.reload(false);
            $(e).parent().parent().hide(150);
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



//Get checked selected transactions 
//@return int[] - array of checked items in the table
function GetSelectedPayables_ForPrint() {
    var checkedArr = $("#settlement-table").find("input[type=checkbox]:checked").map(function () {
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

        $.post('/Receivables/ArMgt/SaveSettlementPrintIds', { transIds: ForPrintIds }, (response) => {

        }).done((response) => {
            if (response == "OK") {
                console.log(response);
                //alert("Generating Print Request form");
                //window.location.href = "/Receivables/ArMgt/SettlementPrint";
                window.open('/Receivables/ArMgt/SettlementPrint', '_blank');
            } else {
                alert("Done: Unable to update settlement print status.");
                console.log(response);
            }
        }).fail(() => {
            alert("Done: Unable to update settlement print status.");
        });

    } else {
        alert("Please select atleast 1 settlement to print.");
    }

}

//click on payables 
function CheckAllPayableItems() {
    $("#settlement-table").find("input[type=checkbox]").map(function () {
        $('input[type="checkbox"]').prop('checked', true);
        // CheckRepeatItems(this);
    });
}


function UpdateStatusAndDeposit(id) {
    var result = $.post("/Receivables/ArMgt/UpdatePaymentAsDeposited",
        {
            transId: id
        },
        (response) => {
            console.log("Update Status : " + response);
            if (response == "True") {
                window.location.reload(false);
            } else {
                alert("Unable to Update Deposit.");
            }
        }
    );
}


function DepositAndClosedChecked() {

    let chckedIds = GetSelectedPayables_ForPrint();

    if (chckedIds.length == 0) {
        alert("Please select at least one item");
    }

    var result = $.post("/Receivables/ArMgt/UpdateTranPaymentsAsDeposited",
        {
            transIds: chckedIds
        },
        (response) => {
            console.log("Update Status : " + response);
            if (response == "True") {
                window.location.reload(false);
            } else {
                alert("Unable to Update Deposit.");
            }
        }
    );
}


function ClosedChecked() {

    let chckedIds = GetSelectedPayables_ForPrint();

    if (chckedIds.length == 0) {
        alert("Please select at least one item");
    }

    var result = $.post("/Receivables/ArTransactions/CloseTransctions",
        {
            TransIds: chckedIds
        },
        (response) => {
            console.log("Update Status : " + response);
            if (response == "True") {
                window.location.reload(false);
            } else {
                alert("Unable to Close Selected Items.");
            }
        }
    );
}


