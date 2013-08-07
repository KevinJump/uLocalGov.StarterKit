/* basic mega menu behaviour */

$(function () {

    function addMega() {
        $(this).find(".menu-drop").stop().fadeTo(75, 1).show();
    }

    function removeMega() {
        $(this).find(".menu-drop").stop().fadeTo(75, 0, function () {
            $(this).hide();
        });
    }

    var megaConfig = {
        interval: 100,
        sensitivity: 1.25,
        over: addMega,
        timeout: 500,
        out: removeMega
    }

    $("li.mega-item").hoverIntent(megaConfig);
});