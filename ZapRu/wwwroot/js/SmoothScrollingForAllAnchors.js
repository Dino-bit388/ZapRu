document.addEventListener('click', function (e) {
    const anchor = e.target.closest('a[href^="#"]');
    if (anchor) {
        e.preventDefault();

        const targetId = anchor.getAttribute('href');
        const targetElement = document.querySelector(targetId);

        if (targetElement) {
            targetElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
            history.pushState(null, null, targetId);
        }
    }
});
