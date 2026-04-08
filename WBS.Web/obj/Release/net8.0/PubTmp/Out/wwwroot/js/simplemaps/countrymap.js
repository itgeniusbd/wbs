// SimpleMaps Bangladesh Country Map Implementation
// Compatible with SimpleMaps data structure

(function() {
    'use strict';
    
    var simplemaps_countrymap = {
        mapdata: null,
        mapobject: null,
        mapdiv: null,
        
        load: function() {
            if (typeof simplemaps_countrymap_mapdata === 'undefined') {
                console.error('SimpleMaps: Map data not found');
                return;
            }
            
            this.mapdata = simplemaps_countrymap_mapdata;
            var divId = this.mapdata.main_settings.div || 'map';
            this.mapdiv = document.getElementById(divId);
            
            if (!this.mapdiv) {
                console.error('SimpleMaps: Map container not found');
                return;
            }
            
            this.init();
        },
        
        init: function() {
            // Create SVG container
            var settings = this.mapdata.main_settings;
            var svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
            svg.setAttribute('viewBox', '0 0 1000 1200');
            svg.setAttribute('style', 'width: 100%; height: 100%;');
            
            // Add definitions for gradients and patterns
            var defs = document.createElementNS('http://www.w3.org/2000/svg', 'defs');
            
            // Add gradient for background
            var gradient = document.createElementNS('http://www.w3.org/2000/svg', 'linearGradient');
            gradient.setAttribute('id', 'mapBg');
            gradient.setAttribute('x1', '0%');
            gradient.setAttribute('y1', '0%');
            gradient.setAttribute('x2', '0%');
            gradient.setAttribute('y2', '100%');
            
            var stop1 = document.createElementNS('http://www.w3.org/2000/svg', 'stop');
            stop1.setAttribute('offset', '0%');
            stop1.setAttribute('style', 'stop-color:#e8f4f8;stop-opacity:1');
            
            var stop2 = document.createElementNS('http://www.w3.org/2000/svg', 'stop');
            stop2.setAttribute('offset', '100%');
            stop2.setAttribute('style', 'stop-color:#d1e9f0;stop-opacity:1');
            
            gradient.appendChild(stop1);
            gradient.appendChild(stop2);
            defs.appendChild(gradient);
            svg.appendChild(defs);
            
            // Add background
            var bg = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            bg.setAttribute('width', '1000');
            bg.setAttribute('height', '1200');
            bg.setAttribute('fill', settings.background_transparent === 'yes' ? 'transparent' : settings.background_color);
            svg.appendChild(bg);
            
            // Draw realistic Bangladesh country outline
            this.drawBangladeshMap(svg);
            
            // Draw divisions (states)
            this.drawDivisions(svg);
            
            // Draw locations (district markers)
            this.drawLocations(svg);
            
            // Add zoom controls
            if (settings.zoom === 'yes' && settings.manual_zoom === 'yes') {
                this.addZoomControls();
            }
            
            // Clear and append
            this.mapdiv.innerHTML = '';
            this.mapdiv.appendChild(svg);
            
            // Store reference
            this.mapobject = svg;
            
            console.log('SimpleMaps Bangladesh Map initialized successfully');
        },
        
        drawBangladeshMap: function(svg) {
            // More realistic Bangladesh country outline with curves
            // This approximates the actual shape of Bangladesh
            var countryPath = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            
            // Detailed Bangladesh outline (approximated)
            var d = 'M 400,50 ' + // Start from top (near Panchagarh)
                    // Northeast - Sylhet region
                    'L 450,80 Q 480,90 500,110 L 550,140 Q 570,160 580,190 L 600,230 ' +
                    // East - Chittagong region (bulge)
                    'Q 620,260 640,300 L 650,350 Q 655,390 650,430 L 640,480 ' +
                    'Q 635,520 620,560 L 600,600 ' +
                    // Southeast - Cox's Bazar area (narrow southern part)
                    'Q 590,630 580,660 L 570,700 Q 565,730 555,760 L 545,800 ' +
                    'Q 540,820 530,840 L 520,860 ' +
                    // South - Chittagong coast
                    'Q 500,880 480,890 L 450,900 ' +
                    // Southwest - Sundarbans delta region
                    'Q 420,905 390,900 L 350,890 Q 320,885 290,880 ' +
                    'L 250,870 Q 220,865 190,855 L 150,840 ' +
                    // West - Khulna region
                    'Q 130,830 120,810 L 110,780 Q 105,750 100,720 ' +
                    // Northwest going up
                    'L 95,680 Q 92,650 95,620 L 100,580 ' +
                    'Q 105,550 115,520 L 130,480 ' +
                    // West side - Rajshahi region
                    'Q 140,450 155,420 L 175,380 Q 190,350 210,320 ' +
                    'L 235,280 Q 255,250 275,220 ' +
                    // Northwest - Rangpur region (top bulge on west)
                    'L 300,180 Q 320,150 340,130 L 365,100 Q 385,70 400,50 Z';
            
            countryPath.setAttribute('d', d);
            countryPath.setAttribute('fill', 'url(#mapBg)');
            countryPath.setAttribute('stroke', '#0077b6');
            countryPath.setAttribute('stroke-width', '3');
            countryPath.setAttribute('class', 'country-outline');
            countryPath.style.filter = 'drop-shadow(0 4px 8px rgba(0,0,0,0.15))';
            svg.appendChild(countryPath);
            
            // Add subtle inner shadow effect
            var innerPath = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            innerPath.setAttribute('d', d);
            innerPath.setAttribute('fill', 'none');
            innerPath.setAttribute('stroke', '#ffffff');
            innerPath.setAttribute('stroke-width', '1.5');
            innerPath.setAttribute('opacity', '0.4');
            svg.appendChild(innerPath);
        },
        
        drawDivisions: function(svg) {
            // Draw division boundaries (8 divisions of Bangladesh)
            // These are approximate positions and shapes
            var divisions = [
                // Dhaka Division (center)
                {
                    id: 'BDC', 
                    name: 'Dhaka',
                    path: 'M 300,350 L 400,320 L 480,380 L 470,480 L 380,520 L 280,480 L 270,420 Z',
                    labelX: 380, 
                    labelY: 420
                },
                // Chittagong Division (southeast)
                {
                    id: 'BDB', 
                    name: 'Chittagong',
                    path: 'M 480,380 L 580,340 L 640,430 L 620,560 L 555,760 L 480,700 L 470,580 L 470,480 Z',
                    labelX: 540, 
                    labelY: 550
                },
                // Sylhet Division (northeast)
                {
                    id: 'BDG', 
                    name: 'Sylhet',
                    path: 'M 400,80 L 500,110 L 580,190 L 580,280 L 500,300 L 400,280 L 360,200 Z',
                    labelX: 470, 
                    labelY: 210
                },
                // Rajshahi Division (northwest-west)
                {
                    id: 'BDE', 
                    name: 'Rajshahi',
                    path: 'M 175,380 L 270,350 L 300,420 L 280,480 L 200,520 L 130,480 Z',
                    labelX: 230, 
                    labelY: 440
                },
                // Rangpur Division (north)
                {
                    id: 'BDF', 
                    name: 'Rangpur',
                    path: 'M 275,130 L 400,80 L 480,140 L 450,220 L 360,280 L 270,250 L 235,180 Z',
                    labelX: 360, 
                    labelY: 200
                },
                // Khulna Division (southwest)
                {
                    id: 'BDD', 
                    name: 'Khulna',
                    path: 'M 150,600 L 270,580 L 320,650 L 310,750 L 250,820 L 150,840 L 110,780 Z',
                    labelX: 230, 
                    labelY: 720
                },
                // Barisal Division (south-center)
                {
                    id: 'BDA', 
                    name: 'Barisal',
                    path: 'M 280,520 L 380,520 L 420,600 L 400,720 L 320,780 L 250,770 L 270,650 L 270,580 Z',
                    labelX: 340, 
                    labelY: 650
                },
                // Mymensingh Division (north-center)
                {
                    id: 'BDH', 
                    name: 'Mymensingh',
                    path: 'M 270,250 L 360,280 L 400,320 L 370,380 L 270,420 L 220,380 L 200,320 Z',
                    labelX: 300, 
                    labelY: 330
                }
            ];
            
            var settings = this.mapdata.main_settings;
            var stateSpecific = this.mapdata.state_specific;
            var self = this;
            
            divisions.forEach(function(div) {
                var path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
                path.setAttribute('d', div.path);
                path.setAttribute('fill', settings.state_color);
                path.setAttribute('fill-opacity', '0.25');
                path.setAttribute('stroke', settings.border_color);
                path.setAttribute('stroke-width', settings.border_size);
                path.setAttribute('class', 'sm_state_back');
                path.setAttribute('data-id', div.id);
                path.style.transition = 'all 0.3s ease';
                
                // Add hover effect
                path.addEventListener('mouseenter', function() {
                    this.setAttribute('fill', settings.state_hover_color);
                    this.setAttribute('fill-opacity', '0.5');
                    
                    // Show division info
                    var stateInfo = stateSpecific[div.id];
                    if (stateInfo) {
                        self.showPopup(stateInfo.name, stateInfo.description || '', div.labelX, div.labelY);
                    }
                });
                
                path.addEventListener('mouseleave', function() {
                    this.setAttribute('fill', settings.state_color);
                    this.setAttribute('fill-opacity', '0.25');
                    self.hidePopup();
                });
                
                svg.appendChild(path);
                
                // Add label
                if (settings.hide_labels !== 'yes') {
                    var text = document.createElementNS('http://www.w3.org/2000/svg', 'text');
                    text.setAttribute('x', div.labelX);
                    text.setAttribute('y', div.labelY);
                    text.setAttribute('text-anchor', 'middle');
                    text.setAttribute('dominant-baseline', 'middle');
                    text.setAttribute('fill', settings.label_color);
                    text.setAttribute('font-size', settings.label_size);
                    text.setAttribute('font-family', settings.label_font);
                    text.setAttribute('font-weight', '600');
                    text.setAttribute('class', 'sm_label');
                    text.style.textShadow = '1px 1px 2px rgba(0,0,0,0.5)';
                    text.style.pointerEvents = 'none';
                    text.textContent = div.name;
                    svg.appendChild(text);
                }
            });
        },
        
        drawLocations: function(svg) {
            if (!this.mapdata.locations) return;
            
            var settings = this.mapdata.main_settings;
            var locations = this.mapdata.locations;
            var self = this;
            
            Object.keys(locations).forEach(function(key) {
                var loc = locations[key];
                
                // Convert lat/lng to SVG coordinates
                var coords = self.latLngToXY(parseFloat(loc.lat), parseFloat(loc.lng));
                var x = coords.x;
                var y = coords.y;
                
                // Create location marker group
                var g = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                g.setAttribute('class', 'sm_location');
                g.setAttribute('data-name', loc.name);
                g.style.cursor = 'pointer';
                
                // Pulsing circle animation
                var pulseCircle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
                pulseCircle.setAttribute('cx', x);
                pulseCircle.setAttribute('cy', y);
                pulseCircle.setAttribute('r', parseInt(loc.size || settings.location_size) + 5);
                pulseCircle.setAttribute('fill', loc.color || settings.location_color);
                pulseCircle.setAttribute('fill-opacity', '0.3');
                
                var animate1 = document.createElementNS('http://www.w3.org/2000/svg', 'animate');
                animate1.setAttribute('attributeName', 'r');
                animate1.setAttribute('values', (parseInt(loc.size || settings.location_size) + 5) + ';' + (parseInt(loc.size || settings.location_size) + 12) + ';' + (parseInt(loc.size || settings.location_size) + 5));
                animate1.setAttribute('dur', '2s');
                animate1.setAttribute('repeatCount', 'indefinite');
                pulseCircle.appendChild(animate1);
                
                var animate2 = document.createElementNS('http://www.w3.org/2000/svg', 'animate');
                animate2.setAttribute('attributeName', 'opacity');
                animate2.setAttribute('values', '0.3;0.1;0.3');
                animate2.setAttribute('dur', '2s');
                animate2.setAttribute('repeatCount', 'indefinite');
                pulseCircle.appendChild(animate2);
                
                g.appendChild(pulseCircle);
                
                // Main marker circle
                var circle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
                circle.setAttribute('cx', x);
                circle.setAttribute('cy', y);
                circle.setAttribute('r', parseInt(loc.size || settings.location_size) / 2);
                circle.setAttribute('fill', loc.color || settings.location_color);
                circle.setAttribute('fill-opacity', loc.opacity || settings.location_opacity);
                circle.setAttribute('stroke', settings.location_border_color);
                circle.setAttribute('stroke-width', settings.location_border);
                circle.setAttribute('class', 'sm_location_back');
                circle.style.transition = 'all 0.3s ease';
                
                // Add hover effects
                circle.addEventListener('mouseenter', function() {
                    this.setAttribute('fill-opacity', settings.location_hover_opacity);
                    this.setAttribute('stroke-width', settings.location_hover_border);
                    this.setAttribute('r', parseInt(loc.size || settings.location_size) / 2 + 3);
                    
                    // Show popup
                    if (loc.description) {
                        self.showPopup(loc.name, loc.description, x, y);
                    }
                });
                
                circle.addEventListener('mouseleave', function() {
                    this.setAttribute('fill-opacity', loc.opacity || settings.location_opacity);
                    this.setAttribute('stroke-width', settings.location_border);
                    this.setAttribute('r', parseInt(loc.size || settings.location_size) / 2);
                    self.hidePopup();
                });
                
                // Add click handler
                circle.addEventListener('click', function() {
                    console.log('Clicked on:', loc.name);
                    // You can add custom click behavior here
                });
                
                g.appendChild(circle);
                
                // Add WBS text on marker
                var text = document.createElementNS('http://www.w3.org/2000/svg', 'text');
                text.setAttribute('x', x);
                text.setAttribute('y', y + 3);
                text.setAttribute('text-anchor', 'middle');
                text.setAttribute('dominant-baseline', 'middle');
                text.setAttribute('fill', '#ffffff');
                text.setAttribute('font-size', '9');
                text.setAttribute('font-weight', 'bold');
                text.setAttribute('font-family', 'Arial');
                text.style.pointerEvents = 'none';
                text.textContent = 'WBS';
                g.appendChild(text);
                
                svg.appendChild(g);
            });
        },
        
        latLngToXY: function(lat, lng) {
            // Bangladesh bounds: lat 20.5-26.6, lng 88.0-92.7
            // Map these to our SVG viewBox (1000x1200)
            
            // Longitude 88-92.7 maps to X: 100-900 (800 pixel width with margins)
            var x = ((lng - 88.0) / 4.7) * 800 + 100;
            
            // Latitude 26.6-20.5 maps to Y: 50-1150 (inverted, north is top)
            var y = ((26.6 - lat) / 6.1) * 1100 + 50;
            
            return {x: x, y: y};
        },
        
        showPopup: function(title, description, x, y) {
            this.hidePopup();
            
            var popup = document.createElement('div');
            popup.id = 'sm_popup';
            popup.className = 'sm_popup';
            popup.innerHTML = '<strong>' + title + '</strong><br/>' + description;
            
            // Position relative to map container
            var rect = this.mapdiv.getBoundingClientRect();
            var scaleX = rect.width / 1000; // viewBox width
            var scaleY = rect.height / 1200; // viewBox height
            
            popup.style.position = 'absolute';
            popup.style.left = (x * scaleX) + 'px';
            popup.style.top = (y * scaleY - 60) + 'px';
            popup.style.transform = 'translateX(-50%)';
            popup.style.zIndex = '1000';
            
            this.mapdiv.style.position = 'relative';
            this.mapdiv.appendChild(popup);
        },
        
        hidePopup: function() {
            var popup = document.getElementById('sm_popup');
            if (popup) {
                popup.remove();
            }
        },
        
        addZoomControls: function() {
            var controls = document.createElement('div');
            controls.className = 'sm_zoom_controls';
            controls.style.position = 'absolute';
            controls.style.top = '10px';
            controls.style.right = '10px';
            controls.style.zIndex = '100';
            
            var zoomIn = document.createElement('button');
            zoomIn.className = 'sm_zoom_button';
            zoomIn.innerHTML = '+';
            zoomIn.title = 'Zoom In';
            zoomIn.onclick = function() {
                console.log('Zoom in - Feature coming soon');
            };
            
            var zoomOut = document.createElement('button');
            zoomOut.className = 'sm_zoom_button';
            zoomOut.innerHTML = '?';
            zoomOut.title = 'Zoom Out';
            zoomOut.onclick = function() {
                console.log('Zoom out - Feature coming soon');
            };
            
            controls.appendChild(zoomIn);
            controls.appendChild(zoomOut);
            
            this.mapdiv.style.position = 'relative';
            this.mapdiv.appendChild(controls);
        }
    };
    
    // Auto-load if enabled
    if (typeof simplemaps_countrymap_mapdata !== 'undefined' && 
        simplemaps_countrymap_mapdata.main_settings.auto_load === 'yes') {
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', function() {
                simplemaps_countrymap.load();
            });
        } else {
            simplemaps_countrymap.load();
        }
    }
    
    // Expose globally
    window.simplemaps_countrymap = simplemaps_countrymap;
})();
