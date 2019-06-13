


//global variables
var status = "ACT";
var viewType = "SIMPLE";

//load initial on page ready
$(document).ready(ajax_loadContent());

//update status value on click
//change color of the text
//$('#ACTIVE').click(function () {
//    status = "ACT";
//    $('#ACTIVE').css("color", "black");
//    $('#ACTIVE').siblings().css("color", "steelblue");
//    StatusRefresh(); //load active suppliers
//});

//$('#INACTIVE').click(function () {
//    status = "INC";
//    $('#INACTIVE').css("color", "black");
//    $('#INACTIVE').siblings().css("color", "steelblue");
//    StatusRefresh(); // load inactive suppliers
//});

//$('#BAD').click(function () {
//    status = "BAD";
//    $('#BAD').css("color", "black");
//    $('#BAD').siblings().css("color", "steelblue");
//    StatusRefresh(); // load bad suppliers
//});

//$('#ALL').click(function () {
//    status = "ALL";
//    $('#ALL').css("color", "black");
//    $('#ALL').siblings().css("color", "steelblue");
//    StatusRefresh(); // load all suppliers
//});

//$('#simple-Table').click(function () {
//    viewType = "SIMPLE";
//    $('#simple-Table').css("color", "black");
//    $('#simple-Table').siblings().css("color", "steelblue");
//    StatusRefresh();
//});

//$('#expanded-Table').click(function () {
//    viewType = "EXPAND";
//    $('#expanded-Table').css("color", "black");
//    $('#expanded-Table').siblings().css("color", "steelblue");
//    StatusRefresh();
//});


//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadContent() {
    var query = $('#srch-field').val();
    //console.log("status: " + status);
    console.log('load:'+status);

    //build json object
    var data = {
        search: query,
        status: status
    };

    //console.log(query);
    //request data from server using ajax call
    $.ajax({
        url: '/Products/SMProducts/TableResult?search=' + query + '&status=' + status,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
            
        },
        error: function (data) {
            // console.log("ERROR");
            console.log(data);
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
        default:
            SimpleTable(data);
    }

}

//display simple/limited information 
//of suppliers
function SimpleTable(data) {
    //console.log("SimpleTable");
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    //clear table contents except header
    $("#prodTable").find("tr:gt(0)").remove();


    //populate table content
    for (var x = 0; x < temp.length; x++) {
        if (moment().diff(temp[x]["ValidityEnd"], 'days') > 0 ) {

        content = "<tr style='color:orangered;'>";
        content += "<td>" + temp[x]["Code"] + "</td>";
        content += "<td>" + temp[x]["Name"] + "</td>";
        content += "<td>" + temp[x]["Price"] + "</td>";
        content += "<td>" + temp[x]["ValidityStart"] + "</td>";
        content += "<td>" + temp[x]["ValidityEnd"] + "</td>";
        content += "<td>" + temp[x]["Remarks"] + "</td>";
        content += "<td>" + temp[x]["Status"] + "</td>";
        content += "<td>" +
            "<a href='SMProducts/Details/" + temp[x]["Id"] + "'>Details</a> |" +
            "<a href='SmProducts/Deactivate/" + temp[x]["Id"] + "'>Deactivate</a> " +
            "</td>";
        content += "<tr>";

        } else {

            content = "<tr>";
            content += "<td>" + temp[x]["Code"] + "</td>";
            content += "<td>" + temp[x]["Name"] + "</td>";
            content += "<td>" + temp[x]["Price"] + "</td>";
            content += "<td>" + temp[x]["ValidityStart"] + "</td>";
            content += "<td>" + temp[x]["ValidityEnd"] + "</td>";
            content += "<td>" + temp[x]["Remarks"] + "</td>";
            content += "<td>" + temp[x]["Status"] + "</td>";
            content += "<td>" +
                "<a href='SMProducts/Details/" + temp[x]["Id"] + "'>Details</a> |" +
                "</td>";
            content += "<tr>";

        }


        $(content).appendTo("#prodTable");

    }
}


//load table with ACTIVE suppliers
function StatusRefresh() {
    //clear search field
    $('#srch-field').val('');

    //load table content
    ajax_loadContent();
}

function setActiveTrue() {
    status = "ACT";
    $("#status-act").css('color', 'black');
    $("#status-inc").css('color', 'dodgerblue');
    //console.log("select" + status);
    ajax_loadContent();
}

function setInactiveTrue() {
    status = "INC";
    $("#status-act").css('color', 'dodgerblue');
    $("#status-inc").css('color', 'black');
    //console.log("select" + status);
    ajax_loadContent();
}