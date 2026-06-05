document.addEventListener('DOMContentLoaded', function () {
    var toggle = document.getElementById('themeToggle');
    var icon = document.getElementById('themeIcon');

    function updateIcon() {
        var current = document.documentElement.getAttribute('data-bs-theme');
        icon.className = current === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
    }

    updateIcon();

    toggle.addEventListener('click', function () {
        var next = document.documentElement.getAttribute('data-bs-theme') === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-bs-theme', next);
        localStorage.setItem('theme', next);
        updateIcon();
    });
});
