# Invoice Fraud Detection Rules

## Overview
This document outlines the patterns used by `fraud_detector.py` to flag suspicious invoices.

## Red Flags

### 1. Structural Anomalies
*   **Invalid GSTIN**: Checksum failure or incorrect state code matches.
*   **Missing Fields**: Absence of Invoice Date or HSN Code.

### 2. Behavioral Patterns
*   **Round Numbering**: High value transactions (e.g., ₹1,00,000) exactly are statistically rare in genuine business.
*   **Backdating**: Invoices issued with dates significantly in the past (more than 30 days) without e-Way bill correlation.
*   **Circular Trading**: Frequent transactions of same value between same two entities.

### 3. Rate Mismatches
*   Applying 5% GST on 18% HSN category goods.
*   Applying IGST for intra-state supply (Supplier State = Recipient State).

## Agent Action
If any of these flags are raised, the Agent must:
1.  Return a `risk_level` (HIGH/MEDIUM/LOW).
2.  Cite the specific rule violated from this document.
3.  Suggest manual audit.
