

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="DtPayment"]').val();

    $('input[name="DtPayment"]').daterangepicker(
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

    //$('input[name="DtStart"]').val(ddd1.substr(0, ddd1.indexOf(" ")));

}


