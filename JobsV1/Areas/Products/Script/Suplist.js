


//global variables
var status = "ACT";
var viewType = "SIMPLE";

//load initial on page ready
$(document).ready(ajax_loadContent());


//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadContent() {
    var query = $('#srch-field').val();
    //console.log("status: " + status);

    //build json object
    var data = {
        search: query
    };

    //console.log(query);
    //request data from server using ajax call
    $.ajax({
        url: '/Products/SmSuppliers/TableResult',
        type: "GET",
        data: data,
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
        content += "<td>" + temp[x]["Description"] + "</td>";
        content += "<td>" + temp[x]["Remarks"] + "</td>";
        content += "<td>" +
            "<a href='SmSuppliers/Details/" + temp[x]["Id"] + "'>Details</a> |" +
            "<a href='SmSuppliers/Delete/" + temp[x]["Id"] + "'>Delete</a> " +
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
