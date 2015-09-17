( function() {
    var is_webkit = navigator.userAgent.toLowerCase().indexOf( 'webkit' ) > -1,
        is_opera  = navigator.userAgent.toLowerCase().indexOf( 'opera' )  > -1,
        is_ie     = navigator.userAgent.toLowerCase().indexOf( 'msie' )   > -1;

    if ( ( is_webkit || is_opera || is_ie ) && 'undefined' !== typeof( document.getElementById ) ) {
        var eventMethod = ( window.addEventListener ) ? 'addEventListener' : 'attachEvent';
        window[ eventMethod ]( 'hashchange', function() {
            var element = document.getElementById( location.hash.substring( 1 ) );

            if ( element ) {
                if ( ! /^(?:a|select|input|button|textarea)$/i.test( element.tagName ) )
                    element.tabIndex = -1;

                element.focus();
            }
        }, false );
    }
})();	

document.getElementById('js-primary-nav').insertAdjacentHTML('beforeEnd', '<li class="expanding-search__toggle"><a class="icon-search" id="js-expanding-search__toggle" href="javascript:void(0)"><span class="away">Search</span></a></li>');

function navToggle(){
    document.getElementById('js-expanding-search').classList.toggle('show');
}
document.getElementById('js-expanding-search__toggle').addEventListener('click', navToggle );