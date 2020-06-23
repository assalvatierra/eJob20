﻿/**
 *  Supplier Details
 * 
 */

$(document).ready(initial());

function initial() {
    var status = filterStatus($("#status-companyName").text());

    $("#status-companyName").text(" ( " + status + " ) ");
    $("#status-Details").text(" " + status + " ");
}



//display confirmation message for removal of contact
function ConfirmRemoveContact(id, name) {
    var res = confirm("Do you want to remove " + name + " ?")
    if (res) {
        ajax_deleteContactId(id);
    }
}

//Contact Delete
function ajax_deleteContactId(Id) {

    //build json object
    var data = {
        id: Id
    };

    console.log(data);

    var url = '/Suppliers/deleteSupContact';

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

function filterStatus(status) {
    switch (status) {
        case "ACT":
            return "Active";
            break;
        case "INC":
            return "Inactive";
            break;
        case "BAD":
            return "Bad Account";
            break;
        case "AOP":
            return "Accreditation on Progress";
            break;
        case "ACC":
            return "Accredited";
            break;
        case "PRI":
            return "PRIORITY";
            break;
        default:
            return "Active";
            break;
    }
}
