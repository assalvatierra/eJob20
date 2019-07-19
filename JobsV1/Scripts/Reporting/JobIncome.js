
$(document).ready(function () {

    //initialize
    Update();
    generateMonthList();
    generateYearList();

    $("#unitblock").css("display", "none");
    $("#reportBy").val("SUMMARY");
});

function Update() {
    var url_string = window.location.href;
    var url = new URL(url_string);

    //url param
    var date1 = url.searchParams.get("sDate") != null ? url.searchParams.get("sDate") : getToday();
    var date2 = url.searchParams.get("eDate") != null ? url.searchParams.get("eDate") : getToday();

    $('#startDate').val(moment(date1).format("MM / DD / YYYY"));
    $('#endDate').val(moment(date2).format("MM / DD / YYYY"));
}

function setPrev30() {
    var today = new Date();
    var pastDate = new Date();

    pastDate = moment().add(-30, 'days').format('YYYY-MM-DD');
    today = moment().format('YYYY-MM-DD');

    $("#startDate").val(pastDate);
    $("#endDate").val(today);

}

function setPrev60() {
    var today = new Date();
    var pastDate = new Date();

    pastDate = moment().add(-60, 'days').format('YYYY-MM-DD');
    today = moment().format('YYYY-MM-DD');

    $("#startDate").val(pastDate);
    $("#endDate").val(today);
}


function getToday() {

    var today = moment().format('YYYY-MM-DD');
    return today;
}

function reportUpdate() {
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();

    sDateVal = moment(sDateVal).format("YYYY-MM-DD");
    eDateVal = moment(eDateVal).format("YYYY-MM-DD");

    var requestString = "/Reporting?";

    requestString += "reportkey=income&";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }

    window.location.href = requestString;
}

function reportPrint() {
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();

    sDateVal = moment(sDateVal).format("YYYY-MM-DD");
    eDateVal = moment(eDateVal).format("YYYY-MM-DD");

    var requestString = "/Reporting/JobIncomePrint?";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }

    window.location.href = requestString;
}

function generateMonthList() {
    var months = moment.months();

    var ddl = $("#monthlist");
    for (k = 0; k < months.length; k++)
        ddl.append("<option value='" + moment().month(months[k]).format("M") + "'>" + months[k] + "</option>");
}

function generateYearList() {
    var year = moment().year();
    var yearArr = [];

    //add prev years to array
    for (var i = 0; i < 5; i++) {

        var yearTemp = year - i;

        yearArr.push(yearTemp);
    }

    var ddl = $("#yearlist");

    for (k = 0; k < yearArr.length; k++)
        ddl.append("<option value='" + yearArr[k] + "'>" + yearArr[k] + "</option>");
}

function setCurrentMonthYear() {
    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

    var startDate = moment(firstDay).format("MM / DD / YYYY");
    var endDate = moment(lastDay).format("MM / DD / YYYY");

    $("#startDate").val(startDate);
    $("#endDate").val(endDate);
}


function setCurrentEndDate() {
    var getYear = moment().format("YYYY");
    var getMonth = moment().format("M");

    //set Year
    var curStartDate = moment($("#enddate").val()).format("YYYY-MM-DD"); //get stardate value
    var tempDate = moment(curStartDate).year(getYear).format("YYYY-MM-DD");

    //set month
    var tempDate = moment(curStartDate).month(getMonth - 1).format("MM / DD / YYYY");

    document.getElementById("enddate").value = tempDate;
}

function setMonthYear() {

    var getYear = $("#yearlist").val();
    var getMonth = $("#monthlist").val();

    //set Year
    var curStartDate = moment($("#startDate").val()).format("YYYY-MM-DD"); //get stardate value
    var tempDate = moment(curStartDate).year(getYear).format("YYYY-MM-DD");

    console.log(tempDate);

    //set month
    var tempDate = moment(tempDate).month(getMonth - 1).format("YYYY-MM-DD");
    console.log(tempDate);

    document.getElementById("startDate").value = tempDate;
    document.getElementById("endDate").value = tempDate;
}

$("#reportBy").on('change', function () {

    console.log("reportBy:" + $("#reportBy").val());

    if ($("#reportBy").val() == "UNIT") {
        console.log("UNIT");
        $("#unitblock").css("display", "block");
        $("#unitblock").css("display", "block");
    } else {
        console.log("SUMMARY");
        $("#unitblock").css("display", "none");
        $("#unitblock").css("display", "none");
    }
});