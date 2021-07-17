
/*
 *  Transaction Create
 * 
 */

async function GetAccountCreditLimit() {
    var accountId = $('#ArAccountId :selected').val();

    await $.get('/Receivables/ArTransactions/CheckAccountCredit', { id: accountId }, (response) => {

        console.log(response);

        let creditLimit = (parseInt(response['CreditLimit']));
        let creditWarning = (parseInt(response['CreditWarning']));
        let creditOverLimit = creditLimit + (parseInt(response['OverLimitAllowed']));
        let amount_val = parseInt($('input[name="Amount"]').val());

        console.log(amount_val + " >= " + creditLimit);
        console.log(creditOverLimit + " >= " + creditLimit);

        //init
        $('#warning-message').hide();
        $("#submit-btn").prop('disabled', false);

        //credit warning reached
        let cond_creditWarningLimit = amount_val >= creditWarning;
        console.log("check credit warning :" + cond_creditWarningLimit);
        if (cond_creditWarningLimit) {
            var account = $('#ArAccountId :selected').text();

            //display warning message on credit limit
            $('#warning-message').show();
            $('#warning-message-text').text(account + " account credit warning is " + creditWarning);
        }


        //credit limit
        let cond_creditLimit = (amount_val >= creditLimit);
        console.log("check credit limit :" + cond_creditLimit);
        if (cond_creditLimit) {
            var account = $('#ArAccountId :selected').text();
            //disable submit button
            //$("#submit-btn").prop('disabled', true);

            //display warning message on credit limit
            $('#warning-message').show();
            $('#warning-message-text').text(account + " account credit limit is " + creditLimit);
        }

        //credit warning reached
        let cond_creditOverLimit = (amount_val >= creditOverLimit);
        console.log("check credit overlimit :" + cond_creditOverLimit);
        if (cond_creditOverLimit) {
            var account = $('#ArAccountId :selected').text();
            //disable submit button
            //$("#submit-btn").prop('disabled', true);

            //display warning message on credit limit
            $('#warning-message').show();
            $('#warning-message-text').text(account + " account reached the credit over limit: " + creditLimit);
        }

        return creditLimit;
    });
}

$('input[name="Amount"]').on('input', async function () {

    GetAccountCreditLimit();

});

$('input[name="Interval"]').on('input', async function () {

    var interval = $(this).val();
    console.log(interval);

    if (interval > 0) {
        $("#chkbox-IsReapeating").prop('checked', true);
    } else {
        $("#chkbox-IsReapeating").prop('checked', false);
    }

    var bx = $("#chkbox-IsReapeating").prop('checked');
    console.log(bx);


});


$('#chkbox-IsReapeating').click(() => {
    if ($(this).is(':checked')) {
        $("#repeat-Interval").show();
    } else {
        $("#repeat-Interval").hide();
    }
});
