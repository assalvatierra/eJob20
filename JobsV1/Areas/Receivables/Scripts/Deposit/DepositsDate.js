

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="DtDeposit"]').val();

    $('input[name="DtDeposit"]').daterangepicker(
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

    $('input[name="DtDeposit"]').val(moment(ddd1).format('MM/DD/YYYY '));

}


