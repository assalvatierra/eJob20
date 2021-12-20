
/*
 *  ApTransaction Create/Edit
 *  Initialize date time picker on input fields
 */



$(document).ready(function () {
    InitDatePicker();

})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="Date"]').val();

    $('input[name="Date"]').daterangepicker(
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

    $('input[name="Date"]').val(moment(ddd1).format('MM/DD/YYYY'));

    if (ddd1 === '' || ddd1 === '1/1/0001 12:00:00 am') {
        $('input[name="Date"]').val(moment().format('MM/DD/YYYY'));
    }


}



