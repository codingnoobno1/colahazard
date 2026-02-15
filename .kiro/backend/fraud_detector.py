import re

def validate_gstin(gstin: str) -> bool:
    """
    Validates the structure of a GSTIN (Goods and Services Tax Identification Number).
    Format: 2 digits (State) + 10 chars (PAN) + 1 digit (Entity) + 'Z' + 1 char (Check)
    Example: 29ABCDE1234F1Z5
    """
    regex = r"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$"
    return bool(re.match(regex, gstin))

def fraud_check_invoice(invoice_data: dict) -> dict:
    """
    Analyzes invoice data for potential fraud indicators.
    
    Args:
        invoice_data (dict): Dictionary containing invoice details.
                             Required keys: 'gstin', 'amount', 'date', 'invoice_id'
    
    Returns:
        dict: Risk analysis report.
    """
    risks = []
    score = 0
    
    # 1. GSTIN Structural Validation
    if not validate_gstin(invoice_data.get("gstin", "")):
        risks.append("Invalid GSTIN format")
        score += 50
        
    # 2. High Value Transaction Check (Round number anomaly)
    amount = invoice_data.get("amount", 0)
    if amount > 50000 and amount % 1000 == 0:
        risks.append("Round number high-value transaction (Potential anomaly)")
        score += 10
        
    # 3. Weekend Invoice Check (Mock logic)
    # real implementation would parse date
    
    risk_level = "LOW"
    if score > 20: risk_level = "MEDIUM"
    if score > 40: risk_level = "HIGH"
    
    return {
        "invoice_id": invoice_data.get("invoice_id"),
        "risk_score": score,
        "risk_level": risk_level,
        "flags": risks
    }

if __name__ == "__main__":
    # Test
    test_inv = {
        "invoice_id": "INV-001",
        "gstin": "29ABCDE1234F1Z5", 
        "amount": 100000
    }
    print(fraud_check_invoice(test_inv))
