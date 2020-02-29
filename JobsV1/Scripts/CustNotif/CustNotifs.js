/*******************************
 *   CustNotif/Index Script    *
 *******************************/

function sendEmail(id) {
    //build json object
    var data = {
        id: id
    };
    $("#send-test").text("Sending Email");
    //console.log(data);

    var url = '/CustNotifs/SendEmail';

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
            if (data["responseText"] == "200") {

                $("#send-test").text("Email Sent");
            } else {
                $("#send-test").text("Email Failed");
            }
        }
    });
}

//Set Notification Id to the field
function setNotifId(id) {
    //console.log("NotifID:" + id);
    $("#add-NotifId").val(id);
    ajax_UpdateList();
}

//CREATE: Add recipient info to the notification recipient list
function ajax_AddInfo() {
    //build json object
    var data = {
        id: parseInt($("#add-NotifId").val()),
        customerId: parseInt($("#add-customerId").find(":selected").val()),
        email: $("#add-email").val(),
        mobile: $("#add-mobile").val()
    };

    //console.log(data);

    var url = '/CustNotifs/AddRecipient';

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
            //update recipient table
            ajax_UpdateList();
        }
    });
}

//CREATE: Add recipient info to the notification recipient list
function ajax_GetCustomer() {
    //build json object
    var data = {
        id: parseInt($("#add-customerId").find(":selected").val()),
    };

    //console.log(data);

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
            //console.log(data["responseText"]);
            //update recipient table
            // ajax_UpdateList();
            var temp = jQuery.parseJSON(data["responseText"]);
            var Name = temp["Name"].toString();
            var Email = temp["Email"] != null ? temp["Email"] : "--";
            var Mobile = temp["Mobile"] != null ? temp["Mobile"] : "--";

            $("#add-email").val(Email);
            $("#add-mobile").val(Mobile);
        }
    });
}


//READ: Get list of recipient with the given notificaiton Id
function ajax_UpdateList() {

    var setId = $("#add-NotifId").val();
    //build json object
    var data = {
        id: setId
    };

    //console.log(data);

    var url = '/CustNotifs/GetRecipientList';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "GET",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            //console.log("ERROR");
            //update recipient table
            loadList(data);
        }
    });
}


//DELETE: Remove Recipient from the recipient
function RemoveRecipient(id) {
    //build json object
    var data = {
        id
    };

    var url = '/CustNotifs/DeleteRecipient';

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
            console.log("Remove Recipient Success");
            ajax_UpdateList();
        }
    });
}

//update list of recipients in the table
function loadList(data) {

    //clear table contents except header
    $("#r-Table").find("tr:gt(0)").remove();

    var temp = jQuery.parseJSON(data["responseText"]);
    //console.log(temp);
    content = "";
    //populate table content
    for (var x = 0; x < temp.length; x++) {
        var Id = temp[x]["Id"].toString();
        var Name = temp[x]["Name"].toString();
        var Email = temp[x]["Email"] != null ? temp[x]["Email"] : "--";
        var Mobile = temp[x]["Mobile"] != null ? temp[x]["Mobile"] : "--";

        content += "<tr>";
        content += "<td>" + Email + "</td>";
        content += "<td>" + Mobile + "</td>";
        content += "<td> <a href='#' onclick='RemoveRecipient(" + Id + ")'> Remove</a> </td>";
        content += "</tr>";
    }

    if (temp.length == 0) {
        content += "<tr>";
        content += "<td colspan=3>  No Recipients Found </td>";
        content += "</tr>";
    }

    $(content).appendTo("#recipientTable");

}
