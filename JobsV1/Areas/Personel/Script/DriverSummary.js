
/*
 *  Drivers Summary 
 */ 

$("select[name=crLogDriverId]").change(() => {
    let selectedDriver = $("select[name=crLogDriverId] :selected").val();

    window.location.href = "../DriverSummary/" + selectedDriver;
})

function SelectAllCheckBox() {
    if ($("#select-all-checkbox").is(":checked")) {
        $(".item-checkbox").prop("checked", true);
        CalculateTotalSalary();
    }
}

function OnSelectCheckBox(e) {
    //re-calculate salary
    CalculateTotalSalary();
}

function CalculateTotalSalary() {
    var driverId = parseInt($("#DriversId").text().trim());
    var jsonReqData = {
        Amount: 0,
        DriverId: driverId,
        TripIds: [],
        Remarks: ""
    }

    //run through each row
    var TotalSalary = 0;
    $('#summary-table tr[name="summary-data"]').each(function (i, row) {

        // reference all the stuff you need first
        var $row = $(row),
            $driverfee = $row.find('td[name*="driversFee"]').text().trim(),
            $tripId = $row.find('td[name*="id"]').text().trim(),
            $checkedBoxStatus = $row.find('input:checked').is(":checked");

        console.log($tripId + " - " + $driverfee + " - " + $checkedBoxStatus);

        if ($checkedBoxStatus) {
            TotalSalary = TotalSalary + parseInt($driverfee);
            jsonReqData.TripIds.push($tripId);
        }
    });

    $("#Total-Selected-DriverFee").text(TotalSalary);

    //update json object of amount
    jsonReqData.Amount = TotalSalary;

    return jsonReqData;
}

function CreateSalaryRequest() {
    var logReqData = CalculateTotalSalary();
    if (logReqData.TripIds.length != 0) {

        $("#ReqForm-Date").val(moment().format("MM/DD/YYYY hh:mm A"));
        $("#ReqForm-Amount").val(logReqData.Amount);

        //show modal
        $("#DriverSalaryReqModal").modal("show");
    } else {
        alert("No Trips selected. Please Select check at least 1 trip.");
    }
}

function SubmitSalaryRequestForm() {
    var logReqData = CalculateTotalSalary();
    logReqData.Remarks = $("#ReqForm-Remarks").val();
    console.log(logReqData);

    $.post("/Personel/CarRentalCashRelease/CreateDriverRelease", logReqData, (result) => {
        console.log(result);
        if (result == 'True') {
            window.location.href = "../DriverSummary?id=" + logReqData.DriverId + "&reqStatus=1";
        } else {
            alert("An error occured while processing your request");
        }
    })

}

function CreatePaymentRequest() {
        $("#Payment-Amount").val(0);

        //show modal
        $("#DriverPaymentModal").modal("show");
}


function SubmitPaymentForm(driverId) {
    var data = {
        DriverId: driverId,
        Amount: parseFloat($("#Payment-Amount").val()),
        Remarks: $("#Payment-Remarks").val()
    }
    //logReqData.Remarks = $("#ReqForm-Remarks").val();
    console.log(data);

    $.post("/Personel/CarRentalCashRelease/CreateDriverPayment", data, (result) => {
        console.log(result);
        if (result == 'True') {
            window.location.href = "/Personel/CarRentalLog/DriverSummary?id=" + data.DriverId ;
        } else {
            alert("An error occured while processing your request");
        }
    })

}


function CreateCARequest() {
    $("#CA-Amount").val(0);

    //show modal
    $("#DriverCAModal").modal("show");
}


function SubmitCAForm(driverId) {
    var data = {
        DriverId: driverId,
        Amount: parseFloat($("#CA-Amount").val()),
        Remarks: $("#CA-Remarks").val()
    }
    //logReqData.Remarks = $("#ReqForm-Remarks").val();
    console.log(data);

    $.post("/Personel/CarRentalCashRelease/CreateDriverCA", data, (result) => {
        console.log(result);
        if (result == 'True') {
            window.location.href = "/Personel/CarRentalLog/DriverSummary?id=" + data.DriverId + "&reqStatus=2";
        } else {
            alert("An error occured while processing your request");
        }
    })

}