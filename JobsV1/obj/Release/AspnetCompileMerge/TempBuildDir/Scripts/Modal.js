/***************
* Modal Scripts
*
****************/

    /**
    *   Handles footer. Show Footer only when device is
    *   mobile or screen width less than 600 pixels.
    */
    function toggleScreen() {
        var footer = document.getElementById("mobile-footer");
        var screenwidth = document.documentElement.clientWidth;

        if(footer != null){
            if (screenwidth < 600) {
                footer.style.display = "block";
            }
            else {
                footer.style.display = "none";
            }
        }
    }

    $(document).ready(function () {

        // Click event for any anchor tag that's href starts with #
        $('a[href^="#"]').click(function (event) {

            // The id of the section we want to go to.
            var id = $(this).attr("href");

            // An offset to push the content down from the top.
            var offset = 60;

            // Our scroll target : the top position of the
            // section that has the id referenced by our href.
            var target = $(id).offset().top - offset;

            // The magic...smooth scrollin' goodness.
            $('html, body').animate({ scrollTop: target }, 500);

            //prevent the page from jumping down to our section.
            event.preventDefault();
        });
    });

function HighlightSelectedCar(carId) {
    $("#" + carId).children('.car-container-details').addClass("Selected-Active");
    $("#" + carId).siblings().children('.car-container-details').removeClass("Selected-Active");

    if (carId == "CarUnit-1") {
        $("#CarUnit-2").children('.car-container-details').addClass("Selected-Active");

        $("#CarUnit-7").children('.car-container-details').addClass("Selected-Active");
    }

    if (carId == "CarUnit-4") {
        $("#CarUnit-8").children('.car-container-details').addClass("Selected-Active");
    }
}



