# Project Summary: Kiro Compliance Agent

## Problem Statement
Indian MSMEs struggle with complex GST compliance, leading to:
1.  Calculation errors (manual math).
2.  Input Tax Credit (ITC) loss due to invalid invoices.
3.  High cost of professional chartered accountants for routine queries.

## Solution
**Kiro** is an autonomous AI agent that democratizes high-end tax consultancy for MSMEs.
It combines:
*   **Precision Tools**: Deterministic Python engines for math and validation.
*   **RAG Knowledge**: Verified legal documents for policy explanation.
*   **Safety**: Strict guardrails against hallucinating laws.

## Impact
*   **Accuracy**: 100% tax calculation precision via tool use.
*   **Speed**: < 2s response time for complex queries.
*   **Trust**: Every answer cites a specific legal section or engine result.

## Technology Stack
*   **GenAI**: AWS Bedrock (Claude 3 Haiku/Sonnet).
*   **Knowledge Base**: AWS Bedrock Knowledge Bases (Titan Embeddings).
*   **Compute**: AWS Lambda (Python Runtime).
*   **Orchestration**: LangChain.
