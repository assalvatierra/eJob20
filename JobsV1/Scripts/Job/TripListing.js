/****************************/
/***** Supplier Listing *****/
/****************************/

$(document).ready(function () {
    InitDatePicker();
    
    var dateDiff = (moment().subtract(10, "days").format("MM/DD/YYYY"));

    $('input[name="dtEnd"]').val(moment().format("MM/DD/YYYY"));
    $('input[name="dtStart"]').val(dateDiff);
})


function InitDatePicker() {
    var ddd1 = $('input[name="dtStart"]').val();

    $('input[name="dtStart"]').daterangepicker(
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
    $('input[name="dtStart"]').val(ddd1.substr(0, ddd1.indexOf(" ")));

    //Date End
    var ddd1 = $('input[name="dtEnd"]').val();

    $('input[name="dtEnd"]').daterangepicker(
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


    $('input[name="dtEnd"]').val(ddd1.substr(0, ddd1.indexOf(" ")));
}


//Add Item Expense
function ajax_AddExpenses() {


    //build json object
    var data = {
        id:         $("#Exp-JobServiceId").val(),
        payment:    $("#Exp-Payment").val(),
        fuel:       $("#Exp-Fuel").val(),
        driver:     $("#Exp-Driver").val(),
        Operator:   $("#Exp-Operator").val(),
        others:     $("#Exp-Others").val(),
        remarks:    $("#Exp-Remarks").val(),
        driverForRelease: $("#driver-readyReleased").is(":disabled"),
        operatorForRelease: $("#operator-readyReleased").is(":disabled"),

    };

    console.log(data);

    var url = '/JobMains/AddExpenses';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            // console.log("SUCCESS");
        },
        error: function (data) {
            // console.log("ERROR");
             console.log(data);
            location.reload();
        }
    });
}

function addExpenses(jsId) {
    $("#Exp-JobServiceId").val(jsId);
    //console.log(jsId);

    //load fields with expenses record
    ajax_getExpenses();
}



//load table content on search btn click
function ajax_getExpenses() {
    var id = $("#Exp-JobServiceId").val();
    //build json object
    var data = {
        jsId: id
    };

    //console.log(query);
    //request data from server using ajax call
    $.ajax({
        url: '/JobMains/getExpenseRecord?jsId=' + id,
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            // console.log("ERROR");
            console.log(data);
            ExpenseRecord(data);
        }
    });
}

//display products
function ExpenseRecord(data) {
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    console.log("products view");

    var PaymentCash  = temp["PaymentCash"]   != null ? temp["PaymentCash"]   : "0";
    var PaymentBank  = temp["PaymentBank"]   != null ? temp["PaymentBank"]   : "0";
    var ActualAmt    = temp["ActualAmt"]     != null ? temp["ActualAmt"]     : "0";
    var DriverComi   = temp["DriverComi"]    != null ? temp["DriverComi"]    : "0";
    var Fuel         = temp["Fuel"]          != null ? temp["Fuel"]          : "0";
    var OperatorComi = temp["OperatorComi"]  != null ? temp["OperatorComi"]  : "0";
    var Others       = temp["Others"]        != null ? temp["Others"]        : "0";
    var Remarks      = temp["Remarks"]       != null ? temp["Remarks"]       : "";
    var Total        = temp["Total"]         != null ? temp["Total"]         : null;
    var Net          = temp["Net"]           != null ? temp["Net"]           : null;
    var DvrForRelease= temp["DriverForRelease"] != null ? temp["DriverForRelease"] : false;
    var OptForRelease= temp["OperatorForRelease"] != null ? temp["OperatorForRelease"] : false;

    //transfer to fields
    $("#Exp-Payment").val(PaymentCash);
    $("#Exp-PaymentBank").val(PaymentBank);
    $("#Exp-Fuel").val(Fuel);
    $("#Exp-Driver").val(DriverComi);
    $("#Exp-Operator").val(OperatorComi);
    $("#Exp-Others").val(Others);
    $("#Exp-Remarks").val(Remarks);

    //disable fields
    $("#Exp-Driver").attr("disabled", DvrForRelease);
    $("#driver-readyReleased").attr("disabled", DvrForRelease);

    $("#Exp-Operator").attr("disabled", OptForRelease);
    $("#operator-readyReleased").attr("disabled", OptForRelease);

    //calculate 
    calculateTotal();
}

function calculateTotal() {

    var payment  = $("#Exp-Payment").val();
    var fuel     = $("#Exp-Fuel").val();
    var driver   = $("#Exp-Driver").val();
    var operator = $("#Exp-Operator").val();
    var others   = $("#Exp-Others").val();

    var total = Number(fuel) + Number(driver) + Number(operator) + Number(others);

    $("#Exp-Total").text(total);

    var net = payment - total;

    $("#Exp-Net").text(net);

}


function FilterTrip() {

    //get values from driver, date start and date end
    var driverId = parseInt($("#trip-driver").val());
    var startDate = $("#trip-startDate").val();
    var endDate = $("#trip-endDate").val();

    ajax_getDriverTrips(driverId, startDate, endDate);
}


//load table content on search btn click
function ajax_getDriverTrips(id,startDate,endDate) {

    //build json object
    var data = {
        id: id,
        sDate : startDate,
        eDate : endDate
    };

    console.log(data);
    //request data from server using ajax call
    $.ajax({
        url: '/JobMains/GetDriverTrip?id='+id+"&sDate="+startDate+"&eDate="+endDate,
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            // console.log("ERROR");
            //console.log(data);
            LoadTripTable(data);
        }
    });
}


//display products
function LoadTripTable(data) {
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    var today = moment(new Date());
    var total = 0;
    //console.log(temp);

    //clear table contents except header
    $("#DriverTripTable").find("tr:gt(0)").remove();

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        var id = temp[x]["Id"] != null ? temp[x]["Id"] : "0";
        var jobMainId = temp[x]["JobMainId"] != null ? temp[x]["JobMainId"] : "--";
        var DtStart = temp[x]["DtStart"] != null ? temp[x]["DtStart"] : "--";
        var DtEnd = temp[x]["DtEnd"] != null ? temp[x]["DtEnd"] : "--";
        var Description = temp[x]["Description"] != null ? temp[x]["Description"] : "--";
        var Particulars = temp[x]["Particulars"] != null ? temp[x]["Particulars"] : "--";
        var Name = temp[x]["Name"] != null ? temp[x]["Name"] : "--";
        var ItemCode = temp[x]["ItemCode"] != null ? temp[x]["ItemCode"] : "--";
        var Amount = temp[x]["Amount"] != null ? temp[x]["Amount"] : "--";
        var Remarks = temp[x]["Remarks"] != null ? temp[x]["Remarks"] : "--";
        var isReleased = temp[x]["IsReleased"] != null ? temp[x]["IsReleased"] : "--";
        var ReleaseDate = temp[x]["DtExpense"] != null ? temp[x]["DtExpense"] : "--";

        console.log(isReleased)
            content = "<tr>";
            content += "<td>" + moment(DtStart).format("MMM DD") + " - " + moment(DtEnd).format("MMM DD") + "</td>";
            content += "<td>" + jobMainId + "</td>";
            content += "<td>" + Description + "</td>";
            content += "<td>" + Particulars + "</td>";
            content += "<td>" + Amount + "</td>";
            content += "<td>" + Remarks + "</td>";

            if (isReleased != "1") {
                content += "<td>" +
                           " <button class='btn btn-primary' onclick='DriverRelease(this,"+id+" )'> Release </button>";
                content += "</tr>";
            } else {
                content += "<td>" +
                          moment(ReleaseDate).format("MMM DD");
                content += "</tr>";
            }
            $(content).appendTo("#DriverTripTable");
            //calculate total
            total += parseInt(Amount);
    }
    console.log(total);
    $("#tripDriver-total").text(total);
}


function DriverRelease(btn, expenseId) {
    ajax_DriverComiReleased(btn,expenseId)
}

function ajax_DriverComiReleased(btn,expenseId) {
    var result = "";
    //build json object
    var data = {
        id: expenseId,
    };
    console.log("release ");
    console.log(data);
    //request data from server using ajax call
    $.ajax({
        url: '/JobMains/ComiRelease?id=' + expenseId,
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            // console.log("ERROR");
            console.log(data["responseText"] == "True");
            // LoadTripTable(data);

            result = data["responseText"];
            console.log("result:" + result);
            if (result == "True") {
                $(btn).attr("disabled", true);
            }
        }
    });

}