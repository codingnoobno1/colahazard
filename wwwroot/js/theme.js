window.theme = {
    get: () => localStorage.getItem('theme'),
    set: (theme) => {
        localStorage.setItem('theme', theme);
        if (theme === 'dark') {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
    },
    toggle: () => {
        const current = localStorage.getItem('theme');
        const next = current === 'dark' ? 'light' : 'dark';
        window.theme.set(next);
        return next;
    },
    init: () => {
        // Check local storage or system preference
        const stored = localStorage.getItem('theme');
        if (stored === 'dark' || (!stored && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
            // However, user requested "bright by default", so we might prioritize light if no storage.
            // But good UX usually respects system pref. 
            // Let's stick to explicit toggle or default light if they want "bright by default".
            // Re-reading user request: "bright by default then shitable to dark mode button"
            // So if nothing stored, default to light.
            if (stored === 'dark') {
                document.documentElement.classList.add('dark');
            }
        } else {
            // ensure light
            document.documentElement.classList.remove('dark');
        }
    }
};

// Initialize on load
window.theme.init();
