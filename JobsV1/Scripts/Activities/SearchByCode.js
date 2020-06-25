function SearchCodeActivities(code) {
    $.get("/Activities/GetCodeHistoryList", { salesCode: code }, (result) => {
        console.log(result);
        $("#SearchCode-Activities").children().remove();


        for (var i = 0; i < result.length; i++) {
            var remarks = result[i]["Remarks"] ?? "";
            var projectName = result[i]["ProjectName"] == null ? " " : result[i]["ProjectName"] + " / ";

            var activity = "<a href='../CustEntActivities/Index/" + result[i]["CustEntMainId"] + "' class='list-group-item'> "
                + result[i]["Month"] + "/" + result[i]["Day"] + "/" + result[i]["Year"] + " - " + result[i]["SalesCode"] + " - <b> " + projectName
                + result[i]["Name"] + "</b> -  " + result[i]["ActivityType"] + " (" + result[i]["Status"] + ") - " + remarks;
            

            //var activity = "<tr>"
            //   + "<td> " + result[i]["Month"] + "/" + result[i]["Day"] + "/" + result[i]["Year"] +"<td>"
            //    + "<tr>"
            $("#SearchCode-Activities").append(activity);
        }

        $("#SearchCodeModal").modal("show");
    });
}

function SearchCodeSupActivities(code) {
    $.get("/Activities/GetSupCodeHistoryList", { salesCode: code }, (result) => {
        console.log(result);
        $("#SearchCode-Activities").children().remove();


        for (var i = 0; i < result.length; i++) {
            var remarks = result[i]["Remarks"] ?? "";

            var activity = "<a href='../SupplierActivities/Records/" + result[i]["SupplierId"] + "' class='list-group-item'> "
                + result[i]["Month"] + "/" + result[i]["Day"] + "/" + result[i]["Year"] + " - " + result[i]["Code"] + " - <b> "
                + result[i]["Name"] + "</b> -  " + result[i]["Type"] + " - " + remarks;
            $("#SearchCode-Activities").append(activity);
        }

        $("#SearchCodeModal").modal("show");
    });
}