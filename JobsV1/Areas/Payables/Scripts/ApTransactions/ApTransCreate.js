
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

    if (ddd1 === '' || ddd1 === '1/1/0001 12:00:00 AM') {
        $('input[name="DtInvoice"]').val(moment().format('MM/DD/YYYY'));
    } else {
        $('input[name="DtInvoice"]').val(moment(ddd1).format('MM/DD/YYYY'));
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

    if (ddd2 === '' || ddd2 === '1/1/0001 12:00:00 AM') {
        $('input[name="DtEncoded"]').val(moment().format('MM/DD/YYYY'));
    } else {
        $('input[name="DtEncoded"]').val(moment(ddd2).format('MM/DD/YYYY'));
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

    if (ddd3 === '' || ddd3 === '1/1/0001 12:00:00 AM') {
        $('input[name="DtDue"]').val(moment().format('MM/DD/YYYY'));
    } else {
        $('input[name="DtDue"]').val(moment(ddd3).format('MM/DD/YYYY'));
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
    if (ddd4 === '' || ddd4 === '1/1/0001 12:00:00 AM') {
        $('input[name="DtService"]').val(moment().format('MM/DD/YYYY'));
    } else {
        $('input[name="DtService"]').val(moment(ddd4).format('MM/DD/YYYY'));
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

    if (ddd5 === '' || ddd5 === '01/01/0001 12:00:00 AM') {
        $('input[name="DtServiceTo"]').val(moment().format('MM/DD/YYYY'));
    } else {
        $('input[name="DtServiceTo"]').val(moment(ddd5).format('MM/DD/YYYY'));
    }

}



