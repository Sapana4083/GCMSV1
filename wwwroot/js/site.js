document.addEventListener("DOMContentLoaded", function () {

    const password =
        document.getElementById("password");

    const toggle =
        document.getElementById("togglePassword");

    if (password && toggle) {

        const icon =
            toggle.querySelector("i");

        toggle.addEventListener("click", function () {

            if (password.type === "password") {

                password.type = "text";

                icon.classList.remove("bi-eye");

                icon.classList.add("bi-eye-slash");

            }
            else {

                password.type = "password";

                icon.classList.remove("bi-eye-slash");

                icon.classList.add("bi-eye");

            }

        });

    }

    const toastElement =
        document.getElementById("loginToast");

    if (toastElement) {

        var toast =
            new bootstrap.Toast(toastElement);

        toast.show();

    }

});

const MIN_FONT = 12;
const MAX_FONT = 24;
const DEFAULT_FONT = 16;
const STEP = 2;

let currentSize = DEFAULT_FONT;
const increaseBtn = document.getElementById('increase-font');
const defaultBtn = document.getElementById('default-font');
const decreaseBtn = document.getElementById('decrease-font');

if (increaseBtn) {
    increaseBtn.addEventListener('click', () => {
        applyFontSize(currentSize + STEP);
    });
}

if (defaultBtn) {
    defaultBtn.addEventListener('click', () => {
        applyFontSize(DEFAULT_FONT);
    });
}

if (decreaseBtn) {
    decreaseBtn.addEventListener('click', () => {
        applyFontSize(currentSize - STEP);
    });
}
function applyFontSize(size) {

    currentSize = Math.min(
        MAX_FONT,
        Math.max(MIN_FONT, size));

    document.documentElement.style.fontSize =
        currentSize + 'px';

    const decreaseBtn =
        document.getElementById('decrease-font');

    const increaseBtn =
        document.getElementById('increase-font');

    if (decreaseBtn)
        decreaseBtn.disabled =
            currentSize <= MIN_FONT;

    if (increaseBtn)
        increaseBtn.disabled =
            currentSize >= MAX_FONT;

    localStorage.setItem(
        'preferredFontSize',
        currentSize);
}

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


