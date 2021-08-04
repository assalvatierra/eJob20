

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="dateStart"]').val();

    $('input[name="dateStart"]').daterangepicker(
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

    //Date 2
    var ddd2 = $('input[name="dateEnd"]').val();

    $('input[name="dateEnd"]').daterangepicker(
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

    console.log(ddd2.trim())

    if (ddd2.trim() == "" || ddd2.trim() == '01/01/0001 12:00 AM' || ddd2.trim() == '01/01/2001') {
        //$('input[name="DtInvoice"]').val(moment().format('MM/DD/YYYY'));
    } else {
        //$('input[name="DtInvoice"]').val(moment(ddd1).format('MM/DD/YYYY'));
    }
}

//Subtract 1 day from date filter
function PrevDayFilter() {
    var ddd1 = $('input[name="dateStart"]').val();
    $('input[name="dateStart"]').val(moment(ddd1).add(-1, "days").format('MM/DD/YYYY'));
    SubmitDateFilter();
}

//Add 1 day from date filter
function NextDayFilter() {
    var ddd1 = $('input[name="dateStart"]').val();
    $('input[name="dateStart"]').val(moment(ddd1).add(1, "days").format('MM/DD/YYYY'));
    SubmitDateFilter();
}

//Submit date filter
function SubmitDateFilter() {
    var dateStart = $('input[name="dateStart"]').val();
    var dateEnd = $('input[name="dateEnd"]').val();
    window.location.href = "/Payables/ApTransactions/ReleasedWeekly?dateStart=" + dateStart + "&dateEnd=" + dateEnd;
}

function FilterDateLast(days) {
    $('input[name="dateStart"]').val(moment().add(-days, "days").format('MM/DD/YYYY'));
    $('input[name="dateEnd"]').val(moment().format('MM/DD/YYYY'));
    SubmitDateFilter();

}

function FilterCurrentMonth() {
     $('input[name="dateStart"]').val(moment().startOf('month').format('MM/DD/YYYY'));
    $('input[name="dateEnd"]').val(moment().endOf('month').format('MM/DD/YYYY'));
    SubmitDateFilter();

}


