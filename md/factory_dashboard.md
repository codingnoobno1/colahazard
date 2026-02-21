# 🏭 Factory Control Center v2.0 (Enterprise)

Welcome to the **EcoCola Enterprise Factory Control Center**. This system provides a high-fidelity, industrial-grade traceability mesh for beverage manufacturing and logistics.

## 🏗 System Architecture

The dashboard implements a multi-tier hierarchical tracking system:

1.  **Plant Level**: Physical manufacturing facility metadata.
2.  **Production Batch**: High-level production run with liquid batch codes and quality status.
3.  **Pallet Level**: Logistics grouping of units for warehouse movement.
4.  **Carton Level**: Packaging sub-groups.
5.  **Bottle Unit (Unique ID)**: Individual unit-level tracking with specific IoT metadata.

## 🚀 Key Features

### 1. Real-time Traceability Mesh
Every individual bottle produced is assigned a **Unique QR ID** (e.g., `ECO-B-20260221-3130-01-0005`).
Each unit tracks:
- **Liquid Specs**: pH Value, Carbonation Level, Fill Temperature.
- **Packaging Integrity**: Pressure at Sealing, Cap Type, Label Version, Bottle Thickness (Microns).
- **Material & Grade**: Material Grade (e.g., A+), Material Type (rPET/Thick/Thin).
- **Lifecycle Status**: Produced → In Transit → Retail → Sold → Returned → Recycled.

### 2. Financial & Compliance Intelligence
The dashboard automatically calculates:
- **Total Revenue**: Based on successfully produced units and wholesale rates.
- **Projected Profit**: Calculated with a dynamic 40% margin.
- **Compliance Tracking**: Tax Codes (GST), Operator/Supervisor IDs, and Batch Expiry.
- **Yield Monitoring**: Real-time tracking of rejection rates (simulated at 2% for industrial realism).


### 3. Professional Action Center
- **Industrial PDF Export**: Generates branded production summaries using QuestPDF.
- **Enterprise CSV Export**: Detailed data dump for further analysis in ERP systems.
- **System Health Monitoring**: Live "pulse" indicator showing traceability mesh status.

## 🛠 Technical Stack
- **Frontend**: Blazor Server with MudBlazor (Premium Glassmorphism).
- **Backend**: C# 12 / .NET 9.
- **Database**: LiteDB (Filesystem-based NoSQL).
- **Reporting**: QuestPDF (PDF) & Custom CSV Serialization.
- **Interoperability**: JavaScript interop for browser-level file stream handling.

## 📊 Industrial KPIs
| KPI | Description | Target |
| :--- | :--- | :--- |
| **Yield %** | Ratio of approved units to planned units. | > 98% |
| **Machine Efficiency** | Simulated based on downtime and batch speed. | 95% |
| **Traceability Integrity** | Verification rate of Unique Bottle IDs. | 100% |

## 📁 Directory Structure
- `/Components/Pages/Factory/FactoryDashboard.razor`: Main Control Center page.
- `/Services/FactoryService.cs`: Core industrial logic.
- `/Services/PdfReportService.cs`: Professional PDF generator.
- `/Models/EnterpriseModels.cs`: The 12-tier hierarchical data models.
- `/Data/LiteDbContext.cs`: Collection registry.
- `/wwwroot/js/fileDownload.js`: Export helper.

---
*Enterprise Edition - PackSight Industrial Suite*
