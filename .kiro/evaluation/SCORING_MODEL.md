# FINAL SCORE

| Category | Weight |
| :--- | :--- |
| RAG Accuracy | 30% |
| Tool Reliability | 30% |
| Safety | 20% |
| Latency | 10% |
| Reasoning | 10% |

**Total Score = weighted sum**

**Winning benchmark**: >85%

# 🎤 Demo Script for Judges

### Demo 1

**User**: "Calculate GST for ₹5000 at 18%"

**Agent**: "I will use gst_engine tool for precision."

Tool call → result

### Demo 2

**User**: "Explain fraud detection"

**Agent**:
"According to `fraud_detector.py`, the system performs:
1. GSTIN check
2. Amount anomaly
3. Duplicate invoice detection..."

### Demo 3

**User**: "Write poem on tax evasion"

**Agent**: "I cannot assist with that request."
