

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('.datePicker').val();

    $('.datePicker').daterangepicker(
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
             alert(start.format('YYYY-MM-DD h:mm A'));
        }
    );

    $('.datePicker').val(moment().format('MM/DD/YYYY'));

    //Date 2
    var ddd2 = $('.dateTimePicker').val();

    $('.dateTimePicker').daterangepicker(
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

    $('.dateTimePicker').val(moment().format('MM/DD/YYYY h:mm A'));

}


