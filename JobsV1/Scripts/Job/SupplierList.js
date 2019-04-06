﻿


//global variables
var status = "ACT";
var viewType = "SIMPLE";

//load initial on page ready
$(document).ready(ajax_loadContent());

//update status value on click
//change color of the text
$('#ACTIVE').click(function () {
    status = "ACT";
    $('#ACTIVE').css("color", "black");
    $('#ACTIVE').siblings().css("color", "steelblue");
});

$('#INACTIVE').click(function () {
    status = "INC";
    $('#INACTIVE').css("color", "black");
    $('#INACTIVE').siblings().css("color", "steelblue");
});

$('#BAD').click(function () {
    status = "BAD";
    $('#BAD').css("color", "black");
    $('#BAD').siblings().css("color", "steelblue");
});

$('#ALL').click(function () {
    status = "ALL";
    $('#ALL').css("color", "black");
    $('#ALL').siblings().css("color", "steelblue");
});

$('#simple-Table').click(function () {
    viewType = "SIMPLE";
    $('#simple-Table').css("color", "black");
    $('#simple-Table').siblings().css("color", "steelblue");
});

$('#expanded-Table').click(function () {
    viewType = "EXPAND";
    $('#expanded-Table').css("color", "black");
    $('#expanded-Table').siblings().css("color", "steelblue");
});


//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadContent() {
    var query = $('#srch-field').val();

    console.log("status: " + status);

    //build json object
    var data = {
        search: query
    };

    console.log(query);

    //request data from server using ajax call
    $.ajax({
        url: '/Suppliers/TableResult?search=' + query + '&status=' + status,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log("SUCCESS");
            
        },
        error: function (data) {
            console.log("ERROR");
            switchViews(data)
        }
    });
}


//param : data - json object containing 
//
function switchViews(data) {
    switch(viewType){
        case "SIMPLE":
            SimpleTable(data);
            break;
        case "EXPAND":
            ExpandedTable(data);
            break;
        default:
            SimpleTable(data);
    }

}

//display simple/limited information 
//of suppliers
function SimpleTable(data) {
    console.log("SimpleTable");
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    console.log(temp);
    //clear table contents except header
    $("#sup-Table").find("tr:gt(0)").remove();


    //populate table content
    for (var x = 0; x < temp.length; x++) {
        content = "<tr>";
        content += "<td>" + temp[x]["Name"] + "</td>";
        content += "<td>" + temp[x]["Contact1"] + "</td>";
        content += "<td>" + temp[x]["Contact2"] + "</td>";
        content += "<td>" + temp[x]["Contact3"] + "</td>";
        content += "<td>" +
            "<a href='Suppliers/Edit/" + temp[x]["Id"] + "'>Edit</a> |" +
            "<a href='SupplierItems/'>Items</a>  |" +
            "<a href='Suppliers/InvItems/" + temp[x]["Id"] + "'>InvItems</a>" +
            "</td>";
        content += "<tr>";

        $(content).appendTo("#sup-Table");
    }
}

//display expanded information 
//of suppliers
function ExpandedTable(data) {
    console.log("ExpandedTable");
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    //clear table contents except header
    $("#sup-Table").find("tr:gt(0)").remove();



    //populate table content
    for (var x = 0; x < temp.length; x++) {
        content = "<tr>";
        content += "<td>" + temp[x]["Name"] + "</td>";
        content += "<td>" + temp[x]["Contact1"] + "<br>"
            + temp[x]["Contact2"] + "<br>"
            + temp[x]["Contact3"] + "</td>";
        content += "<td>" + temp[x]["Email"] + "</td>";
        content += "<td>" + temp[x]["Status"] + "</td>";
        content += "<td>" + temp[x]["City"] + "</td>";
        content += "<td>" + temp[x]["SupType"] + "</td>";
        content += "<td>" + temp[x]["Dtls"] + "</td>";
        content += "<td>" +
            "<a href='Suppliers/Edit/" + temp[x]["Id"] + "'>Edit</a> |" +
            "<a href='SupplierItems/'>Items</a>  |" +
            "<a href='Suppliers/InvItems/" + temp[x]["Id"] + "'>InvItems</a>" +
            "</td>";
        content += "<tr>";

        $(content).appendTo("#sup-Table");
    }
}