# Retailer Intelligence System (RIS)

## Overview
A comprehensive control system for beverage retailers to manage inventory, billing, shelf optimization, and sales intelligence. 
**Architecture**: Service-Oriented (Blazor Server adapted from MVC pattern)
**Database**: Local NoSQL (LiteDB)

## Technical Architecture

### Core Stack
*   **Framework**: ASP.NET Core Blazor (Server)
*   **Database**: LiteDB (Embedded NoSQL)
*   **Reporting/UI**: Syncfusion / MudBlazor
*   **OCR**: Tesseract / IronOCR (planned)

### Project Structure
The system follows a clean separation of concerns:

```
RetailerDashboard/
├── Services/               # (Controllers logic)
│   ├── InventoryService.cs
│   ├── RackService.cs
│   ├── InvoiceService.cs
│   ├── OcrService.cs
│   ├── BillingPredictionService.cs
│   └── ShelfService.cs
├── Models/                 # Data Models
│   ├── Bottle.cs
│   ├── Rack.cs
│   ├── Invoice.cs
│   └── PredictionModel.cs
├── Data/                   # Database Context
│   ├── LiteDbContext.cs
│   └── Repositories/
│       ├── InvoiceRepository.cs
│       └── InventoryRepository.cs
└── Components/            # UI Views
    ├── Pages/Retailer/
    │   ├── Inventory.razor
    │   ├── Invoices.razor
    │   └── OcrScan.razor
```

## Core Modules

### 1. Inventory & Rack Management
*   **Tracked Items**: Thick, Thin, rPET bottles.
*   **Features**:
    *   Rack allocation and visualization.
    *   Stock counting with visual indicators.
    *   Shelf capacity alerts.

### 2. Invoice Generation
*   **Library**: Syncfusion (PDF) / QuestPDF.
*   **Features**:
    *   Generate GST-compliant invoices.
    *   Export to PDF and Print.
    *   QR code embedding on invoices.
*   **Data Fields**: Retailer Name, Date, Items (Type/Qty/Rate), GST %, Total.

### 3. OCR Bill Scanning
*   **Input**: Camera scan or Image upload of supplier bills.
*   **Process**: Extract text -> Parse fields (Date, Amount, Bottle Counts) -> Auto-entry.
*   **Tools**: Tesseract OCR or Azure AI (optional).

### 4. Billing Prediction Engine
*   **Goal**: Predict future stock needs and revenue.
*   **Logic**: Moving averages based on past sales history.
*   **Output**: "Recommended Order: 240 Thin Bottles".

### 5. On-Device Database (LiteDB)
*   **Collections**:
    *   `invoices`
    *   `inventory`
    *   `racks`
    *   `predictions`
*   **Benefits**: Zero configuration, single file storage, fast local access.

## Implementation Roadmap
1.  **Setup**: Install `LiteDB` and configured `LiteDbContext`.
2.  **Core Services**: Implement Inventory and Rack services.
3.  **UI Migration**: Upgrade current `RetailerDashboard` to use real DB data.
4.  **Invoicing**: Add PDF generation.
5.  **Advanced**: Implement OCR and Predictions.
