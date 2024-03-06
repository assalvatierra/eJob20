﻿/*********************
 * Activities Script *
 *********************/

$(document).ready(new function () {
    var sDate ;
    var eDate;
    var name;
    let searchParams = new URLSearchParams(window.location.search)

    if (searchParams.has('sDate')) {

        //get dates from url parameter
        sDate = getUrlParameter('sDate');
        eDate = getUrlParameter('eDate');
        name = getUrlParameter('name');
        
    } else {

        //adjust date to today and one month before 
        eDate = moment().format('MM/DD/YYYY');
        sDate = moment(eDate).add(-1, 'month').format('MM/DD/YYYY');

    }
    $("#DtStart").val(sDate);
    $("#DtEnd").val(eDate);
    $("#Name").val(name);
});


function Filter() {
   console.log("test");
   var startDate = $("#DtStart").val();
    var endDate = $("#DtEnd").val();
    var name = $("#Name").val();

    window.location.href = "/Activities?sDate=" + startDate + "&eDate=" + endDate + "&name=" + name;
}

//get url parameter
function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};
