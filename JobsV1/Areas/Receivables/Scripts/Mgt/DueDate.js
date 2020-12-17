

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="DtEnd-New"]').val();

    $('input[name="DtEnd-New"]').daterangepicker(
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
    console.log("Ddd1: "+ddd1)
    console.log(ddd1.trim() == "")

    if (ddd1.trim() == "") {
        //$('input[name="DtInvoice"]').val(moment().format('MM/DD/YYYY h:mm A'));
    }


}


