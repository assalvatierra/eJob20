
/*
 * Filter Numbers only input 
 */
$(function () {
    $("input[type='number']").keydown(function (event) {

        if (event.shiftKey == true) {
            event.preventDefault();
        }

        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190 || event.keyCode == 110) {

        } else {
            event.preventDefault();
        }

        if ($(this).val().indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110))
            event.preventDefault();

    });
});


/*
 * Dropdown Search
 */

//For Initializing search in dropdowns
//Add class dropdown-search in dropdown inputs
function InitDropdownSearch() {
    $('.dropdown-search').select2();
}