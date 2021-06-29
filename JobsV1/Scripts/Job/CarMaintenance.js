
/*****
 *  Car Maintenance Records
 * 
 **/ 
$(document).ready(function () {
    InitDatePicker();
    GetRecordNextSchedule();
})


function InitDatePicker()
{
    var ddd1 = $('input[name="dtDone"]').val();
    var ddd2 = $('input[name="NextSched"]').val();

    $('input[name="dtDone"]').daterangepicker(
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
        //alert(start.format('YYYY-MM-DD h:mm A'));
        
    }
    );


    $('input[name="NextSched"]').daterangepicker(
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
        //alert(start.format('YYYY-MM-DD h:mm A'));
        
    }
    );

    $('input[name="dtDone"]').val(ddd1.substr(0, ddd1.indexOf(" ")));
    $('input[name="NextSched"]').val(ddd2.substr(0, ddd2.indexOf(" ")));

}


function GetRecordNextSchedule() {
    var recordTypeId = $("#RecordTypeId").val();
    var currentOdo = $("#CurrentOdo").val();
    var DateDone = $('input[name="dtDone"]').val();

    $.get("/InvCarRecords/GetRecordNextSchedule", { id: recordTypeId })
        .done((res) => {
            console.log(res);

            var nextOdo = parseInt(res["OdoInterval"]) + parseInt(currentOdo);
            var nextDate = moment(DateDone).add(parseInt(res["DaysInterval"]), 'days').format("MM/DD/YYYY") ;

            console.log("nextOdo: " + nextOdo);
            console.log("DateDone: " + DateDone);
            console.log("nextDate: " + nextDate);

            $("#NxtOdometer").val(nextOdo);
            $('input[name="NextSched"]').val(nextDate);
        })
        .fail();
}

$('#RecordTypeId, #CurrentOdo,input[name="dtDone"]').on("change", () => {
    GetRecordNextSchedule();
});

