# Event Participants Count Not Showing - Fix Guide

## Problem
Participant count shows "0 / 1000" in both Admin Panel and Frontend Events page, even though registrations exist in the database.

## Root Cause
The `RegisteredCount` property in Event model is a computed property that counts from the `Registrations` collection. However, the queries were not including (`Include`) the Registrations navigation property, so the collection was always empty.

## Solution Applied

### 1. Admin Panel - Events Index
**File:** `WBS.Web/Areas/Admin/Controllers/EventsController.cs`

**Before:**
```csharp
public async Task<IActionResult> Index()
{
    var events = await _context.Events
        .OrderByDescending(e => e.StartDate)
        .ToListAsync();
    return View(events);
}
```

**After:**
```csharp
public async Task<IActionResult> Index()
{
    var events = await _context.Events
        .Include(e => e.Registrations)  // ? Added this line
        .OrderByDescending(e => e.StartDate)
        .ToListAsync();
    return View(events);
}
```

### 2. Admin Panel - Events Edit
**File:** `WBS.Web/Areas/Admin/Controllers/EventsController.cs`

**Before:**
```csharp
public async Task<IActionResult> Edit(int id)
{
    var eventModel = await _context.Events.FindAsync(id);
    // ...
}
```

**After:**
```csharp
public async Task<IActionResult> Edit(int id)
{
    var eventModel = await _context.Events
        .Include(e => e.Registrations)  // ? Added this line
        .FirstOrDefaultAsync(e => e.Id == id);
    // ...
}
```

### 3. Frontend - Events Page
**File:** `WBS.Web/Controllers/GetInvolvedController.cs`

**Before:**
```csharp
public async Task<IActionResult> Events()
{
    var events = await _context.Events
        .Where(e => e.IsActive && ...)
        .ToListAsync();
    return View(events);
}
```

**After:**
```csharp
public async Task<IActionResult> Events()
{
    var events = await _context.Events
        .Include(e => e.Registrations)  // ? Added this line
        .Where(e => e.IsActive && ...)
        .ToListAsync();
    return View(events);
}
```

## How RegisteredCount Works

```csharp
public class Event
{
    // Navigation property
    public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();

    // Computed property - counts "Confirmed" registrations
    [NotMapped]
    public int RegisteredCount => Registrations?.Count(r => r.Status == "Confirmed") ?? 0;

    // Available seats calculation
    [NotMapped]
    public int AvailableSeats => TotalCapacity.HasValue ? TotalCapacity.Value - RegisteredCount : 0;
}
```

## Testing

### Option 1: Use Existing Registrations
If you already have registrations in the database:
1. Restart the application
2. Go to Admin Panel ? Events
3. You should now see the correct count (e.g., "5 / 1000")

### Option 2: Insert Sample Registrations
Run this SQL script to add test registrations:
**File:** `WBS.Web/DbScripts/InsertSampleRegistrations.sql`

This will:
- Find the first active event
- Insert 5 sample registrations (4 Confirmed, 1 Pending)
- Display the summary

### Option 3: Check Database Manually
Run this SQL script to verify:
**File:** `WBS.Web/DbScripts/CheckEventRegistrations.sql`

This will show:
- EventRegistrations table structure
- Total registration count
- Registrations by event
- Recent registrations

## Verification Steps

1. **Check Database:**
```sql
-- Quick check
SELECT 
    e.Id, 
    e.Title, 
    COUNT(er.Id) as TotalReg,
    SUM(CASE WHEN er.Status = 'Confirmed' THEN 1 ELSE 0 END) as Confirmed
FROM Events e
LEFT JOIN EventRegistrations er ON e.Id = er.EventId
GROUP BY e.Id, e.Title
```

2. **Restart Application**
   - Stop the application
   - Start again (Hot Reload might not work for this change)

3. **Verify Admin Panel:**
   - Go to: `/admin/events`
   - Check "Registered" column shows correct count
   - Click "Participants (X)" button to see details

4. **Verify Frontend:**
   - Go to: `/events` or `/getinvolved/Events`
   - Check "Seats: X / Total" shows correct count

## Expected Results

**Before Fix:**
- Admin: "0 / 1000" (1000 seats left)
- Frontend: "Seats: 0 / 1000 (1000 left)"

**After Fix (with 5 registrations, 4 confirmed):**
- Admin: "4 / 1000" (996 seats left)
- Frontend: "Seats: 4 / 1000 (996 left)"

## Important Notes

1. **Only "Confirmed" registrations are counted**
   - Registrations with Status = "Confirmed" are included
   - Status = "Pending" or "Cancelled" are NOT counted

2. **Entity Framework Include is required**
   - Without `.Include(e => e.Registrations)`, the collection is empty
   - This is called "Lazy Loading" vs "Eager Loading"

3. **Performance Consideration**
   - Including Registrations loads all registration data
   - For events with many participants, this is acceptable
   - For better performance, could use projection:
   ```csharp
   .Select(e => new {
       e.Id,
       e.Title,
       RegisteredCount = e.Registrations.Count(r => r.Status == "Confirmed")
   })
   ```

## Troubleshooting

### Issue: Count still shows 0
**Solution:** 
- Check if EventRegistrations table exists
- Run `CheckEventRegistrations.sql` script
- Verify registration Status is "Confirmed"

### Issue: Application error after changes
**Solution:**
- Check build succeeded
- Verify all files saved
- Restart Visual Studio if needed

### Issue: Different count in Admin vs Frontend
**Solution:**
- Check both controllers have `.Include(e => e.Registrations)`
- Clear browser cache
- Hard refresh (Ctrl+F5)

## Files Changed
1. ? `WBS.Web/Areas/Admin/Controllers/EventsController.cs`
2. ? `WBS.Web/Controllers/GetInvolvedController.cs`

## Files Created
1. ?? `WBS.Web/DbScripts/CheckEventRegistrations.sql`
2. ?? `WBS.Web/DbScripts/InsertSampleRegistrations.sql`
3. ?? `WBS.Web/DbScripts/EVENT_PARTICIPANTS_COUNT_FIX.md` (this file)
