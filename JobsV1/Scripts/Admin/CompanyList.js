/**
*   Company List Script
*   Add, Remove, Update Table
*/

//Add Item Expense
function CompanyUserList(companyId, companyName) {

    $("#companyId").text(companyId);
    $("#companyName").text(companyName);

    //build json object
    var data = {
        companyId: parseInt(companyId)
    };

   // console.log(data);

    var url = '/Admin/GetCompanyUsers';

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
            LoadUsersTable(data);
        }
    });
}


//display products
function LoadUsersTable(data) {

    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    var total = 0;

    //console.log(temp);
    //clear table contents except header
    $("#UsersTable").find("tr:gt(0)").remove();

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        //console.log("Load data");
        var id = temp[x]["Id"] != null ? temp[x]["Id"] : "0";
        var Name = temp[x]["Name"] != null ? temp[x]["Name"] : "--";

        //console.log(Name);

        content = "<tr>";
        content += "<td>" + Name + "</td>";
        content += "<td onclick='RemoveCompany(" + id + ")'><a class='cursor-hand'> x </a></td>";
        content += "</tr>";

        $(content).appendTo("#UsersTable");
    }
}

//DELETE ITEM
function RemoveCompany(id) {
    console.log("removeitem");
    var companyId = parseInt($("#companyId").text());
    var companyName = parseInt($("#companyName").text());
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
            CompanyUserList(companyId, companyName);
        }
    });
}


//ADD : add user to company
function AddUser(user) {
    var companyId = parseInt($("#companyId").text());
    var companyName = parseInt($("#companyName").text());

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
            CompanyUserList(companyId, companyName);

            //toggle modal
            $("#UserList").modal('hide');
        }
    });
}

