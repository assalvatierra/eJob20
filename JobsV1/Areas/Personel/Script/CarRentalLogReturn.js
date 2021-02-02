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
            window.location.reload(false);
        } else {
            alert("An Error occured while process your request");
        }
    });
}


function ApproveRequest(e, id) {
    $.post("/Personel/crLogFuels/ApproveRequest", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            //window.location.reload(false);
            $(e).parent().parent().parent().remove();
        } else {
            alert("An Error occured while process your request");
        }
    });
}




function ApproveRelease(id) {
    $.post("/Personel/crLogFuels/ApproveRelease", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            window.location.reload(false);
        } else {
            alert("An Error occured while process your request");
        }
    });
}


function ApproveRelease(e, id) {
    $.post("/Personel/crLogFuels/ApproveRelease", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            //window.location.reload(false);
            $(e).parent().parent().parent().remove();
        } else {
            alert("An Error occured while process your request");
        }
    });
}

function InitReturnLogModal(id, desc, amount, dtReq) {
    $("#return-LogFuelId").val(id);
    $("#return-Odo").val(0);
    $("#return-Amount").val("0.00");
    $("#return-Date").val(moment().format("MM/DD/YYYY hh:mm A"));
    $("#return-Desc").text(desc);
    $("#return-ReqAmount").text(amount);
    $("#return-ReqDate").text(dtReq);
    $("#ReturnLogModal").modal("show");
}


function InitReturnLogModal(id, desc, amount, dtReq, remarks) {

    console.log("return expense " + remarks);

    $("#return-LogFuelId").val(id);
    $("#return-Odo").val(0);
    $("#return-Amount").val("0.00");
    $("#return-Date").val(moment().format("MM/DD/YYYY hh:mm A"));
    $("#return-Desc").text(desc);
    $("#return-ReqAmount").text(amount);
    $("#return-ReqDate").text(dtReq);
    $("#return-Remarks").val(remarks);

    $("#ReturnLogModal").modal("show");
}

function SubmitReturnLog(e) {

    ShowLoading();

    var data = {
        id: $("#return-LogFuelId").val(),
        date: $("#return-Date").val(),
        odo: $("#return-Odo").val(),
        amount: parseFloat($("#return-Amount").val()),
        remarks: $("#return-Remarks").val(),
        isFullTank: $("#return-isFullTank").prop('checked'),
        paymentTypeId: $("#return-PaymentType").val()
    }

    console.log(data);
    var res = $.post("/Personel/CrLogFuels/SubmitReturnLog", data, (result) => {
        console.log(result);
        if (result == 'True') {
            //window.location.reload(false);
            $("#" + $("#return-LogFuelId").val()).remove();
            $("#ReturnLogModal").modal("hide");
            HideLoading();
        } else {
            alert("An Error occured while process your request");
            HideLoading();
        }
    });
}


$("#status-request, #status-approved, #status-released, #status-returned").on('click', () => {
    ShowLoading();
});

function ShowLoading() {
    $("#overlay").show();
}

function HideLoading() {
    $("#overlay").hide();
}