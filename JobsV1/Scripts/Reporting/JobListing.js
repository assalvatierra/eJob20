
function Update() {
    var url_string = window.location.href;
    var url = new URL(url_string);

    var date1 = url.searchParams.get("sDate") != null ? url.searchParams.get("sDate") : getToday();
    var date2 = url.searchParams.get("eDate") != null ? url.searchParams.get("eDate") : getToday();
    var company = url.searchParams.get("company");
    var unitDriver = url.searchParams.get("unitDriver");

    $('#startDate').val(moment(date1).format('MM / DD / YYYY'));
    $('#endDate').val(moment(date2).format('MM / DD / YYYY'));
    $('#company-input').val(company);
    $('#unitDriverInput').val(unitDriver);
}

$(function () {
    Update();
});

Update();

function getToday() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0! 
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = yyyy + '-' + mm + '-' + dd;
    return today;
}

function reportUpdate() {
    var CompanyVal = document.getElementById("company-input").value;
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();
    var unitDriverVal = document.getElementById("unitDriverInput").value;

    sDateVal = moment(sDateVal).format("YYYY-MM-DD");
    eDateVal = moment(eDateVal).format("YYYY-MM-DD");

    var requestString = "/Reporting?";

    requestString += "reportkey=job&";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }

    if (CompanyVal == "") {
        CompanyVal = "all";
    }
    requestString += "&company=" + CompanyVal;

    if (unitDriverVal == "") {
        unitDriverVal = "all";
    }
    requestString += "&unitDriver=" + unitDriverVal;

    window.location.href = requestString;
}

function reportPrint() {
    var CompanyVal = document.getElementById("company-input").value;
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();
    var unitDriverVal = document.getElementById("unitDriverInput").value;

    var requestString = "/Reporting/JobListingPrint?";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }

    if (CompanyVal == "") {
        CompanyVal = "all";
    }
    requestString += "&company=" + CompanyVal;

    if (unitDriverVal == "") {
        unitDriverVal = "all";
    }
    requestString += "&unitDriver=" + unitDriverVal;

    window.location.href = requestString;
}
