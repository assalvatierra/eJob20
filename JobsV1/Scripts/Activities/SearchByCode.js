



function SearchCodeActivities(code) {
    $.get("/Activities/GetCodeHistoryList", { salesCode: code }, (result) => {

        $("#SearchCode-Code").text(code);
        console.log(result);

        $("#SearchCode-Activities").children().remove();


        for (var i = 0; i < result.length; i++) {
            var remarks = result[i]["Remarks"] == null ? "" : result[i]["Remarks"] ;
            var projectName = result[i]["ProjectName"] == null ? " " : result[i]["ProjectName"] ;
            var Date = moment( result[i]["Date"] ).format("MMM DD YYYY");

            $("#SearchCode-Company").text(result[i]["Name"]);
            $("#SearchCode-Amount").text( "Amount : " + result[i]["Amount"]);
            $("#SearchCode-Project").text(projectName);

            var activity = "<a href='../CustEntActivities/Index/" + result[i]["CustEntMainId"] + "' class='list-group-item'> "
                + "<div class='blue-circle-history'></div>"
                + Date + " - " 
                + " <span class='label label-default'>" + result[i]["Status"] + "</span> - " + result[i]["ActivityType"]
                + " | <span class='text-muted'> " + remarks + " </span> ";
            
            //var activity = "<a href='../CustEntActivities/Index/" + result[i]["CustEntMainId"] + "' class='list-group-item'> "
            //    + result[i]["Month"] + "/" + result[i]["Day"] + "/" + result[i]["Year"] + " - " + result[i]["SalesCode"] + " - <b> " + projectName
            //    + result[i]["Name"] + "</b> -  " + result[i]["ActivityType"] + " (" + result[i]["Status"] + ") - " + remarks;

            $("#SearchCode-Activities").append(activity);
        }

        $("#SearchCodeModal").modal("show");
    });
}

function SearchCodeSupActivities(code) {
    $.get("/Activities/GetSupCodeHistoryList", { salesCode: code }, (result) => {
        console.log(result);
        $("#SearchCode-Code").text(code);

        $("#SearchCode-Activities").children().remove();

        for (var i = 0; i < result.length; i++) {
            var remarks = result[i]["Remarks"] == null ? "" : result[i]["Remarks"];
            var projectName = result[i]["ProjectName"] == null ? " " : result[i]["ProjectName"];
            var Date = moment(result[i]["Date"]).format("MMM DD YYYY");

            $("#SearchCode-Company").text(result[i]["Name"]);
            $("#SearchCode-Project").text(projectName);
            $("#SearchCode-Amount").text(" ");

            var activity = "<a href='../CustEntActivities/Index/" + result[i]["CustEntMainId"] + "' class='list-group-item'> "
                + "<div class='blue-circle-history'></div>"
                + Date + " - "
                + " <span class='label label-default'>" + result[i]["Status"] + "</span> - " + result[i]["ActivityType"]
                + " | <span class='text-muted'> " + remarks + " </span> "
                + " <div class='Activity-Timeline-Amount'>  Amount: " + result[i]["Amount"] + " </div>";

            //var activity = "<a href='../SupplierActivities/Records/" + result[i]["SupplierId"] + "' class='list-group-item'> "
            //    + result[i]["Month"] + "/" + result[i]["Day"] + "/" + result[i]["Year"] + " - " + result[i]["Code"] + " - <b> "
            //    + result[i]["Name"] + "</b> -  " + result[i]["Type"] + " - " + remarks;

            $("#SearchCode-Activities").append(activity);
        }

        $("#SearchCodeModal").modal("show");
    });
}