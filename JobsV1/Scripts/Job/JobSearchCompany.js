/**
 *  Job Create / Edit
 *  Job Search Company
 */ 

//Change job desription bsed on the cutomer name

function SearchCompany() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("SearchBarCompany");
    filter = input.value.toUpperCase();
    ul = document.getElementById("SearchListCompany");
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

function setCompanyValue(value) {
    console.log(value);
    $('#CompanyId').val(value);
    $('#company-textfield').val($("#CompanyId option:selected").text());

    //$('#customer-textfield').val($("#CompanyId option:selected").text());
    $('#CompanySearchModal').modal('hide');
}

