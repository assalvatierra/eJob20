
/*  
 *   Availability
 *   InvItems/Availability
 *   7/2/2020
 */ 

//Open modal and display jobs on that selected date and item
function ajax_LoadItemDetails(itemId, itemDesc, date) {

    console.log("getting item details");
    $("#Itemmodal-categorylist").children().remove();
    $("#Itemmodal-DateDesc").text(itemDesc + " - " + moment(date).format("MMMM DD (ddd)"));

    var data = {
        itemId: itemId,
        date: date
    }


    $.get("/InvItems/GetItemDetails", { itemId: itemId, date: date }, (result) => {

        console.log(result);
        for (var i = 0; i < result.length; i++) {
            var company = result[i]["Company"] != null ? " ( " + result[i]["Company"] + " ) " : "";
            var jobId = result[i]["JobId"];

            var jobStatus = GetStatus(result[i]["Status"]);

            var jobService = result[i]["Service"];

            var item = "<a href='/JobOrder/JobServices?JobMainId=" + jobId + "' class='list-group-item'> " +
                " <b>" + result[i]["Customer"] + company + " </b><br> " +
                result[i]["ServiceDesc"] +  " <br>" + 
                jobStatus + " - " +  jobService + " </a>";

            $("#Itemmodal-categorylist").append(item);
        }
    });

    $("#ItemDetailsModal").modal('show');
}

function GetStatus(jobStatus) {
    switch (jobStatus) {
        case 'CONFIRMED':
            return " <span class='label label-success'>" + jobStatus + "</span>";
        case 'INQUIRY':
            return " <span class='label label-primary'>" + jobStatus + "</span>";
        case 'RESERVATION':
            return " <span class='label label-info'>" + jobStatus + "</span>";
        case 'CANCELLED':
            return " <span class='label label-default'>" + jobStatus + "</span>";
        case 'CLOSED':
            return " <span class='label label-success'>" + jobStatus + "</span>";
        default:
            return " <span class='label label-default'>" + jobStatus + "</span>";
    }
}

//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadUnits() {

    //build json object
    var data = {
        search: ""
    };

    //console.log(query);
    //request data from server using ajax call
    $.ajax({
        url: '/JobOrder/getMoreItems',
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");

        },
        error: function (data) {
            // console.log("ERROR");
            console.log(data);
            //console.log(data['responseText']);
            SimpleTable(data);
        }
    });
}

//display simple/limited information
//of inventory items of sort order more
//than 110.
function SimpleTable(data) {
    //console.log("SimpleTable");
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    //clear table contents except header
    $("#moreItemsTable").find("tr:gt(0)").remove();
    //$("#moreItemsTable").remove();

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        var remarks = "";
        if (temp[x]["remarks"] == null) {
            remarks = "";
        } else {
            remarks = temp[x]["remarks"];
        }

        content = "<tr>";
        content += "<td>"
            + " <img src='" + temp[x]["icon"] + "' height='21'> "
            + "<a href='/JobOrder/AddItems/' >"
            + " " + temp[x]["itemCode"] + " - "
            + " " + temp[x]["Name"] + "</a> "
            + " " + remarks + " "
            + "</td>";
        content += "<tr>";

        $(content).appendTo("#moreItemsTable");
    }
}

function hideUnits() {
    $("#moreItemsTable").find("tr:gt(0)").remove();
}