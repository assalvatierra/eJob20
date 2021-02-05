
/*
 *  ApTransaction Create/Edit
 *  Initialize date time picker on input fields
 */



$(document).ready(function () {
    InitDatePicker();

})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="DtInvoice"]').val();

    $('input[name="DtInvoice"]').daterangepicker(
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

    $('input[name="DtInvoice"]').val(ddd1);

    if (ddd1 === '' || ddd1 === '1/1/0001 12:00:00 AM' || ddd1 === '1/1/0001 12:00:00 am') {
        $('input[name="DtInvoice"]').val(moment().format('MM/DD/YYYY'));
    }


    //Date 2
    var ddd2 = $('input[name="DtEncoded"]').val();

    $('input[name="DtEncoded"]').daterangepicker(
        {
            timePicker: true,
            timePickerIncrement: 1,
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MM/DD/YYYY h:mm A'
            }
        },
        function (start, end, label) {
            alert(start.format('YYYY-MM-DD h:mm A'));
        }
    );

    $('input[name="DtEncoded"]').val(ddd2);

    if (ddd2 === '' || ddd2 === '1/1/0001 12:00:00 AM' || ddd2 === '1/1/0001 12:00:00 am') {
        $('input[name="DtEncoded"]').val(moment().format('MM/DD/YYYY h:mm A'));
    }

    //Date 3
    var ddd3 = $('input[name="DtDue"]').val();

    $('input[name="DtDue"]').daterangepicker(
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

    $('input[name="DtDue"]').val(ddd3);

    if (ddd3 === '' || ddd3 === '1/1/0001 12:00:00 AM' || ddd3 === '1/1/0001 12:00:00 am') {
        $('input[name="DtDue"]').val(moment().format('MM/DD/YYYY'));
    }

    //Date 4
    var ddd4 = $('input[name="DtService"]').val();

    $('input[name="DtService"]').daterangepicker(
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

    $('input[name="DtService"]').val(ddd4);
    if (ddd4 === '' || ddd4 === '1/1/0001 12:00:00 AM' || ddd4 === '1/1/0001 12:00:00 am') {
        $('input[name="DtService"]').val(moment().format('MM/DD/YYYY'));
    }


    //Date 5
    var ddd5 = $('input[name="DtServiceTo"]').val();

    $('input[name="DtServiceTo"]').daterangepicker(
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

    $('input[name="DtServiceTo"]').val(ddd5);

    console.log(ddd5);

    if (ddd5 === '' || ddd5 === '1/1/0001 12:00:00 AM' || ddd5 === '1/1/0001 12:00:00 am') {
        $('input[name="DtServiceTo"]').val(moment().format('MM/DD/YYYY'));
    }

}



