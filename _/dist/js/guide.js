/**
* jQuery Cookie plugin
*
* Copyright (c) 2010 Klaus Hartl (stilbuero.de)
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
*
*/

jQuery.cookie = function (key, value, options) {

    // key and at least value given, set cookie...
    if (arguments.length > 1 && String(value) !== "[object Object]") {
        options = jQuery.extend({}, options);

        if (value === null || value === undefined) {
            options.expires = -1;
        }

        if (typeof options.expires === 'number') {
            var days = options.expires, t = options.expires = new Date();
            t.setDate(t.getDate() + days);
        }

        value = String(value);

        return (document.cookie = [
            encodeURIComponent(key), '=',
            options.raw ? value : encodeURIComponent(value),
            options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
            options.path ? '; path=' + options.path : '',
            options.domain ? '; domain=' + options.domain : '',
            options.secure ? '; secure' : ''
        ].join(''));
    }

    // key and possibly options given, get cookie...
    options = value || {};
    var result, decode = options.raw ? function (s) { return s; } : decodeURIComponent;
    return (result = new RegExp('(?:^|; )' + encodeURIComponent(key) + '=([^;]*)').exec(document.cookie)) ? decode(result[1]) : null;
};

$(function(){
    $(".jsPrevNextContainer").append(
        '<a href="javascript:void(0)" class="jsNavigateToPrevTab prev-tab icon-angle-left">Prev</a>\n'+
        '<a href="javascript:void(0)" class="jsNavigateToNextTab next-tab icon-angle-right after">Next</a>'
    );
});

$(function() {    

    var tabs = $('.guide .tab');
    var initialTabId = window.location.hash.replace("_tab", "").replace(/#/, "");
    if (initialTabId.length === 0){
        initialTabId = tabs.first().attr("id");
    }

    var activeTabId = undefined;

    setActiveTab(initialTabId);
    
    function setActiveTab(tabId){

        function showTabWithId(tabId) {
            tabs
                .hide()
                .attr("aria-hidden", "true");

            $("#" + tabId)
                .show()
                .attr("aria-hidden", "false");
        }

        function activateGuideAnchor(tabId) {
            $("ol.guide__steps li a")
                .removeClass("active")
                .attr("aria-selected", "false");
            $("ol.guide__steps li a span.away").remove();

            $("ol.guide__steps li a[href='#"+tabId+"']")
                .addClass("active")
                .attr("aria-selected", "true")
                .append("<span class='away'> (selected)</span>");
        }

        function updatePrevNextAnchors(tabId){
            var tab = tabs.filter("#" + tabId);

            var prevAnchor = $(".jsNavigateToPrevTab");
            var prevTab = tab.prev(".tab");

            var nextAnchor = $(".jsNavigateToNextTab");
            var nextTab = tab.next(".tab");

            updateAnchor(prevAnchor, prevTab);

            updateAnchor(nextAnchor, nextTab);

            function updateAnchor(anchor, tab){
                anchor.hide();
                if (tab.length > 0) {
                    var tabTitle = tab.children(".jsTabTitle").text();
                    anchor.attr("href", "#" + tab.attr("id"));
                    anchor.text(tabTitle);
                    anchor.show();
                }
            }
        }

        showTabWithId(tabId);
        activateGuideAnchor(tabId);
        updatePrevNextAnchors(tabId);
        activeTabId = tabId;
    }

  
    $("ol.guide__steps a, .jsNavigateToPrevTab, .jsNavigateToNextTab").click(function(e) {
        e.preventDefault();
        var tabId = $(this).attr("href").replace(/#/, "");
        setActiveTab(tabId);

        var hash = "#" + tabId + "_tab";
        window.location.hash = hash;
        $.cookie("tab", hash);
    });
    
    $(".jsNavigateToPrevTab, .jsNavigateToNextTab").click(function(e) {
        $("html, body").animate({ scrollTop: 0 }, "slow");
    });


}); 