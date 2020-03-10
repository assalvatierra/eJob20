
/* ********************************************************
*
* By Abel S. Salvatierra                
* @2017 - Real Breeze Travel & Tours    
* 
*********************************************************** */

$(document).ready(function () {
    InitDatePicker();
});

function InitDatePicker()
{
    //Date 1
    var ddd1 = $('input[name="DtScheduled"]').val();

    $('input[name="DtScheduled"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 30,
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
    $('input[name="DtScheduled"]').val(ddd1);

    $('input[name="DtScheduled"]').val(moment().add(1, 'days').format("MM/DD/YYYY h:mm A"));
}

/**
  * TODO: 
  * 1. add Recipient - Done
  * 2. Remove Recipient - Done
  * 3. Edit Recipient - Error
  * 4. Save Recipient - 
  **/

//ADD Recipient to the Table
function AddRecipientModal() {
    //Show Modal
    $('#addRecipientModal').modal('show');
}

//GET: Get customer email and mobile 
function ajax_GetCustomer() {

    //build json object
    var data = {
        id: parseInt($("#add-customerId").find(":selected").val()),
    };

    var url = '/CustNotifs/GetRecipient';

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
            var temp = jQuery.parseJSON(data["responseText"]);
            var Name = temp["Name"].toString();
            var Id = temp["Id"] != null ? temp["Id"] : "--";
            var Email = temp["Email"] != null ? temp["Email"] : "--";
            var Mobile = temp["Mobile"] != null ? temp["Mobile"] : "--";

            $("#add-NotifId").val(Id);
            $("#add-email").val(Email);
            $("#add-mobile").val(Mobile);
        }
    });
}

//ADD: Add Recipient Details to the Table
function ajax_AddInfo() {

   var id = $("#add-NotifId").val();
   var email = $("#add-email").val();
   var mobile = $("#add-mobile").val();

   content = ""; 
   content += "<tr>";
   content += "<td>" + id + "</td>";
   content += "<td>" + email + "</td>";
   content += "<td>" + mobile + "</td>";
   content += "<td>";
   //content += '<td> <a class="cursor-hand" onclick="EditRecipient('+ id +', ' + email + ', '+ mobile +')" > Edit </a> |';
   content += " <a class='cursor-hand' onclick='RemoveRecipient(this)'> Remove </a> </td>";
   content += "</tr>";

   $(content).appendTo("#RecipientTable");
}

//DELETE : remove recipient row from the table
function RemoveRecipient(row) {
    $(row).parents("tr").remove();
}

//EDIT : get row details ready for edit
function EditRecipient( id, email, mobile) {

    $("#add-NotifId").val(id);
    $("#add-customerId").val(id);
    $("#add-email").val(email);
    $("#add-mobile").val(mobile);
}

//SAVE: Save Notification and Recipients
function Submit() {

}

//CREATE : Submit Notification and return Notificatoin Id
function ajax_SubmitNotif() {

    //build json object
    var data = {
        MsgTitle: $("#notif-Title").val(),
        MsgBody: $("textarea:input[name=MsgBody]").val(),
        DtSched: $("input[name='DtScheduled']").val(),
        Occurence: $("#notif-Occurence").val(),
        IsSms: $("#notif-IsSms").val(),
        IsEmail: $("#notif-IsEmail").val(),
        Status: $("#notif-Status").val()
    };

    //console.log(data);

    var url = '/CustNotifs/CreateNotifRecord';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
             console.log("SUCCESS");
            console.log(data["responseText"]);
            ajax_AddRecipients();
        },
        error: function (data) {
            console.log("ERROR");
            console.log(data["responseText"]);

            resp = data["responseText"];
            if (resp != "0") {
                ajax_AddRecipients(data["responseText"]);
            }
        }
    });
}

//GET : Loop through the recipients table and save to notification recipients
function ajax_AddRecipients(id) {
    $('#RecipientTable > tbody  > tr').each(function (i, tr) {
        console.log(i);
        var $tds = $(this).find('td'),
            custId = $tds.eq(0).text(),
            custEmail = $tds.eq(1).text(),
            custMobile = $tds.eq(2).text();

        ajax_AddNotifRecipient(id, custId, custEmail, custMobile );
    });
}

//CREATE : submit notification recipient to the server
function ajax_AddNotifRecipient(id, customerId, customerEmail, customerMobile) {

    //build json object
    var data = {
        id : id,
        customerId : customerId,
        email: customerEmail,
        mobile: customerMobile
    };
    console.log(data);
    var url = '/CustNotifs/AddRecipient';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
             console.log("SUCCESS");
             console.log(customerId +" : " + data["responseText"]);
        },
        error: function (data) {
            console.log("ERROR");
            console.log(customerId + " : " + data["responseText"]);
        }
    });


}
