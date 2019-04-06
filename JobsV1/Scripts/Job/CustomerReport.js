﻿/**
 * CustomerReport.js
 * Filter by Date, job status, last 30 and 60 days.
 * Reload and displays the jobs in the table with 
 * the applied filter or previous filters.
 */

//GLOBALS
var STATUS = "";
var SDATE = "";
var EDATE = "";
var TOP = 30;
var SORTDATE = 1;
 
//loads previous 30 jobs on load of the page
$(document).ready(function () {
    Update();
    updateStatusCSS();

});


//Updates the date filter values
function Update() {
    var url_string = window.location.href;
    var url = new URL(url_string);

     sDateVal = url.searchParams.get("sDate") != null ? url.searchParams.get("sDate") : getToday();
     eDateVal = url.searchParams.get("eDate") != null ? url.searchParams.get("eDate") : getToday();
     TOP = url.searchParams.get("top") != null ? url.searchParams.get("top") : 30;
     STATUS = url.searchParams.get("status") != null ? url.searchParams.get("status") : "ALL";
     SORTDATE = url.searchParams.get("sortdate") != null ? url.searchParams.get("sortdate") : 1;

    //convert to format (YYYY-MM-DD)
     sDateVal = moment(sDateVal).format('YYYY-MM-DD');
     eDateVal = moment(eDateVal).format('YYYY-MM-DD');

     updateJobDateArrow();
     topUpdate(TOP);

     $('#startDate').val(sDateVal);
     $('#endDate').val(eDateVal);
}


function updateStatusCSS() {
    $('#' + STATUS + '').css("color", "Black");
    $('#' + STATUS + '').siblings().css("color", "#3366BB");
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

    STATUS = status
    updateStatusCSS();
}

//|FILTER BUTTON
//reloads the page by building the url string with 
//previously selected filters are applied
//param : status = job status
//        custId = customer Id
function detailsUpdate(custId) {
    var sDateVal = document.getElementById("startDate").value != "" ? document.getElementById("startDate").value : getToday();
    var eDateVal = document.getElementById("endDate").value != "" ? document.getElementById("endDate").value : getToday();

    //build request url string
    var requestString = "/Customers/Details/" + custId + "?";
    requestString += "top=" + TOP + "&";
    if (eDateVal != null && sDateVal != null) {
        requestString = requestString + "sDate=" + sDateVal + "&eDate=" + eDateVal;
    }
    requestString += "&status=" + STATUS;
    requestString += "&sortdate=" + SORTDATE;

    //delay 2 sec
    //wait(2000);

    //load screen
    window.location.href = requestString;

}

//get jobs from today to 30 previous days
function getLastJobs(days) {
    var eDateVal = getToday();//end date = today
    var sDateVal = new Date();

    sDateVal.setDate(sDateVal.getDate() - days);
    sDateVal = sDateVal.toLocaleString();

    //convert to format (YYYY-MM-DD)
    sDateVal = moment(sDateVal).format('YYYY-MM-DD');
    eDateVal = moment(eDateVal).format('YYYY-MM-DD');

    console.log(sDateVal + " - " + eDateVal);

    $('#startDate').val(sDateVal);
    $('#endDate').val(eDateVal);
}

function filterdate(date) {
    
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);

    return today = date.getFullYear() + "-" + (month) + "-" + (day);

}

//freeze browser, delay by milliseconds,
function wait(ms) {
    var start = new Date().getTime();
    var end = start;
    while (end < start + ms) {
        end = new Date().getTime();
    }
}


//update status value on click
//change color of the text
$('#last30').click(function () {
    $('#last30').css("color", "black");
    $('#last30').siblings().css("color", "steelblue");
});


//update status value on click
//change color of the text
$('#last60').click(function () {
    $('#last60').css("color", "black");
    $('#last60').siblings().css("color", "steelblue");
});

//update status value on click
//change color of the text
$('#take30').click(function () {
    TOP = 30;
    $('#take30').css("color", "black");
    $('#take30').siblings().css("color", "steelblue");
});

//update status value on click
//change color of the text
$('#take60').click(function () {
    TOP = 60;
    $('#take60').css("color", "black");
    $('#take60').siblings().css("color", "steelblue");
});

//update status value on click
//change color of the text
$('#take100').click(function () {
    TOP = 100;
    $('#take100').css("color", "black");
    $('#take100').siblings().css("color", "steelblue");
});

function topUpdate(top){
    TOP = top;

    $('#take' + top).css("color", "black");
    $('#take' + top).siblings().css("color", "steelblue");
}


function updateJobDateArrow(){
    
    if (SORTDATE == 1) { //descending order - top : newest - bottom : oldest
        $('#jobdate-arrowDown').hide();
        $('#jobdate-arrowUp').show();
    } else { //ascending
        $('#jobdate-arrowUp').hide();
        $('#jobdate-arrowDown').show();
    }
}

function sortbyJobDate(custId) {

    if (SORTDATE == 1) { //descending
        //change to ascending
        SORTDATE = 0;
    } else { //ascending
        //change to descending
        SORTDATE = 1;
    }
    updateJobDateArrow();
}