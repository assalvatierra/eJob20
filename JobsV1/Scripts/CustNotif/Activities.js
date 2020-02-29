/*******************************
 * CustNotif/Activities Script *
 *******************************/

//GLOBAL VARIABLE
var timer;

//On Page Ready
$(document).ready(new function () {
    ajax_UpdateList();
    UpdatePage();
});

//UPDATE: Get list of recipient with the given notificaiton Id
function ajax_UpdateList() {
    console.log("Updating Page")
    //var setId = $("#add-NotifId").val();
    //build json object
    var data = {
    };

    //console.log(data);
    var url = '/CustNotifs/GetActivity';

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
            //console.log(data);
            //update recipient table
            loadList(data);
        }
    });
}

//update list of recipients in the table
function loadList(data) {

    //clear table contents except header
    $("#activityTable").find("tr:gt(0)").remove();

    var temp = jQuery.parseJSON(data["responseText"]);
    //console.log(temp);
    content = "";
    //populate table content
    for (var x = 0; x < temp.length; x++) {
        var Id = temp[x]["Id"].toString();
        var Name = temp[x]["Recipient"].toString();
        var Title = temp[x]["Title"] != null ? temp[x]["Title"] : "--";
        var Status = temp[x]["Status"] != null ? temp[x]["Status"] : "--";
        var DtActivity = temp[x]["DtActivity"] != null ? temp[x]["DtActivity"] : "--";

        content += "<tr>";
        content += "<td>" + moment(DtActivity).format("MMM DD YYYY h:mm A") + "</td>";
        content += "<td>" + Title + "</td>";
        content += "<td>" + Name + "</td>";
        content += "<td>" + Status + "</td>";

        content += "<td>";
        if (Status == "DONE") {
            content += "<a href='#' onclick='Undone_UpdateStatus(" + Id + ")'> Resend </a> ";
        }

        if (Status == "CANCELLED") {
            content += "<a href='#' onclick='Undone_UpdateStatus(" + Id + ")'> Resend </a> ";
        }

        if (Status == "PENDING") {
            content += "<a href='#' onclick='Cancel_UpdateStatus(" + Id + ")'> Cancel </a> ";
            content += "| <a href='#' onclick='SendEmail(" + Id + ")'> Send <a/> ";
        }

        content += "<td>";

        content += "</td>";
        content += "</tr>";
    }

    $(content).appendTo("#activityTable");

}

//Update the table every 60 seconds
function UpdatePage() {
    myVar = setInterval(ajax_UpdateList, 60000); // in milliseconds // 60 seconds
}

function Undone_UpdateStatus(id) {
    Act_UpdateStatus(id,"PENDING");
}

function Cancel_UpdateStatus(id) {
    Act_UpdateStatus(id, "CANCELLED");
}

//UPDATE: Get list of recipient with the given notificaiton Id
function Act_UpdateStatus(id, status) {

    //build json object
    var data = {
        id: id,
        status: status
    };

    //console.log(data);
    var url = '/CustNotifs/UpdateActivityStatus';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            //console.log("ERROR");
            console.log(data);
            
            var res = data["responseText"];
            if (res == "200") {
                ajax_UpdateList();
            }
        }
    });
}

