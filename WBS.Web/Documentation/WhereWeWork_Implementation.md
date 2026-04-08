# Where We Work Feature Implementation

## Overview
This feature allows WBS to showcase where they work across Bangladesh with an interactive map, progress indicators, and detailed district/upazila information.

## Features Implemented

### 1. Database Models
- **District Model** (`WBS.Web/Models/District.cs`)
  - District name (English & Bangla)
  - HasWork flag to indicate if WBS works in this district
  - Latitude & Longitude for map positioning
  - One-to-many relationship with Upazilas

- **Upazila Model** (`WBS.Web/Models/Upazila.cs`)
  - Upazila name (English & Bangla)
  - HasWork flag to indicate if WBS works in this upazila
  - Foreign key relationship to District

### 2. Admin Management
- **Districts Controller** (`WBS.Web/Areas/Admin/Controllers/DistrictsController.cs`)
  - Full CRUD operations for districts
  - List view with upazila counts
  
- **Upazilas Controller** (`WBS.Web/Areas/Admin/Controllers/UpazilasController.cs`)
  - Full CRUD operations for upazilas
  - Filter by district functionality

### 3. Admin Views
Located in `WBS.Web/Areas/Admin/Views/`:
- **Districts/**
  - Index.cshtml - List all districts with statistics
  - Create.cshtml - Add new district
  - Edit.cshtml - Edit district details
  - Delete.cshtml - Delete confirmation
  
- **Upazilas/**
  - Index.cshtml - List all upazilas with district filter
  - Create.cshtml - Add new upazila
  - Edit.cshtml - Edit upazila details
  - Delete.cshtml - Delete confirmation

### 4. Frontend Display
- **Where We Work Page** (`WBS.Web/Views/About/WhereWeWork.cshtml`)
  - Beautiful gradient header
  - Bangladesh map with WBS logo markers on working districts
  - Progress bars showing:
    - District coverage (X/64)
    - Upazila coverage (X/495)
  - Responsive table showing:
    - District names (English/Bangla based on language)
    - Number of upazilas covered per district
    - List of all upazilas where work has been completed
  - Full bilingual support (English/Bangla)
  - Animated progress bars
  - Beautiful modern design with gradients and shadows

### 5. View Model
- **WhereWeWorkViewModel** (`WBS.Web/ViewModels/WhereWeWorkViewModel.cs`)
  - Aggregates all data needed for the page
  - Calculates statistics
  - Provides district work information

### 6. Controller Updates
- **AboutController** updated with data fetching logic for Where We Work page
- Includes district/upazila data with statistics

### 7. Navigation
- Added "Districts" and "Upazilas" menu items in Admin sidebar under "About WBS" section
- Located in `WBS.Web/Areas/Admin/Views/Shared/_AdminLayout.cshtml`

## Database Migration
- Migration created: `20260117082458_AddDistrictAndUpazilaTables`
- Tables created:
  - `Districts` table
  - `Upazilas` table with foreign key to Districts
- Database updated successfully

## Seed Data
- SQL script provided: `WBS.Web/SeedData/BangladeshDistricts.sql`
- Contains all 64 districts of Bangladesh with:
  - English and Bangla names
  - Approximate latitude/longitude coordinates
  - Organized by division
  
To load seed data, run the SQL script against your database.

## Usage Instructions

### Admin Panel
1. Navigate to Admin > About WBS > Districts
2. Add or edit districts, mark which ones have WBS work
3. For each district, add upazilas via Admin > About WBS > Upazilas
4. Mark which upazilas have WBS activities

### Frontend Display
- Visit: `/about/wherewework`
- The page will automatically:
  - Show map with markers on districts where HasWork = true
  - Calculate and display progress bars
  - List all districts and their working upazilas
  - Support both English and Bangla languages

## Design Features
- Modern gradient color scheme (teal to blue)
- Responsive design works on all devices
- Animated progress bars
- Pulse animation on map markers
- Beautiful card-based layout
- Bilingual support throughout
- Clean table design with badges
- Professional typography

## Technologies Used
- ASP.NET Core 8 MVC
- Entity Framework Core
- Bootstrap 5
- Font Awesome icons
- Custom CSS with gradients and animations
- SVG for Bangladesh map visualization

## Future Enhancements (Optional)
1. Replace simplified SVG map with detailed Bangladesh map
2. Add interactive tooltips on map markers
3. Add filtering/search in the table
4. Add charts for visual statistics
5. Add export functionality for district data
6. Integration with actual WBS project/program data
