// Language Switcher functionality
document.addEventListener('DOMContentLoaded', function() {
    // Smooth scroll to top on language change
    const languageLinks = document.querySelectorAll('.language-switcher a');
    
    languageLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            // Show loading indicator
            const btnText = this.innerHTML;
            this.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
            this.style.pointerEvents = 'none';
            
            // Optional: Add smooth transition effect
            document.body.style.opacity = '0.7';
            document.body.style.transition = 'opacity 0.3s';
        });
    });

    // Set language preference in localStorage
    const currentLang = document.documentElement.lang || 'en';
    localStorage.setItem('preferredLanguage', currentLang);
});

// Helper function to get current language
function getCurrentLanguage() {
    return document.documentElement.lang || 'en';
}

// Helper function to translate common texts
const translations = {
    en: {
        loading: 'Loading...',
        error: 'Error',
        success: 'Success',
        confirm: 'Are you sure?',
        yes: 'Yes',
        no: 'No',
        cancel: 'Cancel',
        save: 'Save',
        delete: 'Delete',
        edit: 'Edit'
    },
    bn: {
        loading: '??? ?????...',
        error: '??????',
        success: '???',
        confirm: '???? ?? ????????',
        yes: '?????',
        no: '??',
        cancel: '?????',
        save: '???????',
        delete: '?????',
        edit: '????????'
    }
};

function t(key) {
    const lang = getCurrentLanguage();
    return translations[lang][key] || key;
}
