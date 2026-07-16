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
    // ======================
    document.querySelectorAll('.sidebar .dropdown-toggle').forEach(function (toggle) {
        toggle.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            const li = this.closest('.dd-parent');      // the <li> that owns this toggle
            const menu = this.nextElementSibling;         // its direct <ul class="dropdown-menu">
            if (!li || !menu) return;

            const willOpen = !li.classList.contains('show');

            // Close sibling items at the SAME level (same parent <ul>) only
            const parentUl = li.parentElement;
            parentUl.querySelectorAll(':scope > li.dd-parent').forEach(function (sibling) {
                if (sibling !== li) {
                    sibling.classList.remove('show');
                    // collapse any open descendants inside that sibling too
                    sibling.querySelectorAll('.dd-parent.show').forEach(el => el.classList.remove('show'));
                }
            });

            // Toggle current item
            li.classList.toggle('show', willOpen);

            // If we just closed it, collapse any open descendants too
            if (!willOpen) {
                li.querySelectorAll('.dd-parent.show').forEach(el => el.classList.remove('show'));
            }
        });
    });

    // Clicking outside the sidebar closes all open dropdown levels (desktop, non-collapsed)
    document.addEventListener('click', function (e) {
        if (sidebar && !sidebar.contains(e.target)) {
            document.querySelectorAll('.sidebar .dd-parent.show')
                .forEach(el => el.classList.remove('show'));
        }
    });

});
