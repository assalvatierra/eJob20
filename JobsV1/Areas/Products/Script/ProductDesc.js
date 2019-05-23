
/**
  * ProductDesc.js
  * Perform Add, Edit and Remove functions 
  * using modal. Used with Partial Views in 
  * details.
  */


//CREATE
//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_PostDesc(prodId) {
    var prodDesc = $("#prod-desc").val();
    var sort = $("#prod-sort").val();

    // build json object
    var data = {
        prodId: prodId,
        sort: sort,
        text: prodDesc
    };

    var url = '/Products/SMProducts/AddDesc';

    // request data from server using ajax call
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
            // console.log(data);
            //location.reload();
            addDescToDisplay(prodDesc);
        }
    });
}

//EDIT Desciption- 
//load table content on search btn click
//request data from server using ajax call
//then update product description
function ajax_EditPostDesc(prodId) {
    var descId = $("#prod-Id").val();
    var prodDesc = $("#prod-desc").val();
    var sort = $("#prod-sort").val();

    // build json object
    var data = {
        prodId: prodId,
        descId: descId,
        sort: sort,
        text: prodDesc
    };

    var url = '/Products/SMProducts/EditDesc';

    // request data from server using ajax call
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
            location.reload();
        }
    });
}


function AddDescModal() {
    //hide button
    $("#addDescBtn").css('display', 'block');
    $("#editDescBtn").css('display', 'none');
}

function EditDescModal(descId) {
    //hide button
    $("#addDescBtn").css('display', 'none');
    $("#editDescBtn").css('display', 'block');

    ajax_getDesc(descId);
}

//get description content
function ajax_getDesc(descId) {
    var prodDesc = $("#prod-desc").val();
    var sort = $("#prod-sort").val();

    // build json object
    var data = {
        Id: descId,
    };

    var url = '/Products/SmProducts/getDesc';

    // request data from server using ajax call
    $.ajax({
        url: url,
        type: "GET",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log("SUCCESS");
        },
        error: function (data) {
            console.log("ERROR");
            console.log(data);
            DescLoadTable(data);
            //location.reload();
        }
    });
}

//display simple/limited information 
//of suppliers
function DescLoadTable(data) {
    console.log("SimpleTable");

    //parse data response to json object
    var data = jQuery.parseJSON(data["responseText"]);

    console.log("Description: " + data["Desc"]);
    $("#prod-Id").val(data["Id"]);
    $("#prod-desc").val(data["Desc"]);
    $("#prod-sort").val(data["Sort"]);

}


function refreshPage() {
    location.reload();
}


//Show hide edit | delete of description
$(document).ready(function () {
    $("p.prod-desc").hover(
      function () { //on hover
          //console.log("hover");
          //$(this).siblings(".prod-desc-act").show();
          $(this).children(".prod-desc-act").show();
      },
      function () { //on leave
          //console.log("leave");
          //$(this).siblings(".prod-desc-act").hide();
          $(this).children(".prod-desc-act").hide();
      }
    );
});



function addDescToDisplay(desc) {
    content = "<p>";
    content += desc;
    content += "</p>";
    $(content).appendTo("#section-desc");

}

