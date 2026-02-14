# 🟦 PackSight Blazor Portfolio — UI/UX Structure (Hackathon Demo)

This file defines the **complete portfolio + UI flow** for your Blazor web app.
It is designed as a **polished landing website + role-based enterprise dashboard entry**.

Goal:
A visually clean portfolio site where judges click **“Track Cola”** → choose role → login → dashboard preview.

This is NOT backend-heavy.
This is **presentation-first enterprise UI** for hackathon demo.

---

# 🌐 APP STRUCTURE

```
Landing Portfolio (Home)
 ├── Track Cola (Primary CTA)
 │    └── Role Selection Page
 │         ├── Factory Packers (Hindustan Beverages Associate)
 │         ├── Transport Partner
 │         ├── Wholesaler
 │         ├── Retail Shop
 │         ├── Event/Bulk Manager
 │
 ├── About System
 ├── How It Works
 ├── Impact
 ├── Contact / Join Us
```

---

# 🎨 UI STYLE GUIDE

## Color theme

* Primary: #8B0000 (cola red)
* Secondary: #111111
* Accent: #00E0FF
* Background: gradient dark

## Font

Use:

```
Inter
Poppins
Segoe UI
```

---

# 📦 UI LIBRARIES FOR BLAZOR

Install these for aesthetic UI:

### MudBlazor (recommended)

```bash
dotnet add package MudBlazor
```

### Add to `Program.cs`

```csharp
builder.Services.AddMudServices();
```

### In `MainLayout.razor`

```razor
<MudThemeProvider />
<MudDialogProvider />
<MudSnackbarProvider />
```

---

# 🏠 LANDING PAGE DESIGN

## Pages/Index.razor

### Sections

1. Hero section
2. Track Cola button
3. System overview
4. Role cards
5. Footer

---

## HERO SECTION

Text:

**PackSight**
Smart Packaging Lifecycle Intelligence

Subtext:
Track bottles from factory to recycler in real time.

### Main CTA button

**Track Cola**

Button style:

* glowing hover
* scale animation
* red gradient

---

# 🟥 TRACK COLA BUTTON

When clicked:
navigate to `/roles`

Button code concept:

```
<MudButton Variant="Filled" Color="Error" Size="Large">
   Track Cola
</MudButton>
```

Hover:

* scale 1.05
* glow shadow

---

# 👥 ROLE SELECTION PAGE

## Pages/Roles.razor

Grid layout of role cards.

### Roles:

1. Factory Packers
   (Hindustan Beverages Associate)

2. Transport Partner

3. Wholesaler

4. Retail Shop

5. Event/Bulk Manager

---

# 🟦 ROLE CARD DESIGN

Each card:

* icon
* title
* short description
* hover animation
* login button

### Hover effect

* elevate shadow
* slight zoom
* color border glow

---

# 🔐 LOGIN PAGE FLOW

Each role opens:

```
/login/factory
/login/transport
/login/wholesaler
/login/retail
/login/event
```

Login pages are visually themed differently.

---

# 🏭 FACTORY LOGIN PAGE

Theme:
industrial / dark blue

Fields:

* factory ID
* operator name
* shift

Button:
Enter Factory Dashboard

---

# 🚛 TRANSPORT LOGIN PAGE

Theme:
dark + route map pattern

Fields:

* truck ID
* driver name
* route

---

# 🏪 WHOLESALER LOGIN

Theme:
warehouse style

Fields:

* distributor ID
* location

---

# 🛍 RETAIL LOGIN

Theme:
shop UI

Fields:

* shop name
* city

---

# 🎉 EVENT LOGIN (Wedding / Bulk)

Theme:
light festive gradient

Fields:

* event name
* location
* expected bottle count

---

# 🎛 DASHBOARD PREVIEW (for demo only)

After login:
show preview dashboard:

* total bottles
* in transit
* returned
* recycled
* thickness chart

No full backend required for hackathon.

---

# ✨ HOVER ANIMATIONS

## Buttons

* scale 1.05
* shadow glow
* color shift

## Cards

* lift on hover
* border glow
* smooth transition 0.3s

---

# 🎬 VIDEO FLOW USING THIS UI

1. Open landing page
2. Hover on Track Cola
3. Click
4. Role selection page
5. Choose Factory
6. Login
7. Dashboard preview

Repeat for:

* transport
* retailer
* event

This shows full pipeline visually.

---

# 🧠 UI COMPONENT LIST

Create components:

```
Components/
  Hero.razor
  RoleCard.razor
  DashboardPreview.razor
```

---

# 📱 RESPONSIVE DESIGN

Must work for:

* laptop (demo screen)
* tablet

Not required for mobile in hackathon.

---

# 🏁 MINIMUM BUILD FOR HACKATHON

You must implement:

✔ Landing page
✔ Track Cola button
✔ Role selection page
✔ 5 login pages
✔ Dashboard preview
✔ Smooth hover effects

This is enough for judges.

---

# 🔮 FUTURE ADD

* QR scanner integration
* AI thickness detection
* real database
* truck routing

---

# FINAL NOTE

This portfolio acts as:

* landing website
* enterprise product showcase
* role-based system entry

It should feel like a **real industry SaaS platform**.

---
