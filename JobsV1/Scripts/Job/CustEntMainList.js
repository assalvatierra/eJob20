
//global variables
var status = "ACT";
var sort = "NAME";
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

//
$('#DATE').click(function () {
    sort = "DATE";
    setActiveSort(this);
});

//
$('#NAME').click(function () {
    sort = "NAME";
    setActiveSort(this);
});

//
$('#JOBSCOUNT').click(function () {
    sort = "JOBSCOUNT";
    setActiveSort(this);
});

function setActiveSort(element){
    $(element).css("color", "black");
    $(element).siblings().css("color", "steelblue");
    //StatusRefresh() // load inactive suppliers
    //load table content
    ajax_loadContent();
}

//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadContent() {
    var query = $('#srch-field').val();
   
    console.log("status: " + status);
    console.log("sort: " + sort);
    //console.log("q: " + query);

    //build json object
    var data = {
        search: query,
        status : status,
        sort : sort
    };

    //request data from server using ajax call
    $.ajax({
        url: 'CustEntMains/TableResult?search=' + query + '&status=' + status + '&sort=' + sort,
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
    
    console.log(data);
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    //console.log(temp);
    //clear table contents except header
    $("#company-Table").find("tr:gt(0)").remove();
    var jobcount = 0;
    var company  = "";
    var contact1 = "";
    var contact2 = "";


    //populate table content
    for (var x = 0; x < temp.length; x++) {
        Address = temp[x]["Address"] != null ? temp[x]["Address"] : "--";
        company = temp[x]["Remarks"] != null ? temp[x]["Remarks"] : "--";
        company = temp[x]["Website"] != null ? temp[x]["Website"] : "--";
        contact1 = temp[x]["Contact1"] != null ? temp[x]["Contact1"] : "--";
        contact2 = temp[x]["Contact2"] != null ? temp[x]["Contact2"] : "--";

        content  = "<tr>";
        content += "<td>" + temp[x]["Name"] + "</td>";
        content += "<td>" + Address + "</td>";
        content += "<td>" + contact1 + "<br />";
        content += " " + contact2 + "</td>";

        content += "<td>" + "<a href='CustEntMains/Details/" + temp[x]["Id"] + "'>Details</a> "
                + "</td>";
        content += "<tr>";

        $(content).appendTo("#company-Table");
    }
}

//load table with ACTIVE suppliers
function StatusRefresh() {
    //clear search field
    $('#srch-field').val('');

    //load table content
    ajax_loadContent();
}

