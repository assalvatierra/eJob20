/**
 * CustomerReport.js
 * Filter by Date, job status, last 30 and 60 days.
 * Reload and displays the jobs in the table with 
 * the applied filter or previous filters.
 */

$(document).load(function () {
    Update();
    getLastJobs(30);
});

//Updates the date filter values
function Update() {
    var url_string = window.location.href;
    var url = new URL(url_string);
    var date1 = url.searchParams.get("sDate") != null ? url.searchParams.get("sDate") : getToday();
    var date2 = url.searchParams.get("eDate") != null ? url.searchParams.get("eDate") : getToday();
    var top = url.searchParams.get("top");
    var status = url.searchParams.get("status");

    $('#startDate').val(date1);
    $('#endDate').val(date2);
}

//get the date today
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

//reloads the page by building the url string with 
//previously selected filters are applied
//param : status = job status
//        custId = customer Id
function statusUpdate(status, custId) {

    var url_string = window.location.href;
    var url = new URL(url_string);
    var top = url.searchParams.get("top");

    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();
    var topVal = top;

    var requestString = "/CustEntMains/Details/" + custId + "?";

    requestString += "top=" + topVal + "&";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }

    requestString += "&status=" + status

    //delay 2 sec
    wait(2000);

    window.location.href = requestString;

}


//reloads the page by building the url string with 
//previously selected filters are applied
//param : status = job status
//        custId = customer Id
function detailsUpdate(top, custId) {
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();
    var topVal = top;


    var requestString = "/CustEntMains/Details/" + custId + "?";

    requestString += "top=" + topVal + "&";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }
    requestString += "&status=ALL"

    //delay 2 sec
    wait(2000);
    window.location.href = requestString;

}

//get jobs from today to 30 previous days
function getLastJobs(days, custId) {
    var eDateVal = getToday();//end date = today
    var sDateVal = new Date();

    sDateVal.setDate(sDateVal.getDate() - days);

    // console.log(eDateVal);

    sDateVal = sDateVal.toLocaleString();

    console.log(sDateVal);

    var topVal = 10; //10 items

    var requestString = "/CustEntMains/Details/" + custId + "?";

    requestString += "top=" + topVal + "&";

    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }
    requestString += "&status=" + status
    
    //delay 2 sec
    wait(2000);

    window.location.href = requestString;

}

//freeze browser, delay by milliseconds,
function wait(ms) {
    var start = new Date().getTime();
    var end = start;
    while (end < start + ms) {
        end = new Date().getTime();
    }
}
