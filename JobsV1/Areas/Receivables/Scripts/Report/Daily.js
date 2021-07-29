

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="dateSrch"]').val();

    $('input[name="dateSrch"]').daterangepicker(
        {
            timePicker: false,
            timePickerIncrement: 1,
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MM/DD/YYYY'
            }
        },
        function (start, end, label) {
            // alert(start.format('YYYY-MM-DD h:mm A'));
        }
    );

    console.log(ddd1.trim())

    if (ddd1.trim() == "" || ddd1.trim() == '01/01/0001 12:00 AM' || ddd1.trim() == '01/01/2001') {
        //$('input[name="DtInvoice"]').val(moment().format('MM/DD/YYYY'));
    } else {
        //$('input[name="DtInvoice"]').val(moment(ddd1).format('MM/DD/YYYY'));
    }
}

//Subtract 1 day from date filter
function PrevDayFilter() {
    var ddd1 = $('input[name="dateSrch"]').val();
    $('input[name="dateSrch"]').val(moment(ddd1).add(-1, "days").format('MM/DD/YYYY'));
    SubmitDateFilter();
}

//Add 1 day from date filter
function NextDayFilter() {
    var ddd1 = $('input[name="dateSrch"]').val();
    $('input[name="dateSrch"]').val(moment(ddd1).add(1, "days").format('MM/DD/YYYY'));
    SubmitDateFilter();
}

//Submit date filter
function SubmitDateFilter() {
    var date = $('input[name="dateSrch"]').val();
    window.location.href = "/Receivables/ArReports/Daily?dateSrch=" + date;
}


