# System Architecture

## Overview
The Kiro MSME Compliance Agent is built on a retrieval-augmented generation (RAG) architecture powered by AWS Bedrock.

## Components

### 1. Frontend
*   Interface: Streamlit / React
*   Function: User query capture, response display, file upload for audit.

### 2. Orchestration Layer (AWS Lambda / LangChain)
*   Routes user queries to either:
    *   **RAG Pipeline**: For policy questions.
    *   **Tool Execution**: For calculations and validations.
*   Maintains conversation state (DynamoDB).

### 3. Knowledge Base (AWS Bedrock)
*   **Source**: Regulatory PDFs, Internal Spec Sheets (`.md`).
*   **Embeddings**: Titan Text Embeddings v2.
*   **Vector Store**: OpenSearch Serverless.

### 4. Tool Backend (Python Layers)
*   `gst_engine.py`: Deterministic tax engine.
*   `fraud_detector.py`: Rule-based anomaly detection.

## Data Flow
1.  User Query -> Orchestrator
2.  Orchestrator -> Intent Classification (LLM)
3.  Case A (Info): -> Retrieve from Bedrock KB -> Synthesize Answer
4.  Case B (Action): -> Extract Params -> Call Python Tool -> Return Result
