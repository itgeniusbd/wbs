// WBS Site JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Quick Donate Form
    const quickDonateForm = document.getElementById('quickDonateForm');
    if (quickDonateForm) {
        quickDonateForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const frequency = document.getElementById('donationFrequency').value;
            const typeId = document.getElementById('donationType').value;
            const amount = document.getElementById('donationAmount').value;
            
            window.location.href = `/donation?typeId=${typeId}&amount=${amount}&frequency=${frequency}`;
        });
    }

    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function(e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Add animation on scroll
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
            }
        });
    }, observerOptions);

    document.querySelectorAll('.card, .sdg-card').forEach(el => {
        observer.observe(el);
    });
});

// Format currency
function formatCurrency(amount) {
    return '?' + parseFloat(amount).toLocaleString('en-BD');
}

// Copy to clipboard
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function() {
        alert('Copied to clipboard!');
    });
}
