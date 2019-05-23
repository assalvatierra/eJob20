
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
    StatusRefresh(); //load active suppliers
});

$('#INACTIVE').click(function () {
    status = "INC";
    $('#INACTIVE').css("color", "black");
    $('#INACTIVE').siblings().css("color", "steelblue");
    StatusRefresh() // load inactive suppliers
});

$('#BAD').click(function () {
    status = "BAD";
    $('#BAD').css("color", "black");
    $('#BAD').siblings().css("color", "steelblue");
    StatusRefresh() // load inactive suppliers
});

$('#ALL').click(function () {
    status = "ALL";
    $('#ALL').css("color", "black");
    $('#ALL').siblings().css("color", "steelblue");
    StatusRefresh() // load inactive suppliers
});


//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadContent() {
    var query = $('#srch-field').val();
    status = "ACT";
    //console.log("status: " + status);
    //console.log("q: " + query);

    //build json object
    var data = {
        search: query
    };

    //request data from server using ajax call
    $.ajax({
        url: '/Customers/TableResult?search=' + query + '&status=' + status,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            //console.log("ERROR");
            //console.log(data);
            LoadTable(data);
        }
    });
}


//display simple/limited information 
//of suppliers
function LoadTable(data) {

    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    //console.log(temp);
    //clear table contents except header
    $("#sup-Table").find("tr:gt(0)").remove();

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        content  = "<tr>";
        content += "<td>" + temp[x]["Name"]      + "</td>";
        content += "<td>" + temp[x]["Contact1"]  + "</td>";
        content += "<td>" + temp[x]["Contact2"]  + "</td>";
        content += "<td>" + temp[x]["Company"]   + "</td>";
        content += "<td>" + temp[x]["JobsCount"] + "</td>";
        content += "<td>" + temp[x]["Status"]    + "</td>";
        content += "<td>" +
            "<a href='Customers/Details/" + temp[x]["Id"] + "'>Details</a> "
        "</td>";
        content += "<tr>";

        $(content).appendTo("#sup-Table");
    }
}

//load table with ACTIVE suppliers
function StatusRefresh() {
    //clear search field
    $('#srch-field').val('');

    //load table content
    ajax_loadContent();
}

