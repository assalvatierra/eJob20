
/*
 *  ApTransaction/ReleasedDaily
 *  Initialize date time picker on input fields
 *  Handle Date filter input 
 */



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
            // alert(start.format('YYYY-MM-DD'));
        }
    );

    $('input[name="dateSrch"]').val(moment(ddd1).format('MM/DD/YYYY'));

    if (ddd1 === '' || ddd1 === '1/1/0001 12:00:00 am') {
        //$('input[name="dateSrch"]').val(moment().format('MM/DD/YYYY'));
    }
}

function PrevDayFilter() {
    var ddd1 = $('input[name="dateSrch"]').val();
    $('input[name="dateSrch"]').val(moment(ddd1).add(-1, 'days').format('MM/DD/YYYY'));

    $("#SubmitSearchBtn").submit();
    SubmitDateFilter();
}

function NextDayFilter() {
    var ddd1 = $('input[name="dateSrch"]').val();
    $('input[name="dateSrch"]').val(moment(ddd1).add(1, 'days').format('MM/DD/YYYY'));

    $("#SubmitSearchBtn").submit();
    SubmitDateFilter();
}

function SubmitDateFilter() {
    var date = $('input[name="dateSrch"]').val();

    window.location.href = "/Payables/ApTransactions/ReleasedDaily?dateSrch=" + date;
}


