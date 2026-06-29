// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


document.addEventListener("DOMContentLoaded", function () {

    const password =
        document.getElementById("password");

    const toggle =
        document.getElementById("togglePassword");

    const icon =
        toggle.querySelector("i");

    toggle.addEventListener("click", function () {

        if (password.type === "password") {

            password.type = "text";
            icon.classList.remove("bi-eye");
            icon.classList.add("bi-eye-slash");

        } else {

            password.type = "password";
            icon.classList.remove("bi-eye-slash");
            icon.classList.add("bi-eye");

        }
    });


    var toast =
        new bootstrap.Toast(
            document.getElementById('loginToast'));

    toast.show();
});


const MIN_FONT = 12;
const MAX_FONT = 24;
const DEFAULT_FONT = 16;
const STEP = 2;

let currentSize = DEFAULT_FONT;

function applyFontSize(size) {
    currentSize = Math.min(MAX_FONT, Math.max(MIN_FONT, size));
    document.documentElement.style.fontSize = currentSize + 'px';

    // Optional: disable buttons at limits
    document.getElementById('decrease-font').disabled = currentSize <= MIN_FONT;
    document.getElementById('increase-font').disabled = currentSize >= MAX_FONT;

    // Optional: persist across page loads
    localStorage.setItem('preferredFontSize', currentSize);
}

document.getElementById('increase-font').addEventListener('click', () => {
    applyFontSize(currentSize + STEP);
});

document.getElementById('default-font').addEventListener('click', () => {
    applyFontSize(DEFAULT_FONT);
});

document.getElementById('decrease-font').addEventListener('click', () => {
    applyFontSize(currentSize - STEP);
});

// Restore saved preference on page load
const saved = localStorage.getItem('preferredFontSize');
applyFontSize(saved ? parseInt(saved) : DEFAULT_FONT);

// ===== Tab Switch =====
function setActive(clickedBtn) {
    const buttons = document.querySelectorAll('.nav-btn');
    const views = document.querySelectorAll('.tab-view');

    buttons.forEach((btn, index) => {
        btn.classList.remove('active');
        views[index].classList.add('d-none');
    });

    const activeIndex = Array.from(buttons).indexOf(clickedBtn);
    clickedBtn.classList.add('active');
    views[activeIndex].classList.remove('d-none');
}

function setSubActive(clickedBtn, viewId) {
    document.querySelectorAll('.sub-btn').forEach(btn => btn.classList.remove('active-sub'));
    document.querySelectorAll('.sub-view').forEach(view => view.classList.add('d-none'));

    clickedBtn.classList.add('active-sub');
    document.getElementById(viewId).classList.remove('d-none');
}

// Write your JavaScript code.
jQuery("#carousel").owlCarousel({
    autoplay: true,
    rewind: false, /* use rewind if you don't want loop */
    margin: 20,
    loop: true,

    animateOut: 'fadeOut',
    animateIn: 'fadeIn',

    responsiveClass: true,
    autoHeight: true,
    autoplayTimeout: 7000,
    smartSpeed: 800,
    nav: true,

    responsive: {
        0: {
            items: 2
        },

        600: {
            items: 4
        },

        1024: {
            items: 6
        },

        1366: {
            items: 7
        }
    }
});