
//global variables
var status = "ALL";
var sort = "NAME";
var srchcategory = "All";
var viewType = "SIMPLE";

//load initial on page ready
$(document).ready(intial());

function intial() {

    $('#ALL').css("color", "black");;
    ajax_loadContent();
}

//update status value on click
//change color of the text
$('#ACT').click(function() {
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

$(".sort-panel .btn-group > .btn").click(function () {
    $(this).addClass("active").siblings().removeClass("active");
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

    //console.log(data);

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
            //console.log(data);
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

    //console.log(temp);

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
        website = temp[x]["Website"] != null ? temp[x]["Website"] : " ";
        contact1 = temp[x]["Contact1"] != null ? temp[x]["Contact1"] : "--";
        contact2 = temp[x]["Contact2"] != null ? temp[x]["Contact2"] : "--";
        mobile = temp[x]["Mobile"] != null ? temp[x]["Mobile"] : "--";
        City = temp[x]["City"] != null ? temp[x]["City"] : "--";
        Assigned = temp[x]["AssignedTo"] != null ? temp[x]["AssignedTo"] : "--";

        var categories = temp[x]["Category"] != null ? temp[x]["Category"] : "--";
        var Status = temp[x]["Status"] != null ? temp[x]["Status"] : "--";
        var email = temp[x]["ContactEmail"] != null ? temp[x]["ContactEmail"] : "--";
        var code = temp[x]["Code"] != null ? temp[x]["Code"] : " ";

        var ContactPersons = temp[x]["ContactName"] != null ? temp[x]["ContactName"] : "--";
        var ContactPosition = temp[x]["ContactPosition"] != null ? temp[x]["ContactPosition"] : "--";
        var ContactMobileEmail = temp[x]["ContactMobileEmail"] != null ? temp[x]["ContactMobileEmail"] : "--";
        var Exclusive = temp[x]["Exclusive"] != null ? temp[x]["Exclusive"] : "PUBLIC";
        var IsAssigned = temp[x]["IsAssigned"] != null ? temp[x]["IsAssigned"] : "--";


        website = "<a href='https://" + website + "' target='_blank' >" + website.substring(0, 15) + "... </a>";
        

        content = "<tr>";

        content += "<td class='table-name-col'><b><a href='/CustEntMains/Details/" + temp[x]["Id"] +"'> " + temp[x]["Name"] + "</a></b></td>";
        content += "<td>" + code + "</td>";
        content += "<td>" + website + "</td>";
        content += "<td>" + City + "</td>";
        content += "<td>" + categories + "</td>";
        content += "<td>" + parseStatus(Status) + "</td>";

            //Contact Person Names
        content += "<td>";
                for (var prods = 0; prods < ContactPersons.length; prods++) {
                    if (typeof ContactPersons[prods] === "undefined") {
                        console.log("something is undefined");
                    } else {
                        var contact = ContactPersons[prods].toString();
                        content += " " + contact+ " <br><br>";
                    }
                }
        content += "</td>";

        //Contact Person Positions
        content += "<td>";

        for (var pos = 0; pos < ContactPosition.length; pos++) {
            if (typeof ContactPosition[pos] === "undefined") {
                console.log("something is undefined");
            } else {
               if (ContactPosition[pos] != null) {
                   var positions = ContactPosition[pos].toString();
                   content += " " + positions + " <br><br>";
               }
                       
            }
        }
        content += "</td>";

         //Contact Person Positions
        content += "<td>";
        for (var contact = 0; contact < ContactMobileEmail.length; contact++) {
            if (typeof ContactMobileEmail[contact] === "undefined") {
                console.log("something is undefined");
            } else {
            var contactdetails = ContactMobileEmail[contact].toString();
                content += " " + contactdetails + " <br>";
            }
        }
        content += "</td>";
        content += "<td>" + Assigned + "</td>";

        //  EXCLUSIVE permissions to DETAILS and HISTORY 
        //  for assigned login and admin only, 
        //  hide for not assigned login 
        if (Exclusive == "EXCLUSIVE") {
            if (IsAssigned == true) {
                content += "<td>"
                content += "<a href='CustEntMains/Details/" + temp[x]["Id"] + "'> Details</a>|";
                content += "<a href='CustEntActivities/Index/" + temp[x]["Id"] + "'> History </a><br> ";
                //content += "<p>" + Exclusive + "</p>";
                content += "</td>";
            } else {
                content += "<td>"
                //content += "<p>" + Exclusive + "</p>";
                content += "</td>";

            }
        }

        //  PUBLIC permissions to HISTORY 
        //  for assigned login and admin only
        //  hide for not assigned login
        if (Exclusive == "PUBLIC") {
            if (IsAssigned == true) {
                content += "<td>"
                content += "<a href='CustEntMains/Details/" + temp[x]["Id"] + "'> Details</a>|";
                content += "<a href='CustEntActivities/Index/" + temp[x]["Id"] + "'> History </a><br> ";
                //content += "<p>" + Exclusive + "</p>";
                content += "</td>";
            } else {
                content += "<td>"
                content += "<a href='CustEntMains/Details/" + temp[x]["Id"] + "'> Details</a>|";
                //content += "<p>" + Exclusive + "</p>";
                content += "</td>";
            }
        }



        content += "</tr>";

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

//On Enter, Search and reload Table
$('#srch-field').on('keypress', function (e) {
    if (e.which === 13) {
        ajax_loadContent(); //Load Table
    }
});