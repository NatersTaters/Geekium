// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.carousel').carousel({
    interval: 5000 //Speed of navigating the carousel images
})

// Filter the Search bar in the FAQ page

function filterListing() {
    // Declare variables
    var input, filter, ul, li, a, i, txtValue;
    input = document.getElementById('searchFAQ');
    filter = input.value.toUpperCase();
    ul = document.getElementById("listFAQ");
    li = ul.getElementsByTagName('li');

    // Loop through and filter list to what is typed in search bar
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}
