import json

def calculate_gst(amount: float, rate: float) -> dict:
    """
    Calculates the GST breakdown for a given amount and rate.
    
    Args:
        amount (float): The taxable value.
        rate (float): The GST rate (e.g., 5.0, 12.0, 18.0, 28.0).
        
    Returns:
        dict: A dictionary containing CGST, SGST, IGST, and Total Tax.
    """
    if rate not in [0, 5, 12, 18, 28]:
        return {"error": "Invalid GST Rate. Standard rates are 0%, 5%, 12%, 18%, 28%."}

    tax_amount = (amount * rate) / 100
    cgst = tax_amount / 2
    sgst = tax_amount / 2
    
    # For intra-state (Standard assumption for local calculation tool)
    return {
        "taxable_value": amount,
        "gst_rate": f"{rate}%",
        "cgst_amount": round(cgst, 2),
        "sgst_amount": round(sgst, 2),
        "igst_amount": 0.0,  # Assuming intra-state for basic tool
        "total_tax": round(tax_amount, 2),
        "total_amount": round(amount + tax_amount, 2)
    }

if __name__ == "__main__":
    # Test cases
    print(json.dumps(calculate_gst(5000, 18), indent=2))
