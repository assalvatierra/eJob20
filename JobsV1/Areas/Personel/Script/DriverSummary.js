
/*
 *  Drivers Summary 
 */ 

$("select[name=crLogDriverId]").change(() => {
    let selectedDriver = $("select[name=crLogDriverId] :selected").val();

    window.location.href = "/Personel/crLogDrivers/DriverSummary/" + selectedDriver;
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
        Remarks: "",
        CalculateOT: $("#select-ot-checkbox").is(":checked")
    }

    var isOTChecked = $("#select-ot-checkbox").is(":checked");

    console.log("isOTChecked: " + isOTChecked);

    //run through each row
    var TotalSalary = 0;
    var TotalSelected = 0;

    $('#summary-table tr[name="summary-data"]').each(function (i, row) {

        // reference all the stuff you need first
        var $row = $(row),
            $driverfee = $row.find('td[name*="driversFee"]').text().trim(),
            $driverOT = $row.find('td[name*="driversOT"]').text().trim(),
            $tripId = $row.find('td[name*="id"]').text().trim(),
            $checkedBoxStatus = $row.find('input:checked').is(":checked");

        if ($checkedBoxStatus) {
            if (parseFloat($driverfee) > 0 ) {

                TotalSalary = TotalSalary + parseFloat($driverfee);

                if (isOTChecked) {
                    if (parseFloat($driverOT) > 0) {
                        TotalSalary = TotalSalary + parseFloat($driverOT);
                    }
                }
                jsonReqData.TripIds.push($tripId);
            }
            TotalSelected++;
        }
    });

    $("#Total-Selected-DriverFee").text(TotalSalary);
    $("#Total-Selected-Count").text(TotalSelected);

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
    
    $.post("/Personel/CarRentalCashRelease/CreateDriverRelease", logReqData, (result) => {
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

    $.post("/Personel/CarRentalCashRelease/CreateDriverPayment", data, (result) => {
        if (result == 'True') {
            window.location.href = "/Personel/crLogDrivers/DriverSummary?id=" + data.DriverId ;
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

    $.post("/Personel/CarRentalCashRelease/CreateDriverCA", data, (result) => {
        //console.log(result);
        if (result == 'True') {
            window.location.href = "/Personel/crLogDrivers/DriverSummary?id=" + data.DriverId + "&reqStatus=2";
        } else {
            alert("An error occured while processing your request");
        }
    })

}


function Initialize(reqStatus) {
    CalculateTotalSalary();

    if (reqStatus == '1') {
        $("#request-success-message").show();
    }

    if (reqStatus == '2') {
        $("#ca-success-message").show();
    }


    if (reqStatus == '4') {
        $("#closeTRx-success-message").show();
    }

    if (reqStatus == '5') {
        $("#closeTRx-error-message").show();
    }
}


function Initialize_Date(dtStart, dtEnd, driver) {

    if (dtStart == '') {
        $('#DtStart').val(moment().format("MM/DD/YYYY"));
    } else {
        $('#DtStart').val(moment(dtStart).format("MM/DD/YYYY"));
    }
    if (dtEnd == '') {
        $('#DtEnd').val(moment().format("MM/DD/YYYY"));
    } else {
        $('#DtEnd').val(moment(dtEnd).format("MM/DD/YYYY"));
    }

    if (dtStart != '' && dtEnd != '') {
        $("#show-date-text").show();
    }

    var driverId = parseInt(driver);

    if (driverId > 0) {
        $("#driverId").val(driverId);
    } else {
        $("#driverId").val(0);
    }
}

function DriverSummaryFilter_AddDays(days) {

    $("#DtStart").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


function SelectAllOTCheckBox() {
    CalculateTotalSalary();
}