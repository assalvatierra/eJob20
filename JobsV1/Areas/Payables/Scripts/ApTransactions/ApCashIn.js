
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
    console.log(ddd1);

    console.log(ddd1 == '01/01/2001');
    
    if (ddd1 == '' || ddd1 == '01/01/0001 12:00:00 AM' || ddd1 == '01/01/2001' || ddd1 == '1/1/0001 12:00:00 AM') {
        console.log(moment().format('MM/DD/YYYY'));
        $('input[name="Date"]').val(moment().format('MM/DD/YYYY'));
    }


}



