

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
            // alert(start.format('YYYY-MM-DD h:mm A'));
        }
    );

    console.log(ddd1.trim() == "")
    if (ddd1.trim() == "") {
        $('input[name="DtInvoice"]').val(moment().format('MM/DD/YYYY h:mm A'));
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
            // alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );

    if (ddd2.trim() == "") {
        $('input[name="DtEncoded"]').val(moment().format('MM/DD/YYYY h:mm A'));
    }

    //Date 2
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
            // alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );

    if (ddd3.trim() == "") {
        $('input[name="DtDue"]').val(moment().format('MM/DD/YYYY h:mm A'));
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
            // alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );

    if (ddd4.trim() == "") {
        $('input[name="DtService"]').val(moment().format('MM/DD/YYYY h:mm A'));
    }

}


