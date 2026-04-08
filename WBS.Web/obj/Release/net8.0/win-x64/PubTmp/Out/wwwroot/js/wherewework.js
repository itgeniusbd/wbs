/**
 * Where We Work Page - Interactive Features
 * Enhances the SimpleMaps Bangladesh map with additional functionality
 */

(function() {
    'use strict';
    
    // Initialize when DOM is ready
    document.addEventListener('DOMContentLoaded', function() {
        initializeMapFeatures();
        initializeTableInteractions();
        initializeProgressAnimations();
    });
    
    /**
     * Initialize map-specific features
     */
    function initializeMapFeatures() {
        console.log('Initializing map features...');
        
        // Wait for map to be fully loaded
        var checkMapInterval = setInterval(function() {
            if (window.simplemaps_countrymap && window.simplemaps_countrymap.mapobject) {
                clearInterval(checkMapInterval);
                onMapReady();
            }
        }, 100);
        
        // Timeout after 5 seconds
        setTimeout(function() {
            clearInterval(checkMapInterval);
        }, 5000);
    }
    
    /**
     * Called when map is fully initialized
     */
    function onMapReady() {
        console.log('Map is ready!');
        
        // Add custom event listeners
        var locations = document.querySelectorAll('.sm_location');
        locations.forEach(function(location) {
            location.addEventListener('click', function() {
                var districtName = this.getAttribute('data-name');
                highlightDistrictInTable(districtName);
            });
        });
        
        // Add loading complete class
        var mapContainer = document.getElementById('map');
        if (mapContainer) {
            mapContainer.classList.add('map-loaded');
            console.log('Map container classes updated');
        }
    }
    
    /**
     * Highlight corresponding district row in table
     */
    function highlightDistrictInTable(districtName) {
        // Find the district row
        var rows = document.querySelectorAll('.district-table tbody tr');
        rows.forEach(function(row) {
            var nameCell = row.querySelector('.district-name');
            if (nameCell && nameCell.textContent.trim().includes(districtName)) {
                // Scroll to row
                row.scrollIntoView({ 
                    behavior: 'smooth', 
                    block: 'center' 
                });
                
                // Highlight effect
                row.style.transition = 'background-color 0.3s ease';
                row.style.backgroundColor = '#fff3cd';
                
                // Remove highlight after 2 seconds
                setTimeout(function() {
                    row.style.backgroundColor = '';
                }, 2000);
            }
        });
    }
    
    /**
     * Add interactivity to the district table
     */
    function initializeTableInteractions() {
        var rows = document.querySelectorAll('.district-table tbody tr');
        
        rows.forEach(function(row) {
            row.addEventListener('click', function() {
                // Get district name
                var nameCell = this.querySelector('.district-name');
                if (nameCell) {
                    var districtName = nameCell.textContent.trim().replace(/^\s*\S+\s*/, ''); // Remove icon
                    
                    // Try to highlight on map
                    var locations = document.querySelectorAll('.sm_location');
                    locations.forEach(function(location) {
                        if (location.getAttribute('data-name') === districtName) {
                            // Trigger hover effect
                            var circle = location.querySelector('.sm_location_back');
                            if (circle) {
                                circle.dispatchEvent(new Event('mouseenter'));
                                setTimeout(function() {
                                    circle.dispatchEvent(new Event('mouseleave'));
                                }, 2000);
                            }
                        }
                    });
                }
            });
            
            // Add hover effect hint
            row.style.cursor = 'pointer';
            row.title = 'Click to highlight on map';
        });
    }
    
    /**
     * Animate progress bars with smooth count-up
     */
    function initializeProgressAnimations() {
        // Animate progress bars
        var progressBars = document.querySelectorAll('.progress-bar');
        progressBars.forEach(function(bar) {
            var finalWidth = bar.style.width;
            bar.style.width = '0%';
            
            setTimeout(function() {
                bar.style.width = finalWidth;
            }, 200);
        });
        
        // Animate count badges
        var countBadges = document.querySelectorAll('.count-badge');
        countBadges.forEach(function(badge) {
            var text = badge.textContent;
            var match = text.match(/(\d+)\/(\d+)/);
            
            if (match) {
                var current = parseInt(match[1]);
                var total = parseInt(match[2]);
                animateCounter(badge, 0, current, total, 1000);
            }
        });
    }
    
    /**
     * Animate a counter from start to end
     */
    function animateCounter(element, start, end, total, duration) {
        var range = end - start;
        var increment = range / (duration / 16);
        var currentValue = start;
        
        var timer = setInterval(function() {
            currentValue += increment;
            
            if (currentValue >= end) {
                currentValue = end;
                clearInterval(timer);
            }
            
            element.textContent = Math.floor(currentValue) + '/' + total;
        }, 16);
    }
    
    /**
     * Add print functionality
     */
    window.printMap = function() {
        window.print();
    };
    
    /**
     * Export map statistics
     */
    window.exportMapData = function() {
        if (typeof simplemaps_countrymap_mapdata === 'undefined') {
            console.error('Map data not available');
            return;
        }
        
        var data = {
            divisions: simplemaps_countrymap_mapdata.state_specific,
            locations: simplemaps_countrymap_mapdata.locations,
            timestamp: new Date().toISOString()
        };
        
        var dataStr = JSON.stringify(data, null, 2);
        var dataBlob = new Blob([dataStr], { type: 'application/json' });
        var url = URL.createObjectURL(dataBlob);
        
        var link = document.createElement('a');
        link.href = url;
        link.download = 'wbs-map-data-' + new Date().toISOString().split('T')[0] + '.json';
        link.click();
        
        URL.revokeObjectURL(url);
    };
    
    /**
     * Responsive map adjustments
     */
    function handleResize() {
        var mapContainer = document.getElementById('map');
        if (!mapContainer) return;
        
        var width = mapContainer.offsetWidth;
        
        // Adjust label sizes for mobile
        if (width < 576) {
            mapContainer.classList.add('map-mobile');
        } else {
            mapContainer.classList.remove('map-mobile');
        }
    }
    
    window.addEventListener('resize', debounce(handleResize, 250));
    handleResize(); // Initial check
    
    /**
     * Debounce helper function
     */
    function debounce(func, wait) {
        var timeout;
        return function executedFunction() {
            var context = this;
            var args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(function() {
                func.apply(context, args);
            }, wait);
        };
    }
    
    // Log initialization
    console.log('Where We Work - Interactive features initialized');
})();
