# Bill Template System Specification

## Purpose
Defines the standard format for generating compliant invoice templates for MSMEs within the Kiro ecosystem.

## Template Variables

| Variable | Description | format |
| :--- | :--- | :--- |
| `{{gstin_sender}}` | Supplier's GSTIN | Alphanumeric (15) |
| `{{invoice_no}}` | Unique Invoice Number | String |
| `{{hsn_code}}` | Harmonized System Nomenclature | 4-8 Digits |
| `{{taxable_val}}` | Value before tax | Float |

## Validation Rules

1.  **Header Compliance**: Must contain "TAX INVOICE" specified in Rule 46 of CGST Rules.
2.  **Date Format**: ISO 8601 or DD-MM-YYYY.
3.  **Signature**: Digital signature placeholder must be present.

## Auto-Generation Logic
The Agent uses `gst_engine.py` to fill the tax columns (`cgst_amt`, `sgst_amt`, `igst_amt`) automatically based on the `place_of_supply`.
