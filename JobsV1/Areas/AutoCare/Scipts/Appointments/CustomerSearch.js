/**
 *  Job Create / Edit
 *  
 */ 

//Change job desription bsed on the cutomer name
$("#customerList").change(function () {
    var customer = $("#customerList option:selected").text();
    //console.log("customer : " + customer);
    //$("#jobdesc").val(customer);
    getEmail();
    getNumber();
    getCompany();
});

//get email using ajax post
function getEmail() {
    $.post("/JobOrder/getCustomerEmail",
        {
            id: $("#customerList option:selected").val()
        },
        function (data, status) {
            $("#CustContactEmail").val(data);
            //  alert("Data: " + data + "\nStatus: " + status);
        });
}

//get number using ajax post
function getNumber(id) {
   $.post("/JobOrder/getCustomerNumber",
        {
            id: id
        },
        function (data, status) {
            $("#CustContactNumber").val(data);
       });
}

//get company using ajax post
function getCompany() {
    $.post("/JobOrder/getCustomerCompany",
        {
            id: $("#customerList option:selected").val()
        },
        function (data, status) {
            console.log(data);
            $('#CompanyId').val(data);
            $('#company-textfield').val($("#CompanyId option:selected").text());
        });
}


function searchCustomer() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("SearchBarCustomer");
    filter = input.value.toUpperCase();
    ul = document.getElementById("SearchListCustomer");
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

function setValue(value, name) {
    $('#customerList').val(value);
    $('#Customer').val(name);
    $('#CustomersModal').modal('toggle');
    $('#CustCode').val(padDigits(value, 4))
    //console.log(name);
    console.log("cust code: " + padDigits(value, 4))
    getNumber(value);


}

function padDigits(number, digits) {
    return Array(Math.max(digits - String(number).length + 1, 0)).join(0) + number;
}
