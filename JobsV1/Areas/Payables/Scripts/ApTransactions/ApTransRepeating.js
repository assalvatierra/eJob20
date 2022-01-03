
/*
 *  
 *  Repeating Transactions
 * 
 * 
 */ 


function Init_Repeating() {

    //Repeating Payables
    if (sessionStorage.getItem('ApTrans-RepeatingShowed') == null) {
        GetRepeatingPayablesCount();
    }

    //Due Payables
    if ($('#RepeatingPayables-Modal').is(':visible')) {
        //GetDuePayables();
    } else {
        if (!sessionStorage.getItem('ApTrans-DueShowed')) {
            //GetDuePayables();
        }
    }

}

//highlight repeating payables
function RepeatCheck(e) {
    if ($(e).hasClass('active')) {
        $(e).removeClass('active');
    } else {
        $(e).addClass('active');
    }
}

//copy selected repeating 
function RepeatSelected(e) {

    $(e).text("Copying");
    $(e).prop('disabled', true);

    var selectedIds = GetAllSelectedItems();

    console.log(selectedIds.length);

    if (selectedIds.length > 0) {

        $.post('/Payables/ApTransactions/RepeatSelectedPayables', { payableIds: selectedIds }, (response) => {
            console.log(response);

            if (response == "True") {
                $(e).text("Copying Done");
                $(e).prop('disabled', false);

                alert("Copying Repeating Payables Done.  " + selectedIds.length + " items copied.");

                window.location.reload(true);
            } else {
                alert("Unable to copy selected items.");
            }

        });

    } else {
        alert("Please select at least 1 item.");
        $(e).prop('disabled', false);
        $(e).text("Repeat Selected");
    }
}

//cancel repeated payable on modal popup
function CancelRepeat(e, id) {

    $.post('/Payables/ApTransactions/CancelRepeatingTrans', { transId: id }, (response) => {
        if (response) {
            $(e).parent().parent().remove();
        } else {
            alert("Unable to update payables transaction.")
        }
    });
}



function GetRepeatingPayablesCount() {
    $.get("/Payables/ApTransactions/GetRepeatingPayablesCount", null, (response) => {
        console.log(response);

        $("#RepeatingPayables-loading").show();

        if (response > 0) {
            //if system have repeating payables
            GetRepeatingPayables();

            //add to session storage
            //will expire on browser closed
            sessionStorage.setItem('ApTrans-RepeatingShowed', true);
        }

    });
}

//get and check repeating payables
//show modal 
function GetRepeatingPayables() {
    $("#RepeatingPayables-Modal").modal("show");

    $.get("/Payables/ApTransactions/GetRepeatingPayables", null, (response) => {

        $("#RepeatingPayables-loading").hide();

        //remove all rows except header
        $("#RepeatingPayables-table").find("tr:gt(0)").remove();

        var repeatingPayables = response;

        for (var i = 0; i < repeatingPayables.length; i++) {
            let interval = parseInt(repeatingPayables[i]['Interval']);
            let payableId = parseInt(repeatingPayables[i]['Id']);

            let payablesRow = '<tr>'
                + '<td> <input type="checkbox" onclick="CheckRepeatItems(this)" name="check-repeat" value="' + payableId + '"/> </td>'
                + '<td><b>' + repeatingPayables[i]['Name'] + '</b></td>'
                + '<td>' + repeatingPayables[i]['Description'] + '</td>'
                + '<td>' + repeatingPayables[i]['Amount'] + '</td>'
                + '<td>' + interval + ' Days </td>'
                + '<td>' + moment(repeatingPayables[i]['DtDue']).format('MMM DD YYYY') + ' </td>'
                + '<td> &#8594; </td>'
                + '<td>' + moment(repeatingPayables[i]['DtDue']).add(interval, 'days').format('MMM DD YYYY') + '</td>'
                + '<td> <a target="_blank" href="/Payables/ApTransactions/Details/' + payableId + '" onclick="ShowPayableDetails(' + payableId + ')"> Details </a> | '
                + '<a href="#" onclick="CancelRepeat(this,' + payableId + ')"> Cancel Repeat</a>'
                + '</td>'
                + '</tr>';

            $("#RepeatingPayables-table").append(payablesRow);
        }
    });
}

//check all selected repeating payables
function CheckAllRepeatItems() {
    $("#RepeatingPayables-table").find("input[type=checkbox]").map(function () {
        $('input[type="checkbox"]').prop('checked', true);
        CheckRepeatItems(this);
    });
}

//get selected repeating items on modal payables
function GetAllSelectedItems() {
    var checkedArr = $("#RepeatingPayables-table").find("input[type=checkbox]:checked").map(function () {
        return this.value;
    }).get();

    return checkedArr;
}

//select repeating 
function CheckRepeatItems(e) {
    if ($(e).prop('checked')) {
        $(e).parent().parent().addClass('active')
    } else {
        $(e).parent().parent().removeClass('active')
    }
}

