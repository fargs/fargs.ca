module.exports = {
    purge: {
        enabled: false,
        content: [
            './**/*.html',
            './**/*.razor',
            './**/*.cshtml',
        ],
    },
    darkMode: false, // or 'media' or 'class'
    theme: {
        extend: {},
    },
    variants: {
        extend: {},
    },
    plugins: [
        'tailwindcss/forms',
    ]
}


