
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
    var unit  = url.searchParams.get("unit")  != null ? url.searchParams.get("unit")  : 0  ;

    var startDate = moment(date1).format("MM / DD / YYYY");
    var endDate = moment(date2).format("MM / DD / YYYY");

    $('#startDate').val(startDate);
    $('#endDate').val(endDate);
    $('#unitList').val(unit);
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

$("#unitList").on("change",function () {
    console.log("unitList change");
    console.log($("#unitList").find(":selected").val());

});


function checkUnitList() {

    console.log("unitList change");
    console.log($("#unitList").find(":selected").val());
}

function getToday() {

    var today = moment().format('YYYY-MM-DD');
    return today;
}

function reportUpdate() {
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();
    var unit = $("#unitList").find(":selected").val();

    var requestString = "/Reporting?";

    requestString += "reportkey=income-unit";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "&sDate=" + sDateVal + "&eDate=" + eDateVal;
    }

    if (unit != "") {
        requestString = requestString + "&unit=" +unit;
    }

    window.location.href = requestString;

}

function reportPrint() {
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();
    var unit = $("#unitList").find(":selected").val();

    var requestString = "/Reporting/UnitIncomePrint?";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }

    if (unit != "") {
        requestString = requestString + "&unit=" + unit;
    } else {
        requestString = requestString + "&unit=0" ;

    }

    window.location.href = requestString;
    //window.location.href = "/Reporting/Index/" + idVal + "&sDate=" + sDateVal + "&sDate=" + eDateVal;
}

function generateMonthList() {
    var months = moment.months();
    //console.log(months);
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

    //get list of years 5 years before
    //console.log(yearArr);

    var ddl = $("#yearlist");

    for (k = 0; k < yearArr.length; k++)
        ddl.append("<option value='" + yearArr[k] + "'>" + yearArr[k] + "</option>");
}

//set the start date and end date 1-30/31
function setCurrentMonthYear() {
    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

    var startDate = moment(firstDay).format("MM / DD / YYYY");
    var endDate = moment(lastDay).format("MM / DD / YYYY");

    $("#startDate").val(startDate);
    $("#endDate").val(endDate);

}

function setCurrentMonth() {
    var getYear = moment().format("YYYY");
    var getMonth = moment().format("M");
    console.log(getMonth);

    $("#monthlist").val(getMonth);
    $("#yearlist").val(getYear);


}

function setCurrentEndDate() {
    var getYear = moment().format("YYYY");
    var getMonth = moment().format("M");

    //set Year
    var curStartDate = moment($("#enddate").val()).format("YYYY-MM-DD"); //get stardate value
    var tempDate = moment(curStartDate).year(getYear).format("YYYY-MM-DD");

    //console.log(moment(curStartDate).month(getMonth));

    //set month
    var tempDate = moment(curStartDate).month(getMonth - 1).format("YYYY-MM-DD");

    document.getElementById("enddate").value = tempDate;
}

function setMonthYear() {

    var getYear = $("#yearlist").val();
    var getMonth = $("#monthlist").val();

    //console.log(getYear);

    //set Year
    var curStartDate = moment($("#startDate").val()).format("YYYY-MM-DD"); //get stardate value
    var tempDate = moment(curStartDate).year(getYear).format("YYYY-MM-DD");

    console.log(tempDate);
    //console.log(moment(curStartDate).month(getMonth));

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