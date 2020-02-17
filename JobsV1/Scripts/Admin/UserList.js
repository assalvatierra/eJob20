/**
*   User List Script
*   Add, Remove, Update Table
*/

//GET: list of companies
function UserCompanyList(user) {
    $("#userName").text(user);

    //build json object
    var data = {
        user: user
    };

    //console.log(data);

    var url = '/Admin/GetUserCompanies';

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
            //console.log(data);
            LoadCompaniesTable(data);
        }
    });
}

//POST: Display products
function LoadCompaniesTable(data) {

    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    var total = 0;

    //console.log(temp);
    //clear table contents except header
    $("#CompaniesTable").find("tr:gt(0)").remove();

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        console.log("Load data");
        var id = temp[x]["Id"] != null ? temp[x]["Id"] : "0";
        var Name = temp[x]["Name"] != null ? temp[x]["Name"] : "--";

        //console.log(Name);

        content = "<tr>";
        content += "<td>" + Name + "</td>";
        content += "<td onclick='RemoveCompany(" + id + ")'><a class='cursor-hand'> x </a></td>";
        content += "</tr>";

        $(content).appendTo("#CompaniesTable");
    }
}

//DELETE : user from comapny 
function RemoveCompany(id) {
    console.log("removeitem");
    var user = $("#userName").text();

    //build json object
    var data = {
        id: id
    };

    //console.log(data);

    var url = '/Admin/RemoveUser';

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
            //console.log(data);

            //update list
            UserCompanyList(user);
        }
    });
}

//ADD : add user to company assigned
function AddCompany(companyId) {
    var user = $("#userName").text();
    //build json object
    var data = {
        user: user,
        companyId: companyId
    };

    //console.log(data);

    var url = '/Admin/AddUserToCompany';

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
            //console.log(data);
            // LoadCompaniesTable(data);

            //update list
            UserCompanyList(user);

            //toggle modal
            $("#AddCompanyList").modal('hide');
        }
    });
}