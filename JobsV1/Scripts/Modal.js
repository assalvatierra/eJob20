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



