/*
 * Cashier Job Listing 
 * Filters and Search
 * 
 */ 


function Init_JobStatusFilter(jobStatus) {
    $("#jobStatus").children("button").removeClass("active");
    switch (jobStatus) {
        case '1':
            $("#JobStatus-All").addClass("active");
            break;
        case '2':
            $("#Reservation").addClass("active");
            break;
        case '3':
            $("#Confirmed").addClass("active");
            break;
        case '4':
        default:
            $("#Closed").addClass("active");
            break;
    }
}


function Init_PaymentStatusFilter(paymentStatus) {
    $("#paymentStatus").children("button").removeClass("active");
    switch (paymentStatus) {
        case '1':
            $("#Paid").addClass("active");
            break;
        case '3':
            $("#PaymentStatus-All").addClass("active");
            break;
        case '2':
        default:
            $("#UnPaid").addClass("active");
            break;
    }
}

//Job Status Filter
function filterJobStatus(statusID) {
    jobStatusID = statusID;
    filterPayments();
}

//Job Payment Status Filter
function filterPaymentStatus(statusID) {
    paymentStatusID = statusID;
    filterPayments();
}

//submit search button
function SubmitSearch() {
    var srchVal = $("#srch-input").val();
    srchString = srchVal;
    filterPayments();
}


//Submit All filters and reload page
function filterPayments() {
    //console.log(jobStatusID);
    window.location.href = "/Cashier?srch=" + srchString + "&paymentStatus=" + paymentStatusID + "&jobStatus=" + jobStatusID;
}
