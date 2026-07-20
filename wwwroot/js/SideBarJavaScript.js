document.addEventListener('DOMContentLoaded', function () {

    const sidebar = document.getElementById('sidebar');
    const toggleBtn = document.getElementById('sidebarToggle');
    const mobileToggleBtn = document.getElementById('sidebarToggleMobile');
    const backdrop = document.getElementById('sidebarBackdrop');

    function isMobile() {
        return window.innerWidth < 992;
    }

    // ======================
    // Sidebar Toggle (collapse on desktop, slide-in on mobile)
    // ======================
    if (toggleBtn) {
        toggleBtn.addEventListener('click', () => {
            if (isMobile()) {
                sidebar.classList.toggle('show');
                backdrop.classList.toggle('show');
            } else {
                sidebar.classList.toggle('collapsed');
                // Collapsing the sidebar should also close any open dropdowns
                if (sidebar.classList.contains('collapsed')) {
                    document.querySelectorAll('.sidebar .dd-parent.show')
                        .forEach(el => el.classList.remove('show'));
                }
            }
        });
    }

    if (mobileToggleBtn) {
        mobileToggleBtn.addEventListener('click', () => {
            sidebar.classList.add('show');
            backdrop.classList.add('show');
        });
    }

    if (backdrop) {
        backdrop.addEventListener('click', () => {
            sidebar.classList.remove('show');
            backdrop.classList.remove('show');
        });
    }

    window.addEventListener('resize', () => {
        if (!isMobile()) {
            sidebar.classList.remove('show');
            backdrop.classList.remove('show');
        }
    });

    // ======================
    // Sidebar Dropdown - RECURSIVE, works for unlimited depth (2, 3, 4+ levels)
    // Includes ARIA state (4.1.2 Name, Role, Value) and keyboard support (2.1.1 Keyboard)
    // ======================
    const dropdownToggles = document.querySelectorAll('.sidebar .dropdown-toggle');

    dropdownToggles.forEach(function (toggle) {
        // Static ARIA setup
        toggle.setAttribute('role', 'button');
        toggle.setAttribute('aria-haspopup', 'true');
        toggle.setAttribute('aria-expanded', 'false');

        function toggleDropdown() {
            const li = toggle.closest('.dd-parent');
            const menu = toggle.nextElementSibling;
            if (!li || !menu) return;

            const willOpen = !li.classList.contains('show');

            // Close sibling items at the SAME level (same parent <ul>) only
            const parentUl = li.parentElement;
            parentUl.querySelectorAll(':scope > li.dd-parent').forEach(function (sibling) {
                if (sibling !== li) {
                    sibling.classList.remove('show');
                    sibling.querySelectorAll('.dd-parent.show').forEach(el => el.classList.remove('show'));
                    const sibToggle = sibling.querySelector(':scope > .dropdown-toggle');
                    if (sibToggle) sibToggle.setAttribute('aria-expanded', 'false');
                    sibling.querySelectorAll('.dropdown-toggle').forEach(t => t.setAttribute('aria-expanded', 'false'));
                }
            });

            li.classList.toggle('show', willOpen);
            toggle.setAttribute('aria-expanded', willOpen ? 'true' : 'false');

            if (!willOpen) {
                li.querySelectorAll('.dd-parent.show').forEach(el => el.classList.remove('show'));
                li.querySelectorAll('.dropdown-toggle').forEach(t => t.setAttribute('aria-expanded', 'false'));
            }
        }

        toggle.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            toggleDropdown();
        });

        // Anchors fire click on Enter natively, but NOT on Space — add it explicitly
        toggle.addEventListener('keydown', function (e) {
            if (e.key === ' ' || e.key === 'Spacebar') {
                e.preventDefault();
                toggleDropdown();
            }
            if (e.key === 'Escape') {
                const li = toggle.closest('.dd-parent');
                if (li && li.classList.contains('show')) {
                    li.classList.remove('show');
                    toggle.setAttribute('aria-expanded', 'false');
                    li.querySelectorAll('.dd-parent.show').forEach(el => el.classList.remove('show'));
                    li.querySelectorAll('.dropdown-toggle').forEach(t => t.setAttribute('aria-expanded', 'false'));
                    toggle.focus();
                }
            }
        });
    });

    // Escape anywhere in the sidebar closes all open levels and returns focus sensibly
    sidebar.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            document.querySelectorAll('.sidebar .dd-parent.show').forEach(el => el.classList.remove('show'));
            document.querySelectorAll('.sidebar .dropdown-toggle').forEach(t => t.setAttribute('aria-expanded', 'false'));
        }
    });

    // Clicking outside the sidebar closes all open dropdown levels (desktop, non-collapsed)
    document.addEventListener('click', function (e) {
        if (sidebar && !sidebar.contains(e.target)) {
            document.querySelectorAll('.sidebar .dd-parent.show')
                .forEach(el => el.classList.remove('show'));
        }
    });

});
