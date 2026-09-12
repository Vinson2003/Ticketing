function applyTheme(theme) {
    document.documentElement.setAttribute(
        'data-bs-theme',
        theme
    );

    localStorage.setItem(
        'theme',
        theme
    );

    const icon = document.getElementById('themeIcon');

    if (!icon) {
        return;
    }

    if (theme === 'dark') {
        icon.className = 'bi bi-sun';
    } else {
        icon.className = 'bi bi-moon-stars';
    }
}

document.addEventListener('DOMContentLoaded', function () {

    const currentTheme = localStorage.getItem('theme') ?? 'dark';

    applyTheme(currentTheme);

    document.getElementById('btnTheme')
        ?.addEventListener('click', function () {

            const current = document.documentElement.getAttribute('data-bs-theme');

            const newTheme = current === 'dark' ? 'light' : 'dark';

            applyTheme(newTheme);
        });
});