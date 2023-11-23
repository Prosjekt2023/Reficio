//This JS function is for the active hover effect over navigation elements
$(document).ready(function () {
    var currentPath = window.location.pathname; // Get the current URL path

    // Loop through each nav link and compare with the current path
    $('.navbar-nav .nav-link').each(function () {
        var linkPath = $(this).attr('href');

        // Check if the link's href matches the current path
        if (currentPath === linkPath) {
            $(this).addClass('active'); // Add 'active' class to the matching link
        }
    });
});
