//import { transcode } from "buffer";


$(document).ready(function () {
    InitDatePicker();
})

function InitDatePicker()
{
    $('#return-Date').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY hh:mm A'
        }
    },
    function (start, end, label) {
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
        }
    );

}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}


function FilterStatus(statusId) {
    switch (statusId) {
        case '1':
            $("#status-request").css('color', 'black');
            break;
        case '2':
            $("#status-approved").css('color', 'black');
            break;
        case '3':
            $("#status-released").css('color', 'black');
            break;
        case '4':
            $("#status-returned").css('color', 'black');
            break;
        default:
            $("#status-request").css('color', 'black');
            break;
    }
}


function ApproveRequest(id) {
    $.post("/Personel/crLogFuels/ApproveRequest", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            window.location.reload();
        } else {
            alert("An Error occured while process your request");
        }
    });
}



function ApproveRelease(id) {
    $.post("/Personel/crLogFuels/ApproveRelease", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            window.location.reload();
        } else {
            alert("An Error occured while process your request");
        }
    });
}

function InitReturnLogModal(id) {
    $("#return-LogFuelId").val(id);
    $("#return-Odo").val(0);
    $("#return-Amount").val("0.00");
    $("#return-Date").val(moment().format("MM/DD/YYYY hh:mm A"));
    $("#ReturnLogModal").modal("show");
}

function SubmitReturnLog() {

    var isFullTank = false;
    if ($("#return-isFullTank").val() == "on") {
        isFullTank = true;
    }

    var data = {
        id: $("#return-LogFuelId").val(),
        date: $("#return-Date").val(),
        odo: $("#return-Odo").val(),
        amount: parseFloat($("#return-Amount").val()),
        remarks: $("#return-Remarks").val(),
        isFullTank: isFullTank,
        paymentTypeId: $("#return-PaymentType").val()
    }

    console.log(data);
    var res = $.post("/Personel/CrLogFuels/SubmitReturnLog", data, (result) => {
        console.log(result);
        if (result == 'True') {
            window.location.reload();
        } else {
            alert("An Error occured while process your request");
        }
    });

}
