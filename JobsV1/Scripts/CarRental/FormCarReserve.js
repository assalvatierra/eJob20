

/******** Car Reserve ************/
//redirect to Packages selection form
function CarReserve_FormPackages(controller,carId, noDays, rentalType, meals, fuel) {
    window.location.href = '/' + controller + '/FormPackages?carId=' + carId + '&days=' + noDays +
        '&rentType=' + rentalType + '&meals=' + meals + '&fuel=' + fuel;
}

//clear border highlight on car selection
function CarBorderReset() {
    $('#car1').css('border', '1px solid lightgray');
    $('#car2').css('border', '1px solid lightgray');
    $('#car3').css('border', '1px solid lightgray');
    $('#car4').css('border', '1px solid lightgray');
    $('#car5').css('border', '1px solid lightgray');
    $('#car6').css('border', '1px solid lightgray');
    $('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');
}

//selfdrive: redirect to self drive reservation form
function selfDrive(id){
   window.location.href = '/CarRental/Reservation?unitid='+id ;
}

//set default reservation
function CarReserve_Default(carid, days, meals, fuelId) {

    $('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');
    $('#withdriver').attr('checked', true);
    $("#" + carid ).attr('checked', true);
    $('#rsv-days').val(days);

    //clear car border highlight
    CarBorderReset();

    //meals and accomodation
    // 1 = included in the package
    // 0 = by renter

    //fuel inclusion
    // 1 = included in the package
    // 0 = by renter
    checkbox()

    //update reservation text summary
    updateTransaction();

}

    //listener on rental type click
    //values: with driver, self drive, longterm
    $('#btn-rentalType').click(function () {

        //update reservation text summary
        rentalReset();
        rentalTypeSelection();

        //initial value
        $('#3').attr('checked', true);

        updateTransaction();

    });

    //handle changes when selecting rental types
    function rentalTypeSelection() {

        if ($('input:radio[name=options]:checked').val() == "Self Drive") {
            //Long Term rental Type
            var selectedCar = $('input:radio[name=cars]:checked').attr("id");
            //change selection when van is selectedCar
            if (selectedCar == 1 || selectedCar == 2) {
                $('#3').attr('checked', true);
            }
            //highlight rental type button
            $('#rental-type-sdrive').addClass('active').siblings().removeClass('active');

            CarBorderReset();
            updateTransaction();
            $('.isSelfDrive1').hide();

        } else if ($('input:radio[name=options]:checked').val() == "longterm") {
            //Long Term rental Type
            //set car3 as default
            $('#3').attr('checked', true);

            //highlight rental type button
            $('#rental-type-longterm').addClass('active').siblings().removeClass('active');

            CarBorderReset();
            updateTransaction();
            $('.isSelfDrive1').hide();

        } else {
            //With Driver rental Type
            $('#1').attr('checked', true);

            //highlight rental type button
            $('#rental-type-wdriver').addClass('active').siblings().removeClass('active');

            CarBorderReset();
            updateTransaction();
            $('.isSelfDrive1').show();
        }
    }

    //meal selection on click
    $('#rsv-meal').click(function () {
        updateTransaction();
    });
    //fuel selection on click
    $('#rsv-fuel').click(function () {
        updateTransaction();
    });

    //carSelection on click
    $('#btn-rentalUnit').click(function () {
        updateTransaction();

        $('#car' + +$('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue').siblings().css('border', '1px solid lightgray');
    });

    //update reservation text details
    function updateTransaction() {
        var mealPackage = $('#rsv-meal-package').is(":checked") ? 'Driver Meals/Accomodation Included ' : 'Driver Meals/Accomodation by renter';
        var fuelPackage = $('#rsv-fuel-package').is(":checked") ? 'Driver Fuel Included ' : 'Driver Fuel by renter';

        //display text on reservation summary
        $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val() +
        ' - ' + $('#rsv-days').val() + ' Days - ' + mealPackage + ' - ' + fuelPackage);

    }

    //reset text reservation summary
    function rentalReset() {
        $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - Please select a vehicle');
    }

    //handle change of days
    function dayschange() {
        if ($('#rsv-days').val() < 1) {
            $('#rsv-days').val(1);
        }

        updateTransaction();
    }

    //handle the count of number of days  on addition
    $('#days-add').click(function () {
        $('#rsv-days').val(+$('#rsv-days').val() + 1);
        updateTransaction();
    });

    //handle no of days limit and update text on subtraction
    $('#days-sub').click(function () {
        if ($('#rsv-days').val() > 1) {
            $('#rsv-days').val(+$('#rsv-days').val() - 1);
            updateTransaction();
        }
    });

    function LoadOverlay(){
        $("#overlay").css("display","flex");
    }


    // the selector will match all input controls of type :checkbox
    // and attach a click event handler
    $("input:checkbox").on('click', function () {
        // in the handler, 'this' refers to the box clicked on
        var $box = $(this);

        if ($box.is(":checked")) {
            // the name of the box is retrieved using the .attr() method
            // as it is assumed and expected to be immutable

            var group = "input:checkbox[name='" + $box.attr("name") + "']";
            // the checked state of the group/box on the other hand will change
            // and the current value is retrieved using .prop() method

            $(group).prop("checked", false);
            $box.prop("checked", true);

            //console.log($box.attr("name") + ": " + $(this).val() );

            switch ($box.attr("name")) {
                case "meals":
                    mealsSelected = $(this).val();
                    break;
                case "fuel":
                    fuelSelected = $(this).val();
                    break;
                default:
                    break;
            }

            //console.log("meals: " + mealsSelected);
            //console.log("fuel: " + fuelSelected);
        } else {
            $box.prop("checked", false);
        }

        updateTransaction();
    });
