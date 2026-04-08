// Bangladesh Map Data Configuration
// This file contains the map configuration for SimpleMaps Bangladesh Country Map

var simplemaps_countrymap_mapdata = {
    main_settings: {
        // General settings
        width: "responsive",
        background_color: "#FFFFFF",
        background_transparent: "yes",
        border_color: "#ffffff",
        
        // State defaults
        state_description: "State description",
        state_color: "#88A4BC",
        state_hover_color: "#3B729F",
        state_url: "",
        border_size: 1.5,
        all_states_inactive: "no",
        all_states_zoomable: "yes",
        
        // Location defaults
        location_description: "Location description",
        location_url: "",
        location_color: "#00a99d",
        location_opacity: 0.8,
        location_hover_opacity: 1,
        location_size: 25,
        location_type: "circle",
        location_image_source: "frog.png",
        location_border_color: "#FFFFFF",
        location_border: 2,
        location_hover_border: 2.5,
        all_locations_inactive: "no",
        all_locations_hidden: "no",
        
        // Label defaults
        label_color: "#ffffff",
        label_hover_color: "#ffffff",
        label_size: 16,
        label_font: "Arial",
        label_display: "auto",
        label_scale: "yes",
        hide_labels: "no",
        hide_eastern_labels: "no",
       
        // Zoom settings
        zoom: "yes",
        manual_zoom: "yes",
        back_image: "no",
        initial_back: "no",
        initial_zoom: "-1",
        initial_zoom_solo: "no",
        region_opacity: 1,
        region_hover_opacity: 0.6,
        zoom_out_incrementally: "yes",
        zoom_percentage: 0.99,
        zoom_time: 0.5,
        
        // Popup settings
        popup_color: "white",
        popup_opacity: 0.9,
        popup_shadow: 1,
        popup_corners: 5,
        popup_font: "12px/1.5 Verdana, Arial, Helvetica, sans-serif",
        popup_nocss: "no",
        
        // Advanced settings
        div: "map",
        auto_load: "yes",
        url_new_tab: "no",
        images_directory: "default",
        fade_time: 0.1,
        link_text: "View Website",
        popups: "detect",
        state_image_url: "",
        state_image_position: "",
        location_image_url: ""
    },
    
    state_specific: {
        BDA: {
            name: "Barisal",
            description: "Barisal Division"
        },
        BDB: {
            name: "Chittagong",
            description: "Chittagong Division"
        },
        BDC: {
            name: "Dhaka",
            description: "Dhaka Division"
        },
        BDD: {
            name: "Khulna",
            description: "Khulna Division"
        },
        BDE: {
            name: "Rajshahi",
            description: "Rajshahi Division"
        },
        BDF: {
            name: "Rangpur",
            description: "Rangpur Division"
        },
        BDG: {
            name: "Sylhet",
            description: "Sylhet Division"
        },
        BDH: {
            name: "Mymensingh",
            description: "Mymensingh Division"
        }
    },
    
    locations: {
        "0": {
            name: "Dhaka",
            lat: "23.723056",
            lng: "90.408611",
            description: "Capital City"
        }
    },
    
    labels: {
        BDA: {
            name: "Barisal",
            parent_id: "BDA"
        },
        BDB: {
            name: "Chittagong",
            parent_id: "BDB"
        },
        BDC: {
            name: "Dhaka",
            parent_id: "BDC"
        },
        BDD: {
            name: "Khulna",
            parent_id: "BDD"
        },
        BDE: {
            name: "Rajshahi",
            parent_id: "BDE"
        },
        BDF: {
            name: "Rangpur",
            parent_id: "BDF"
        },
        BDG: {
            name: "Sylhet",
            parent_id: "BDG"
        },
        BDH: {
            name: "Mymensingh",
            parent_id: "BDH"
        }
    },
    
    legend: {
        entries: []
    },
    
    regions: {}
};
