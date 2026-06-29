const sidebar = document.getElementById('sidebar');
const toggleBtn = document.getElementById('sidebarToggle');
const mobileToggleBtn = document.getElementById('sidebarToggleMobile');
const backdrop = document.getElementById('sidebarBackdrop');

function isMobile() { return window.innerWidth < 992; }

toggleBtn.addEventListener('click', () => {
    if (isMobile()) {
        sidebar.classList.toggle('show');
        backdrop.classList.toggle('show');
    } else {
        sidebar.classList.toggle('collapsed');
    }
});

mobileToggleBtn.addEventListener('click', () => {
    sidebar.classList.add('show');
    backdrop.classList.add('show');
});

backdrop.addEventListener('click', () => {
    sidebar.classList.remove('show');
    backdrop.classList.remove('show');
});

window.addEventListener('resize', () => {
    if (!isMobile()) {
        sidebar.classList.remove('show');
        backdrop.classList.remove('show');
    }
});
