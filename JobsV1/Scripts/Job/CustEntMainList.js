
//global variables
var status = "ACT";
var sort = "NAME";
var srchcategory = "All";
var viewType = "SIMPLE";

//load initial on page ready
$(document).ready(ajax_loadContent());

//update status value on click
//change color of the text
$('#ACT').click(function () {
    status = "ACT";
    $('#ACT').css("color", "black");
    $('#ACT').siblings().css("color", "steelblue");
    StatusRefresh(); //load active suppliers
});

$('#INC').click(function () {
    status = "INC";
    $('#INC').css("color", "black");
    $('#INC').siblings().css("color", "steelblue");
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

$('#PRI').click(function () {
    status = "PRI";
    $('#PRI').css("color", "black");
    $('#PRI').siblings().css("color", "steelblue");
    StatusRefresh(); //load active suppliers
});

$('#ACC').click(function () {
    status = "ACC";
    $('#ACC').css("color", "black");
    $('#ACC').siblings().css("color", "steelblue");
    StatusRefresh(); //load active suppliers
});

$('#AOP').click(function () {
    status = "AOP";
    $('#AOP').css("color", "black");
    $('#AOP').siblings().css("color", "steelblue");
    StatusRefresh(); //load active suppliers
});

$('#BOT').click(function () {
    status = "BOT";
    $('#BOT').css("color", "black");
    $('#BOT').siblings().css("color", "steelblue");
    StatusRefresh(); //load active suppliers
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
    var category = $('#srch-category').val();

    console.log("category: " + category);
    console.log("status: " + status);
    console.log("sort: " + sort);
    console.log("q: " + query);

    //build json object
    var data = {
        search: query,
        searchCat: category,
        status: status,
        sort : sort
    };

    console.log(data);

    //request data from server using ajax call
    $.ajax({
        url: '/CustEntMains/TableResult?search=' + query + '&searchCat=' + category + '&status=' + status + '&sort=' + sort,
        //url: 'CustEntMains/TableResult',
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            //console.log("ERROR");
            console.log(data);
            LoadTable(data);
        }
    });
}

//display simple/limited information 
//of suppliers
function LoadTable(data) {
    
    //console.log(data);
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    console.log(temp);
    //clear table contents except header
    $("#company-Table").find("tr:gt(0)").remove();
    var jobcount = 0;
    var company  = "";
    var contact1 = "";
    var contact2 = "";
    var tempDetails = "-";
    var prevId = 0;
    var City = "-";
    var Assigned = "-";

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        Address = temp[x]["Address"] != null ? temp[x]["Address"] : "--";
        company = temp[x]["Remarks"] != null ? temp[x]["Remarks"] : "--";
        website = temp[x]["Website"] != null ? temp[x]["Website"] : "--";
        contact1 = temp[x]["Contact1"] != null ? temp[x]["Contact1"] : "--";
        contact2 = temp[x]["Contact2"] != null ? temp[x]["Contact2"] : "--";
        mobile = temp[x]["Mobile"] != null ? temp[x]["Mobile"] : "--";
        City = temp[x]["City"] != null ? temp[x]["City"] : "--";
        Assigned = temp[x]["AssignedTo"] != null ? temp[x]["AssignedTo"] : "--";

        var categories = temp[x]["Category"] != null ? temp[x]["Category"] : "--";
        var ContactPerson = temp[x]["ContactPerson"] != null ? temp[x]["ContactPerson"] : "--";
        var ContactPersonPos = temp[x]["ContactPersonPos"] != null ? temp[x]["ContactPersonPos"] : "--";
        var Status = temp[x]["Status"] != null ? temp[x]["Status"] : "--";
        var email = temp[x]["ContactPersonEmail"] != null ? temp[x]["ContactPersonEmail"] : "--";
        var code = temp[x]["Code"] != null ? temp[x]["Code"] : "--";

        if (temp[x]["Id"] == prevId) {

            content  = "<td></td>";
            content += "<td> &nbsp;  " + categories + "</td>";
            content += "<td></td><td></td><td></td><td></td><td></td>";
        
        } else {

            content = "<tr>";
            content += "<td>" + temp[x]["Name"] + "</td>";
            content += "<td>" + code + "</td>";
            content += "<td>" + website + "</td>";
            content += "<td>" + City + "</td>";
            content += "<td>" + categories + "</td>";
            content += "<td>" + parseStatus(Status) + "</td>"; // status
            content += "<td>" + ContactPerson + "</td>";
            content += "<td>" + ContactPersonPos + "</td>"; // status
            content += "<td>" + mobile + "<br>" + email + "</td>"; // status
            content += "<td>" + Assigned + "</td>"; // status

            content += "<td>"
                    + "<a href='CustEntMains/Details/" + temp[x]["Id"] + "'> Details </a><br> "
             
                    + "</td>";
            content += "</tr>";
        }
        prevId = temp[x]["Id"]
        $(content).appendTo("#company-Table");
        
    }
}

//load table with ACTIVE suppliers
function StatusRefresh() {
    console.log("status refresh");

    //clear search field
    $('#srch-field').val('');

    //load table content
    ajax_loadContent();
}

function parseStatus(status) {
    switch (status) {
        case 'PRI':
            return '1-PRIORITY';
            break;
        case 'ACC':
            return '2-ACCREDITED';
            break;
        case 'ACP':
            return '3-ACCREDITATION ON PROCESS';
            break;
        case 'BIL':
            return '4-BILLING/TERMS';
            break;
        case 'ACT':
            return '5-ACTIVE';
            break;
        case 'INC':
            return '6-INACTIVE';
            break;
        case 'BAD':
            return '6-BAD ACCOUNT';
            break;
        case 'SUS':
            return '7-SUSPENDED';
            break;
        default:
            return 'NA'
            break;
    }
}

