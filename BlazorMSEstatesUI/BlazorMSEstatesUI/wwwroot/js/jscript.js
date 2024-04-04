if ($(window).width() < 992) {
    $('#navigation .dropdown-toggle').on('click', function () {
        $(this).siblings('.dropdown-menu').animate({
            height: 'toggle'
        }, 300);
    });
}

function navbarToggler() {
    document.addEventListener('click', function (event) {
        if (document.querySelector('.navbar-toggler').className === 'navbar-toggler') {
            document.querySelector('.navbar-toggler').click();
        }
    });
}

function scrollToTop() {
    window.scrollTo(0, 0);
}

function counter() {
    var oTop;
    if ($('.counter').length !== 0) {
        oTop = $('.counter').offset().top - window.innerHeight;
    }
    if ($(window).scrollTop() > oTop) {
        $('.counter').each(function () {
            var $this = $(this),
                countTo = $this.attr('data-count');
            $({
                countNum: $this.text()
            }).animate({
                countNum: countTo
            }, {
                duration: 1000,
                easing: 'swing',
                step: function () {
                    $this.text(Math.floor(this.countNum));
                },
                complete: function () {
                    $this.text(this.countNum);
                }
            });
        });
    }
}

//navbar add class
$(window).scroll(function () {
    counter();

    var scroll = $(window).scrollTop();
    if (scroll > 50) {
        $('.navigation').addClass('sticky-header');
    } else {
        $('.navigation').removeClass('sticky-header');
    }
});
