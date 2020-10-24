

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

    //$('input[name="DtStart"]').val(ddd1.substr(0, ddd1.indexOf(" ")));

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


}


