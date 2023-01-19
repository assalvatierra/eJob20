/**
 *  Job Create / Edit
 *  
 */ 

//Change job desription bsed on the cutomer name
$("#ac-custId").change(function () {
    var customer = $("#ac-custId option:selected").text();
    //console.log("customer : " + customer);
    //$("#jobdesc").val(customer);
    getEmail();
    getNumber();
    //getCompany();
});

//get email using ajax post
function getEmail() {
    $.post("/JobOrder/getCustomerEmail",
        {
            id: $("#ac-custId option:selected").val()
        },
        function (data, status) {
            $("#CustContactEmail").val(data);
            //  alert("Data: " + data + "\nStatus: " + status);
        });
}

//get number using ajax post
function getNumber() {
    $.post("/JobOrder/getCustomerNumber",
        {
            id: $("#ac-custId option:selected").val()
        },
        function (data, status) {
            $("#CustContactNumber").val(data);
        });
}

//get company using ajax post
function getCompany() {
    $.post("/JobOrder/getCustomerCompany",
        {
            id: $("#ac-custId option:selected").val()
        },
        function (data, status) {
            console.log(data);
            $('#CompanyId').val(data);
            $('#company-textfield').val($("#CompanyId option:selected").text());
        });
}

function searchAgent() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("SearchBarAgent");
    filter = input.value.toUpperCase();
    ul = document.getElementById("SearchListAgent");
    li = ul.getElementsByTagName("li");

    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}




function SelectAgent(agentId) {

    $('#CustAgentId').val(agentId);
    $('#AgentSearchModal').modal('toggle');
    ajax_getContactDetails(agentId);
    ajax_getCompanyDetails(agentId);
}

//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_getContactDetails(agentId) {
  
    //build json object
    var data = {
        id: agentId
    };

    //request data from server using ajax call
    $.ajax({
        url: '/CustEntMains/getCustomerAccount',
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function (data) {
            LoadContactFields(data);
        }
    });
}


//display simple/limited information 
//of suppliers
function LoadContactFields(data) {
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    $("[name='Name']").val(temp["Name"]);
    $("[name='Email']").val(temp["Email"]);
    $("[name='Contact1']").val(temp["Telephone"]);
    $("[name='Contact2']").val(temp["Mobile"]);
    $("[name='Remarks']").val(temp["SocialMedia"]);
    //$('#ac-position').val(temp["Position"]);
}



function ajax_getCompanyDetails(agentId) {

    //build json object
    var data = {
        id: agentId
    };

    //request data from server using ajax call
    $.ajax({
        url: '/Customers/GetAgentCompanyDetails',
        type: "GET",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function (data) {
            LoadCompanyFields(data);
        }
    });
}


//display simple/limited information 
//of suppliers
function LoadCompanyFields(data) {
    console.log(data);

    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    $("[name='AgentCompany']").val(temp["Company"]);
    $("[name='AgentPosition']").val(temp["Position"]);
    //$('#ac-position').val(temp["Position"]);
}
