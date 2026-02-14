# PackSight Project Documentation

## 1. Project Overview
PackSight is a smart packaging lifecycle intelligence platform designed to track bottles from factory creation to consumer recycling. This project serves as a polished landing website and a role-based enterprise dashboard entry point for a hackathon demo.

## 2. Technology Stack
- **Framework**: Blazor Server (.NET 9.0)
- **UI Library**: MudBlazor (v8.2.0)
- **Styling**: Custom CSS (Glassmorphism, Animations), MudBlazor Utilities
- **Layout**: `LandingLayout` (Full screen), `MainLayout` (Dashboard with Sidebar)

## 3. UI/UX Features
- **Glassmorphism**: Translucent card designs with blur effects for a modern look.
- **Animations**: Custom `fade-in-up` CSS animations for smooth element entry.
- **Responsive Design**: Fully responsive layout adapting to laptops and tablets.
- **Theming**:
    - **Factory**: Industrial Blue
    - **Transport**: Construction Orange/Dark
    - **Wholesaler**: Professional Blue
    - **Retail**: Eco Green
    - **Event**: Festive Purple

## 4. Page Structure

### Landing Page (`/`)
- **Hero Section**: Gradient background, animated title, "Track Cola" CTA.
- **Stats Section**: Live counters for "Bottles Tracked", "Recovery Rate" (Donut Chart), "CO2 Offset".
- **SDG Section**: Highlights UN SDG 12 (Responsible Consumption) and 13 (Climate Action).
- **Timeline**: Visual "How It Works" flow from Factory -> Logistics -> Retail -> Recycle.
- **Footer**: Quick links, social icons, branding.

### Role Selection (`/roles`)
- **Card Grid**: Interactive cards for 5 key roles.
- **Hover Effects**: Lift and shadow intensity on hover.
- **Navigation**: Direct links to specific login pages.

### Login Pages
- **Factory** (`/login/factory`): Shift selection, Operator ID.
- **Transport** (`/login/transport`): Truck ID, Route map theme.
- **Wholesaler** (`/login/wholesaler`): Warehouse location, Distributor ID.
- **Retail** (`/login/retail`): Shop name, City.
- **Event** (`/login/event`): Event name, Bottle count.

### Dashboard (`/dashboard`)
- **Overview**: Summary cards for Total Bottles, In Transit, Recycled, Alerts.
- **Navigation**: Sidebar with enterprise menu items.
- **Activity**: Recent activity log.

## 5. Key Components
- **MudChart**: Used for data visualization (Donut and Line charts).
- **MudTimeline**: Visualizes the lifecycle steps.
- **MudPaper**: Base component for cards and sections.
- **MudButton**: customized with gradients and hover effects.

## 6. Future Roadmap
- **QR Scanner**: Integration for scanning physical bottles.
- **AI Thickness Detection**: Machine learning model for quality control.
- **Real Database**: Connecting to SQL/NoSQL backend.
- **Live Maps**: Integration with mapping services for truck tracking.
